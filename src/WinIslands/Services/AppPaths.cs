using System.IO;

namespace WinIslands.Services;

/// <summary>Well-known file/directory locations used by the app.</summary>
public static class AppPaths
{
    /// <summary>
    /// %APPDATA%\WinIslands - config, logs, cache and lyrics live here.
    /// Tests can redirect via the WINISLANDS_APPDATA environment variable.
    /// </summary>
    public static string AppDataDir
    {
        get
        {
            var overrideDir = Environment.GetEnvironmentVariable("WINISLANDS_APPDATA");
            return string.IsNullOrWhiteSpace(overrideDir)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinIslands")
                : overrideDir;
        }
    }

    // Note: computed (not cached) so tests/runtime can redirect via WINISLANDS_APPDATA.
    public static string SettingsFile => Path.Combine(AppDataDir, "settings.json");

    public static string LogsDir => Path.Combine(AppDataDir, "logs");

    public static string ThumbCacheDir => Path.Combine(AppDataDir, "thumbcache");

    public static string LyricsDir => Path.Combine(AppDataDir, "Lyrics");

    /// <summary>崩溃自动恢复标记文件：异常退出时写入，下次启动检测到后提示「已恢复」。</summary>
    public static string CrashMarkerFile => Path.Combine(AppDataDir, "crash-recovery.json");

    public static string ExePath { get; } = Environment.ProcessPath ?? string.Empty;

    public static void EnsureDirectories()
    {
        Directory.CreateDirectory(AppDataDir);
        Directory.CreateDirectory(LogsDir);
        Directory.CreateDirectory(ThumbCacheDir);
        Directory.CreateDirectory(LyricsDir);
    }
}


