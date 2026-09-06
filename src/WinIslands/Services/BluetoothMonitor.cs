using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace WinIslands.Services;

/// <summary>
/// 监听已配对蓝牙设备的连接/断开。
/// 采用「DeviceWatcher 事件即时触发 + 4s 轮询兜底」：设备连接/断开时 AEP 会触发事件，
/// 立即用 BluetoothDevice.ConnectionStatus 确认并弹出提示；轮询负责基线/漏检兜底。
/// </summary>
public sealed class BluetoothMonitor : IDisposable
{
    private const string NameKey = "System.ItemNameDisplay";

    private DeviceWatcher? _watcher;
    private System.Threading.Timer? _timer;
    private readonly Dictionary<string, string> _names = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, bool> _connected = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _gate = new();
    private bool _started;
    private bool _baselineReady;

    /// <summary>蓝牙设备连接（参数：设备名）。</summary>
    public event EventHandler<string>? DeviceConnected;
    /// <summary>蓝牙设备断开（参数：设备名）。</summary>
    public event EventHandler<string>? DeviceDisconnected;

    public void Start()
    {
        lock (_gate)
        {
            if (_started) return;
            _started = true;
            _baselineReady = false;
            _names.Clear();
            _connected.Clear();
        }

        try
        {
            var selector = Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelector();
            _watcher = DeviceInformation.CreateWatcher(selector, new[] { NameKey }, DeviceInformationKind.AssociationEndpoint);
            _watcher.Added += OnChanged;
            _watcher.Updated += OnChanged;
            _watcher.Removed += OnRemoved;
            _watcher.EnumerationCompleted += (_, _) =>
            {
                lock (_gate) _baselineReady = true;
                AppLogger.Info("Bluetooth watcher: enumeration completed (baseline ready).");
            };
            _watcher.Start();
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Bluetooth watcher start failed: {ex.Message}");
        }

        _timer?.Dispose();
        _timer = new System.Threading.Timer(_ => Poll(), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(8));
        AppLogger.Info("Bluetooth monitor started (watcher + poll).");
    }

    // ── 即时路径：设备状态变化 → 立即确认 ──
    private void OnChanged(DeviceWatcher sender, DeviceInformation info)
    {
        var name = (info.Properties.TryGetValue(NameKey, out var n) && n is string s && s.Length > 0) ? s : info.Name;
        lock (_gate) _names[info.Id] = name;
        _ = CheckDeviceImmediateAsync(info.Id);
    }

    private void OnChanged(DeviceWatcher sender, DeviceInformationUpdate update)
        => _ = CheckDeviceImmediateAsync(update.Id);

    private void OnRemoved(DeviceWatcher sender, DeviceInformationUpdate update)
        => _ = CheckDeviceImmediateAsync(update.Id);

    private async System.Threading.Tasks.Task CheckDeviceImmediateAsync(string id)
    {
        try
        {
            var bt = await BluetoothDevice.FromIdAsync(id);
            if (bt is null) return;
            var connected = bt.ConnectionStatus == BluetoothConnectionStatus.Connected;
            // 连接/断开变化由 ApplyState 以 Info 记录，这里不再逐设备轮询刷 Debug 日志
            ApplyState(id, connected);
        }
        catch (Exception ex)
        {
            AppLogger.Debug($"BT immediate check failed: {ex.Message}");
        }
    }

    // ── 兜底路径：4s 轮询 ──
    private void Poll()
    {
        try
        {
            var devices = DeviceInformation.FindAllAsync(
                BluetoothDevice.GetDeviceSelector(), new[] { NameKey }, DeviceInformationKind.AssociationEndpoint)
                .AsTask().GetAwaiter().GetResult();

            foreach (var d in devices)
            {
                var name = (d.Properties.TryGetValue(NameKey, out var n) && n is string s && s.Length > 0) ? s : d.Name;
                lock (_gate) { if (!_names.ContainsKey(d.Id)) _names[d.Id] = name; }
                _ = CheckDeviceImmediateAsync(d.Id);
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Bluetooth poll failed: {ex.Message}");
        }
    }

    /// <summary>比对状态并触发事件（基线建立前只记录，不触发）。</summary>
    private void ApplyState(string id, bool connected)
    {
        EventHandler<string>? ev = null;
        lock (_gate)
        {
            if (!_baselineReady)
            {
                _names[id] = SafeName(id);
                _connected[id] = connected;
                return;
            }
            var was = _connected.TryGetValue(id, out var w) && w;
            if (connected && !was) { _connected[id] = true; ev = DeviceConnected; }
            else if (!connected && was) { _connected[id] = false; ev = DeviceDisconnected; }
        }

        if (ev is not null)
        {
            var name = SafeName(id);
            AppLogger.Info($"Bluetooth event: {(ev == DeviceConnected ? "connected" : "disconnected")} '{name}'");
            ev(this, name);
        }
    }

    private string SafeName(string id)
    {
        lock (_gate) { return _names.TryGetValue(id, out var n) ? n : id; }
    }

    /// <summary>通过设备名反查设备 ID（#9 通知操作按钮：断开）。</summary>
    public string? FindDeviceId(string deviceName)
    {
        lock (_gate)
        {
            foreach (var kv in _names)
            {
                if (string.Equals(kv.Value, deviceName, System.StringComparison.OrdinalIgnoreCase))
                    return kv.Key;
            }
        }
        return null;
    }

    /// <summary>
    /// #9 断开指定蓝牙设备。Windows 没有「仅断开」官方 API，
    /// 最接近的是解除配对（Unpair）；失败/未找到时返回 false，由调用方回退打开蓝牙设置页。
    /// </summary>
    public async System.Threading.Tasks.Task<bool> DisconnectAsync(string deviceName)
    {
        try
        {
            var id = FindDeviceId(deviceName);
            if (string.IsNullOrEmpty(id))
            {
                AppLogger.Warn($"BT disconnect: device not found: {deviceName}");
                return false;
            }
            var bt = await BluetoothDevice.FromIdAsync(id);
            if (bt is null || bt.DeviceInformation is null) return false;
            var result = await bt.DeviceInformation.Pairing.UnpairAsync();
            AppLogger.Info($"BT disconnect (unpair) '{deviceName}' -> {result.Status}");
            return result.Status == DeviceUnpairingResultStatus.Unpaired;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"BT disconnect failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 读取已连接蓝牙设备的电量百分比（0..100）— GATT Battery Service (0x180F) / Battery Level (0x2A19)。
    /// 仅适用于支持该服务的 Bluetooth LE 设备；经典蓝牙、桌面设备或不支持时返回 null，
    /// 调用方优雅降级为只显示设备名。全部异常/超时均吞掉，绝不影响主流程。
    /// </summary>
    public async System.Threading.Tasks.Task<int?> GetBatteryLevelAsync(string deviceName)
    {
        System.Threading.CancellationTokenSource? cts = null;
        try
        {
            var id = FindDeviceId(deviceName);
            if (string.IsNullOrEmpty(id)) return null;
            cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(4));

            // 第一次尝试：AEP id 直接解析 LE 设备（部分 AEP 本身就是 LE 接口，可省一次全量枚举）
            Windows.Devices.Bluetooth.BluetoothLEDevice? le = null;
            try { le = await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(id).AsTask().WaitAsync(cts.Token); }
            catch { le = null; }

            // 兜底：按设备名在全部 LE 设备列表中匹配（经典蓝牙 AEP 无法直接转 LE，需按名称找）
            if (le is null)
            {
                try
                {
                    var allLe = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(
                        Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelector()).AsTask().WaitAsync(cts.Token);
                    foreach (var d in allLe)
                    {
                        if (string.Equals(d.Name, deviceName, StringComparison.OrdinalIgnoreCase))
                        {
                            try { le = await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(d.Id).AsTask().WaitAsync(cts.Token); }
                            catch { le = null; }
                            if (le is not null) break;
                        }
                    }
                }
                catch { le = null; }
            }
            if (le is null) return null;

            var svcResult = await le.GetGattServicesForUuidAsync(
                Windows.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery).AsTask().WaitAsync(cts.Token);
            var svc = svcResult.Services.FirstOrDefault();
            if (svc is null) return null;
            var chResult = await svc.GetCharacteristicsForUuidAsync(
                Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicUuids.BatteryLevel).AsTask().WaitAsync(cts.Token);
            var ch = chResult.Characteristics.FirstOrDefault();
            if (ch is null) return null;
            var read = await ch.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached).AsTask().WaitAsync(cts.Token);
            if (read.Status != Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success) return null;
            if (read.Value is null || read.Value.Length == 0) return null;
            var reader = Windows.Storage.Streams.DataReader.FromBuffer(read.Value);
            var v = reader.ReadByte(); // 电池百分比 0..100
            return v;
        }
        catch (Exception ex)
        {
            AppLogger.Debug($"BT battery read failed for '{deviceName}': {ex.Message}");
            return null;
        }
        finally
        {
            try { cts?.Dispose(); } catch { }
        }
    }

    public void Stop()
    {
        lock (_gate)
        {
            _started = false;
            _baselineReady = false;
            _names.Clear();
            _connected.Clear();
        }
        try
        {
            if (_watcher is not null)
            {
                _watcher.Added -= OnChanged;
                _watcher.Updated -= OnChanged;
                _watcher.Removed -= OnRemoved;
                _watcher.Stop();
                _watcher = null;
            }
            _timer?.Dispose();
            _timer = null;
        }
        catch { /* ignore */ }
        AppLogger.Info("Bluetooth monitor stopped.");
    }

    public void Dispose() => Stop();
}
