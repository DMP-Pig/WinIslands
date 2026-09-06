using System.Runtime.InteropServices;

namespace WinIslands.Services;

/// <summary>
/// Controls the *system* master volume through the Windows CoreAudio COM API
/// (IMMDeviceEnumerator / IAudioEndpointVolume). Used for non-Cider sources when
/// the user opts into system-volume control.
/// </summary>
public static class SystemVolume
{
    private static readonly Guid IID_IMMDeviceEnumerator = new("A95664D2-9614-4F35-A746-DE8DB63617E6");
    private static readonly Guid IID_IAudioEndpointVolume = new("5CDF2C82-841E-4546-9722-0CF74078229A");
    private static readonly Guid CLSID_MMDeviceEnumerator = new("BCDE0395-E52F-467C-8E3D-C4579291692E");
    private static readonly Guid GUID_NULL = Guid.Empty;

    private static IAudioEndpointVolume? GetEndpointVolume()
    {
        var enumerator = Activator.CreateInstance(
            Type.GetTypeFromCLSID(CLSID_MMDeviceEnumerator, server: null) ?? throw new InvalidOperationException("MMDeviceEnumerator not available"))
            as IMMDeviceEnumerator ?? throw new InvalidOperationException("MMDeviceEnumerator activation failed");
        try
        {
            enumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole, out var device);
            if (device is null) return null;
            try
            {
                device!.Activate(IID_IAudioEndpointVolume, CLSCTX.CLSCTX_ALL, IntPtr.Zero, out var volume);
                return volume as IAudioEndpointVolume;
            }
            finally
            {
                Marshal.ReleaseComObject(device);
            }
        }
        finally
        {
            Marshal.ReleaseComObject(enumerator);
        }
    }

    public static double? GetVolume()
    {
        try
        {
            var vol = GetEndpointVolume();
            if (vol is null) return null;
            try
            {
                vol.GetMasterVolumeLevelScalar(out var level);
                return Math.Clamp(level, 0, 1);
            }
            finally
            {
                Marshal.ReleaseComObject(vol!);
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SystemVolume.Get failed: {ex.Message}");
            return null;
        }
    }

    public static void SetVolume(double value01)
    {
        try
        {
            var vol = GetEndpointVolume();
            if (vol is null) return;
            try
            {
                var context = Guid.Empty;
                vol.SetMasterVolumeLevelScalar((float)Math.Clamp(value01, 0, 1), ref context);
            }
            finally
            {
                Marshal.ReleaseComObject(vol!);
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SystemVolume.Set failed: {ex.Message}");
        }
    }

    public static bool IsMuted()
    {
        try
        {
            var vol = GetEndpointVolume();
            if (vol is null) return false;
            try
            {
                vol.GetMute(out var muted);
                return muted;
            }
            finally
            {
                Marshal.ReleaseComObject(vol!);
            }
        }
        catch { return false; }
    }

    /// <summary>设置系统主音量静音/取消静音。</summary>
    public static void SetMute(bool muted)
    {
        try
        {
            var vol = GetEndpointVolume();
            if (vol is null) return;
            try
            {
                var context = Guid.Empty;
                vol.SetMute(muted, ref context);
            }
            finally
            {
                Marshal.ReleaseComObject(vol!);
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SystemVolume.SetMute failed: {ex.Message}");
        }
    }


    // ── 音频输出设备（8 音频输出切换）─────────────────────────────

    /// <summary>一个可用的音频输出（渲染）设备。</summary>
    public sealed record AudioDeviceInfo(string Id, string Name, bool IsDefault)
    {
        public override string ToString() => Name;
    }

    /// <summary>枚举当前活动的音频输出设备，并标记系统默认设备。</summary>
    public static List<AudioDeviceInfo> GetDevices()
    {
        var result = new List<AudioDeviceInfo>();
        try
        {
            var enumerator =
                Activator.CreateInstance(
                    Type.GetTypeFromCLSID(CLSID_MMDeviceEnumerator, server: null)
                    ?? throw new InvalidOperationException("MMDeviceEnumerator not available"))
                as IMMDeviceEnumerator
                ?? throw new InvalidOperationException("MMDeviceEnumerator activation failed");
            try
            {
                // 1) 系统默认渲染端点（用于标记 IsDefault）
                string? defaultId = null;
                if (enumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole, out var defDev) == 0
                    && defDev is not null)
                {
                    try { defaultId = GetDeviceId(defDev); }
                    finally { Marshal.ReleaseComObject(defDev); }
                }

                // 2) 枚举全部活动的渲染端点
                var hr = enumerator.EnumAudioEndpoints(EDataFlow.eRender, 0x1 /* DEVICE_STATE_ACTIVE */, out var coll);
                if (hr != 0 || coll is null) return result;
                try
                {
                    coll.GetCount(out var count);
                    for (uint i = 0; i < count; i++)
                    {
                        if (coll.Item(i, out var dev) != 0 || dev is null) continue;
                        try
                        {
                            var id = GetDeviceId(dev);
                            if (string.IsNullOrEmpty(id)) continue;
                            var name = GetDeviceFriendlyName(dev);
                            result.Add(new AudioDeviceInfo(id,
                                string.IsNullOrWhiteSpace(name) ? id : name,
                                string.Equals(id, defaultId, StringComparison.OrdinalIgnoreCase)));
                        }
                        finally { Marshal.ReleaseComObject(dev); }
                    }
                }
                finally { Marshal.ReleaseComObject(coll); }
            }
            finally { Marshal.ReleaseComObject(enumerator); }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SystemVolume.GetDevices failed: {ex.Message}");
        }
        return result;
    }

    /// <summary>把指定设备设为系统默认输出设备（eConsole 角色）。</summary>
    public static bool SetDefaultDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId)) return false;
        try
        {
            // IPolicyConfig 是未公开接口，这里使用社区广泛使用的两个 CLSID：
            //   x64：CPolicyConfigVistaClient；x86：CPolicyConfigClient
            var clsid = new Guid(IntPtr.Size == 8
                ? "870AF99C-171D-4F9E-AF0D-E63DF40C2BC9"
                : "294935CE-F637-4E7C-A41B-AB255460B862");
            var type = Type.GetTypeFromCLSID(clsid, server: null);
            if (type is null) return false;
            var policy = Activator.CreateInstance(type) as IPolicyConfig;
            if (policy is null) return false;
            try
            {
                return policy.SetDefaultEndpoint(deviceId, 0 /* eConsole */) == 0;
            }
            finally { Marshal.ReleaseComObject(policy); }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SystemVolume.SetDefaultDevice failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>读取设备 ID（由调用方负责在 finally 中 ReleaseComObject 设备）。</summary>
    private static string GetDeviceId(IMMDevice dev)
    {
        var ptr = IntPtr.Zero;
        try
        {
            if (dev.GetId(out ptr) != 0 || ptr == IntPtr.Zero) return string.Empty;
            return Marshal.PtrToStringUni(ptr) ?? string.Empty;
        }
        finally
        {
            // IMMDevice::GetId 返回的 LPWSTR 由 CoTaskMemAlloc 分配，需释放
            if (ptr != IntPtr.Zero) Marshal.FreeCoTaskMem(ptr);
        }
    }

    /// <summary>读取设备友好名称（PKEY_Device_FriendlyName）。</summary>
    private static string GetDeviceFriendlyName(IMMDevice dev)
    {
        try
        {
            if (dev.OpenPropertyStore(0 /* STGM_READ */, out var storePtr) != 0 || storePtr == IntPtr.Zero)
                return string.Empty;
            try
            {
                var store = (IPropertyStore)Marshal.GetObjectForIUnknown(storePtr);
                try
                {
                    // PKEY_Device_FriendlyName
                    var key = new PROPERTYKEY(new Guid("A45C254E-DF1C-4EFD-8020-67D146A850E0"), 14);
                    if (store.GetValue(ref key, out var pv) != 0) return string.Empty;
                    try
                    {
                        // VT_LPWSTR = 31
                        return pv.vt == 31 && pv.pointerValue != IntPtr.Zero
                            ? Marshal.PtrToStringUni(pv.pointerValue) ?? string.Empty
                            : string.Empty;
                    }
                    finally { PropVariantClear(ref pv); }
                }
                finally { Marshal.ReleaseComObject(store); }
            }
            finally { Marshal.Release(storePtr); }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"SystemVolume friendly name failed: {ex.Message}");
            return string.Empty;
        }
    }

    // ── COM interfaces ─────────────────────────────────────────

    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    private class MMDeviceEnumeratorComObject { }

    [ComImport, Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IMMDeviceEnumerator
    {
        [PreserveSig] int EnumAudioEndpoints(EDataFlow dataFlow, uint dwStateMask, out IMMDeviceCollection? devices);
        [PreserveSig] int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice? device);
        [PreserveSig] int GetDevice(string id, out IMMDevice device);
        [PreserveSig] int RegisterEndpointNotificationCallback(IntPtr client);
        [PreserveSig] int UnregisterEndpointNotificationCallback(IntPtr client);
    }

    [ComImport, Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IMMDevice
    {
        [PreserveSig] int Activate(in Guid iid, CLSCTX clsCtx, IntPtr activationParams, [MarshalAs(UnmanagedType.IUnknown)] out object? interfacePtr);
        [PreserveSig] int OpenPropertyStore(uint stgmAccess, out IntPtr properties);
        [PreserveSig] int GetId(out IntPtr id);
        [PreserveSig] int GetState(out uint state);
    }

    [ComImport, Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IAudioEndpointVolume
    {
        [PreserveSig] int RegisterControlChangeNotify(IntPtr notify);
        [PreserveSig] int UnregisterControlChangeNotify(IntPtr notify);
        [PreserveSig] int GetChannelCount(out uint count);
        [PreserveSig] int SetMasterVolumeLevel(float level, ref Guid eventContext);
        [PreserveSig] int SetMasterVolumeLevelScalar(float level, ref Guid eventContext);
        [PreserveSig] int GetMasterVolumeLevel(out float level);
        [PreserveSig] int GetMasterVolumeLevelScalar(out float level);
        [PreserveSig] int SetChannelVolumeLevel(uint channel, float level, ref Guid eventContext);
        [PreserveSig] int SetChannelVolumeLevelScalar(uint channel, float level, ref Guid eventContext);
        [PreserveSig] int GetChannelVolumeLevel(uint channel, out float level);
        [PreserveSig] int GetChannelVolumeLevelScalar(uint channel, out float level);
        [PreserveSig] int SetMute(bool mute, ref Guid eventContext);
        [PreserveSig] int GetMute(out bool mute);
    }

    private enum EDataFlow { eRender = 0, eCapture = 1, eAll = 2 }

    // ── 音频输出设备：COM 接口（IMMDeviceCollection / IPropertyStore / IPolicyConfig）──

    [ComImport, Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IMMDeviceCollection
    {
        [PreserveSig] int GetCount(out uint count);
        [PreserveSig] int Item(uint index, out IMMDevice? device);
    }

    [ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IPropertyStore
    {
        [PreserveSig] int GetCount(out uint count);
        [PreserveSig] int GetAt(uint index, out PROPERTYKEY key);
        [PreserveSig] int GetValue(ref PROPERTYKEY key, out PROPVARIANT value);
        [PreserveSig] int SetValue(ref PROPERTYKEY key, ref PROPVARIANT value);
        [PreserveSig] int Commit();
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PROPERTYKEY
    {
        public Guid fmtid;
        public uint pid;
        public PROPERTYKEY(Guid fmtid, uint pid) { this.fmtid = fmtid; this.pid = pid; }
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct PROPVARIANT
    {
        [FieldOffset(0)] public ushort vt;
        [FieldOffset(8)] public IntPtr pointerValue; // x64 下联合体起始于偏移 8；本项目仅发布 x64
    }

    /// <summary>
    /// IPolicyConfig：用来切换「系统默认输出设备」的 COM 接口。属于未公开 API，
    /// 但被广泛使用；vtable 布局来自社区资料，在 Win10/Win11 上可用，失败时静默返回 false。
    /// </summary>
    [ComImport, Guid("F8679F50-850A-41CF-9C72-430F290290C8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IPolicyConfig
    {
        [PreserveSig] int GetMixFormat([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr fmt);
        [PreserveSig] int GetDeviceFormat([MarshalAs(UnmanagedType.LPWStr)] string device, int def, IntPtr fmt);
        [PreserveSig] int ResetDeviceFormat([MarshalAs(UnmanagedType.LPWStr)] string device);
        [PreserveSig] int SetDeviceFormat([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr ep, IntPtr mix);
        [PreserveSig] int GetProcessingPeriod([MarshalAs(UnmanagedType.LPWStr)] string device, int def, IntPtr d, IntPtr min, IntPtr max, IntPtr fmt);
        [PreserveSig] int SetProcessingPeriod([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr d, IntPtr min, IntPtr max, IntPtr fmt);
        [PreserveSig] int GetShareMode([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr fmt, IntPtr mode);
        [PreserveSig] int SetShareMode([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr fmt, int mode);
        [PreserveSig] int GetEndpointVolumeControl([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr fmt, int restrictive, IntPtr vol);
        [PreserveSig] int SetEndpointVolumeControl([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr fmt, int restrictive, IntPtr vol);
        [PreserveSig] int GetEndpointVolumeControlRange([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr fmt, IntPtr vol, IntPtr a, IntPtr b);
        [PreserveSig] int SetEndpointVolumeControlRange([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr fmt, IntPtr vol, uint a, uint b);
        [PreserveSig] int GetGlobalVolumeControl(IntPtr vol);
        [PreserveSig] int SetGlobalVolumeControl(IntPtr vol);
        [PreserveSig] int GetGlobalVolumeControlRange(IntPtr vol, IntPtr a, IntPtr b);
        [PreserveSig] int SetGlobalVolumeControlRange(IntPtr vol, uint a, uint b);
        [PreserveSig] int GetConfigPriority([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr prio);
        [PreserveSig] int SetConfigPriority([MarshalAs(UnmanagedType.LPWStr)] string device, int prio);
        [PreserveSig] int AddToStringArray([MarshalAs(UnmanagedType.LPWStr)] string device, [MarshalAs(UnmanagedType.LPWStr)] string s);
        [PreserveSig] int DeleteFromStringArray([MarshalAs(UnmanagedType.LPWStr)] string device, [MarshalAs(UnmanagedType.LPWStr)] string s);
        [PreserveSig] int ClearStringArray([MarshalAs(UnmanagedType.LPWStr)] string device);
        [PreserveSig] int GetStringArray([MarshalAs(UnmanagedType.LPWStr)] string device, IntPtr s);
        [PreserveSig] int SetStringArray([MarshalAs(UnmanagedType.LPWStr)] string device, [MarshalAs(UnmanagedType.LPWStr)] string s);
        [PreserveSig] int SetDefaultEndpoint([MarshalAs(UnmanagedType.LPWStr)] string device, int role);
        [PreserveSig] int SetEndpointVisibility([MarshalAs(UnmanagedType.LPWStr)] string device, int visible);
    }

    [DllImport("ole32.dll")]
    private static extern int PropVariantClear(ref PROPVARIANT pvar);

    private enum ERole { eConsole = 0, eMultimedia = 1, eCommunications = 2 }
    private enum CLSCTX : uint { CLSCTX_ALL = 0x17 }
}


