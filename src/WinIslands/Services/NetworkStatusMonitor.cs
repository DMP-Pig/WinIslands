using System;
using System.Net.NetworkInformation;

namespace WinIslands.Services;

/// <summary>
/// 监听系统网络连接状态变化（断开 / 恢复各提示一次），带去抖：
/// NetworkAvailabilityChanged 在网络抖动时可能连续触发多次，统一延迟 1.5s
/// 后用「最终状态 + 接口实际状态」双重确认，避免误报、漏报。
/// </summary>
public sealed class NetworkStatusMonitor : IDisposable
{
    private readonly object _gate = new();
    private bool _available;
    private bool _started;
    private bool? _pending;
    private System.Threading.Timer? _debounce;

    /// <summary>网络断开。</summary>
    public event EventHandler? NetworkLost;
    /// <summary>网络恢复。</summary>
    public event EventHandler? NetworkRestored;

    /// <summary>是否当前有可用网络。</summary>
    public bool IsAvailable { get { lock (_gate) return _available; } }

    public void Start()
    {
        lock (_gate)
        {
            if (_started) return;
            _started = true;
            _available = IsNetworkAvailable();
        }
        NetworkChange.NetworkAvailabilityChanged += OnAvailabilityChanged;
        AppLogger.Info($"Network status monitor started (available={_available}).");
    }

    private static bool IsNetworkAvailable()
    {
        try { return NetworkInterface.GetIsNetworkAvailable(); }
        catch { return false; }
    }

    private void OnAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
    {
        bool changed;
        lock (_gate)
        {
            if (!_started || e.IsAvailable == _available) return;
            _pending = e.IsAvailable;
            changed = true;
        }
        if (!changed) return;
        // 去抖：1.5s 后确认最终状态，避免网络抖动 / 多网卡切换连发
        _debounce?.Dispose();
        _debounce = new System.Threading.Timer(Confirm, null, TimeSpan.FromMilliseconds(1500), Timeout.InfiniteTimeSpan);
    }

    private void Confirm(object? state)
    {
        bool? pending;
        lock (_gate)
        {
            pending = _pending;
            _pending = null;
            if (pending is null || !_started) return;
        }
        var now = pending.Value;
        var actual = IsNetworkAvailable();
        if (actual != now) return; // 与最终实际状态不符（抖动），丢弃
        lock (_gate)
        {
            if (!_started || actual == _available) return;
            _available = actual;
        }
        AppLogger.Info($"Network state changed → available={actual}.");
        if (actual) NetworkRestored?.Invoke(this, EventArgs.Empty);
        else NetworkLost?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        lock (_gate)
        {
            if (!_started) return;
            _started = false;
        }
        NetworkChange.NetworkAvailabilityChanged -= OnAvailabilityChanged;
        _debounce?.Dispose();
        _debounce = null;
    }
}
