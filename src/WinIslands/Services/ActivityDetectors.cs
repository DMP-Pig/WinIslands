using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WinIslands.Services;

/// <summary>
/// 1.0.10 新增：文件复制/移动 与 下载进行中 的轻量本机检测器。
/// 纯启发式、纯本地：不联网、不上报任何数据。检测不到或被误判时不影响主流程。
/// </summary>

/// <summary>
/// 文件复制/移动检测：通过前台窗口标题识别（如资源管理器「正在复制 N 个项目…」）。
/// Windows 11 的复制/移动对话框标题通常含「正在复制/正在移动/Copying/Moving」等字样。
/// </summary>
public static class FileTransferMonitor
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    /// <summary>标题关键词（小写，包含匹配；中英各一组，避免误伤普通窗口）。</summary>
    private static readonly string[] CopyKeywords =
    {
        "正在复制", "正在移动", "正在将", "正在下载到", "复制到", "移动 ",
        "copying", "moving ", "copy to", "transferring",
    };

    /// <summary>当前前台窗口是否看起来正在执行文件复制/移动。</summary>
    public static bool IsCopyingOrMoving()
    {
        try
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero) return false;
            var len = GetWindowTextLength(hwnd);
            if (len <= 0) return false;
            var sb = new StringBuilder(len + 1);
            GetWindowText(hwnd, sb, sb.Capacity);
            var title = sb.ToString();
            if (string.IsNullOrWhiteSpace(title)) return false;
            foreach (var key in CopyKeywords)
                if (title.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }
        catch { return false; }
    }
}

/// <summary>
/// 下载进行中检测：扫描用户「下载」目录中的浏览器临时下载文件
/// （.crdownload / .part / .download / .partial / .opdownload），
/// 只统计最近 30 分钟内有写入的，避免把残留文件误判为进行中。
/// </summary>
public static class DownloadDetector
{
    private static readonly string[] TempExtensions =
    {
        ".crdownload", ".part", ".download", ".partial", ".opdownload",
    };

    /// <summary>当前正在下载的文件个数（无法判断时返回 0）。</summary>
    public static int ActiveDownloadCount()
    {
        try
        {
            var downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            if (!Directory.Exists(downloads)) return 0;
            var now = DateTime.Now;
            var count = 0;
            foreach (var file in Directory.EnumerateFiles(downloads))
            {
                var ext = Path.GetExtension(file);
                if (string.IsNullOrEmpty(ext)) continue;
                var active = false;
                foreach (var t in TempExtensions)
                {
                    if (string.Equals(ext, t, StringComparison.OrdinalIgnoreCase)) { active = true; break; }
                }
                if (!active) continue;
                try
                {
                    var lastWrite = File.GetLastWriteTime(file);
                    if ((now - lastWrite).TotalMinutes <= 30) count++;
                }
                catch { /* 文件被占用等，跳过 */ }
            }
            return count;
        }
        catch { return 0; }
    }
}
