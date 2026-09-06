using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace WinIslands.Services;

/// <summary>
/// 键盘指示灯：轮询 CapsLock / NumLock / ScrollLock 状态（500ms），
/// 状态变化时触发事件，上层可在灵动岛短暂显示指示灯。
/// </summary>
public sealed class KeyboardIndicatorMonitor : IDisposable
{
    private readonly DispatcherTimer _timer;
    private bool _caps, _num, _scroll;

    public KeyboardIndicatorMonitor()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
        _timer.Tick += (_, _) => Poll();
    }

    /// <summary>
    /// 是否轮询键盘指示灯状态。仅在 CapsLock 组件启用时才轮询（默认关闭），
    /// 避免后台始终每 400ms 唤醒 Dispatcher。
    /// </summary>
    public void SetPolling(bool polling)
    {
        if (polling && !_timer.IsEnabled)
        {
            Poll();
            _timer.Start();
        }
        else if (!polling && _timer.IsEnabled)
        {
            _timer.Stop();
        }
    }

    /// <summary>状态变化（参数：CapsLock / NumLock / ScrollLock）。</summary>
    public event Action<string>? StateChanged;

    public (bool Caps, bool Num, bool Scroll) Current => (_caps, _num, _scroll);

    private void Poll()
    {
        var caps = (GetKeyState(0x14) & 1) != 0;
        var num = (GetKeyState(0x90) & 1) != 0;
        var scroll = (GetKeyState(0x91) & 1) != 0;
        if (caps != _caps) { _caps = caps; StateChanged?.Invoke("CapsLock"); }
        if (num != _num) { _num = num; StateChanged?.Invoke("NumLock"); }
        if (scroll != _scroll) { _scroll = scroll; StateChanged?.Invoke("ScrollLock"); }
    }

    public void Dispose() => _timer.Stop();

    [DllImport("user32.dll")]
    private static extern short GetKeyState(int nVirtKey);
}
