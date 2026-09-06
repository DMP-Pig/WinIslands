using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Radios;

namespace WinIslands.Services;

/// <summary>
/// 快捷开关组件：WiFi / 蓝牙 / 夜间模式 / 静音 的状态读取与一键切换。
/// 全部走本地 API（Windows.Devices.Radios / NetworkInterface / 注册表 / CoreAudio），无任何联网。
/// Radio 状态缓存 2 秒，避免每帧调用 WinRT 造成开销。
/// </summary>
public static class QuickSwitchService
{
    private static readonly SemaphoreSlim RadioGate = new(1, 1);
    private static RadioState _wifiState = RadioState.Unknown;
    private static RadioState _btState = RadioState.Unknown;
    private static bool _wifiRadioPresent;
    private static bool _btRadioPresent;
    private static volatile bool _switching;
    private static DateTime _lastRadioRefresh = DateTime.MinValue;

    /// <summary>WiFi 是否开启（无 WiFi Radio 时用无线网卡连接状态兜底）。</summary>
    public static bool IsWifiOn
        => _wifiState == RadioState.On || (!_wifiRadioPresent && WifiNicUp());

    /// <summary>系统是否存在可用的 WiFi 网卡（用于开关按钮可用性）。</summary>
    public static bool HasWifi
        => _wifiRadioPresent || HasWifiNic();

    /// <summary>蓝牙是否开启。</summary>
    public static bool IsBluetoothOn => _btState == RadioState.On;

    /// <summary>系统是否存在蓝牙 Radio。</summary>
    public static bool HasBluetooth => _btRadioPresent;

    /// <summary>夜间模式（系统深色主题）是否开启。</summary>
    public static bool IsNightMode => SystemTheme.IsNightMode();

    /// <summary>系统主音量是否静音。</summary>
    public static bool IsMuted => SystemVolume.IsMuted();

    /// <summary>
    /// 刷新 Radio 状态（2 秒缓存）。线程安全；失败保留旧值，避免状态忽有忽无。
    /// </summary>
    public static async Task RefreshRadiosAsync()
    {
        if ((DateTime.UtcNow - _lastRadioRefresh).TotalSeconds < 2) return;
        await RadioGate.WaitAsync();
        try
        {
            if ((DateTime.UtcNow - _lastRadioRefresh).TotalSeconds < 2) return;
            var radios = await Radio.GetRadiosAsync();
            _wifiRadioPresent = false;
            _btRadioPresent = false;
            _wifiState = RadioState.Unknown;
            _btState = RadioState.Unknown;
            foreach (var r in radios)
            {
                if (r.Kind == RadioKind.WiFi) { _wifiRadioPresent = true; _wifiState = r.State; }
                else if (r.Kind == RadioKind.Bluetooth) { _btRadioPresent = true; _btState = r.State; }
            }
            _lastRadioRefresh = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"QuickSwitch.RefreshRadios failed: {ex.Message}");
        }
        finally
        {
            RadioGate.Release();
        }
    }

    /// <summary>
    /// 切换 WiFi / 蓝牙 Radio 开关。返回是否成功；失败时调用方应给用户可见的兜底动作。
    /// </summary>
    public static async Task<bool> SetRadioAsync(bool bluetooth, bool on)
    {
        if (_switching) return true; // 防止连点重复请求
        _switching = true;
        try
        {
            await RefreshRadiosAsync();
            var radios = await Radio.GetRadiosAsync();
            foreach (var r in radios)
            {
                if (r.Kind != (bluetooth ? RadioKind.Bluetooth : RadioKind.WiFi)) continue;
                if (r.State == RadioState.Disabled) return false; // 硬件禁用（如笔记本飞行模式硬开关）
                var target = on ? RadioState.On : RadioState.Off;
                if (r.State == target) return true;
                var access = await r.SetStateAsync(target);
                _lastRadioRefresh = DateTime.MinValue; // 强制下次立即刷新
                return access == RadioAccessStatus.Allowed;
            }
            return false;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"QuickSwitch.SetRadio(bt={bluetooth},on={on}) failed: {ex.Message}");
            return false;
        }
        finally
        {
            _switching = false;
        }
    }

    /// <summary>切换夜间模式（深色主题开/关）。</summary>
    public static void ToggleNightMode()
    {
        try { SystemTheme.SetNightMode(!SystemTheme.IsNightMode()); }
        catch (Exception ex) { AppLogger.Warn($"QuickSwitch.ToggleNight failed: {ex.Message}"); }
    }

    /// <summary>切换系统主音量静音。</summary>
    public static void ToggleMute()
    {
        try { SystemVolume.SetMute(!SystemVolume.IsMuted()); }
        catch (Exception ex) { AppLogger.Warn($"QuickSwitch.ToggleMute failed: {ex.Message}"); }
    }

    private static bool WifiNicUp()
    {
        try
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up)
                    return true;
        }
        catch { /* 读取网卡失败按未知处理 */ }
        return false;
    }

    private static bool HasWifiNic()
    {
        try
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    return true;
        }
        catch { /* 读取网卡失败按未知处理 */ }
        return false;
    }
}
