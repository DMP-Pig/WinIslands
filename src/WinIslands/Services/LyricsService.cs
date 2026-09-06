using System.IO;
using System.Text;

namespace WinIslands.Services;

public enum LyricsSourceKind { None, LocalFile, Cider, Online, AmllTtml }

/// <summary>
/// 歌词解析结果：<see cref="Document"/> 是行级 LRC 时间轴（用于定位/滚动），
/// <see cref="Ttml"/> 是可选的原生 TTML 逐字时间轴（用于逐字卡拉OK，可能为 null）。
/// </summary>
public sealed record LyricsResult(LrcDocument Document, LyricsSourceKind Source, string SourceDetail, TtmlDocument? Ttml = null)
{
    public bool IsEmpty => Document.IsEmpty;
    public static LyricsResult Empty { get; } = new(new LrcDocument(), LyricsSourceKind.None, string.Empty);
}

/// <summary>
/// Resolves lyrics for the currently playing track.
/// Priority: local .lrc file > AMLL TTML API (逐字) > player/client lyrics (Cider API) > online API (opt-in).
/// Missing lyrics degrade gracefully to an empty result — callers just hide the panel.
/// </summary>
public sealed class LyricsService
{
    private readonly SettingsService _settings;
    private readonly CiderMediaProvider? _cider;
    private readonly OnlineLyricsService _online = new();
    private readonly AmllTtmlApiService _amll = new();
    private readonly Dictionary<string, LyricsResult> _cache = new();
    private readonly object _cacheLock = new();

    public LyricsService(SettingsService settings, CiderMediaProvider? cider)
    {
        _settings = settings;
        _cider = cider;
    }

    /// <summary>Get lyrics for a track, using cached results when the same song repeats.</summary>
    public async Task<LyricsResult> GetLyricsAsync(MediaSnapshot snapshot, CancellationToken ct = default)
    {
        var key = TrackKey(snapshot.Track);
        lock (_cacheLock)
        {
            if (_cache.TryGetValue(key, out var cached)) return cached;
        }

        var result = await LoadAsync(snapshot, ct);
        lock (_cacheLock)
        {
            if (_cache.Count > 8) _cache.Clear(); // tiny LRU-ish cache
            _cache[key] = result;
        }

        return result;
    }

    private async Task<LyricsResult> LoadAsync(MediaSnapshot snapshot, CancellationToken ct)
    {
        var track = snapshot.Track;

        // 多歌词源一键切换（1.2.0）：用户可指定首选来源；首选源未命中时按 Auto 优先级降级，
        // 保证「选了 Cider 但切歌到网易云」这类场景依然有歌词可用。
        var preferred = _settings.Current.LyricsPreferredSource;
        if (!string.IsNullOrEmpty(preferred) && !string.Equals(preferred, "Auto", StringComparison.OrdinalIgnoreCase))
        {
            var picked = await LoadPreferredAsync(snapshot, preferred, ct);
            if (!picked.IsEmpty) return picked;
        }

        return await LoadAutoAsync(snapshot, ct);
    }

    /// <summary>只从用户指定的单一来源取词；该来源未命中返回 Empty（随后自动回退 Auto 优先级）。</summary>
    private async Task<LyricsResult> LoadPreferredAsync(MediaSnapshot snapshot, string preferred, CancellationToken ct)
    {
        var track = snapshot.Track;
        switch (preferred.ToUpperInvariant())
        {
            case "LOCAL":
            {
                var localPath = FindLocalLrc(track);
                if (localPath is null) return LyricsResult.Empty;
                try
                {
                    var text = await File.ReadAllTextAsync(localPath, ct);
                    var doc = LrcParser.Parse(text);
                    return doc.IsEmpty ? LyricsResult.Empty : new LyricsResult(doc, LyricsSourceKind.LocalFile, localPath);
                }
                catch (Exception ex)
                {
                    AppLogger.Warn($"Failed to read local LRC {localPath}: {ex.Message}");
                    return LyricsResult.Empty;
                }
            }
            case "AMLL":
            {
                if (!_settings.Current.AmllTtmlEnabled) return LyricsResult.Empty;
                var amll = await _amll.FetchAsync(track.Title, track.Artist, track.Album, ct);
                return amll.Lrc.IsEmpty ? LyricsResult.Empty : new LyricsResult(amll.Lrc, LyricsSourceKind.AmllTtml, "AMLL TTML", amll.Ttml);
            }
            case "CIDER":
            {
                if (snapshot.Source != MediaSourceKind.Cider || _cider is null) return LyricsResult.Empty;
                var lrc = await _cider.GetLyricsAsync();
                if (string.IsNullOrWhiteSpace(lrc)) return LyricsResult.Empty;
                var doc = LrcParser.Parse(lrc);
                return doc.IsEmpty ? LyricsResult.Empty : new LyricsResult(doc, LyricsSourceKind.Cider, "Cider API");
            }
            case "ONLINE":
            {
                if (!_settings.Current.OnlineLyricsEnabled) return LyricsResult.Empty;
                var lrc = await _online.FetchLrcAsync(track.Title, track.Artist, ct);
                if (string.IsNullOrWhiteSpace(lrc)) return LyricsResult.Empty;
                var doc = LrcParser.Parse(lrc);
                return doc.IsEmpty ? LyricsResult.Empty : new LyricsResult(doc, LyricsSourceKind.Online, "Netease");
            }
            default:
                return LyricsResult.Empty;
        }
    }

    /// <summary>自动优先级：本地 .lrc → AMLL TTML → Cider API → 在线（仅开启时）。</summary>
    private async Task<LyricsResult> LoadAutoAsync(MediaSnapshot snapshot, CancellationToken ct)
    {
        var track = snapshot.Track;

        // 1) Local .lrc files.
        var localPath = FindLocalLrc(track);
        if (localPath is not null)
        {
            try
            {
                var text = await File.ReadAllTextAsync(localPath, ct);
                var doc = LrcParser.Parse(text);
                if (!doc.IsEmpty) return new LyricsResult(doc, LyricsSourceKind.LocalFile, localPath);
            }
            catch (Exception ex)
            {
                AppLogger.Warn($"Failed to read local LRC {localPath}: {ex.Message}");
            }
        }

        // 2) AMLL TTML API —— 逐字卡拉OK歌词库（独立开关，默认开启；失败自动降级到下一来源）。
        if (_settings.Current.AmllTtmlEnabled)
        {
            var amll = await _amll.FetchAsync(track.Title, track.Artist, track.Album, ct);
            if (!amll.Lrc.IsEmpty)
            {
                return new LyricsResult(amll.Lrc, LyricsSourceKind.AmllTtml, "AMLL TTML", amll.Ttml);
            }
        }

        // 3) Cider API lyrics (only meaningful when the active source is Cider).
        if (snapshot.Source == MediaSourceKind.Cider && _cider is not null)
        {
            var lrc = await _cider.GetLyricsAsync();
            if (!string.IsNullOrWhiteSpace(lrc))
            {
                var doc = LrcParser.Parse(lrc);
                if (!doc.IsEmpty) return new LyricsResult(doc, LyricsSourceKind.Cider, "Cider API");
            }
        }

        // 4) Online API — strictly opt-in.
        if (_settings.Current.OnlineLyricsEnabled)
        {
            var lrc = await _online.FetchLrcAsync(track.Title, track.Artist, ct);
            if (!string.IsNullOrWhiteSpace(lrc))
            {
                var doc = LrcParser.Parse(lrc);
                if (!doc.IsEmpty) return new LyricsResult(doc, LyricsSourceKind.Online, "Netease");
            }
        }

        return LyricsResult.Empty;
    }

    /// <summary>
    /// Find a matching .lrc file in configured + conventional folders.
    /// File names are matched after normalisation (lowercase, spaces/punctuation removed),
    /// so "Demo Artist - Demo Song.lrc" matches artist "Demo Artist" + title "Demo Song".
    /// Supported layouts: "Title.lrc", "Artist - Title.lrc", "Artist-Title.lrc", "Artist-Title-Album.lrc".
    /// </summary>
    public string? FindLocalLrc(TrackInfo track)
    {
        try
        {
            var dirs = new List<string>();
            var configured = _settings.Current.LyricsFolder;
            if (!string.IsNullOrWhiteSpace(configured)) dirs.Add(configured.Trim());
            dirs.Add(AppPaths.LyricsDir);
            dirs.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Lyrics"));
            dirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));

            var title = Sanitize(track.Title);
            var artist = Sanitize(track.Artist);
            var album = Sanitize(track.Album);
            var keys = new HashSet<string>(StringComparer.Ordinal);
            if (title.Length > 0) keys.Add(title);
            if (artist.Length > 0 && title.Length > 0)
            {
                keys.Add(artist + title);                       // "Demo Artist - Demo Song" -> demoartistdemosong
                if (album.Length > 0) keys.Add(artist + title + album);
            }

            foreach (var dir in dirs)
            {
                if (!Directory.Exists(dir)) continue;
                try
                {
                    foreach (var file in Directory.EnumerateFiles(dir, "*.lrc", SearchOption.TopDirectoryOnly))
                    {
                        // 文件名同样去掉空格/连字符/大小写后与 key 比对
                        var name = Sanitize(Path.GetFileNameWithoutExtension(file));
                        if (name.Length == 0) continue;
                        foreach (var key in keys)
                        {
                            if (name == key || name.EndsWith(key, StringComparison.Ordinal)) return file;
                        }
                    }
                }
                catch { /* unreadable folder */ }
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"FindLocalLrc failed: {ex.Message}");
        }

        return null;
    }

    /// <summary>清空歌词缓存（例如在线歌词开关变化后强制重新获取）。</summary>
    /// <summary>设置首选歌词来源（Auto | Local | Amll | Cider | Online）并清空缓存，下次加载立即生效。</summary>
    public void SetPreferredSource(string source)
    {
        _settings.Update(s => s.LyricsPreferredSource = source);
        ClearCache();
    }
    public void ClearCache()
    {
        lock (_cacheLock) _cache.Clear();
    }

    internal static string TrackKey(TrackInfo track) =>
        $"{Sanitize(track.Artist)}\u0001{Sanitize(track.Title)}\u0001{Sanitize(track.Album)}";

    internal static string Sanitize(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        var sb = new StringBuilder(s.Length);
        foreach (var ch in s.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(ch)) sb.Append(ch);
        }

        return sb.ToString();
    }
}

