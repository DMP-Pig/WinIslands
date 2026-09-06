using System.IO;

using System.Text;

namespace WinIslands.Services;

/// <summary>
/// Minimal, dependency-free file logger. Writes one line per entry to
/// %APPDATA%\WinIslands\logs\app-yyyyMMdd.log. Never throws.
/// </summary>
public static class AppLogger
{
    private static readonly object Gate = new();
    private static string _currentDay = string.Empty;
    private static StreamWriter? _writer;
    private static int _pendingLines;              // 未落盘行数（批量 flush，减少 UI 线程同步磁盘写）
    private static DateTime _lastFlushUtc = DateTime.UtcNow;
    private const int FlushThreshold = 20;         // 每 20 行强制落盘
    private static readonly TimeSpan FlushInterval = TimeSpan.FromSeconds(2); // 或每 2 秒落盘

    public static void Info(string message) => Write("INFO", message);
    public static void Debug(string message) => Write("DEBUG", message);
    public static void Warn(string message) => Write("WARN", message);
    public static void Error(string message, Exception? ex = null)
        => Write("ERROR", ex is null ? message : $"{message}{Environment.NewLine}{ex}");

    private static void Write(string level, string message)
    {
        try
        {
            lock (Gate)
            {
                var now = DateTime.Now;
                var day = now.ToString("yyyyMMdd");
                if (_writer is null || day != _currentDay)
                {
                    _writer?.Dispose();
                    _currentDay = day;
                    var path = Path.Combine(AppPaths.LogsDir, $"app-{day}.log");
                    _writer = new StreamWriter(path, append: true, Encoding.UTF8);
                    _writer.AutoFlush = false; // 批量落盘：避免每次写日志同步刷磁盘卡顿（动画更流畅）
                }

                _writer.WriteLine($"[{now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}");
                _pendingLines++;
                if (_pendingLines >= FlushThreshold || DateTime.UtcNow - _lastFlushUtc >= FlushInterval)
                {
                    _writer.Flush();
                    _pendingLines = 0;
                    _lastFlushUtc = DateTime.UtcNow;
                }
            }
        }
        catch
        {
            // Logging must never crash the app.
        }
    }
}


