using System.Diagnostics;
using System.IO;
using System.Text;
using WinIslands.Services;

namespace WinIslands.Diagnostics;

/// <summary>
/// Collects environment diagnostics for the "Diagnostics" button / --diagnose CLI flag.
/// Helps verify SMTC sessions, Cider connectivity, settings and DPI without a GUI.
/// </summary>
public static class DiagnosticsCommand
{
    public static async Task<string> RunAsync(SettingsService settings, CiderMediaProvider? cider, CancellationToken ct = default)
    {
        var sb = new StringBuilder();
        sb.AppendLine("== WinIslands diagnostics ==");
        sb.AppendLine($"OS: {Environment.OSVersion} ({(Environment.Is64BitOperatingSystem ? "x64" : "x86")})");
        sb.AppendLine($"Process: {Environment.ProcessPath}");
        sb.AppendLine($".NET: {Environment.Version}");
        sb.AppendLine($"Settings file: {AppPaths.SettingsFile} exists={File.Exists(AppPaths.SettingsFile)}");
        sb.AppendLine($"Settings: language={settings.Current.Language} theme={settings.Current.Theme} monitor={settings.Current.Monitor} ciderEnabled={settings.Current.CiderEnabled}");

        // SMTC
        sb.AppendLine();
        sb.AppendLine("-- System media sessions (SMTC) --");
        try
        {
            var manager = await Windows.Media.Control.GlobalSystemMediaTransportControlsSessionManager.RequestAsync().AsTask(ct);
            var sessions = manager?.GetSessions()?.ToList() ?? new List<Windows.Media.Control.GlobalSystemMediaTransportControlsSession>();
            sb.AppendLine($"Manager acquired. Sessions: {sessions.Count}");
            foreach (var s in sessions)
            {
                try
                {
                    var info = s.GetPlaybackInfo();
                    sb.AppendLine($"  [{s.SourceAppUserModelId}] status={info.PlaybackStatus}");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"  [{s.SourceAppUserModelId}] (playback info error: {ex.Message})");
                }
            }

            if (sessions.Count == 0)
                sb.AppendLine("  (none — start playing something and re-run)");
        }
        catch (Exception ex)
        {
            sb.AppendLine($"  SMTC unavailable: {ex.Message}");
        }

        // Cider
        sb.AppendLine();
        sb.AppendLine("-- Cider local API --");
        if (cider is not null)
        {
            await cider.EnsureConnectedAsync();
            if (cider.Client.IsConnected)
            {
                sb.AppendLine($"Connected: profile={cider.Client.Profile} port={cider.Client.Port}");
                var snap = await cider.GetSnapshotAsync();
                if (snap is not null)
                    sb.AppendLine($"Now playing: {snap.Track.Title} — {snap.Track.Artist} ({snap.Status})");
                else
                    sb.AppendLine("Connected but no track loaded.");
            }
            else
            {
                sb.AppendLine($"Not connected: {cider.Client.LastError ?? "unknown"}");
                sb.AppendLine("Hint: enable 'Allow external control' in Cider settings (Settings > Connectivity).");
            }
        }
        else
        {
            sb.AppendLine("Cider provider disabled.");
        }

        // Player processes
        sb.AppendLine();
        sb.AppendLine("-- Media player processes --");
        var players = Process.GetProcesses()
            .Where(p => SafeName(p).Contains("spotify", StringComparison.OrdinalIgnoreCase)
                        || SafeName(p).Contains("cloudmusic", StringComparison.OrdinalIgnoreCase)
                        || SafeName(p).Contains("qqmusic", StringComparison.OrdinalIgnoreCase)
                        || SafeName(p).Contains("cider", StringComparison.OrdinalIgnoreCase)
                        || SafeName(p).Contains("wmplayer", StringComparison.OrdinalIgnoreCase))
            .Select(p => $"{SafeName(p)} (pid {SafePid(p)})")
            .Distinct();
        sb.AppendLine(players.Any() ? string.Join(", ", players) : "  (none detected)");

        // DPI
        sb.AppendLine();
        sb.AppendLine($"Primary screen DPI scale: {UI.ScreenHelper.GetDpiScale(System.Windows.Forms.Screen.PrimaryScreen!):0.##}");
        sb.AppendLine($"Screens: {System.Windows.Forms.Screen.AllScreens.Length}");

        return sb.ToString();
    }

    private static string SafeName(Process p)
    {
        try { return p.ProcessName; } catch { return "?"; }
    }

    private static int SafePid(Process p)
    {
        try { return p.Id; } catch { return -1; }
    }
}

