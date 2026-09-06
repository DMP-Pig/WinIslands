using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Windows;
using Application = System.Windows.Application;
using Localization = WinIslands.UI.Localization;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Threading;
using WinIslands.Diagnostics;
using WinIslands.Services;
using WinIslands.UI;

namespace WinIslands;

public partial class App : Application
{
    private SettingsService? _settings;
    private ThemeService? _theme;
    private CiderMediaProvider? _cider;
    private MediaCoordinator? _coordinator;
    private LyricsService? _lyrics;
    private IslandViewModel? _vm;
    private readonly List<IslandWindow> _windows = new();
    private LyricsWindow? _lyricsWindow;
    private MiniPlayerWindow? _miniPlayer;
    private TrayIcon? _tray;
    private SingleInstance? _singleInstance;
    private AppSettings? _lastPositionSettings;
    private BluetoothMonitor? _bluetooth;
    private IncomingCallMonitor? _callMonitor;
    private NetworkStatusMonitor? _network;
    private GlobalHotkeyService? _hotkeys;
    private QuickLauncherWindow? _launcher;
    private ClipboardPanelWindow? _clipboardPanel;
    private IslandApiServer? _islandApi;
    private MediaAppRegistry? _mediaApps;
    private ScreenCaptureMonitor? _screenCapture;
    private FullScreenMonitor? _fullScreenMonitor;
    private SessionSwitchEventHandler? _sessionSwitchHandler;   // 锁屏自动隐藏：SessionSwitch 订阅句柄
    private readonly DispatcherTimer _themeScheduleTimer = new() { Interval = TimeSpan.FromSeconds(30) }; // 定时明暗切换：每 30 秒检查一次
    private bool? _lastScheduledDark;   // 上次应用的定时深色状态，避免无变化时重复 Apply
    private CalendarService? _calendar;
    private RssMailService? _rssMail;

    // ── 效率工具 / 波纹 / 更新服务（共享实例：设置页与灵动岛组件共用同一份数据）──
    private AudioWaveService? _wave;
    private KeyboardIndicatorMonitor? _keyboard;
    private ClipboardHistoryService? _clipboard;
    private TodoService? _todo;
    private ScheduleService? _schedule;
    private PomodoroService? _pomodoro;
    private UpdaterService? _updater;

    public App()
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    [DllImport("user32.dll")]
    private static extern bool SetProcessDpiAwarenessContext(IntPtr value);
    private static readonly IntPtr DpiAwarenessPerMonitorV2 = new(-4);

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 窗口创建前强制 PerMonitorV2，避免启动瞬间按系统缩放渲染导致文字先大后恢复正常
        try { SetProcessDpiAwarenessContext(DpiAwarenessPerMonitorV2); }
        catch { /* manifest 已声明，忽略 */ }

        // ── Crash safety: log everything, never show a raw crash dialog. ──
        DispatcherUnhandledException += (_, args) =>
        {
            AppLogger.Error("Unhandled dispatcher exception", args.Exception);
            WriteCrashMarker(); // 崩溃自动恢复（1.2.0）：下次启动提示已恢复
            args.Handled = true;
        };
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            AppLogger.Error("Unhandled AppDomain exception", args.ExceptionObject as Exception);
        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            AppLogger.Error("Unobserved task exception", args.Exception);
            args.SetObserved();
        };

        AppPaths.EnsureDirectories();
        AppLogger.Info($"WinIslands starting. PID={Environment.ProcessId}");

        // ── Single instance ──
        _singleInstance = new SingleInstance();
        if (!_singleInstance.TryAcquire())
        {
            AppLogger.Info("Another instance is running; signaling show and exiting.");
            Shutdown();
            return;
        }

        _singleInstance.ShowRequested += (_, _) => Dispatcher.BeginInvoke(() => _vm?.ForceShow());

        // ── Services ──
        _settings = new SettingsService();
        _lastPositionSettings = _settings.Current.Clone();
        _theme = new ThemeService();
        _theme.Apply(_settings.Current);
        Localization.CurrentLanguage = _settings.Current.Language;

        _cider = new CiderMediaProvider(_settings);
        _mediaApps = new MediaAppRegistry();
        var smtc = new SmtcMediaProvider(_settings, _mediaApps, preferredAppId: "Cider"); // Cider session priority, avoid other active sessions
        var title = new WindowTitleMediaProvider();
        _coordinator = new MediaCoordinator(_settings, smtc, _cider, title, Dispatcher);
        _lyrics = new LyricsService(_settings, _cider);

        // ── 蓝牙监控：连接/断开事件以 iOS 风格卡片在灵动岛上展示（1.2.4：设备名 + 电量，读不到电量时只显示名称）──
        _bluetooth = new BluetoothMonitor();
        _bluetooth.DeviceConnected += async (_, name) =>
        {
            var battery = await _bluetooth.GetBatteryLevelAsync(name); // 异步读电量，失败返回 null，不阻塞 UI
            Dispatcher.BeginInvoke(() => _vm?.ShowDeviceEvent("bt:conn:" + name, connected: true, deviceName: name, batteryPercent: battery));
        };
        _bluetooth.DeviceDisconnected += (_, name) => Dispatcher.BeginInvoke(() =>
            _vm?.ShowDeviceEvent("bt:disc:" + name, connected: false, deviceName: name, batteryPercent: null));
        // 来电提醒：微信/QQ 语音视频通话窗口检测（仅本机，不上传数据）
        _callMonitor = new IncomingCallMonitor();
        _callMonitor.CallStarted += (appName, title, kind) =>
        {
            if (!_settings!.Current.CallNotifyEnabled) return;
            var isIncoming = kind == CallKind.Incoming;
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard(
                "call:" + appName,
                Localization.Get(isIncoming ? "Call_NotifyTitle" : "Call_Title"),
                isIncoming ? $"{appName} · {Localization.Get("Call_Body")}（{Localization.Get("Call_Coming")}）" : $"{appName} · {Localization.Get("Call_Body")}",
                "\uE8F2", "info", 8));
        };
        // 断网 / 网络恢复提醒（每次状态变化只提示一次；去抖在服务内部）
        _network = new NetworkStatusMonitor();
        _network.NetworkLost += (_, _) =>
        {
            if (_settings!.Current.NetworkNotifyEnabled)
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("net:lost",
                    Localization.Get("Network_LostTitle"), Localization.Get("Network_LostBody"), "\uE945", "error", 6));
        };
        _network.NetworkRestored += (_, _) =>
        {
            if (_settings!.Current.NetworkNotifyEnabled)
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("net:back",
                    Localization.Get("Network_BackTitle"), Localization.Get("Network_BackBody"), "\uE945", "success", 6));
        };

        // ── 效率工具 / 波纹 / 更新服务 ──
        _wave = new AudioWaveService();
        _keyboard = new KeyboardIndicatorMonitor();
        _clipboard = new ClipboardHistoryService();
        _todo = new TodoService();
        _schedule = new ScheduleService();
        _pomodoro = new PomodoroService();
        _updater = new UpdaterService();
        if (_settings.Current.WaveVisualizerEnabled) _wave.Start();
        _wave.SetSyncEnabled(_settings.Current.WaveSyncEnabled);
        _wave.SetSensitivity(_settings.Current.WaveSensitivity);
        _clipboard.SetEnabled(_settings.Current.ClipboardHistoryEnabled);
        _clipboard.MaxEntries = _settings.Current.ClipboardHistoryMax;
        // 复制提示（14 已复制 / 15 验证码 / 27 复制进度）：独立于剪贴板历史，任何一项开启即轮询
        _clipboard.EntryAdded += OnClipboardEntryAdded;
        UpdateClipboardPolling();
        UpdateKeyboardPolling();
        _schedule.Reminder += item => Dispatcher.BeginInvoke(() =>
            _vm?.ShowEventCard("schedule:" + item.Id, Localization.Get("Events_ScheduleReminder"), item.Title, "\uE8B7", "info", 8));

        _vm = new IslandViewModel(_coordinator, _settings, _lyrics,
            _wave, _keyboard, _clipboard, _todo, _schedule, _pomodoro);
        TryShowCrashRecoveryCard(); // 崩溃自动恢复（1.2.0）：上次异常退出 → 提示已恢复
        // 番茄钟到点提醒
        _vm.PomodoroCompletedRequested += phase =>
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("pomodoro:done",
                Localization.Get("Events_PomodoroDone"),
                phase == PomodoroPhase.Work ? Localization.Get("Events_PomodoroWorkDone") : Localization.Get("Events_PomodoroBreakDone"),
                "\uE823", "success", 6));
        _vm.OpenSettingsRequested += (_, _) => OpenSettings();
        _vm.ToggleLyricsWindowRequested += (_, _) => ToggleLyricsWindow();
        // 低电量 / 开始充电 / 充电完成 / 磁盘不足：统一以灵动岛卡片展示（iOS 风格）
        _vm.LowBatteryRequested += percent =>
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("battery:low",
                Localization.Get("LowBattery_Title"), $"{percent}%", "\uEBA0", "warning", 6));
        _vm.ChargingStartedRequested += percent =>
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("battery:charging",
                Localization.Get("Events_ChargingStarted"), $"{percent}%", "\uEBA0", "success", 5));
        _vm.ChargedRequested += percent =>
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("battery:charged",
                Localization.Get("Charged_Title"),
                string.Format(Localization.Get("Charged_Body"), percent), "\uEBA0", "success", 6));
        _vm.DiskLowRequested += gb =>
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("disk:low",
                Localization.Get("Disk_Title"),
                string.Format(Localization.Get("Disk_Body"), gb), "\uEDA2", "warning", 6));

        // ── 上岛 API：第三方软件推送信息到灵动岛 ──
        _islandApi = new IslandApiServer(_settings);
        _islandApi.PushReceived += push => Dispatcher.BeginInvoke(() => _vm?.PushIsland(push));
        _islandApi.PushRemoved += id => Dispatcher.BeginInvoke(() => _vm?.RemoveIslandPush(id));
        // #10 上岛按钮回调：notify 动作点击→ 向 WebSocket 订阅端广播 push_button 事件
        if (_vm is not null)
        {
            _vm.PushActionRequested += (button, pushId) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(pushId) && _vm.ActivePush is not null) pushId = _vm.ActivePush.Id;
                    _islandApi?.BroadcastPushButton(pushId ?? string.Empty, button.Label, button.Value);
                }
                catch (Exception ex)
                {
                    AppLogger.Warn($"push_button 广播失败: {ex.Message}");
                }
            };
        }
        if (_settings.Current.IslandApiEnabled) _islandApi.Start();

        // ── 全屏自动隐藏（视频/游戏/演示等全屏时隐藏灵动岛，退出恢复）──
        _fullScreenMonitor = new FullScreenMonitor();
        _fullScreenMonitor.FullScreenChanged += full => Dispatcher.BeginInvoke(() =>
        {
            if (_settings!.Current.FullScreenAutoHideEnabled)
            {
                if (_vm is not null) _vm.FullScreenHidden = full;
                _vm?.UpdateVisibility();
            }
        });
        if (_settings.Current.FullScreenAutoHideEnabled) _fullScreenMonitor.Start();

        // ── 屏幕录制 / 截图提示（PrintScreen 钩子 + 录制进程轮询；默认关）──
        _screenCapture = new ScreenCaptureMonitor();
        _screenCapture.ScreenshotTaken += () =>
        {
            _vm?.NotifyScreenshotTaken(); // 灵动岛「已截图」临时指示
            if (_settings!.Current.ScreenCaptureNotifyEnabled && _settings.Current.ScreenshotNotifyEnabled)
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("screencap:shot",
                    Localization.Get("ScreenCap_ScreenshotTitle"),
                    Localization.Get("ScreenCap_ScreenshotBody"), "\uE7B3", "info", 4));
        };
        _screenCapture.RecordingChanged += (recording, app) =>
        {
            _vm?.SetRecordingStatus(recording, app); // 灵动岛「录制中」指示
            if (!_settings!.Current.ScreenCaptureNotifyEnabled || !_settings.Current.RecordingNotifyEnabled) return;
            if (recording)
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("screencap:rec",
                    Localization.Get("ScreenCap_RecordingTitle"),
                    string.Format(Localization.Get("ScreenCap_RecordingBody"), app), "\uE786", "warning", 6));
        };
        // 录屏智能勿扰（录屏时自动勿扰）也需要轮询录制状态，与提示开关共用监控实例
        if (_settings.Current.ScreenCaptureNotifyEnabled || _settings.Current.RecordingDndEnabled)
        {
            _screenCapture.ScreenshotEnabled = _settings.Current.ScreenshotNotifyEnabled;
            _screenCapture.RecordingEnabled = _settings.Current.RecordingNotifyEnabled || _settings.Current.RecordingDndEnabled;
            _screenCapture.Start();
        }

        // ── 日历事件提醒（.ics 本地解析；事件到点弹右上角横幅，默认关）──
        _calendar = new CalendarService();
        _calendar.Reminder += ev => Dispatcher.BeginInvoke(() =>
        {
            if (!_settings!.Current.CalendarEnabled) return;
            var start = ev.Start.LocalDateTime;
            var isAllDay = start.Date == start && ev.End - ev.Start >= TimeSpan.FromDays(1);
            var body = isAllDay
                ? $"{start:yyyy-MM-dd}  {ev.Title}"
                : $"{start:HH:mm} ~ {ev.End.LocalDateTime:HH:mm}  {ev.Title}";
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("cal:" + ev.Id,
                Localization.Get("Calendar_ReminderTitle"), body, "\uE787", "info", 8));
        });
        _calendar.Refresh(_settings.Current.CalendarIcsPath);

        // ── RSS 订阅 / 邮件提醒（后台轮询；默认关，仅开启后联网）──
        _rssMail = new RssMailService();
        _rssMail.RssItemReceived += (title, summary, link) => Dispatcher.BeginInvoke(() =>
        {
            if (!_settings!.Current.RssNotifyEnabled) return;
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("rss:" + title,
                Localization.Get("Rss_NotifyTitle"),
                string.IsNullOrEmpty(summary) ? title : summary, "\uE8A5", "info", 8));
        });
        _rssMail.MailReceived += (subject, from, date) => Dispatcher.BeginInvoke(() =>
        {
            if (!_settings!.Current.MailNotifyEnabled) return;
            var body = string.IsNullOrEmpty(from)
                ? date
                : (string.IsNullOrEmpty(date) ? from : $"{from} · {date}");
            Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("mail:" + (subject ?? from ?? date),
                Localization.Get("Mail_NotifyTitle"),
                string.IsNullOrEmpty(subject) ? body : $"{subject}\n{body}", "\uE715", "info", 8));
        });
        ApplyRssMail(_settings.Current);

        // ── Island windows (one per selected monitor) ──
        RecreateWindows();

        // ── 迷你播放器（独立悬浮小窗，跟随媒体状态自动显隐）──
        _miniPlayer = new MiniPlayerWindow(_vm, _theme, _settings);
        _vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(IslandViewModel.HasMedia)) UpdateMiniPlayerVisibility();
        };
        UpdateMiniPlayerVisibility();

        // ── Tray ──
        _tray = new TrayIcon(_settings);
        _tray.ShowHideRequested += (_, _) => _vm.ToggleUserVisible();
        _tray.SettingsRequested += (_, _) => OpenSettings();
        _tray.ToggleLyricsRequested += (_, _) => ToggleLyricsWindow();
        _tray.AutoStartRequested += (_, _) =>
        {
            var enable = !AutoStart.IsEnabled();
            AutoStart.SetEnabled(enable);
            _settings.Update(s => s.StartWithWindows = enable);
            _tray.SetAutoStartChecked(enable);
        };
        _tray.DoNotDisturbRequested += (_, _) =>
        {
            var on = !DoNotDisturb.IsActive(_settings!.Current);
            _settings.Update(s => s.DoNotDisturbManual = on);
            _tray.SetDoNotDisturbChecked(on);
        };
        _tray.UpdateRequested += async (_, _) =>
        {
            try
            {
                if (_updater is null) return;
                var found = await _updater.CheckAsync();
                MessageBox.Show(found ? Localization.Get("Update_Found") : Localization.Get("Update_None"),
                    Localization.Get("Update_Title"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { AppLogger.Error("Check update failed", ex); }
        };
        _tray.LogsRequested += (_, _) =>
        {
            try { new LogViewerWindow().Show(); }
            catch (Exception ex) { AppLogger.Error("Open log viewer failed", ex); }
        };
        _tray.ExitRequested += (_, _) => Shutdown();

        // ── 全局快捷键（35 全局快捷键大全：组合键可在设置中自定义，见 GlobalHotkeyService）──
        _hotkeys = new GlobalHotkeyService(_settings.Current);
        _hotkeys.PlayPausePressed += () => _vm?.PlayPauseCommand.Execute(null);
        _hotkeys.NextPressed += () => _ = _coordinator?.NextAsync();
        _hotkeys.PreviousPressed += () => _ = _coordinator?.PreviousAsync();
        _hotkeys.ToggleVisibilityPressed += () => _vm?.ToggleUserVisible();
        _hotkeys.ExpandPressed += () =>
        {
            if (_vm is not null) _vm.IsExpanded = !_vm.IsExpanded; // 展开/收起
        };
        // 快速启动器：Ctrl+Space 弹出/收起（快捷键本身由 GlobalHotkeyService 注册）
        _launcher = new QuickLauncherWindow(_theme);
        _hotkeys.LauncherPressed += () => Dispatcher.BeginInvoke(() => _launcher?.Toggle());
        // 剪贴板历史面板（Ctrl+Alt+V）
        _clipboardPanel = new ClipboardPanelWindow(_theme, _clipboard);
        _hotkeys.ClipboardPanelPressed += () => Dispatcher.BeginInvoke(() => _clipboardPanel?.Toggle());
        _hotkeys.SetEnabled(_settings.Current.GlobalHotkeysEnabled);

        // ── Settings changed → re-apply live ──
        _settings.Changed += (_, s) =>
        {
            Localization.CurrentLanguage = s.Language;
            RefreshScheduledTheme(s);
            _vm?.RebuildQuickActions(); // 快捷操作按钮：开关/勾选/顺序变化即时生效

            if (AutoStart.IsEnabled() != s.StartWithWindows)
                AutoStart.SetEnabled(s.StartWithWindows);

            if (s.StandaloneLyricsWindow && _lyricsWindow is null)
                ShowLyricsWindow();
            if (!s.StandaloneLyricsWindow && _lyricsWindow is { IsVisible: true })
                _lyricsWindow.Hide();
            if (_lyricsWindow is { IsVisible: true })
                _lyricsWindow.ApplySettings(); // #5 歌词小窗不透明度/锁定实时生效

            // 仅位置/显示器类设置变更时重定位；锁定等其它变更保留拖动后的位置
            var posChanged = _lastPositionSettings is null
                || s.Position != _lastPositionSettings.Position
                || s.Monitor != _lastPositionSettings.Monitor
                || s.MonitorIndex != _lastPositionSettings.MonitorIndex
                || s.OffsetX != _lastPositionSettings.OffsetX
                || s.OffsetY != _lastPositionSettings.OffsetY;
            _lastPositionSettings = s.Clone();
            if (posChanged) RecreateWindows();

            // 通知监控开关
            if (s.BluetoothNotifyEnabled) _bluetooth?.Start(); else _bluetooth?.Stop();
            if (s.CallNotifyEnabled) _callMonitor?.Start(s.CallNotifyApps); else _callMonitor?.Stop();

            // 屏幕录制/截图提示：开关或细分项变化时实时生效
            if (_screenCapture is not null)
            {
                _screenCapture.ScreenshotEnabled = s.ScreenshotNotifyEnabled;
                _screenCapture.RecordingEnabled = s.RecordingNotifyEnabled;
                var capRunning = _screenCapture.IsRunning;
                var capWanted = s.ScreenCaptureNotifyEnabled || s.RecordingDndEnabled;
                _screenCapture.RecordingEnabled = s.RecordingNotifyEnabled || s.RecordingDndEnabled;
                if (capWanted && !capRunning) _screenCapture.Start();
                else if (!capWanted && capRunning) _screenCapture.Stop();
            }

            // 日历提醒：路径/开关变化立即重新解析；总开关关闭或无路径时停止轮询
            _calendar?.Refresh(s.CalendarIcsPath);
            _calendar?.SetEnabled(s.CalendarEnabled && !string.IsNullOrWhiteSpace(s.CalendarIcsPath));

            // 日程提醒轮询：仅当日程组件显示时运行，避免空闲空转
            _schedule?.SetPollingEnabled(s.Components.ScheduleWhenIdle || s.Components.ScheduleWhenPlaying);

            // RSS / 邮件提醒：开关、地址、间隔变化立即生效
            ApplyRssMail(s);

            // 全局快捷键：组合键 / 开关变化实时重新注册
            _hotkeys?.Apply(s);

            // 上岛 API：启用/端口变化时重启
            if (_islandApi is not null)
            {
                var running = _islandApi.IsRunning;
                if (s.IslandApiEnabled && !running) _islandApi.Start();
                else if (!s.IslandApiEnabled && running) _islandApi.Stop();
            }

            // 全屏自动隐藏：开关变化即时生效（关闭时立即恢复显示）
            if (_fullScreenMonitor is not null)
            {
                if (s.FullScreenAutoHideEnabled && !_fullScreenMonitor.IsRunning) _fullScreenMonitor.Start();
                else if (!s.FullScreenAutoHideEnabled && _fullScreenMonitor.IsRunning)
                {
                    _fullScreenMonitor.Stop();
                    if (_vm is not null) _vm.FullScreenHidden = false;
                    _vm?.UpdateVisibility();
                }
            }

            // 锁屏自动隐藏：开关关闭时立即恢复显示
            if (!s.LockScreenAutoHideEnabled && _vm is not null && _vm.LockScreenHidden)
            {
                _vm.LockScreenHidden = false;
                _vm.UpdateVisibility();
            }

            // 波纹可视化开关
            if (s.WaveVisualizerEnabled) _wave?.Start(); else _wave?.Stop();
            _wave?.SetSyncEnabled(s.WaveSyncEnabled);
            _wave?.SetSensitivity(s.WaveSensitivity);

            // 剪贴板历史开关与保留条数；复制提示（已复制/验证码/进度）无需历史也可轮询
            if (_clipboard is not null)
            {
                _clipboard.SetEnabled(s.ClipboardHistoryEnabled);
                _clipboard.MaxEntries = s.ClipboardHistoryMax;
            }
            UpdateClipboardPolling();
            UpdateKeyboardPolling();

            // 尺寸设置变更 → 应用到灵动岛
            foreach (var w in _windows) w.ApplySize();

            _vm?.UpdateVisibility();
            UpdateMiniPlayerVisibility();
        };

        // ── 定时明暗切换：到点自动切换深/浅主题（仅 Theme=Auto 且开关开启时生效）──
        _themeScheduleTimer.Tick += (_, _) => RefreshScheduledTheme(_settings!.Current);
        RefreshScheduledTheme(_settings.Current);   // 启动即应用一次
        if (_settings.Current.ThemeScheduledEnabled) _themeScheduleTimer.Start();

        // 启动即按当前设置应用日程/日历轮询状态（避免空闲空转）
        _schedule?.SetPollingEnabled(_settings.Current.Components.ScheduleWhenIdle || _settings.Current.Components.ScheduleWhenPlaying);
        _calendar?.SetEnabled(_settings.Current.CalendarEnabled && !string.IsNullOrWhiteSpace(_settings.Current.CalendarIcsPath));

        // ── 锁屏自动隐藏：锁屏/远程桌面断开时隐藏灵动岛，解锁后恢复 ──
        _sessionSwitchHandler = (_, args) =>
        {
            if (_settings is null || !_settings.Current.LockScreenAutoHideEnabled) return;
            var locked = args.Reason is SessionSwitchReason.SessionLock;
            var unlocked = args.Reason is SessionSwitchReason.SessionUnlock;
            if (!locked && !unlocked) return;
            Dispatcher.BeginInvoke(() =>
            {
                if (_vm is null) return;
                _vm.LockScreenHidden = locked;
                _vm.UpdateVisibility();
            });
        };
        SystemEvents.SessionSwitch += _sessionSwitchHandler;

        // Sync registry state once at startup (in case settings were edited externally).
        if (AutoStart.IsEnabled() != _settings.Current.StartWithWindows)
            AutoStart.SetEnabled(_settings.Current.StartWithWindows);

        // ── Start media pipeline ──
        _coordinator.Start();

        if (_settings.Current.BluetoothNotifyEnabled) _bluetooth?.Start();
        if (_settings.Current.CallNotifyEnabled) _callMonitor?.Start(_settings.Current.CallNotifyApps);
        _network?.Start(); // 网络监控很轻量，始终启动；是否弹横幅由 NetworkNotifyEnabled 开关控制
        _vm.UpdateVisibility();

        if (_settings.Current.StandaloneLyricsWindow)
            ShowLyricsWindow();

        // 启动时自动检查新版本（可选；需联网，默认关闭）
        if (_settings.Current.AutoUpdateCheck)
            _ = CheckForUpdatesAsync(showWhenUpToDate: false);

        // ── Diagnostics mode: report and exit. ──
        if (e.Args.Contains("--diagnose", StringComparer.OrdinalIgnoreCase))
            _ = RunDiagnosticsAsync();

        // ── Demo mode: show a fake track for preview / testing. ──
        if (e.Args.Contains("--demo", StringComparer.OrdinalIgnoreCase))
            _vm.InjectDemoMedia();

        // ── Open the settings window at startup (also handy from the CLI). ──
        if (e.Args.Contains("--settings", StringComparer.OrdinalIgnoreCase))
            Dispatcher.BeginInvoke(OpenSettings);
    }

    /// <summary>检查更新；有新版时弹通知（可带下载链接）。</summary>
    private async Task CheckForUpdatesAsync(bool showWhenUpToDate)
    {
        try
        {
            if (_updater is null) return;
            var hasNew = await _updater.CheckAsync();
            if (!hasNew && showWhenUpToDate)
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("update:none",
                    Localization.Get("Update_Title"), Localization.Get("Update_None"), "\uE72E", "info", 5));
        }
        catch (Exception ex)
        {
            AppLogger.Error("Update check failed", ex);
        }
    }

    private async Task RunDiagnosticsAsync()
    {
        try
        {
            var text = await DiagnosticsCommand.RunAsync(_settings!, _cider);
            var path = Path.Combine(AppPaths.AppDataDir, "diagnostics.txt");
            await File.WriteAllTextAsync(path, text);
            MessageBox.Show($"诊断信息已写入：\n{path}\n\n---\n{text}",
                "WinIslands 诊断", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            AppLogger.Error("Diagnostics failed", ex);
        }
    }

    private void RecreateWindows()
    {
        if (_settings is null || _vm is null || _theme is null) return;

        var screens = ScreenHelper.ResolveScreens(_settings.Current);

        // Recreate only when the monitor selection actually changed.
        if (_windows.Count == screens.Count &&
            _windows.Select(w => w.Screen.DeviceName).SequenceEqual(screens.Select(s => s.DeviceName)))
        {
            foreach (var w in _windows) w.Reposition();
            return;
        }

        foreach (var w in _windows)
        {
            w.Close();
        }

        _windows.Clear();
        foreach (var screen in screens)
        {
            var win = new IslandWindow(_vm, _theme, _settings, screen);
            _windows.Add(win);
            // Force the window to load now so IsVisible changes (which can happen before
            // the first Show) can drive it; OnLoaded re-applies the correct visibility.
            win.Show();
            if (!_vm.IsVisible) win.Hide();
        }
    }

    /// <summary>定时明暗切换：计算当前应处的明/暗状态并应用到主题（跨天与勿扰同理，起止相同视为不生效）。</summary>
    private void RefreshScheduledTheme(AppSettings s)
    {
        if (_theme is null) return;
        var enabled = s.ThemeScheduledEnabled && s.Theme == ThemeMode.Auto;
        bool? dark = null;
        if (enabled)
        {
            var start = TimeSpan.FromHours(Math.Clamp(s.ThemeScheduleDarkStartHour, 0, 23));
            var end = TimeSpan.FromHours(Math.Clamp(s.ThemeScheduleDarkEndHour, 0, 23));
            var now = DateTime.Now.TimeOfDay;
            dark = start == end ? false : start < end ? now >= start && now < end : now >= start || now < end;
        }
        if (dark == _lastScheduledDark && _theme.OverrideDark == dark) return;   // 无变化，避免重复刷新
        _lastScheduledDark = dark;
        _theme.OverrideDark = dark;
        _theme.Apply(s);
    }

    private void OpenSettings()
    {
        if (_settings is null) return;
        var vm = new SettingsViewModel(_settings, _mediaApps);
        var win = new SettingsWindow(vm, _settings, _cider,
            _todo, _schedule, _clipboard, _pomodoro, _updater, _vm);
        win.ShowDialog();
    }

    /// <summary>#9 断开蓝牙设备：解除配对即断开；失败则回退打开蓝牙设置页。</summary>
    private void DisconnectBluetooth(string deviceName)
    {
        _ = DisconnectBluetoothAsync(deviceName);
    }

    private async System.Threading.Tasks.Task DisconnectBluetoothAsync(string deviceName)
    {
        try
        {
            if (_bluetooth is not null && await _bluetooth.DisconnectAsync(deviceName)) return;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"BT disconnect fallback: {ex.Message}");
        }
        OpenBluetoothSettings();
    }

    /// <summary>打开 Windows 蓝牙设置页。</summary>
    private static void OpenBluetoothSettings()
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("ms-settings:bluetooth")
            {
                UseShellExecute = true,
            });
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Open bluetooth settings failed: {ex.Message}");
        }
    }

    /// <summary>把 RSS/邮件设置应用到后台轮询服务（开关/地址/间隔等变化时即时生效）。</summary>
    private void ApplyRssMail(AppSettings s)
    {
        _rssMail?.Configure(
            s.RssNotifyEnabled, s.RssUrls, s.RssIntervalMinutes,
            s.MailNotifyEnabled, s.MailPop3Server, s.MailPop3Port, s.MailUseSsl,
            s.MailUser, s.MailPassword, s.MailCheckMinutes);
    }

    private void ToggleLyricsWindow()
    {
        if (_lyricsWindow is { IsVisible: true })
        {
            _lyricsWindow.Hide();
            _settings!.Update(s => s.StandaloneLyricsWindow = false);
            _tray?.SetLyricsChecked(false);
        }
        else
        {
            ShowLyricsWindow();
        }
    }

    private void ShowLyricsWindow()
    {
        if (_vm is null) return;
        if (_lyricsWindow is null)
        {
            _lyricsWindow = new LyricsWindow(_vm, _settings!);
        }

        _lyricsWindow.PositionNearBottom();
        _lyricsWindow.Show();
        _tray?.SetLyricsChecked(true);
    }

    /// <summary>迷你播放器显隐策略：开关开启且正在播放媒体时显示，否则隐藏并保存位置。</summary>
    private void UpdateMiniPlayerVisibility()
    {
        if (_miniPlayer is null || _settings is null || _vm is null) return;
        var s = _settings.Current;
        if (s.MiniPlayerEnabled && _vm.HasMedia)
        {
            if (!_miniPlayer.IsVisible)
            {
                _miniPlayer.PositionFromSettings();
                _miniPlayer.Show();
            }
        }
        else if (_miniPlayer.IsVisible)
        {
            SaveMiniPlayerPosition();
            _miniPlayer.Hide();
        }
    }

    /// <summary>持久化迷你播放器位置（直接写 Current 并落盘，不触发 Changed 避免递归）。</summary>
    private void SaveMiniPlayerPosition()
    {
        if (_miniPlayer is null || _settings is null) return;
        try
        {
            _settings.Current.MiniPlayerLeft = _miniPlayer.Left;
            _settings.Current.MiniPlayerTop = _miniPlayer.Top;
            _settings.Save();
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"保存迷你播放器位置失败: {ex.Message}");
        }
    }

    /// <summary>键盘指示灯轮询仅在 CapsLock 组件启用时运行（后台减负）。</summary>
    private void UpdateKeyboardPolling()
    {
        if (_keyboard is null || _settings is null) return;
        var comp = _settings.Current.Components ?? new ComponentFlags();
        _keyboard.SetPolling(comp.CapsLockWhenIdle || comp.CapsLockWhenPlaying);
    }

    /// <summary>根据设置决定剪贴板轮询：历史记录或任一复制提示开启时轮询。</summary>
    private void UpdateClipboardPolling()
    {
        if (_clipboard is null || _settings is null) return;
        var s = _settings.Current;
        var copyUi = s.CopyToastEnabled || s.CodeToastEnabled || s.CopyProgressEnabled;
        _clipboard.SetPolling(s.ClipboardHistoryEnabled || copyUi);
    }

    /// <summary>剪贴板新增内容 → 已复制 / 验证码 / 大文本进度提示（14/15/27）。</summary>
    private void OnClipboardEntryAdded(ClipboardEntry entry)
    {
        try
        {
            if (_settings is null) return;
            var s = _settings.Current;
            var text = entry.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            // 验证码高亮提示优先（短信场景，通常很短）
            if (s.CodeToastEnabled && VerificationCodeDetector.TryExtract(text, out var code))
            {
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("clip:code",
                    Localization.Get("Clipboard_CodeTitle"),
                    Localization.Get("Clipboard_CodeBody").Replace("{code}", code),
                    "", "info", 6));
                return;
            }

            // 大文本：复制进度（Windows 不暴露真实进度，按长度估算动画）
            if (s.CopyProgressEnabled && text.Length >= Math.Max(500, s.CopyProgressThreshold))
            {
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("clip:progress",
                    Localization.Get("Clipboard_CopiedTitle"),
                    ClipboardPreview(text), "", "info", 4));
                return;
            }

            // 普通复制：已复制提示
            if (s.CopyToastEnabled)
                Dispatcher.BeginInvoke(() => _vm?.ShowEventCard("clip:copied",
                    Localization.Get("Clipboard_CopiedTitle"), ClipboardPreview(text), "", "info", 4));
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"剪贴板提示失败: {ex.Message}");
        }
    }

    /// <summary>剪贴板预览：取首行、截断 45 字符。</summary>
    private static string ClipboardPreview(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        var t = text.Trim();
        var nl = t.IndexOfAny(new[] { '\r', '\n' });
        if (nl >= 0) t = t.Substring(0, nl);
        return t.Length <= 45 ? t : t.Substring(0, 42) + "…";
    }

    // ── 崩溃自动恢复（1.2.0）──
    private void WriteCrashMarker()
    {
        try
        {
            AppPaths.EnsureDirectories();
            File.WriteAllText(AppPaths.CrashMarkerFile,
                "{\"crashedAt\":\"" + DateTime.UtcNow.ToString("o") + "\"}");
        }
        catch { /* 标记写入失败不影响主流程 */ }
    }

    /// <summary>启动时检测崩溃标记：有则提示「已恢复」并清理；岛位/播放信息由设置持久化自动恢复。</summary>
    private void TryShowCrashRecoveryCard()
    {
        try
        {
            if (!File.Exists(AppPaths.CrashMarkerFile)) return;
            File.Delete(AppPaths.CrashMarkerFile);
            Dispatcher.BeginInvoke(() =>
                _vm?.ShowEventCard("crash:recovered", Localization.Get("Crash_RecoveredTitle"),
                    Localization.Get("Crash_RecoveredBody"), "\uE7BA", "warning", 8));
            AppLogger.Info("Crash recovery marker found; showing recovery card");
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"TryShowCrashRecoveryCard failed: {ex.Message}");
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        AppLogger.Info("WinIslands exiting.");
        try { if (File.Exists(AppPaths.CrashMarkerFile)) File.Delete(AppPaths.CrashMarkerFile); } catch { /* 正常退出清理崩溃标记 */ }
        try
        {
            SaveMiniPlayerPosition();
            _settings?.Save();
            _vm?.SavePlaybackState(); // 退出前保存播放位置，重启后恢复（暂停时不再跳回开头）
            _vm?.Dispose();
            _wave?.Dispose();
            _keyboard?.Dispose();
            _clipboard?.Dispose();
            _todo?.Dispose();
            _schedule?.Dispose();
            _pomodoro?.Dispose();
            _coordinator?.Dispose();
            _bluetooth?.Dispose();
            _network?.Dispose();
            _hotkeys?.Dispose();
            _islandApi?.Dispose();
            _screenCapture?.Dispose();
            _callMonitor?.Dispose();
        _fullScreenMonitor?.Dispose();
            _calendar?.Dispose();
            _rssMail?.Dispose();
            _tray?.Dispose();
            _singleInstance?.Dispose();
            if (_sessionSwitchHandler is not null)
                SystemEvents.SessionSwitch -= _sessionSwitchHandler;
            _themeScheduleTimer.Stop();
            _miniPlayer?.Close();
            foreach (var w in _windows) w.Close();
        }
        catch (Exception ex)
        {
            AppLogger.Error("Error during shutdown", ex);
        }

        base.OnExit(e);
    }
}



