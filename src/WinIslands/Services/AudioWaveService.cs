using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WinIslands.Services;

/// <summary>
/// 播放波纹：优先用 WASAPI 环回（loopback）采集系统输出声音的真实电平，
/// 失败（无声卡/权限/格式）时自动降级为“播放状态驱动”的柔和模拟波纹。
/// 全部为系统 API（P/Invoke），无第三方依赖；后台采集线程任何异常都不会影响主程序。
/// </summary>
public sealed class AudioWaveService : IDisposable
{
    // WASAPI 常量
    private const int AUDCLNT_SHAREMODE_SHARED = 0;
    private const int AUDCLNT_STREAMFLAGS_LOOPBACK = 0x00020000;
    private const int WAVE_FORMAT_PCM = 1;
    private const int WAVE_FORMAT_IEEE_FLOAT = 3;
    private const int WasapiStartTimeoutMs = 4000; // WASAPI 初始化看门狗(ms)：超时则降级为节拍模拟
    private const int SimulateRetryMs = 8000;     // 跟随开启但实时不可用：模拟运行多久后回外层重试实时采集

    private static readonly Guid CLSID_MMDeviceEnumerator = new("BCDE0395-E52F-467C-8E3D-C4579291692E");
    private static readonly Guid IID_IMMDeviceEnumerator = new("A95664D2-9614-4F35-A746-DE8DB63617E6");
    private static readonly Guid IID_IAudioClient = new("1CB9AD4C-DBFA-4C32-B178-C2F568A703B2");
    private static readonly Guid IID_IAudioCaptureClient = new("C8ADBD64-E71E-48A0-A4DE-185C395CD317");

    private volatile bool _running;
    private volatile bool _liveStarted;         // WASAPI 实时采集已就绪（worker 线程置位）
    private volatile bool _wasapiWorkerActive;  // WASAPI 初始化 worker 是否仍在运行（防止阻塞线程累积）
    private Thread? _thread;
    private readonly object _gate = new();
    private double _level;              // 0..1 平滑后的波纹强度
    private volatile bool _playing;   // 跨线程读取（UI 写，采集/模拟线程读）
    private readonly Random _rng = new();
    private volatile bool _syncEnabled = true;   // 跟随音乐节奏：true=真实音频采集，false=节拍模拟
    private double _sensitivity = 1.0;       // 灵敏度倍率（0.2~3.0），用 Volatile 读写保证跨线程可见
    private DateTime _lastUpdate = DateTime.UtcNow; // 相邻数据包时间（用于包络指数平滑）

    /// <summary>当前波纹强度（0..1），UI 每帧轮询。</summary>
    public double Level { get { lock (_gate) return _level; } }

    /// <summary>是否启用了真实音频采集（false = 模拟降级）。</summary>
    public bool LiveCapture { get; private set; }

    /// <summary>设置灵敏度倍率（0.2~3.0），实时生效。</summary>
    public void SetSensitivity(double value)
    {
        if (double.IsFinite(value)) Volatile.Write(ref _sensitivity, Math.Clamp(value, 0.2, 3.0));
    }

    /// <summary>设置是否“跟随音乐节奏”（真实音频采集）；关闭时转为节拍模拟。</summary>
    public void SetSyncEnabled(bool enabled) => _syncEnabled = enabled;

    /// <summary>由媒体状态驱动：播放为 true、暂停/停止为 false。</summary>
    public void SetPlaying(bool playing)
    {
        _playing = playing;
    }

    public void Start()
    {
        if (_running) return;
        _running = true;
        _thread = new Thread(Loop) { IsBackground = true, Name = "AudioWave" };
        _thread.Start();
    }

    public void Stop()
    {
        _running = false;
    }

    private void Loop()
    {
        var simActive = false;
        while (_running)
        {
            try
            {
                if (_syncEnabled && !_wasapiWorkerActive)
                {
                    // WASAPI 初始化（CoCreateInstance / GetDefaultAudioEndpoint 等 COM 调用）在无声卡
                    // 或音频服务异常时可能长时间阻塞。放到独立线程 + 看门狗超时兜底：
                    // 超时未就绪就自动降级为节拍模拟，保证波纹连贯、永不卡死。
                    _liveStarted = false;
                    _wasapiWorkerActive = true;
                    var done = new ManualResetEventSlim(false);
                    var worker = new Thread(() =>
                    {
                        try { TryWasapiLoop(); }
                        catch (Exception ex) { AppLogger.Debug($"Audio wave WASAPI failed: {ex.Message}"); }
                        finally { _liveStarted = false; _wasapiWorkerActive = false; done.Set(); }
                    })
                    { IsBackground = true, Name = "AudioWaveWasapi" };
                    worker.Start();

                    var sw = Stopwatch.StartNew();
                    while (!done.IsSet && !_liveStarted && sw.ElapsedMilliseconds < WasapiStartTimeoutMs)
                        Thread.Sleep(25);

                    if (_liveStarted)
                    {
                        simActive = false;
                        LiveCapture = true;
                        AppLogger.Info("Audio wave: live WASAPI loopback capture active");
                        // 实时采集由 worker 线程持续运行；此处等待其结束（播放停止 / 关闭“跟随音乐节奏”）
                        // 空闲（未播放）时降低唤醒频率：worker 内部已挂起采集，主线程仅需低频等待
                        while (_running && _syncEnabled && !done.IsSet) Thread.Sleep(_playing ? 50 : 100);
                        LiveCapture = false;
                        continue;
                    }

                    if (sw.ElapsedMilliseconds >= WasapiStartTimeoutMs && !done.IsSet)
                        AppLogger.Warn("Audio wave: WASAPI init timed out; using beat simulation");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Debug($"Audio wave WASAPI failed: {ex.Message}");
            }

            // 实时不可用或未开启“跟随音乐节奏”→ 由播放状态驱动的节拍模拟，波纹依旧连贯起伏；
            // 周期回外层重试实时采集（例如用户插入声卡 / 音频服务恢复）。
            LiveCapture = false;
            if (!simActive)
            {
                AppLogger.Info("Audio wave: using beat simulation (no live audio capture)");
                simActive = true;
            }
            SimulateLoop();
        }
    }

    // ── 模拟降级：无实时采集时按“节拍”起伏，暂停时衰减到 0 ────────
    private void SimulateLoop()
    {
        var sw = Stopwatch.StartNew();
        var bpm = 88 + _rng.NextDouble() * 56;                  // 88 ~ 144 BPM
        var beatLen = 60.0 / bpm;
        double pulse = 0, nextBeat = 0;
        var attemptSw = Stopwatch.StartNew();                   // 跟随开启且实时不可用时，周期回外层重试
        while (_running && (!_syncEnabled || attemptSw.ElapsedMilliseconds < SimulateRetryMs))
        {
            lock (_gate)
            {
                if (_playing)
                {
                    var t = sw.Elapsed.TotalSeconds;
                    if (t >= nextBeat)
                    {
                        nextBeat = t + beatLen * (0.6 + _rng.NextDouble() * 0.8); // 略不规整更自然
                        pulse = 0.55 + _rng.NextDouble() * 0.45;                    // 拍点起跳
                    }
                    pulse *= 0.965;                                                // 指数衰减回落
                    var noise = 0.10 + 0.05 * Math.Sin(t * 13.0) + 0.035 * Math.Sin(t * 31.0);
                    _level = Math.Clamp(pulse * (0.55 + 0.45 * noise) * Volatile.Read(ref _sensitivity), 0, 1);
                }
                else
                {
                    _level *= 0.9;
                    if (_level < 0.01) _level = 0;
                }
            }
            Thread.Sleep(_playing ? 16 : 50); // 播放 60Hz 平滑轨迹；暂停时降低唤醒频率
        }
    }

    // ── WASAPI 环回采集 ────────────────────────────────────────────
    private bool TryWasapiLoop()
    {
        if (CoInitializeEx(IntPtr.Zero, 0) < 0) return false; // COINIT_MULTITHREADED

        IntPtr enumerator = IntPtr.Zero, device = IntPtr.Zero, client = IntPtr.Zero, capture = IntPtr.Zero;
        try
        {
            var clsidEnum = CLSID_MMDeviceEnumerator;
            var iidEnum = IID_IMMDeviceEnumerator;
            if (CoCreateInstance(ref clsidEnum, IntPtr.Zero, 23, ref iidEnum, out enumerator) < 0)
                return false;
            var devEnum = (IMMDeviceEnumerator)Marshal.GetObjectForIUnknown(enumerator);
            devEnum.GetDefaultAudioEndpoint(0 /*eRender*/, 1 /*eMultimedia*/, out device);
            if (device == IntPtr.Zero) return false;

            var mmDevice = (IMMDevice)Marshal.GetObjectForIUnknown(device);
            var iidClient = IID_IAudioClient;
            mmDevice.Activate(ref iidClient, 23 /*CLSCTX_ALL*/, IntPtr.Zero, out client);
            if (client == IntPtr.Zero) return false;

            var audioClient = (IAudioClient)Marshal.GetObjectForIUnknown(client);
            audioClient.GetMixFormat(out var fmtPtr);
            if (fmtPtr == IntPtr.Zero) return false;
            var fmt = Marshal.PtrToStructure<WAVEFORMATEX>(fmtPtr);
            if (fmt.nChannels == 0 || fmt.nSamplesPerSec == 0) return false;

            // 环回模式：200ms 缓冲
            const long bufferHns = 200 * 10000; // 200ms
            audioClient.Initialize(AUDCLNT_SHAREMODE_SHARED, AUDCLNT_STREAMFLAGS_LOOPBACK, bufferHns, 0, fmtPtr, IntPtr.Zero);
            var iidCapture = IID_IAudioCaptureClient;
            audioClient.GetService(ref iidCapture, out capture);
            if (capture == IntPtr.Zero) return false;
            var cap = (IAudioCaptureClient)Marshal.GetObjectForIUnknown(capture);

            audioClient.Start();
            _liveStarted = true; // 实时采集已就绪，主循环可切换为“实时”模式
            try
            {
                var blockAlign = Math.Max(1, (int)fmt.nBlockAlign);
                var channels = fmt.nChannels;
                var step = fmt.wBitsPerSample / 8;           // 每样本字节（2 或 4）
                if (step < 2) step = 2;

                while (_running && _syncEnabled)
                {
                    // 无播放时不采集：挂起读取循环（保持 WASAPI 会话打开，恢复播放即刻续采），降低空闲 CPU
                    if (!_playing)
                    {
                        lock (_gate)
                        {
                            _level *= 0.8;
                            if (_level < 0.01) _level = 0;
                        }
                        Thread.Sleep(100);
                        continue;
                    }

                    uint packet = 0;
                    if (cap.GetNextPacketSize(out packet) < 0 || packet == 0)
                    {
                        Thread.Sleep(8);
                        continue;
                    }
                    if (cap.GetBuffer(out var dataPtr, out uint frames, out _, out _, out _) < 0 || frames == 0 || dataPtr == IntPtr.Zero)
                    {
                        cap.ReleaseBuffer(0);
                        Thread.Sleep(8);
                        continue;
                    }

                    var totalBytes = (int)(frames * blockAlign);
                    totalBytes = Math.Min(totalBytes, 1 << 20); // 防异常大包
                    var bytes = new byte[totalBytes];
                    Marshal.Copy(dataPtr, bytes, 0, totalBytes);
                    cap.ReleaseBuffer(frames);

                    double raw = ComputeEnvelope(bytes, fmt, channels, step, totalBytes) * Volatile.Read(ref _sensitivity);
                    var now = DateTime.UtcNow;
                    var dt = (now - _lastUpdate).TotalSeconds;
                    _lastUpdate = now;
                    if (dt <= 0 || dt > 0.25) dt = 0.02;          // 防止时间跳变
                    // 包络跟随：起音 ~25ms 快、释放 ~140ms 慢 → 节拍起伏连贯
                    var tau = raw >= _level ? 0.025 : 0.14;
                    var alpha = 1.0 - Math.Exp(-dt / tau);
                    lock (_gate)
                    {
                        _level = raw < 0.004 ? 0 : _level + (raw - _level) * alpha;
                    }
                }
            }
            finally
            {
                try { audioClient.Stop(); } catch { }
            }
            return true;
        }
        finally
        {
            if (capture != IntPtr.Zero) Marshal.Release(capture);
            if (client != IntPtr.Zero) Marshal.Release(client);
            if (device != IntPtr.Zero) Marshal.Release(device);
            if (enumerator != IntPtr.Zero) Marshal.Release(enumerator);
        }
    }

    /// <summary>
    /// 包络检波：将一包 PCM 样本按 ~10ms 短窗求 RMS 与窗内峰值，返回 0..1 电平。
    /// 短窗峰值可捕捉鼓点等瞬态，使波纹跟随音乐节拍，而不是被整包平均值抹平。
    /// </summary>
    private static double ComputeEnvelope(byte[] bytes, WAVEFORMATEX fmt, ushort channels, int step, int length)
    {
        if (length <= 0) return 0;
        var isFloat = fmt.wFormatTag == WAVE_FORMAT_IEEE_FLOAT;
        var frameSize = Math.Max(1, channels * step);
        var frames = length / frameSize;
        if (frames <= 0) return 0;

        var winFrames = Math.Max(1, (int)(fmt.nSamplesPerSec * 0.010)); // 10ms 分析窗
        double maxRms = 0, maxWinPeak = 0, sumSq = 0;
        var nInWindow = 0;
        var idx = 0;
        for (var f = 0; f < frames; f++)
        {
            double v;
            if (isFloat)
            {
                var x = BitConverter.ToSingle(bytes, idx);
                if (float.IsNaN(x) || float.IsInfinity(x)) x = 0;
                v = Math.Abs(x);
            }
            else
            {
                var s = BitConverter.ToInt16(bytes, idx);
                v = Math.Abs(s / 32768.0);
            }
            if (v > 1) v = 1;
            sumSq += v * v;
            nInWindow++;
            if (v > maxWinPeak) maxWinPeak = v;
            idx += frameSize; // 每帧取第一个声道，足以反映节奏

            if (nInWindow >= winFrames)
            {
                var rms = Math.Sqrt(sumSq / nInWindow);
                if (rms > maxRms) maxRms = rms;
                sumSq = 0;
                nInWindow = 0;
            }
        }
        if (nInWindow > 0)
        {
            var rms = Math.Sqrt(sumSq / nInWindow);
            if (rms > maxRms) maxRms = rms;
        }

        // RMS 为主 + 窗内峰值补充瞬态；sqrt 感知压缩让中低音量也有起伏
        var level = Math.Sqrt(Math.Clamp(maxRms * 1.25, 0, 1));
        return Math.Clamp(level * 0.88 + maxWinPeak * 0.12, 0, 1);
    }

    public void Dispose() => Stop();

    // ── COM 接口（vtable 索引自 IUnknown 后开始）──
    [ComImport, Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IMMDeviceEnumerator
    {
        void EnumAudioEndpoints(int dataFlow, int stateMask, out IntPtr devices);
        void GetDefaultAudioEndpoint(int dataFlow, int role, out IntPtr device);
        void GetDevice(string id, out IntPtr device);
        void RegisterEndpointNotificationCallback(IntPtr client);
        void UnregisterEndpointNotificationCallback(IntPtr client);
    }

    [ComImport, Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IMMDevice
    {
        void Activate(ref Guid iid, int clsCtx, IntPtr activationParams, out IntPtr iface);
        void OpenPropertyStore(int stgmAccess, out IntPtr properties);
        void GetId(out IntPtr id);
        void GetState(out int state);
    }

    [ComImport, Guid("1CB9AD4C-DBFA-4C32-B178-C2F568A703B2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IAudioClient
    {
        void Initialize(int shareMode, int streamFlags, long bufferDurationHns, long periodicityHns, IntPtr waveFormat, IntPtr audioSessionGuid);
        void GetBufferSize(out uint numBufferFrames);
        void GetStreamLatency(out long latency);
        void GetCurrentPadding(out uint numPaddingFrames);
        void IsFormatSupported(int shareMode, IntPtr format, out IntPtr closestMatch);
        void GetMixFormat(out IntPtr deviceFormat);
        void GetDevicePeriod(out long defaultPeriod, out long minPeriod);
        void Start();
        void Stop();
        void Reset();
        void SetEventHandle(IntPtr eventHandle);
        void GetService(ref Guid iid, out IntPtr service);
    }

    [ComImport, Guid("C8ADBD64-E71E-48A0-A4DE-185C395CD317"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IAudioCaptureClient
    {
        int GetBuffer(out IntPtr data, out uint frames, out uint flags, out ulong devicePosition, out ulong qpcPosition);
        void ReleaseBuffer(uint frames);
        int GetNextPacketSize(out uint frames);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WAVEFORMATEX
    {
        public ushort wFormatTag;
        public ushort nChannels;
        public uint nSamplesPerSec;
        public uint nAvgBytesPerSec;
        public ushort nBlockAlign;
        public ushort wBitsPerSample;
        public ushort cbSize;
    }

    [DllImport("ole32.dll")]
    private static extern int CoInitializeEx(IntPtr pvReserved, uint dwCoInit);

    [DllImport("ole32.dll")]
    private static extern int CoCreateInstance(ref Guid rclsid, IntPtr pUnkOuter, uint dwClsContext, ref Guid riid, out IntPtr ppv);
}