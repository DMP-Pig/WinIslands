using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WinIslands.Services;

/// <summary>
/// 夜间模式（系统深色主题）开关：读写 HKCU 主题个性化注册表并广播 WM_SETTINGCHANGE，
/// 与系统「深色模式」开关等效。纯本地，无联网。
/// </summary>
public static class SystemTheme
{
    private const string PersonalizeKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const uint WmSettingChange = 0x001A;
    private const uint SmtoAbortIfHung = 0x0002;
    private static readonly IntPtr HWndBroadcast = new(0xFFFF);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

    /// <summary>当前是否夜间模式（系统深色主题）。</summary>
    public static bool IsNightMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(PersonalizeKey);
            return key?.GetValue("AppsUseLightTheme") is int i ? i == 0 : false;
        }
        catch { return false; }
    }

    /// <summary>设置夜间模式（true=深色, false=浅色），并广播通知系统与应用立即刷新。</summary>
    public static void SetNightMode(bool dark)
    {
        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(PersonalizeKey);
            key?.SetValue("AppsUseLightTheme", dark ? 0 : 1, RegistryValueKind.DWord);
            key?.SetValue("SystemUsesLightTheme", dark ? 0 : 1, RegistryValueKind.DWord);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SetNightMode registry failed: {ex.Message}");
            return;
        }
        try
        {
            _ = SendMessageTimeout(HWndBroadcast, WmSettingChange, IntPtr.Zero, "ImmersiveColorSet", SmtoAbortIfHung, 1000, out _);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SetNightMode broadcast failed: {ex.Message}");
        }
    }
}
