using System;
using Microsoft.Win32;

namespace WinIslands.Services;

/// <summary>
/// 隐私设备（麦克风/摄像头）占用检测。
/// Windows 在 CapabilityAccessManager\ConsentStore 注册表下记录每个应用/进程最近一次
/// 使用麦克风/摄像头的起止时间（FILETIME，100ns 单位）：LastUsedTimeStart / LastUsedTimeStop。
/// 若 Start &gt; Stop 表示该进程当前仍在占用设备。轮询此键开销极小、无需任何系统 API 权限，
/// 且不涉及联网（数据仅本机判断，不上报），符合隐私要求。
/// </summary>
public static class PrivacyDeviceMonitor
{
    private const string ConsentBase =
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore";

    private static readonly string[] Scopes = { "NonPackaged", "Packaged" };

    /// <summary>返回 (麦克风是否被占用, 摄像头是否被占用)。</summary>
    public static (bool Mic, bool Cam) GetUsage()
    {
        bool mic = false, cam = false;
        foreach (var scope in Scopes)
        {
            if (!mic && IsDeviceInUse("microphone", scope)) mic = true;
            if (!cam && IsDeviceInUse("camera", scope)) cam = true;
            if (mic && cam) break;
        }
        return (mic, cam);
    }

    private static bool IsDeviceInUse(string device, string scope)
    {
        try
        {
            using var root = Registry.LocalMachine.OpenSubKey($@"{ConsentBase}\{device}\{scope}");
            if (root is null) return false;
            foreach (var sub in root.GetSubKeyNames())
            {
                using var k = root.OpenSubKey(sub);
                if (k is null) continue;
                var start = ReadQword(k, "LastUsedTimeStart");
                var stop = ReadQword(k, "LastUsedTimeStop");
                // 正在使用：开始时间晚于停止时间（或从未写入停止时间）
                if (start > 0 && (stop == 0 || start > stop)) return true;
            }
        }
        catch { /* 无权限 / 键不存在：视为未占用 */ }
        return false;
    }

    private static ulong ReadQword(RegistryKey k, string name)
    {
        try
        {
            return k.GetValue(name) switch
            {
                long l => (ulong)l,
                int i => (ulong)i,
                byte[] b when b.Length >= 8 => BitConverter.ToUInt64(b, 0),
                _ => 0UL
            };
        }
        catch { return 0UL; }
    }
}
