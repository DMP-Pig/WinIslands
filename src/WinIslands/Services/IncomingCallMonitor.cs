using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WinIslands.Services;

/// <summary>通话类型。</summary>
public enum CallKind { None = 0, Incoming = 1, Active = 2 }

/// <summary>
/// 来电提醒：轮询微信 / QQ 等 IM 应用的顶层窗口，按标题识别语音 / 视频通话窗口。
/// 仅本机前台窗口检测，不联网、不上报数据。检测到新通话窗口弹提醒一次，
/// 窗口消失后可再次检测（同一窗口只提醒一次）。
/// </summary>
public sealed class IncomingCallMonitor : IDisposable
{
    private readonly object _gate = new();
    private System.Threading.Timer? _timer;
    private bool _started;
    private readonly HashSet<IntPtr> _activeCalls = new();
    private string[] _apps = Array.Empty<string>();

    /// <summary>检测到通话窗口（参数：进程名, 窗口标题, 类型）。</summary>
    public event Action<string, string, CallKind>? CallStarted;
    /// <summary>通话窗口消失（参数：进程名）。</summary>
    public event Action<string>? CallEnded;

    /// <summary>启动轮询（间隔 1.5s，去抖窗口出现时序）。</summary>
    public void Start(System.Collections.Generic.IEnumerable<string> apps)
    {
        lock (_gate)
        {
            _started = true;
            _apps = NormalizeApps(apps);
            _activeCalls.Clear();
        }
        _timer?.Dispose();
        _timer = new System.Threading.Timer(_ => Scan(), null, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(1500));
        AppLogger.Info($"Incoming call monitor started (apps: {string.Join(",", _apps)}).");
    }

    public void Stop()
    {
        lock (_gate)
        {
            _started = false;
            _activeCalls.Clear();
        }
        _timer?.Dispose();
        _timer = null;
    }

    /// <summary>把 "Weixin.exe, QQ" 规范化成不含 .exe 的小写进程名数组。</summary>
    internal static string[] NormalizeApps(IEnumerable<string>? apps)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (apps is not null)
        {
            foreach (var a in apps)
            {
                var t = (a ?? string.Empty).Trim();
                if (t.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) t = t[..^4];
                t = t.Trim().ToLowerInvariant();
                if (t.Length > 0) set.Add(t);
            }
        }
        return set.Count == 0 ? new[] { "weixin", "wechat", "qq" } : set.ToArray();
    }

    /// <summary>按窗口标题识别通话类型（纯逻辑，可测试）。</summary>
    internal static CallKind ClassifyTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title)) return CallKind.None;
        var t = title.Trim();
        var low = t.ToLowerInvariant();
        // 来电/邀请语义（优先判定为"来电"）
        if (low.Contains("inviting") || low.Contains("incoming") || low.Contains("calling you")
            || t.Contains("邀请你") || t.Contains("正在邀请") || t.Contains("邀请你进行"))
            return CallKind.Incoming;
        // 通话中
        if (t.Contains("语音通话") || t.Contains("视频通话") || t.Contains("正在通话") || t.Contains("通话中")
            || t.Contains("语音") || t.Contains("视频")
            || low.Contains("voice call") || low.Contains("video call") || low.Contains("in call") || low.Contains("calling"))
            return CallKind.Active;
        return CallKind.None;
    }

    private void Scan()
    {
        if (Monitor.TryEnter(_gate))
        {
            try
            {
                if (!_started) return;
                string[] apps;
                lock (_gate) apps = _apps;
                if (apps.Length == 0) return;

                // 先收集被监控进程的 PID 集合，避免对每个窗口都做进程名查询
                var pids = new HashSet<uint>();
                foreach (var app in apps)
                {
                    try
                    {
                        foreach (var p in Process.GetProcessesByName(app))
                        {
                            try { pids.Add((uint)p.Id); } catch { /* 进程已退出 */ }
                        }
                    }
                    catch { /* 无权限等 */ }
                }
                if (pids.Count == 0) return;

                var current = new HashSet<IntPtr>();
                var newCalls = new List<(string App, string Title, CallKind Kind)>();
                EnumWindows((h, _) =>
                {
                    if (!IsWindowVisible(h)) return true;
                    GetWindowThreadProcessId(h, out var pid);
                    if (!pids.Contains(pid)) return true;
                    var title = GetWindowTitle(h);
                    var kind = ClassifyTitle(title);
                    if (kind == CallKind.None) return true;
                    current.Add(h);
                    bool isNew;
                    lock (_gate) isNew = _activeCalls.Add(h);
                    if (isNew) newCalls.Add((FindAppName(pid), title, kind));
                    return true;
                }, IntPtr.Zero);

                // 已结束的通话窗口移出活跃集合，允许下一次通话再次提醒
                List<IntPtr> ended = new();
                lock (_gate)
                {
                    foreach (var h in _activeCalls) if (!current.Contains(h)) ended.Add(h);
                    foreach (var h in ended) _activeCalls.Remove(h);
                }

                // 事件在锁外触发，避免订阅方回调用到本服务时死锁
                foreach (var (app, title, kind) in newCalls)
                {
                    AppLogger.Info($"Call window detected: '{title}' ({app}, kind={kind})");
                    try { CallStarted?.Invoke(app, title, kind); } catch { /* 订阅方异常不影响轮询 */ }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Debug($"Incoming call scan failed: {ex.Message}");
            }
            finally
            {
                Monitor.Exit(_gate);
            }
        }
    }

    private string FindAppName(uint pid)
    {
        try { using var p = Process.GetProcessById((int)pid); return p.ProcessName; }
        catch { return "IM"; }
    }

    // ── Win32 ──
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    [DllImport("user32.dll")] private static extern bool EnumWindows(EnumWindowsProc cb, IntPtr lParam);
    [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint pid);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
    [DllImport("user32.dll")] private static extern int GetWindowTextLength(IntPtr hWnd);

    private static string GetWindowTitle(IntPtr hWnd)
    {
        try
        {
            var len = GetWindowTextLength(hWnd);
            if (len <= 0) return string.Empty;
            var sb = new StringBuilder(len + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }
        catch { return string.Empty; }
    }

    public void Dispose() => Stop();
}
