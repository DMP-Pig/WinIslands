using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WinIslands.Services;

/// <summary>
/// Last-resort fallback: finds music-player windows by process name and parses
/// "Artist - Title" out of the window title. No remote control is possible here,
/// so control buttons are disabled.
/// </summary>
public sealed class WindowTitleMediaProvider
{
    private static readonly string[] KnownPlayers =
    {
        "spotify", "cloudmusic", "qqmusic", "music.ui", "wmplayer", "groove music",
        "microsoft.media.player", "zunemusic", "apple music", "foobar2000", "aimp",
        "musicbee", "winamp", "kugou", "kuwo", "migu", "netease", "yoyodownloader",
        "listen1", "yesplaymusic",
    };

    public MediaSnapshot? GetSnapshot()
    {
        try
        {
            foreach (var proc in Process.GetProcesses())
            {
                string procName;
                try { procName = proc.ProcessName; }
                catch { continue; }

                if (!IsKnownPlayer(procName)) continue;
                if (proc.MainWindowHandle == IntPtr.Zero) continue;

                string title;
                try { title = proc.MainWindowTitle; }
                catch { continue; }

                if (string.IsNullOrWhiteSpace(title) || title.Length > 120) continue;

                var (artist, track) = ParseTitle(title);
                if (track.Length == 0) continue;

                var trackInfo = new TrackInfo(track, artist, string.Empty, string.Empty,
                    FriendlyName(procName), procName, string.Empty, string.Empty, TimeSpan.Zero);
                return new MediaSnapshot
                {
                    Track = trackInfo,
                    Source = MediaSourceKind.WindowTitle,
                    Status = PlaybackStatus.Playing,
                    CanPlayPause = false,
                    CanNext = false,
                    CanPrevious = false,
                    CanSeek = false,
                    HasVolumeControl = false,
                    HasLyrics = false,
                };
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"WindowTitle scan failed: {ex.Message}");
        }

        return null;
    }

    private static bool IsKnownPlayer(string processName)
    {
        var p = processName.ToLowerInvariant();
        foreach (var known in KnownPlayers)
        {
            if (p.Contains(known, StringComparison.OrdinalIgnoreCase)) return true;
        }

        return false;
    }

    private static string FriendlyName(string processName)
    {
        var p = processName.ToLowerInvariant();
        if (p.Contains("spotify")) return "Spotify";
        if (p.Contains("cloudmusic") || p.Contains("netease")) return "网易云音乐";
        if (p.Contains("qqmusic")) return "QQ音乐";
        if (p.Contains("kugou")) return "酷狗音乐";
        if (p.Contains("kuwo")) return "酷我音乐";
        if (p.Contains("wmplayer")) return "Windows Media Player";
        if (p.Contains("groove") || p.Contains("zune")) return "Groove 音乐";
        if (p.Contains("media.player") || p.Contains("music.ui")) return "电影和电视";
        if (p.Contains("apple music") || p.Contains("cider")) return "Apple Music";
        if (p.Contains("foobar2000")) return "foobar2000";
        return p;
    }

    /// <summary>Best-effort "Artist - Title" parsing from a window title.</summary>
    internal static (string Artist, string Title) ParseTitle(string title)
    {
        var t = title.Trim();
        // Strip player-name prefixes like "Spotify - " or "网易云音乐 - "
        foreach (var known in KnownPlayers)
        {
            var marker = known + " - ";
            if (t.StartsWith(marker, StringComparison.OrdinalIgnoreCase))
            {
                t = t[marker.Length..].Trim();
                break;
            }
        }

        // Common pattern: "Artist - Title" (long dash or hyphen with spaces)
        foreach (var sep in new[] { " - ", " – ", " — " })
        {
            var idx = t.IndexOf(sep, StringComparison.Ordinal);
            if (idx > 0)
            {
                var artist = t[..idx].Trim();
                var trackTitle = t[(idx + sep.Length)..].Trim();
                if (trackTitle.Length > 0 && artist.Length > 0 && trackTitle.Length <= 80)
                    return (artist, trackTitle);
            }
        }

        return (string.Empty, t);
    }
}



