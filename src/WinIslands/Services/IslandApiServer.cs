using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace WinIslands.Services;

/// <summary>
/// 本地上岛 API（类似 iOS 第三方 App 的“灵动岛”集成）。
/// 监听 http://127.0.0.1:{port}，其他软件可推送信息显示到灵动岛：
///   POST   /v1/island/push        推送/更新一条灵动岛卡片
///   DELETE /v1/island/push/{id}   移除一条
///   GET    /v1/island/active      查询当前活跃推送
///   GET    /v1/health             健康检查
/// 端口、可选 Token、默认显示时长等由设置控制；推送方可按条自定义时长与按钮。
/// </summary>
public sealed class IslandApiServer : IDisposable
{
    private readonly SettingsService _settings;
    private readonly HttpListener _listener = new();
    private CancellationTokenSource _cts = new();
    private Task? _loop;
    private readonly ConcurrentDictionary<string, IslandPush> _active = new();
    private readonly ConcurrentDictionary<string, long> _order = new();   // id -> 入队序号（同 id 更新保持原位）
    private readonly ConcurrentDictionary<WebSocket, byte> _wsClients = new();   // WebSocket 订阅端
    private long _seq;   // 入队序号计数器
    private System.Threading.Timer? _heartbeatTimer;   // 心跳保活检查
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public IslandApiServer(SettingsService settings)
    {
        _settings = settings;
    }

    /// <summary>收到新推送/更新时触发（参数：完整推送，含服务端计算的过期时间）。</summary>
    public event Action<IslandPush>? PushReceived;
    /// <summary>推送被移除或过期时触发。</summary>
    public event Action<string>? PushRemoved;

    public bool IsRunning { get; private set; }

    public IReadOnlyList<IslandPush> ActivePushes
        => _active.Values
            .OrderByDescending(p => p.PriorityRank)
            .ThenBy(p => _order.TryGetValue(p.Id, out var s) ? s : long.MaxValue)
            .ToList();

    public void Start()
    {
        if (IsRunning) return;
        var port = Math.Clamp(_settings.Current.IslandApiPort, 1, 65535);
        var prefix = $"http://127.0.0.1:{port}/";
        try
        {
            _listener.Prefixes.Clear();
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            IsRunning = true;
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            _loop = Task.Run(() => AcceptLoopAsync(_cts.Token));
            // 心跳保活：每 5 秒检查一次，超过 2 倍心跳间隔未续期的推送自动移除
            _heartbeatTimer = new System.Threading.Timer(_ => CheckHeartbeats(), null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5));
            AppLogger.Info($"Island API listening on {prefix}");
        }
        catch (Exception ex)
        {
            IsRunning = false;
            AppLogger.Error($"Island API failed to start on {prefix}", ex);
        }
    }

    public void Stop()
    {
        if (!IsRunning) return;
        try
        {
            _cts.Cancel();
            foreach (var ws in _wsClients.Keys) { try { ws.Abort(); } catch { /* ignore */ } }
            _wsClients.Clear();
            _heartbeatTimer?.Dispose();
            _heartbeatTimer = null;
            _listener.Stop();
            IsRunning = false;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Island API stop: {ex.Message}");
        }
    }

    private async Task AcceptLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var ctx = await _listener.GetContextAsync().ConfigureAwait(false);
                _ = Task.Run(() => HandleAsync(ctx, ct));
            }
            catch (HttpListenerException) { break; }
            catch (OperationCanceledException) { break; }
            catch (ObjectDisposedException) { break; }
            catch (Exception ex) { AppLogger.Warn($"Island API accept: {ex.Message}"); }
        }
    }

    private async Task HandleAsync(HttpListenerContext ctx, CancellationToken ct)
    {
        try
        {
            // 可选 Token 校验
            var token = _settings.Current.IslandApiToken;
            if (!string.IsNullOrEmpty(token))
            {
                var got = ctx.Request.Headers["X-WinIslands-Token"]
                          ?? ctx.Request.QueryString["token"];
                if (!string.Equals(got, token, StringComparison.Ordinal))
                {
                    await WriteJsonAsync(ctx, 401, new { error = "unauthorized" }, ct);
                    return;
                }
            }

            var path = ctx.Request.Url?.AbsolutePath ?? "/";
            var method = ctx.Request.HttpMethod;

            if (method == "GET" && path == "/v1/health")
            {
                await WriteJsonAsync(ctx, 200, new { status = "ok" }, ct);
                return;
            }

            if (method == "GET" && (path == "/v1/island/active" || path == "/v3/island/active"))
            {
                await WriteJsonAsync(ctx, 200, ActivePushes, ct);
                return;
            }

            // WebSocket 推送通道（v3）：客户端连接后可通过 JSON 消息 push/update/remove
            if (method == "GET" && path == "/v3/ws" && ctx.Request.IsWebSocketRequest)
            {
                await HandleWebSocketAsync(ctx, ct);
                return;
            }

            if (method == "POST" && (path == "/v1/island/push" || path == "/v3/island/push"))
            {
                using var reader = new StreamReader(ctx.Request.InputStream, Encoding.UTF8);
                var body = await reader.ReadToEndAsync();
                var push = JsonSerializer.Deserialize<IslandPush>(body, JsonOpts);
                if (push is null || string.IsNullOrWhiteSpace(push.Title))
                {
                    await WriteJsonAsync(ctx, 400, new { error = "title is required" }, ct);
                    return;
                }
                var added = AddOrUpdate(push);
                await WriteJsonAsync(ctx, 200, new { id = added.Id, position = PositionOf(added.Id), expires_at = added.ExpiresAt }, ct);
                return;
            }

            if (method == "PATCH" && path.StartsWith("/v3/island/push/"))
            {
                // v3 部分更新：只覆盖请求体里出现的字段，其余保留（含过期时间/队列位置）
                var id = Uri.UnescapeDataString(path.Substring("/v3/island/push/".Length));
                using var reader = new StreamReader(ctx.Request.InputStream, Encoding.UTF8);
                var body = await reader.ReadToEndAsync();
                var merged = MergePatch(id, body);
                if (merged is null)
                {
                    await WriteJsonAsync(ctx, 404, new { error = "not found" }, ct);
                    return;
                }
                var updated = AddOrUpdate(merged);
                await WriteJsonAsync(ctx, 200, new { id = updated.Id, position = PositionOf(updated.Id), expires_at = updated.ExpiresAt }, ct);
                return;
            }

            if (method == "DELETE" && path.StartsWith("/v1/island/push/"))
            {
                var id = Uri.UnescapeDataString(path.Substring("/v1/island/push/".Length));
                _active.TryRemove(id, out _);
                _order.TryRemove(id, out _);
                _ = Task.Run(() => PushRemoved?.Invoke(id));
                BroadcastEvent("push_removed", id);
                await WriteJsonAsync(ctx, 200, new { ok = true }, ct);
                return;
            }

            await WriteJsonAsync(ctx, 404, new { error = "not found" }, ct);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Island API handler: {ex.Message}");
            try { await WriteJsonAsync(ctx, 500, new { error = "internal error" }, ct); }
            catch { /* ignore */ }
        }
    }

    /// <summary>新增或更新推送（供 HTTP 处理与测试复用）。同 id 保留原过期时间与队列位置。</summary>
    public IslandPush AddOrUpdate(IslandPush push)
    {
        if (string.IsNullOrWhiteSpace(push.Id)) push.Id = Guid.NewGuid().ToString("N");
        push.LastSeenUtc = DateTime.UtcNow;   // 心跳续期

        // v3 动态进度：每次完整更新重置锚点，进度条从 progress_from 自动推进到 progress_to
        if (push.ProgressDurationSeconds is int dur && dur > 0)
        {
            push.ProgressAnchorUtc = DateTime.UtcNow;
            push.Progress ??= Math.Clamp(push.ProgressFrom ?? 0, 0, 1);
        }
        else
        {
            push.ProgressAnchorUtc = null;
        }

        if (_active.TryGetValue(push.Id, out var existingPush))
        {
            // 同 id 更新：刷新内容、保留原过期时间与队列位置（位置不变）
            if (existingPush.ExpiresAt is DateTime oldExp) push.ExpiresAt = oldExp;
        }
        else
        {
            // 新推送：显示时长可自定义，留空用全局默认；分配入队序号
            var seconds = push.DurationSeconds ?? Math.Max(1, _settings.Current.IslandApiDefaultDuration);
            push.DurationSeconds = seconds;
            push.ExpiresAt = DateTime.UtcNow.AddSeconds(seconds);
            _order[push.Id] = Interlocked.Increment(ref _seq);
        }

        _active[push.Id] = push;
        _ = Task.Run(() => PushReceived?.Invoke(push));
        BroadcastEvent("push_updated", push);
        return push;
    }

    /// <summary>移除指定推送（过期/点击后）。</summary>
    public void Remove(string id)
    {
        if (_active.TryRemove(id, out _))
        {
            _order.TryRemove(id, out _);
            _ = Task.Run(() => PushRemoved?.Invoke(id));
            BroadcastEvent("push_removed", id);
        }
    }

    /// <summary>心跳保活检查：配置了 heartbeat_seconds 且超过 2 倍间隔未续期的推送自动移除。</summary>
    private void CheckHeartbeats()
    {
        var now = DateTime.UtcNow;
        foreach (var kv in _active.ToArray())
        {
            var p = kv.Value;
            if (p.HeartbeatSeconds is int hb && hb > 0 &&
                (now - p.LastSeenUtc).TotalSeconds > hb * 2.0)
            {
                Remove(kv.Key);
            }
        }
    }

    /// <summary>v3 PATCH 部分更新：把请求体字段合并到现有推送，返回合并结果（不存在返回 null）。</summary>
    private IslandPush? MergePatch(string id, string body)
    {
        if (!_active.TryGetValue(id, out var existing)) return null;
        var merged = JsonNode.Parse(JsonSerializer.Serialize(existing));
        if (merged is not JsonObject obj) return null;
        using var doc = JsonDocument.Parse(body);
        foreach (var prop in doc.RootElement.EnumerateObject())
            obj[prop.Name] = JsonNode.Parse(prop.Value.GetRawText());
        var result = obj.Deserialize<IslandPush>(JsonOpts);
        if (result is null) return null;
        // Preserve server-side state that is [JsonIgnore] and therefore lost in serialization,
        // so a PATCH that only changes e.g. the title does not restart the progress anchor.
        result.ProgressAnchorUtc = existing.ProgressAnchorUtc;
        result.LastSeenUtc = existing.LastSeenUtc;
        return result;
    }

    /// <summary>WebSocket 端点：双向通道，客户端发 JSON 消息（push/update/remove/ping），服务端回 ok/error 并广播事件。</summary>
    private async Task HandleWebSocketAsync(HttpListenerContext ctx, CancellationToken ct)
    {
        WebSocket ws;
        try
        {
            var wsCtx = await ctx.AcceptWebSocketAsync(null);
            ws = wsCtx.WebSocket;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Island API WS accept: {ex.Message}");
            return;
        }
        _wsClients[ws] = 0;
        try
        {
            var buffer = new byte[128 * 1024];
            while (ws.State == WebSocketState.Open && !ct.IsCancellationRequested)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    try { await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", CancellationToken.None); } catch { /* ignore */ }
                    break;
                }
                if (result.MessageType != WebSocketMessageType.Text) continue;
                // Handle fragmented WebSocket messages: accumulate until EndOfMessage
                var sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                while (!result.EndOfMessage)
                {
                    result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                    if (result.MessageType != WebSocketMessageType.Text) break;
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                }
                var text = sb.ToString();
                _ = Task.Run(() => HandleWsMessageAsync(ws, text));
            }
        }
        catch (OperationCanceledException) { /* stop */ }
        catch (Exception ex) { AppLogger.Warn($"Island API WS: {ex.Message}"); }
        finally
        {
            _wsClients.TryRemove(ws, out _);
            try { ws.Dispose(); } catch { /* ignore */ }
        }
    }

    private async Task HandleWsMessageAsync(WebSocket ws, string text)
    {
        try
        {
            using var doc = JsonDocument.Parse(text);
            var root = doc.RootElement;
            var action = root.TryGetProperty("action", out var a) ? a.GetString() ?? "" : "push";
            if (action == "ping")
            {
                await SendWsAsync(ws, new { type = "ok", action = "ping" });
                return;
            }
            if (action == "remove")
            {
                var id = root.TryGetProperty("id", out var rp) ? rp.GetString() ?? "" : "";
                if (_active.ContainsKey(id))
                {
                    Remove(id);
                    await SendWsAsync(ws, new { type = "ok", action = "remove", id });
                }
                else
                {
                    await SendWsAsync(ws, new { type = "error", action = "remove", message = "not found" });
                }
                return;
            }
            // push / update：与 POST /v1/island/push 相同的 JSON 结构，放在 push 字段里
            var push = root.TryGetProperty("push", out var pr) ? pr.Deserialize<IslandPush>(JsonOpts) : null;
            if (push is null || string.IsNullOrWhiteSpace(push.Title))
            {
                await SendWsAsync(ws, new { type = "error", action = action, message = "title is required" });
                return;
            }
            AddOrUpdate(push);
            await SendWsAsync(ws, new { type = "ok", action = "push", id = push.Id, position = PositionOf(push.Id), expires_at = push.ExpiresAt });
        }
        catch (Exception ex)
        {
            try { await SendWsAsync(ws, new { type = "error", message = ex.Message }); } catch { /* ignore */ }
        }
    }

    private static async Task SendWsAsync(WebSocket ws, object payload)
        => await SendWsAsync(ws, JsonSerializer.Serialize(payload));

    private static async Task SendWsAsync(WebSocket ws, string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        try { await ws.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None); }
        catch { /* 已断开 */ }
    }

    /// <summary>向所有 WebSocket 订阅端广播推送变更事件。</summary>
    private void BroadcastEvent(string evt, IslandPush push)
    {
        if (_wsClients.IsEmpty) return;
        var json = JsonSerializer.Serialize(new { type = "event", @event = evt, push });
        BroadcastJson(json);
    }

    private void BroadcastEvent(string evt, string id)
    {
        if (_wsClients.IsEmpty) return;
        var json = JsonSerializer.Serialize(new { type = "event", @event = evt, id });
        BroadcastJson(json);
    }

    /// <summary>
    /// #10 上岛按钮回调：推送方配置了 notify 动作的按钮被点击时，
    /// 向所有 WebSocket 订阅端广播 push_button 事件（推送方接收后自行处理）。
    /// value 为附加载荷：普通按钮为按钮值；输入框提交时为用户填写的文字（#11）。
    /// </summary>
    public void BroadcastPushButton(string pushId, string button, string? value = null)
    {
        if (_wsClients.IsEmpty) return;
        var payload = new { type = "event", @event = "push_button", push_id = pushId, button, value = string.IsNullOrEmpty(value) ? null : value };
        var json = JsonSerializer.Serialize(payload);
        BroadcastJson(json);
    }

    private void BroadcastJson(string json)
    {
        foreach (var ws in _wsClients.Keys)
            _ = Task.Run(() => SendWsAsync(ws, json));
    }

    /// <summary>推送在显示队列中的位置（从 1 开始；不在队列中返回 0），用于 POST push 响应。</summary>
    private int PositionOf(string id)
    {
        var list = ActivePushes;
        for (var i = 0; i < list.Count; i++)
            if (string.Equals(list[i].Id, id, StringComparison.Ordinal)) return i + 1;
        return 0;
    }

    private static async Task WriteJsonAsync(HttpListenerContext ctx, int code, object payload, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(payload);
        var bytes = Encoding.UTF8.GetBytes(json);
        ctx.Response.StatusCode = code;
        ctx.Response.ContentType = "application/json; charset=utf-8";
        ctx.Response.ContentLength64 = bytes.Length;
        await ctx.Response.OutputStream.WriteAsync(bytes, 0, bytes.Length, ct);
        ctx.Response.Close();
    }

    public void Dispose()
    {
        Stop();
        // HttpListener.Close() permanently disposes the listener; only do it here,
        // never in Stop(), so the server can be restarted (e.g. port change).
        try { _listener.Close(); } catch { /* ignore */ }
        _cts.Dispose();
        _heartbeatTimer?.Dispose();
    }
}
