using System.IO;

using System.Net;

using System.Net.Http;

using System.Net.Sockets;

using System.Text;

using System.Text.Json;

namespace WinIslands.Services;

/// <summary>
/// Which generation of the Cider local API we are talking to.
/// </summary>
public enum CiderApiProfile
{
    None = 0,
    /// <summary>Cider 2.5+ REST API: /api/v1/playback/... (default port 10767)</summary>
    V3 = 1,
    /// <summary>Cider 2.x Discord-RPC style API: /active, /currentPlayingSong, ... (port 10769)</summary>
    LegacyV2 = 2,
}

/// <summary>
/// Encapsulated client for the (unofficial) Cider local HTTP API.
///
/// * Default endpoint: http://127.0.0.1:10767 (V3 profile, /api/v1/playback/...)
/// * Legacy fallback:   http://127.0.0.1:10769 (LegacyV2 profile)
/// * Port auto-scan:    10760..10775 + user-configured port
/// * Optional auth:     "apptoken" header (Cider: Settings > Connectivity > Manage External Application Access)
///
/// Cider's API changes often; keep all endpoint knowledge inside this file so the rest
/// of the app only consumes <see cref="MediaSnapshot"/>.
/// </summary>
public sealed class CiderClient
{
    public const int DefaultPort = 10767;
    public const int LegacyPort = 10769;

    private readonly HttpClient _http;
    private string _token;
    private double _lastCiderPosition = -1;   // 上次 Cider 上报位置（用于按移动判定播放/暂停）
    private string? _lastCiderTrackKey;

    public CiderClient(string token = "")
    {
        _token = token ?? string.Empty;
        var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All };
        _http = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(5), // localhost: 留足余量（部分环境有安全软件/代理拦截回环 HTTP，2s 会频繁超时）
        };
    }

    public CiderApiProfile Profile { get; private set; } = CiderApiProfile.None;

    /// <summary>Force the client back to disconnected so the next tick re-probes.</summary>
    public void MarkDisconnected() => Profile = CiderApiProfile.None;
    public int Port { get; private set; }

    /// <summary>更新 API Token（自动检测或用户手动填写）。</summary>
    public void SetToken(string token) => _token = token ?? string.Empty;
    public bool IsConnected => Profile != CiderApiProfile.None;
    public string? LastError { get; private set; }

    private string BaseUrl => $"http://127.0.0.1:{Port}";

    private HttpRequestMessage Build(HttpMethod method, string path, string? jsonBody = null)
    {
        var req = new HttpRequestMessage(method, BaseUrl + path);
        if (!string.IsNullOrEmpty(_token))
        {
            req.Headers.TryAddWithoutValidation("apptoken", _token);
            req.Headers.TryAddWithoutValidation("apitoken", _token); // older builds used this header
        }

        if (jsonBody is not null)
            req.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        return req;
    }

    // ── Discovery ──────────────────────────────────────────────

    /// <summary>
    /// Find a reachable Cider API. Tries: configured port -> default 10767 -> legacy 10769
    /// -> scan 10760..10775 on loopback. Returns true when connected.
    /// </summary>
    public async Task<bool> ConnectAsync(int configuredPort, CancellationToken ct = default)
    {
        var candidates = new List<int>();
        if (configuredPort > 0) candidates.Add(configuredPort);
        candidates.Add(DefaultPort);
        candidates.Add(LegacyPort);
        for (var p = DefaultPort - 7; p <= DefaultPort + 8; p++)
        {
            if (!candidates.Contains(p)) candidates.Add(p);
        }

        foreach (var port in candidates)
        {
            if (ct.IsCancellationRequested) break;
            if (await IsListeningAsync(port, ct))
            {
                if (await TryConnectAsync(port, ct)) return true;
            }
        }

        LastError = "Cider API not reachable on any candidate port";
        Profile = CiderApiProfile.None;
        return false;
    }

    private static async Task<bool> IsListeningAsync(int port, CancellationToken ct)
    {
        try
        {
            using var tcp = new TcpClient();
            using var timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(ct, timeout.Token);
            await tcp.ConnectAsync(IPAddress.Loopback, port, linked.Token).ConfigureAwait(false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> TryConnectAsync(int port, CancellationToken ct)
    {
        Port = port;

        // V3 probe
        try
        {
            using var req = Build(HttpMethod.Get, "/api/v1/playback/active");
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
            if (resp.StatusCode is HttpStatusCode.OK or HttpStatusCode.NoContent)
            {
                Profile = CiderApiProfile.V3;
                LastError = null;
                AppLogger.Info($"Cider API connected (V3) on port {port}.");
                return true;
            }
        }
        catch (Exception ex) { AppLogger.Debug($"V3 probe {port} failed: {ex.Message}"); }

        // Legacy v2 probe
        try
        {
            using var req = Build(HttpMethod.Get, "/active");
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
            if (resp.StatusCode is HttpStatusCode.OK or HttpStatusCode.NoContent)
            {
                Profile = CiderApiProfile.LegacyV2;
                LastError = null;
                AppLogger.Info($"Cider API connected (LegacyV2) on port {port}.");
                return true;
            }
        }
        catch (Exception ex) { AppLogger.Debug($"Legacy probe {port} failed: {ex.Message}"); }

        Profile = CiderApiProfile.None;
        return false;
    }

    // ── Snapshot ───────────────────────────────────────────────

    /// <summary>Fetch the current track + playback state. Null when nothing is loaded.</summary>
    public async Task<MediaSnapshot?> GetSnapshotAsync(CancellationToken ct = default)
    {
        if (Profile == CiderApiProfile.None) return null;

        try
        {
            if (Profile == CiderApiProfile.V3)
            {
                using var req = Build(HttpMethod.Get, "/api/v1/playback/now-playing");
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                if (resp.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.NoContent) return null;
                if (!resp.IsSuccessStatusCode) return null;
                var json = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                var snap = ParseV3NowPlaying(json, out var statusExplicit);
                if (snap is not null && !statusExplicit)
                    snap = await InferPlaybackStatusByMovementAsync(snap, ct).ConfigureAwait(false);
                return snap;
            }
            else
            {
                using var req = Build(HttpMethod.Get, "/currentPlayingSong");
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode) return null;
                var json = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                return ParseLegacyNowPlaying(json);
            }
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            AppLogger.Warn($"Cider GetSnapshotAsync failed: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Cider 无显式 isPlaying/status 字段时，用「位置是否在移动」判断播放/暂停：
    /// 暂停时 currentPlaybackTime 冻结，播放时随时间前进。首次采样做一次快速二次采样。
    /// </summary>
    private async Task<MediaSnapshot> InferPlaybackStatusByMovementAsync(MediaSnapshot snap, CancellationToken ct)
    {
        var trackKey = $"{snap.Track.Artist}\u0001{snap.Track.Title}";
        if (!string.Equals(trackKey, _lastCiderTrackKey, StringComparison.Ordinal))
        {
            _lastCiderTrackKey = trackKey;
            _lastCiderPosition = -1; // 换曲后重新判定
        }

        if (_lastCiderPosition < 0)
        {
            // 首次/换曲后：等 ~350ms 再采一次样，比较位置是否前进
            try
            {
                await Task.Delay(350, ct).ConfigureAwait(false);
                using var req = Build(HttpMethod.Get, "/api/v1/playback/now-playing");
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                if (resp.IsSuccessStatusCode)
                {
                    var json2 = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                    var snap2 = ParseV3NowPlaying(json2);
                    if (snap2 is not null)
                    {
                        var playing = snap2.PositionSeconds > snap.PositionSeconds + 0.2;
                        _lastCiderPosition = snap2.PositionSeconds;
                        return snap with { Status = playing ? PlaybackStatus.Playing : PlaybackStatus.Paused };
                    }
                }
            }
            catch { /* 采样失败走保守处理 */ }
            _lastCiderPosition = snap.PositionSeconds;
            return snap with { Status = PlaybackStatus.Paused };
        }

        // 后续轮询：位置前进超过 0.4s（跨 1s+ 轮询间隔）视为播放中，冻结视为暂停
        var moved = snap.PositionSeconds > _lastCiderPosition + 0.4;
        _lastCiderPosition = snap.PositionSeconds;
        return snap with { Status = moved ? PlaybackStatus.Playing : PlaybackStatus.Paused };
    }
    internal MediaSnapshot? ParseV3NowPlaying(string json) => ParseV3NowPlaying(json, out _);

    internal MediaSnapshot? ParseV3NowPlaying(string json, out bool statusExplicit)
    {
        statusExplicit = false;
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Two documented shapes: { data: { info: {...} } } or { info: {...} }
            JsonElement info = default;
            if (root.TryGetProperty("data", out var data))
            {
                if (data.ValueKind == JsonValueKind.Object && data.TryGetProperty("info", out var inner)) info = inner;
                else info = data;
            }
            else if (root.TryGetProperty("info", out var direct)) info = direct;

            if (info.ValueKind != JsonValueKind.Object || !info.TryGetProperty("name", out _)) return null;

            var name = Str(info, "name");
            var artist = Str(info, "artistName");
            var album = Str(info, "albumName");
            var artUrl = ArtworkUrl(info, 320);
            var durationMs = Num(info, "durationInMillis", 0);
            var positionSec = Num(info, "currentPlaybackTime", 0);

            // 显式状态优先：isPlaying / status 字段
            var isPlaying = false;
            if (info.TryGetProperty("isPlaying", out var ipEl))
            {
                if (ipEl.ValueKind == JsonValueKind.True) isPlaying = true;
                else if (ipEl.ValueKind == JsonValueKind.Number) isPlaying = ipEl.GetDouble() != 0;
                statusExplicit = true;
            }
            var statusStr = Str(info, "status");
            if (statusStr.Length > 0)
            {
                isPlaying = statusStr.Equals("playing", StringComparison.OrdinalIgnoreCase);
                statusExplicit = true;
            }
            // 注意：不能用 remainingTime 推断播放状态——暂停中的歌 remainingTime 也 > 0，
            // 否则暂停会被误判为播放、歌词继续往前走。无显式状态时由位置移动判定。
            var status = isPlaying ? PlaybackStatus.Playing : PlaybackStatus.Paused;
            var hasLyrics = Bool(info, "hasLyrics") || Bool(info, "hasTimeSyncedLyrics");

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(artist)) return null;

            var track = new TrackInfo(name, artist, album, string.Empty, "Cider", "Cider",
                string.Empty, artUrl, TimeSpan.FromMilliseconds(Math.Max(0, durationMs)));
            return new MediaSnapshot
            {
                Track = track,
                Source = MediaSourceKind.Cider,
                Status = status,
                PositionSeconds = Math.Max(0, positionSec),
                DurationSeconds = Math.Max(0, durationMs / 1000.0),
                CanPlayPause = true,
                CanNext = true,
                CanPrevious = true,
                CanSeek = durationMs > 0,
                HasVolumeControl = true,
                HasLyrics = hasLyrics,
            };
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Cider V3 parse failed: {ex.Message}");
            return null;
        }
    }

    internal MediaSnapshot? ParseLegacyNowPlaying(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("info", out var info)) return null;
            if (!info.TryGetProperty("name", out _)) return null;

            var name = Str(info, "name");
            var artist = Str(info, "artistName");
            var album = Str(info, "albumName");
            var artUrl = ArtworkUrl(info, 320);
            var durationMs = Num(info, "durationInMillis", 0);
            var positionSec = Num(info, "currentPlaybackTime", 0);
            var isPlaying = Bool(info, "isPlaying") || Str(info, "status") == "playing"
                            || Num(info, "remainingTime", -1) > 0.5; // 无显式状态时按剩余时间推断

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(artist)) return null;

            var track = new TrackInfo(name, artist, album, string.Empty, "Cider", "Cider",
                string.Empty, artUrl, TimeSpan.FromMilliseconds(Math.Max(0, durationMs)));
            return new MediaSnapshot
            {
                Track = track,
                Source = MediaSourceKind.Cider,
                Status = isPlaying ? PlaybackStatus.Playing : PlaybackStatus.Paused,
                PositionSeconds = Math.Max(0, positionSec),
                DurationSeconds = Math.Max(0, durationMs / 1000.0),
                CanPlayPause = true,
                CanNext = true,
                CanPrevious = true,
                CanSeek = durationMs > 0,
                HasVolumeControl = true,
                HasLyrics = false,
            };
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Cider legacy parse failed: {ex.Message}");
            return null;
        }
    }

    // ── Control ────────────────────────────────────────────────

    public async Task<bool> TogglePlayPauseAsync(CancellationToken ct = default) => await ControlAsync("/playpause", ct);
    public async Task<bool> PlayAsync(CancellationToken ct = default) => await ControlAsync("/play", ct);
    public async Task<bool> PauseAsync(CancellationToken ct = default) => await ControlAsync("/pause", ct);
    public async Task<bool> NextAsync(CancellationToken ct = default) => await ControlAsync("/next", ct);
    public async Task<bool> PreviousAsync(CancellationToken ct = default) => await ControlAsync("/previous", ct);

    private async Task<bool> ControlAsync(string action, CancellationToken ct)
    {
        if (Profile == CiderApiProfile.None) return false;
        try
        {
            var path = Profile == CiderApiProfile.V3 ? $"/api/v1/playback{action}" : action;
            using var req = Build(HttpMethod.Post, path);
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Cider {action} failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SeekAsync(double positionSeconds, CancellationToken ct = default)
    {
        if (Profile == CiderApiProfile.None) return false;
        try
        {
            if (Profile == CiderApiProfile.V3)
            {
                using var req = Build(HttpMethod.Post, "/api/v1/playback/seek", $"{{\"position\":{positionSeconds.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)}}}");
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                return resp.IsSuccessStatusCode;
            }
            else
            {
                var secs = positionSeconds.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
                using var req = Build(HttpMethod.Get, $"/seekto/{secs}");
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                return resp.IsSuccessStatusCode;
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Cider seek failed: {ex.Message}");
            return false;
        }
    }

    public async Task<double?> GetVolumeAsync(CancellationToken ct = default)
    {
        if (Profile == CiderApiProfile.None) return null;
        try
        {
            var path = Profile == CiderApiProfile.V3 ? "/api/v1/playback/volume" : "/audio";
            using var req = Build(HttpMethod.Get, path);
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) return null;
            var text = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            return ParseVolume(text);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Cider get volume failed: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> SetVolumeAsync(double volume01, CancellationToken ct = default)
    {
        if (Profile == CiderApiProfile.None) return false;
        try
        {
            var v = Math.Clamp(volume01, 0.0, 1.0);
            var s = v.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
            if (Profile == CiderApiProfile.V3)
            {
                using var req = Build(HttpMethod.Post, "/api/v1/playback/volume", $"{{\"volume\":{s}}}");
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                return resp.IsSuccessStatusCode;
            }
            else
            {
                using var req = Build(HttpMethod.Get, $"/audio/{s}");
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                return resp.IsSuccessStatusCode;
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Cider set volume failed: {ex.Message}");
            return false;
        }
    }

    internal static double? ParseVolume(string text)
    {
        text = text.Trim();
        if (double.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var v))
            return Math.Clamp(v, 0, 1);

        try
        {
            using var doc = JsonDocument.Parse(text);
            var root = doc.RootElement;
            if (root.TryGetProperty("volume", out var vol))
            {
                if (vol.ValueKind == JsonValueKind.Number) return Math.Clamp(vol.GetDouble(), 0, 1);
            }
            if (root.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Object &&
                data.TryGetProperty("volume", out var vol2) && vol2.ValueKind == JsonValueKind.Number)
                return Math.Clamp(vol2.GetDouble(), 0, 1);
        }
        catch { /* not json */ }

        return null;
    }

    // ── Lyrics ─────────────────────────────────────────────────

    /// <summary>Fetch lyrics for the currently playing song. Returns raw LRC text when available.</summary>
    public async Task<string?> GetLyricsAsync(string? songId = null, CancellationToken ct = default)
    {
        if (Profile == CiderApiProfile.None) return null;
        var attempts = new List<string> { "/api/v1/lyrics/current", "/api/v1/lyrics" };
        if (!string.IsNullOrEmpty(songId)) attempts.Add($"/api/v1/lyrics/current?id={Uri.EscapeDataString(songId)}");

        foreach (var path in attempts)
        {
            try
            {
                using var req = Build(HttpMethod.Get, path);
                using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
                if (resp.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.NoContent or HttpStatusCode.BadRequest) continue;
                if (!resp.IsSuccessStatusCode) continue;
                var text = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                var lrc = TryExtractLrc(text);
                if (!string.IsNullOrEmpty(lrc)) return lrc;
            }
            catch (Exception ex)
            {
                AppLogger.Warn($"Cider lyrics failed: {ex.Message}");
            }
        }

        return null;
    }

    /// <summary>Best-effort extraction of LRC text from Cider's lyrics responses.</summary>
    internal static string? TryExtractLrc(string text)
    {
        text = text.Trim();
        if (text.Length == 0) return null;

        // Plain LRC text already?
        if (text.Contains("[00:") || text.Contains("[0:") || text.StartsWith('['))
            return text;

        try
        {
            using var doc = JsonDocument.Parse(text);
            var root = doc.RootElement;

            // Candidates in order: data.lyrics / data.lrc / lyrics / lrc / data array of {time,text}
            JsonElement? candidate = null;
            if (root.TryGetProperty("data", out var data))
            {
                if (data.ValueKind == JsonValueKind.Object)
                {
                    if (data.TryGetProperty("lyrics", out var l)) candidate = l;
                    else if (data.TryGetProperty("lrc", out var l2)) candidate = l2;
                    else if (data.ValueKind == JsonValueKind.Object) candidate = data;
                }
                else if (data.ValueKind == JsonValueKind.Array) candidate = data;
            }
            if (candidate is null && root.TryGetProperty("lyrics", out var rl)) candidate = rl;
            if (candidate is null && root.TryGetProperty("lrc", out var rl2)) candidate = rl2;

            if (candidate is null) return null;

            var el = candidate.Value;
            if (el.ValueKind == JsonValueKind.String) return el.GetString();

            if (el.ValueKind == JsonValueKind.Array)
            {
                var sb = new StringBuilder();
                foreach (var item in el.EnumerateArray())
                {
                    if (item.ValueKind != JsonValueKind.Object) continue;
                    var time = 0.0;
                    var text2 = string.Empty;
                    if (item.TryGetProperty("time", out var t))
                        time = t.ValueKind == JsonValueKind.Number ? t.GetDouble() : double.TryParse(t.GetString(), out var tv) ? tv : 0;
                    if (item.TryGetProperty("text", out var tx)) text2 = tx.GetString() ?? string.Empty;
                    if (item.TryGetProperty("lrc", out var lx)) text2 = lx.GetString() ?? text2;
                    if (item.TryGetProperty("value", out var vx)) text2 = vx.GetString() ?? text2;
                    if (text2.Length > 0)
                    {
                        var ts = TimeSpan.FromSeconds(time);
                        sb.Append('[').Append(ts.ToString(@"mm\:ss\.ff")).Append(']').AppendLine(text2);
                    }
                }

                return sb.Length > 0 ? sb.ToString() : null;
            }

            if (el.ValueKind == JsonValueKind.Object)
            {
                // Some builds return { data: { "0": {time,text}, ... } } or { data: { lines: [...] } }
                if (el.TryGetProperty("lines", out var lines) && lines.ValueKind == JsonValueKind.Array)
                    return TryExtractLrc(lines.GetRawText());
                var sb = new StringBuilder();
                foreach (var prop in el.EnumerateObject())
                {
                    if (prop.Value.ValueKind == JsonValueKind.Object && prop.Value.TryGetProperty("time", out _))
                    {
                        var time = Num(prop.Value, "time", 0);
                        var text2 = Str(prop.Value, "text");
                        if (text2.Length == 0) text2 = Str(prop.Value, "value");
                        if (text2.Length > 0)
                            sb.Append('[').Append(TimeSpan.FromSeconds(time).ToString(@"mm\:ss\.ff")).Append(']').AppendLine(text2);
                    }
                }

                return sb.Length > 0 ? sb.ToString() : null;
            }
        }
        catch
        {
            // Not JSON - treat as plain text and let the caller decide.
        }

        return null;
    }

    // ── JSON helpers ───────────────────────────────────────────

    private static string Str(JsonElement el, string name) =>
        el.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? string.Empty : string.Empty;

    private static double Num(JsonElement el, string name, double fallback)
    {
        if (!el.TryGetProperty(name, out var v)) return fallback;
        if (v.ValueKind == JsonValueKind.Number) return v.GetDouble();
        return double.TryParse(v.GetString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var d) ? d : fallback;
    }

    private static bool Bool(JsonElement el, string name)
    {
        if (!el.TryGetProperty(name, out var v)) return false;
        return v.ValueKind == JsonValueKind.True || (v.ValueKind == JsonValueKind.String && v.GetString() == "true");
    }

    private static string ArtworkUrl(JsonElement info, int size)
    {
        if (!info.TryGetProperty("artwork", out var art) || art.ValueKind != JsonValueKind.Object) return string.Empty;
        var url = Str(art, "url");
        if (url.Length == 0) return string.Empty;
        var s = size.ToString();
        return url.Replace("{w}", s).Replace("{h}", s);
    }
}





