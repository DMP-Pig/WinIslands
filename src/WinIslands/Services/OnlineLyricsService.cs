using System.Globalization;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Text.Json;

namespace WinIslands.Services;

/// <summary>
/// Optional online-lyrics provider (Netease Cloud Music unofficial API).
/// OFF by default — the user must enable it in settings; see README for the
/// copyright caveat. All calls are short-timeout and never block the UI.
/// </summary>
public sealed class OnlineLyricsService
{
    private readonly HttpClient _http;

    public OnlineLyricsService()
    {
        var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.All };
        _http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(5) };
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) WinIslands/0.1");
        _http.DefaultRequestHeaders.Referrer = new Uri("https://music.163.com/");
    }

    public async Task<string?> FetchLrcAsync(string title, string artist, CancellationToken ct = default)
    {
        try
        {
            // 依次尝试：完整「歌名 歌手」→ 仅歌名 → 去掉括号/修饰后的歌名
            var candidates = new List<string>();
            if (!string.IsNullOrEmpty(artist)) candidates.Add($"{title} {artist}".Trim());
            candidates.Add(title.Trim());
            var clean = System.Text.RegularExpressions.Regex.Replace(title, @"\s*[（(【\[].*?[）)】\]]\s*", "").Trim();
            if (clean.Length > 0 && clean != title.Trim()) candidates.Add(clean);

            // 网易云
            foreach (var q in candidates)
            {
                var lrc = await FetchNeteaseAsync(q, ct);
                if (!string.IsNullOrWhiteSpace(lrc)) return lrc;
            }

            // QQ音乐（兜底，覆盖更多歌曲）
            foreach (var q in candidates)
            {
                var lrc = await FetchQqAsync(q, ct);
                if (!string.IsNullOrWhiteSpace(lrc)) return lrc;
            }

            return null;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Online lyrics failed: {ex.Message}");
            return null;
        }
    }

    private async Task<string?> FetchNeteaseAsync(string query, CancellationToken ct)
    {
        try
        {
            var songId = await SearchNeteaseSongIdAsync(query, ct);
            if (songId is null) return null;

            var url = $"https://music.163.com/api/song/lyric?id={songId}&lv=1&kv=1&tv=-1";
            var json = await _http.GetStringAsync(url, ct);
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("lrc", out var lrc) ||
                !lrc.TryGetProperty("lyric", out var lyric) ||
                lyric.ValueKind != JsonValueKind.String)
                return null;

            var text = lyric.GetString();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        catch { return null; }
    }

    private async Task<string?> FetchQqAsync(string query, CancellationToken ct)
    {
        try
        {
            // 搜索
            var search = $"https://c.y.qq.com/soso/fcgi-bin/client_search_cp?w={Uri.EscapeDataString(query)}&format=json&n=5";
            var json = await _http.GetStringAsync(search, ct);
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("data", out var data) ||
                !data.TryGetProperty("song", out var song) ||
                !song.TryGetProperty("list", out var list) ||
                list.ValueKind != JsonValueKind.Array || !list.EnumerateArray().Any())
                return null;

            var songmid = list.EnumerateArray().First().GetProperty("songmid").GetString();
            if (string.IsNullOrEmpty(songmid)) return null;

            // 歌词
            var lyricUrl = $"https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?songmid={songmid}&format=json&nobase64=1";
            var lyricJson = await _http.GetStringAsync(lyricUrl, ct);
            using var ldoc = JsonDocument.Parse(lyricJson);
            if (!ldoc.RootElement.TryGetProperty("lyric", out var lr) || lr.ValueKind != JsonValueKind.String)
                return null;

            var text = lr.GetString();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        catch { return null; }
    }

    private async Task<string?> SearchNeteaseSongIdAsync(string query, CancellationToken ct)
    {
        var url = $"https://music.163.com/api/search/get/web?s={Uri.EscapeDataString(query)}&type=1&limit=5";
        var json = await _http.GetStringAsync(url, ct);
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("result", out var result) ||
            !result.TryGetProperty("songs", out var songs) ||
            songs.ValueKind != JsonValueKind.Array)
            return null;

        foreach (var song in songs.EnumerateArray())
        {
            if (song.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.Number)
                return id.GetInt64().ToString(CultureInfo.InvariantCulture);
        }

        return null;
    }
}


