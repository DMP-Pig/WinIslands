using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace WinIslands.Services;

/// <summary>
/// 全屏自动隐藏：定时轮询前台窗口，若其矩形覆盖整个显示器工作区（全屏视频/游戏/演示/远程投屏等），
/// 判定为「全屏中」。灵动岛据此自动隐藏，退出全屏后恢复。纯本机检测，不联网。
/// </summary>
public sealed class FullScreenMonitor : IDisposable
{
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromMilliseconds(600) };

    /// <summary>当前是否检测到全屏窗口。</summary>
    public bool IsFullScreen { get; private set; }

    /// <summary>是否正在轮询。</summary>
    public bool IsRunning { get; private set; }

    /// <summary>进入/退出全屏（参数：是否全屏中）。</summary>
    public event Action<bool>? FullScreenChanged;

    public FullScreenMonitor()
    {
        _timer.Tick += (_, _) => Poll();
    }

    public void Start()
    {
        IsRunning = true;
        Poll();         // 立即采样一次，避免启动时状态未知
        _timer.Start();
    }

    public void Stop()
    {
        IsRunning = false;
        _timer.Stop();
    }

    private void Poll()
    {
        try
        {
            var full = IsCurrentFullScreen();
            if (full != IsFullScreen)
            {
                IsFullScreen = full;
                FullScreenChanged?.Invoke(full);
            }
        }
        catch
        {
            // 检测异常不影响主流程
        }
    }

    /// <summary>判定前台窗口是否为全屏：窗口矩形覆盖其所在显示器工作区（容差 4px）。</summary>
    private static bool IsCurrentFullScreen()
    {
        var hwnd = Native.GetForegroundWindow();
        if (hwnd == IntPtr.Zero) return false;

        // 排除自己进程的窗口（灵动岛/设置/托盘等，避免自锁）
        Native.GetWindowThreadProcessId(hwnd, out var pid);
        if (pid == Environment.ProcessId) return false;
        if (!Native.IsWindowVisible(hwnd)) return false;

        Native.GetWindowRect(hwnd, out var rect);
        if (rect.Right - rect.Left < 200 || rect.Bottom - rect.Top < 200) return false;

        // 找该窗口所在显示器的工作区（不含任务栏）
        var monitor = Native.MonitorFromWindow(hwnd, Native.MonitorDefaultToNearest);
        if (monitor == IntPtr.Zero) return false;
        var info = new Native.MonitorInfo { cbSize = (uint)Marshal.SizeOf<Native.MonitorInfo>() };
        if (!Native.GetMonitorInfo(monitor, ref info)) return false;
        var wa = info.rcWork;

        const int tol = 4; // 像素容差（边框/圆角）
        return rect.Left <= wa.Left + tol && rect.Top <= wa.Top + tol
            && rect.Right >= wa.Right - tol && rect.Bottom >= wa.Bottom - tol;
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }

    private static class Native
    {
        public const uint MonitorDefaultToNearest = 2;

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        public struct MonitorInfo
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }
    }
}
