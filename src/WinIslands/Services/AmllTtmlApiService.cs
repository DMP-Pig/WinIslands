using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace WinIslands.Services;

/// <summary>AMLL TTML 歌词源返回结果：LRC 用于行级索引，Ttml 用于逐字卡拉OK。</summary>
public sealed record AmllLyricsResult(LrcDocument Lrc, TtmlDocument? Ttml)
{
    public static AmllLyricsResult Empty { get; } = new(new LrcDocument(), null);
}

/// <summary>
/// AMLL TTML DataBase 歌词 API 客户端（https://api.amll.dev，非官方但公开）。
/// 提供 Apple Music 风格的 TTML 逐字时间轴，用于真正的逐字卡拉OK。
/// 接口路径以 amll-ttml-api 仓库的路由为准：
///   GET /v1/lyrics/search?musicName=&amp;artistName=   原生搜索（返回歌曲条目 id）
///   GET /v1/lyrics/get?id=…                          按 id 取原始 TTML XML（lyrics 字段）
///   GET /v1/lrclib/search?track_name=&amp;artist_name=  LrcLib 兼容搜索（syncedLyrics 为 LRC，兜底）
/// 注意：该接口属于第三方非官方服务，版本可能变动；本模块独立封装，
/// 所有请求 5 秒超时 + 异常捕获，失败时优雅降级（返回 Empty），绝不影响主流程。
/// </summary>
public sealed class AmllTtmlApiService
{
    private const string BaseUrl = "https://api.amll.dev";
    private readonly HttpClient _http;

    public AmllTtmlApiService()
    {
        var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All };
        _http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(5) };
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("WinIslands/1.1.5 (Dynamic Island for Windows)");
        _http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    }

    /// <summary>
    /// 获取歌曲的 AMLL 歌词。优先取原始 TTML（逐字），取不到时退回 LrcLib 的 syncedLyrics（仅逐句）。
    /// </summary>
    public async Task<AmllLyricsResult> FetchAsync(string title, string artist, string album, CancellationToken ct = default)
    {
        try
        {
            // 1) 原生搜索：拿到条目 id
            var item = await SearchFirstAsync(title, artist, album, ct);
            if (item is not null && item.Id.Length > 0)
            {
                var ttmlText = await GetTtmlAsync(item.Id, ct);
                if (!string.IsNullOrWhiteSpace(ttmlText))
                {
                    var ttml = TtmlParser.Parse(ttmlText);
                    // 从 TTML 行重建 LRC 行（同一来源，保证行级索引与逐字时间轴一一对应）
                    var lrc = BuildLrcFromTtml(ttml);
                    if (!lrc.IsEmpty) return new AmllLyricsResult(lrc, ttml);
                }
            }

            // 2) 兜底：LrcLib 兼容搜索直接返回 syncedLyrics（LRC 格式）
            var lrclib = await LrclibSearchAsync(title, artist, ct);
            if (lrclib is not null && !string.IsNullOrWhiteSpace(lrclib.SyncedLyrics))
            {
                var doc = LrcParser.Parse(lrclib.SyncedLyrics);
                if (!doc.IsEmpty) return new AmllLyricsResult(doc, null);
            }

            return AmllLyricsResult.Empty;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"AMLL lyrics failed: {ex.Message}");
            return AmllLyricsResult.Empty;
        }
    }

    private async Task<LyricsSearchItem?> SearchFirstAsync(string title, string artist, string album, CancellationToken ct)
    {
        try
        {
            var query = new List<string>();
            if (!string.IsNullOrEmpty(title)) query.Add($"musicName={Uri.EscapeDataString(title)}");
            if (!string.IsNullOrEmpty(artist)) query.Add($"artistName={Uri.EscapeDataString(artist)}");
            if (!string.IsNullOrEmpty(album)) query.Add($"albumName={Uri.EscapeDataString(album)}");
            if (query.Count == 0) return null;

            var url = $"{BaseUrl}/v1/lyrics/search?{string.Join("&", query)}";
            var json = await _http.GetStringAsync(url, ct);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("data", out var data) ||
                !data.TryGetProperty("items", out var items) ||
                items.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var it in items.EnumerateArray())
            {
                var item = LyricsSearchItem.FromJson(it);
                if (item is not null && item.Id.Length > 0) return item;
            }

            return null;
        }
        catch { return null; }
    }

    private async Task<string?> GetTtmlAsync(string id, CancellationToken ct)
    {
        try
        {
            var url = $"{BaseUrl}/v1/lyrics/get?id={Uri.EscapeDataString(id)}";
            var json = await _http.GetStringAsync(url, ct);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("data", out var data) ||
                !data.TryGetProperty("lyrics", out var lyrics) ||
                lyrics.ValueKind != JsonValueKind.String)
                return null;

            var text = lyrics.GetString();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        catch { return null; }
    }

    private async Task<LrclibItem?> LrclibSearchAsync(string title, string artist, CancellationToken ct)
    {
        try
        {
            var query = new List<string>();
            if (!string.IsNullOrEmpty(title)) query.Add($"track_name={Uri.EscapeDataString(title)}");
            if (!string.IsNullOrEmpty(artist)) query.Add($"artist_name={Uri.EscapeDataString(artist)}");
            if (query.Count == 0) return null;

            var url = $"{BaseUrl}/v1/lrclib/search?{string.Join("&", query)}";
            var json = await _http.GetStringAsync(url, ct);
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return null;

            foreach (var it in doc.RootElement.EnumerateArray())
            {
                var item = LrclibItem.FromJson(it);
                if (item is not null) return item;
            }

            return null;
        }
        catch { return null; }
    }

    /// <summary>把 TTML 行重构为 LRC 文档（时间格式 mm:ss.xx，行文本取各字拼接）。</summary>
    private static LrcDocument BuildLrcFromTtml(TtmlDocument ttml)
    {
        try
        {
            if (ttml.IsEmpty) return new LrcDocument();
            var lines = new List<LyricLine>(ttml.Lines.Count);
            foreach (var line in ttml.Lines)
            {
                var text = string.Concat(line.Words.Select(w => w.Text)).Trim();
                if (text.Length == 0) continue;
                lines.Add(new LyricLine(TimeSpan.FromSeconds(line.BeginSec), text));
            }

            lines.Sort((a, b) => a.Time.CompareTo(b.Time));
            return new LrcDocument { Lines = lines };
        }
        catch
        {
            return new LrcDocument();
        }
    }

    // ── 响应模型 ──────────────────────────────────────────────
    private sealed record LyricsSearchItem(string Id, string[] ArtistNames)
    {
        public static LyricsSearchItem? FromJson(JsonElement it)
        {
            try
            {
                var id = it.TryGetProperty("id", out var idEl) && idEl.ValueKind == JsonValueKind.Number
                    ? idEl.GetInt64().ToString(CultureInfo.InvariantCulture)
                    : string.Empty;

                var artists = Array.Empty<string>();
                if (it.TryGetProperty("artistNames", out var a) && a.ValueKind == JsonValueKind.Array)
                    artists = a.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.String)
                        .Select(x => x.GetString() ?? string.Empty).ToArray();

                return id.Length > 0 ? new LyricsSearchItem(id, artists) : null;
            }
            catch { return null; }
        }
    }

    private sealed record LrclibItem(string? SyncedLyrics)
    {
        public static LrclibItem? FromJson(JsonElement it)
        {
            try
            {
                var synced = it.TryGetProperty("syncedLyrics", out var s) && s.ValueKind == JsonValueKind.String
                    ? s.GetString()
                    : null;
                return new LrclibItem(string.IsNullOrWhiteSpace(synced) ? null : synced);
            }
            catch { return null; }
        }
    }
}
