using System.Text.Json;
using System.Text.Json.Serialization;

using System.IO;

namespace WinIslands.Services;

public enum IslandPosition { Center, Right }

/// <summary>一个媒体程序的配置：是否启用 + 在列表中的位置即优先级。</summary>
public class MediaAppEntry
{
    public string Key { get; set; } = "";   // SMTC SourceAppUserModelId（如 Cider.exe、Spotify.exe）
    public bool Enabled { get; set; } = true;
}
public enum ThemeMode { Auto, Light, Dark }
public enum MonitorSelection { Primary, All, Index }

/// <summary>灵动岛组件开关：无歌曲播放时（Idle）与有歌曲播放时（Playing）可分别勾选。</summary>
public sealed class ComponentFlags
{
    public bool TimeWhenIdle { get; set; } = true;
    public bool TimeWhenPlaying { get; set; } = false;
    public bool WeatherWhenIdle { get; set; } = false;
    public bool WeatherWhenPlaying { get; set; } = false;
    public bool CoverWhenIdle { get; set; } = false;
    public bool CoverWhenPlaying { get; set; } = true;
    public bool TitleWhenIdle { get; set; } = false;
    public bool TitleWhenPlaying { get; set; } = true;
    public bool ArtistWhenIdle { get; set; } = false;
    public bool ArtistWhenPlaying { get; set; } = true;
    public bool LyricsWhenIdle { get; set; } = false;
    public bool LyricsWhenPlaying { get; set; } = true;
    public bool ProgressWhenIdle { get; set; } = false;
    public bool ProgressWhenPlaying { get; set; } = false;
    public bool DateWhenIdle { get; set; } = true;
    public bool DateWhenPlaying { get; set; } = false;
    public bool CpuWhenIdle { get; set; } = false;
    public bool CpuWhenPlaying { get; set; } = false;
    public bool RamWhenIdle { get; set; } = false;
    public bool RamWhenPlaying { get; set; } = false;
    public bool GpuWhenIdle { get; set; } = false;
    public bool GpuWhenPlaying { get; set; } = false;
    public bool MicWhenIdle { get; set; } = false;
    public bool MicWhenPlaying { get; set; } = false;
    public bool CamWhenIdle { get; set; } = false;
    public bool CamWhenPlaying { get; set; } = false;
    public bool NetWhenIdle { get; set; } = false;
    public bool NetWhenPlaying { get; set; } = false;
    public bool BatteryWhenIdle { get; set; } = false;
    public bool BatteryWhenPlaying { get; set; } = false;
    public bool VolumeWhenIdle { get; set; } = false;
    public bool VolumeWhenPlaying { get; set; } = true;
    public bool CapsLockWhenIdle { get; set; } = false;
    public bool CapsLockWhenPlaying { get; set; } = false;
    public bool ClipboardWhenIdle { get; set; } = false;
    public bool ClipboardWhenPlaying { get; set; } = true;
    public bool TodoWhenIdle { get; set; } = true;
    public bool TodoWhenPlaying { get; set; } = false;
    public bool TimerWhenIdle { get; set; } = false;
    public bool TimerWhenPlaying { get; set; } = false;
    public bool ScheduleWhenIdle { get; set; } = true;
    public bool ScheduleWhenPlaying { get; set; } = false;
    public bool HolidayWhenIdle { get; set; } = true;   // 节假日倒计时：空闲时显示
    public bool HolidayWhenPlaying { get; set; } = false;
    public bool MeetingWhenIdle { get; set; } = false;      // 会议中状态（开会静音助手）
    public bool MeetingWhenPlaying { get; set; } = false;
    public bool DiskWhenIdle { get; set; } = false;         // 磁盘剩余空间（系统盘）
    public bool DiskWhenPlaying { get; set; } = false;
    public bool InputMethodWhenIdle { get; set; } = false;   // 输入法状态（中/英 + 输入法名）
    public bool InputMethodWhenPlaying { get; set; } = false;
    public bool QuickTogglesWhenIdle { get; set; } = false;  // 快捷开关（WiFi/蓝牙/夜间模式/静音）
    public bool QuickTogglesWhenPlaying { get; set; } = false;
}
/// <summary>Persisted user configuration. JSON at %APPDATA%\WinIslands\settings.json.</summary>
public sealed class AppSettings
{
    public int Version { get; set; } = 1;

    // ── Appearance ─────────────────────────────────────────────
    public string Language { get; set; } = "zh-CN";
    public ThemeMode Theme { get; set; } = ThemeMode.Auto;
    public bool ThemeScheduledEnabled { get; set; } = false;   // 明暗主题定时切换（仅 Theme=Auto 时生效）
    public int ThemeScheduleDarkStartHour { get; set; } = 19;  // 定时切换：深色开始小时
    public int ThemeScheduleDarkEndHour { get; set; } = 7;     // 定时切换：深色结束小时
    public string AccentColor { get; set; } = "#6C5CE7";
    public IslandPosition Position { get; set; } = IslandPosition.Center;
    public MonitorSelection Monitor { get; set; } = MonitorSelection.Primary;
    public int MonitorIndex { get; set; } = 0;
    public double OffsetX { get; set; } = 0;
    public double OffsetY { get; set; } = 8;
    public double Opacity { get; set; } = 0.92;
    public bool EdgeSnapEnabled { get; set; } = true;        // 拖动松手自动吸附屏幕边缘/居中
    public bool FullScreenAutoHideEnabled { get; set; } = true;   // 全屏（视频/游戏/演示）时自动隐藏灵动岛
    public bool LockScreenAutoHideEnabled { get; set; } = true;   // 锁屏（Win+L/远程桌面断开）时自动隐藏灵动岛，解锁后恢复
    public double? IslandManualLeft { get; set; }            // 手动拖动后的窗口 Left（DIP）；null=跟随默认定位
    public double? IslandManualTop { get; set; }             // 手动拖动后的窗口 Top（DIP）；null=跟随默认定位

    // ── 组件图标定制（Key=组件 Kind，Value=图标字符；空字典表示全部用默认字形）──
    public Dictionary<string, string> ComponentIcons { get; set; } = new();

    // ── Behavior ───────────────────────────────────────────────
    public bool IsLocked { get; set; } = true;   // 上锁后不可拖动，解锁后可拖动
    public bool HideWhenNoMedia { get; set; } = true;
    public bool IslandAlwaysVisible { get; set; } = false;   // 常驻：始终显示（无视媒体/暂停）
    public bool ShowMediaInfo { get; set; } = true;              // 是否显示媒体播放信息（歌名/封面/歌词等）
    public bool ReduceMotion { get; set; } = false;             // 减少动态效果（无障碍/省电）
    public bool GlobalHotkeysEnabled { get; set; } = true;         // 全局快捷键
    public string DoubleClickAction { get; set; } = "PlayPause";   // 双击灵动岛快捷动作：PlayPause | OpenSettings | None
    public string MiddleClickAction { get; set; } = "PlayPause";   // 中键单击灵动岛快捷动作（与双击动作同值域）
    // ── 快捷操作按钮（展开卡片底部一排）──
    // QuickActions = 全部可用操作的顺序；QuickActionsShown = 勾选显示的子集。
    public bool QuickActionsEnabled { get; set; } = true;
    public List<string> QuickActions { get; set; } = new()
    {
        "Lock", "Mute", "PlayPause", "Screenshot", "Settings",
        "Desktop", "TaskManager", "Calculator", "Sleep", "VolumeUp", "VolumeDown",
    };
    public List<string> QuickActionsShown { get; set; } = new() { "Lock", "Mute", "PlayPause", "Screenshot", "Settings" };
    // ── 来电提醒（微信/QQ 语音视频通话窗口检测；仅本机前台窗口，不联网）──
    public bool CallNotifyEnabled { get; set; } = true;
    public List<string> CallNotifyApps { get; set; } = new() { "Weixin", "WeChat", "QQ" };
    // -- 动效与性能（33 动效皮肤 / 37 低功耗模式）--
    public string AnimationStyle { get; set; } = "Spring";   // Spring | Soft | Elastic | Fade（动效皮肤）
    public int IslandAnimationDuration { get; set; } = 700;  // 展开/收起动画时长（毫秒，300~1400，越大越丝滑）
    public bool LowPowerMode { get; set; } = false;          // 低功耗模式：空闲降帧渲染波纹、简化动画

    // -- 全局快捷键（自定义组合键，格式如 Ctrl+Alt+I；35 全局快捷键大全）--
    public string HotkeyToggleVisible { get; set; } = "Ctrl+Alt+I";
    public string HotkeyPlayPause { get; set; } = "Ctrl+Alt+P";
    public string HotkeyNext { get; set; } = "Ctrl+Alt+Right";
    public string HotkeyPrev { get; set; } = "Ctrl+Alt+Left";
    public string HotkeyExpand { get; set; } = "Ctrl+Alt+Space";
    public string HotkeyLauncher { get; set; } = "Ctrl+Space";          // 快速启动器（注意：Ctrl+Space 与中文输入法切换键冲突时可改其它组合）
    public bool QuickLauncherEnabled { get; set; } = true;              // 快速启动器开关
    public string HotkeyClipboardPanel { get; set; } = "Ctrl+Alt+V";   // 剪贴板历史面板快捷键
    public bool ClipboardPanelEnabled { get; set; } = true;             // 剪贴板历史面板开关
    public int LowBatteryThreshold { get; set; } = 20;             // 低电量提醒阈值（%）
    public bool LowBatteryPersistentEnabled { get; set; } = true;   // 1.2.4 低电量常驻指示（右上角电量胶囊，常驻不反复弹通知）
    public bool ChargedNotifyEnabled { get; set; } = true;          // 充电完成提醒：连接电源且电量达到阈值时弹一次
    public int ChargedThreshold { get; set; } = 100;                // 充电完成提醒阈值（%）
    public bool DiskAlertEnabled { get; set; } = true;       // 磁盘剩余空间不足提醒
    public int DiskAlertThresholdGB { get; set; } = 10;      // 磁盘剩余空间提醒阈值（GB）
    public bool ShowWhenPaused { get; set; } = true;
    public bool StartWithWindows { get; set; } = false;
    public bool StartHidden { get; set; } = false;

    // ── Compact mode content ───────────────────────────────────
    public bool CompactShowArt { get; set; } = true;
    public bool CompactShowTitle { get; set; } = true;
    public bool CompactShowProgress { get; set; } = false;
    public bool SingleLineMode { get; set; } = true;    // 单行模式：紧凑态所有组件一行显示（默认开启）
    public bool ShowLunarOnDate { get; set; } = true;      // 日期组件附加显示农历与节气

    // ── Expanded card sections（展开卡片里可独立开关的区块）──
    public bool ExpandedShowArtTitle { get; set; } = true;   // 大封面 + 歌名/歌手/专辑
    public bool ExpandedShowProgress { get; set; } = true;   // 进度条 + 时间
    public bool ExpandedShowControls { get; set; } = true;   // 控制按钮 + 音量
    public bool ExpandedShowLyrics { get; set; } = true;     // 歌词滚动区
    public string ExpandedCardStyle { get; set; } = "Classic"; // 展开卡片模板：Classic（经典小封面）| Hero（媒体大卡片）


    // ── Cider ──────────────────────────────────────────────────
    public bool CiderEnabled { get; set; } = true;
    public int CiderPort { get; set; } = 0;          // 0 = auto-detect (default 10767 then scan)
    public string CiderToken { get; set; } = string.Empty;

    // ── Lyrics ─────────────────────────────────────────────────
    // ── Lyrics ─────────────────────────────────────────────────
    public bool AmllTtmlEnabled { get; set; } = true;         // AMLL TTML 逐字歌词库（api.amll.dev，非官方；基于 LRC/TTML 更精确的逐字卡拉OK）
    public bool OnlineLyricsEnabled { get; set; } = true;    // 在线歌词（网易云非官方接口）；右键灵动岛可一键开关
    public string LyricsFolder { get; set; } = string.Empty; // extra .lrc folder; empty = auto (Music)
    public bool StandaloneLyricsWindow { get; set; } = false;
    public bool KaraokeHighlight { get; set; } = true;
    public string LyricsPreferredSource { get; set; } = "Auto";  // 首选歌词来源：Auto | Local | Amll | Cider | Online（一键切换，未命中自动回退 Auto 优先级）
    public bool LyricsSourcePick { get; set; } = false;          // 在灵动岛歌词区显示来源切换按钮（一键切换歌词来源）
    public bool BilingualLyrics { get; set; } = true;   // 双语歌词：自动合并相邻时间戳的翻译行（可关闭）
    public Dictionary<string, double> LyricTimeOffsets { get; set; } = new();  // #4 歌词时间微调：每首歌的时间偏移（秒）
    public double LyricsWindowOpacity { get; set; } = 0.85;   // #5 独立歌词小窗不透明度（0.3~1.0）
    public bool LyricsWindowLocked { get; set; } = false;     // #5 锁定歌词小窗（锁定后不可拖动且鼠标穿透）
    // ── #5 歌词样式调节（灵动岛歌词区 + 独立歌词小窗）──
    public double LyricFontSize { get; set; } = 13;           // 普通歌词行字号（9~28）
    public double LyricCurrentFontSize { get; set; } = 20;    // 当前句歌词字号（12~36）
    public double LyricLineSpacing { get; set; } = 1.0;       // 歌词行间距倍率（0.8~2.0）
    public string LyricHighlightColor { get; set; } = "";     // 卡拉OK高亮色（#RRGGBB，留空跟随主题）
    public string LyricBaseColor { get; set; } = "";          // 普通歌词颜色（#RRGGBB，留空跟随主题）
    public double KaraokeSpeed { get; set; } = 1.0;           // 卡拉OK推进速度（0.5~2.0）


    // ── Volume ─────────────────────────────────────────────────
    public bool UseSystemVolume { get; set; } = true;   // for non-Cider sources, control system volume

    // ── Island size ────────────────────────────────────────────
    public double CompactWidth { get; set; } = 360;
    public double CompactHeight { get; set; } = 72;
    public double ExpandedWidth { get; set; } = 400;
    public double MaxExpandedHeight { get; set; } = 384;

    // 自动调整：按组件内容自适应尺寸（默认开启；手动拖动滑杆会自动关闭对应项）
    public bool CompactWidthAuto { get; set; } = true;
    public bool CompactHeightAuto { get; set; } = true;
    public bool ExpandedWidthAuto { get; set; } = true;
    public bool MaxExpandedHeightAuto { get; set; } = true;

    // ── 迷你播放器（独立小窗口，始终置顶，随媒体自动显示/隐藏）──
    public bool MiniPlayerEnabled { get; set; } = false;
    public double? MiniPlayerLeft { get; set; } = null; // null = 尚未拖动过，使用自动定位
    public double? MiniPlayerTop { get; set; } = null;


    // ── Idle widgets（无媒体时组件）──────────────────────────
    public bool ShowWidgetsWhenNoMedia { get; set; } = false; // 无媒体时显示组件（旧开关）
    public bool WidgetShowTime { get; set; } = true;
    public bool WidgetShowWeather { get; set; } = false;
    public string WeatherCity { get; set; } = "";             // 天气城市（Open-Meteo，需联网）

    // ── 组件（灵动岛显示内容，Idle/Playing 可分别勾选）──
    public ComponentFlags Components { get; set; } = new();
    public string WidgetOrder { get; set; } = "Time,Weather"; // 组件摆放顺序（逗号分隔的键）

    // ── 媒体程序选择与顺序（空列表 = 全部启用，按默认优先级）──
    public List<MediaAppEntry> MediaApps { get; set; } = new();
    // ── Notifications ──────────────────────────────────────────
    public bool BluetoothNotifyEnabled { get; set; } = false;   // 蓝牙设备连接/断开提示
    public bool NetworkNotifyEnabled { get; set; } = true;       // 断网/网络恢复提示
    public bool NotificationTakeoverEnabled { get; set; } = false; // 接管 Windows 通知（尽力而为）
    public int NotificationTimeoutSeconds { get; set; } = 6;      // 横幅显示时长
    public string NotificationPosition { get; set; } = "TopRight"; // TopRight = 右上角
    public bool NotifyFoldEnabled { get; set; } = true;           // 通知折叠：同来源同标题只保留一条并累加数量
    // ── #10 通知历史（展开卡片底部列表，可点击重新弹出）──
    public bool NotificationHistoryEnabled { get; set; } = true; // 记录通知历史
    public int NotificationHistoryMax { get; set; } = 20;        // 通知历史条数上限（1~100）

    // ── 上岛 API（其他软件推送信息到灵动岛）──
    public bool IslandApiEnabled { get; set; } = true;           // 启用本地上岛 API
    public int IslandApiPort { get; set; } = 9840;               // 本地监听端口
    public string IslandApiToken { get; set; } = "";             // 可选 Token（防局域网误连）
    public int IslandApiDefaultDuration { get; set; } = 30;      // 默认显示时长（秒），推送方可按条覆盖

    // ── 显示规则（条件规则引擎）──
    public List<AppRule> Rules { get; set; } = new();   // 条件满足时自动隐藏/收起/强制显示

    // ── 外观增强（主题预设 / 字体 / 字号 / 圆角 / 封面取色）──
    public string ThemePreset { get; set; } = "Default";   // 皮肤预设：Default | Ocean | Forest | Sunset | Neon | Mono | Grape | Sky | Rose | Amber | Lime | Teal | Lavender | Crimson | Midnight | Coffee | Sakura | Aurora | Custom
    public string ThemeTint { get; set; } = "";
    // 「Custom」自定义皮肤的背景色（#RRGGBB，留空跟随明暗默认底色）
    public string FontFamily { get; set; } = "Segoe UI";   // 界面字体
    public double FontScale { get; set; } = 1.0;           // 字号缩放 0.8 ~ 1.4
    public double CornerRadius { get; set; } = 28;         // 胶囊圆角 16 ~ 40
    public bool CoverTintBackground { get; set; } = true;  // 展开背景随专辑封面取色
    public bool WallpaperThemeColorEnabled { get; set; } = false; // 壁纸取色：从当前壁纸提取主色作为主题色（纯本地，不联网）
    public bool MarqueeTextEnabled { get; set; } = false; // 跑马灯：歌名/歌词超宽时自动横向滚动

    // ── 波纹可视化（媒体按钮左侧，随声音/播放波动）──
    public bool WaveVisualizerEnabled { get; set; } = true;
    public string WaveStyle { get; set; } = "Bars";   // 波纹样式：Bars | Spectrum | Ring | Particles
    public bool WaveSyncEnabled { get; set; } = true;      // 跟随音乐节奏：采集系统输出声音驱动波纹
    public double WaveSensitivity { get; set; } = 1.0;     // 波纹灵敏度 0.2 ~ 3.0
    public double WaveHeight { get; set; } = 1.0;          // 波纹高度 0.4 ~ 1.6
    public bool NetCurveEnabled { get; set; } = true;      // 网络组件显示迷你曲线图（最近 32 秒）

    // ── 勿扰模式 ──
    public bool DoNotDisturbEnabled { get; set; } = false;   // 按时间段自动勿扰
    public bool DoNotDisturbManual { get; set; } = false;    // 手动开关勿扰
    public int DoNotDisturbStartHour { get; set; } = 22;
    public int DoNotDisturbEndHour { get; set; } = 8;
    public int DoNotDisturbStartMinute { get; set; } = 0;    // 定时勿扰开始分钟（0-55，步进 5）
    public int DoNotDisturbEndMinute { get; set; } = 0;      // 定时勿扰结束分钟（0-55，步进 5）
    public List<string> DnDAllowlist { get; set; } = new();   // 勿扰白名单：来源 exe/AppName（大小写不敏感），白名单内的来源仍弹横幅

    // ── 开会静音助手（会议检测 + 自动勿扰；纯本机启发式，不联网）──
    public bool MeetingAssistantEnabled { get; set; } = false;   // 总开关：检测会议（默认关，避免误报）
    public bool MeetingAutoDnd { get; set; } = true;             // 会议中自动开启勿扰（不弹通知横幅）
    public string MeetingKeywords { get; set; } = "";          // 自定义会议关键词（逗号分隔；留空用内置列表）

    // ── 屏幕录制 / 截图提示（默认关，避免打扰；PrintScreen 钩子 + 录制进程轮询）──
    public bool ScreenCaptureNotifyEnabled { get; set; } = false;  // 总开关
    public bool RecordingDndEnabled { get; set; } = false;      // 录屏时自动勿扰（不弹通知横幅）
    public bool ScreenshotNotifyEnabled { get; set; } = true;      // 按 PrintScreen 截图时提示
    public bool RecordingNotifyEnabled { get; set; } = true;       // 检测到录制软件时提示
    // ── 日历事件提醒（.ics 本地解析，默认关）──
    public bool CalendarEnabled { get; set; } = false;    // 总开关
    public string CalendarIcsPath { get; set; } = "";     // .ics 日历文件路径
    public int CalendarAdvanceMinutes { get; set; } = 10; // 提前提醒分钟
    // ── RSS 订阅 / 邮件提醒（默认关；仅用户开启后联网，不上报数据）──
    public bool RssNotifyEnabled { get; set; } = false;   // RSS 订阅总开关
    public string RssUrls { get; set; } = "";             // 逗号分隔的订阅地址（RSS 2.0 / Atom）
    public int RssIntervalMinutes { get; set; } = 15;     // 轮询间隔（分钟）
    public bool MailNotifyEnabled { get; set; } = false;  // 邮件提醒总开关（POP3）
    public string MailPop3Server { get; set; } = "";      // POP3 服务器
    public int MailPop3Port { get; set; } = 995;          // 端口（995 TLS / 110 明文）
    public bool MailUseSsl { get; set; } = true;          // 使用 SSL/TLS
    public string MailUser { get; set; } = "";            // 邮箱账号
    public string MailPassword { get; set; } = "";        // 密码/授权码（仅存本机配置）
    public int MailCheckMinutes { get; set; } = 5;        // 检查间隔（分钟）



    // ── 效率工具 ──
    public bool ClipboardHistoryEnabled { get; set; } = false;  // 剪贴板历史（默认关，随需开启）
    public int ClipboardHistoryMax { get; set; } = 15;
    public bool CopyToastEnabled { get; set; } = true;       // 复制文本时提示「已复制」
    public bool CodeToastEnabled { get; set; } = true;       // 识别短信验证码并高亮提示
    public bool CopyProgressEnabled { get; set; } = true;    // 大文本复制显示进度（估算）
    public int CopyProgressThreshold { get; set; } = 4000;   // 触发进度提示的最小字符数
    public bool PomodoroEnabled { get; set; } = false;          // 番茄钟/计时器
    public int PomodoroWorkMinutes { get; set; } = 25;
    public int PomodoroBreakMinutes { get; set; } = 5;
    public bool AutoUpdateCheck { get; set; } = false;          // 自动检查 GitHub 新版本（默认关，需联网）
    public int KeyIndicatorSeconds { get; set; } = 3;              // 键盘指示灯出现时长（秒）

    // ── 1.0.10 新增：临时上岛 / 合并胶囊 ──
    public bool VolumeTempIndicatorEnabled { get; set; } = true;   // 音量/静音临时上岛（调节音量后自动消失）
    public int VolumeTempIndicatorSeconds { get; set; } = 4;       // 音量指示显示时长（秒）
    public bool FileCopyNotifyEnabled { get; set; } = true;        // 文件复制/移动进行中上岛
    public bool DownloadProgressEnabled { get; set; } = false;     // 下载进行中上岛（默认关）
    public bool UsageMergeEnabled { get; set; } = false;           // 「使用中」合并胶囊（默认关）
    public List<string> UsageMergeItems { get; set; } = new() { "Mic", "Cam", "Meeting", "Recording" }; // 参与合并的组件

    /// <summary>合并胶囊中是否包含麦克风（方便设置界面勾选绑定）。</summary>
    public bool UsageMergeMic
    {
        get => UsageMergeItems.Contains("Mic");
        set { if (value && !UsageMergeItems.Contains("Mic")) UsageMergeItems.Add("Mic"); else if (!value) UsageMergeItems.Remove("Mic"); }
    }

    /// <summary>合并胶囊中是否包含摄像头。</summary>
    public bool UsageMergeCam
    {
        get => UsageMergeItems.Contains("Cam");
        set { if (value && !UsageMergeItems.Contains("Cam")) UsageMergeItems.Add("Cam"); else if (!value) UsageMergeItems.Remove("Cam"); }
    }

    /// <summary>合并胶囊中是否包含会议中。</summary>
    public bool UsageMergeMeeting
    {
        get => UsageMergeItems.Contains("Meeting");
        set { if (value && !UsageMergeItems.Contains("Meeting")) UsageMergeItems.Add("Meeting"); else if (!value) UsageMergeItems.Remove("Meeting"); }
    }

    /// <summary>合并胶囊中是否包含录屏中。</summary>
    public bool UsageMergeRecording
    {
        get => UsageMergeItems.Contains("Recording");
        set { if (value && !UsageMergeItems.Contains("Recording")) UsageMergeItems.Add("Recording"); else if (!value) UsageMergeItems.Remove("Recording"); }
    }


    public string ActiveProfile { get; set; } = "Default";   // 当前配置档案名

    public AppSettings Clone()
    {
        var c = (AppSettings)MemberwiseClone();
        c.DnDAllowlist = new List<string>(DnDAllowlist);
        c.UsageMergeItems = new List<string>(UsageMergeItems);
        c.QuickActions = new List<string>(QuickActions);
        c.QuickActionsShown = new List<string>(QuickActionsShown);
        c.CallNotifyApps = new List<string>(CallNotifyApps);
        c.MediaApps = new List<MediaAppEntry>(MediaApps);
        c.Rules = Rules.Where(r => r is not null).Select(r => new AppRule
        {
            Enabled = r.Enabled, Name = r.Name, Condition = r.Condition,
            StartHour = r.StartHour, EndHour = r.EndHour, AppMatch = r.AppMatch, Action = r.Action,
        }).ToList();
        return c;
    }
}

/// <summary>Loads / saves <see cref="AppSettings"/> as JSON.</summary>
public sealed class SettingsService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly object _gate = new();
    private AppSettings _settings;

    public SettingsService()
    {
        AppPaths.EnsureDirectories();
        _settings = Load();
    }

    public AppSettings Current => _settings;

    public event EventHandler<AppSettings>? Changed;

    private AppSettings Load()
    {
        try
        {
            if (File.Exists(AppPaths.SettingsFile))
            {
                var json = File.ReadAllText(AppPaths.SettingsFile);
                var loaded = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
                if (loaded is not null)
                {
                    // 兼容旧配置：补齐新增字段
                    loaded.Components ??= new ComponentFlags();
                    return loaded;
                }
            }
        }
        catch (Exception ex)
        {
            AppLogger.Error("Failed to read settings; using defaults", ex);
        }

        return new AppSettings();
    }

    public void Save()
    {
        lock (_gate)
        {
            try
            {
                AppPaths.EnsureDirectories();
                var json = JsonSerializer.Serialize(_settings, JsonOptions);
                var tmp = AppPaths.SettingsFile + ".tmp";
                File.WriteAllText(tmp, json);
                File.Move(tmp, AppPaths.SettingsFile, overwrite: true);
            }
            catch (Exception ex)
            {
                AppLogger.Error("Failed to save settings", ex);
            }
        }
    }

    /// <summary>Replace settings with <paramref name="next"/> (e.g. from the settings UI), save and notify.</summary>
    public void Apply(AppSettings next)
    {
        lock (_gate)
        {
            _settings = next;
            Save();
        }
        Changed?.Invoke(this, _settings);
    }

    public void Update(Action<AppSettings> mutate)
    {
        lock (_gate)
        {
            mutate(_settings);
            Save();
        }
        Changed?.Invoke(this, _settings);
    }

    /// <summary>Export current settings as JSON text.</summary>
    public string Export() => JsonSerializer.Serialize(_settings, JsonOptions);

    /// <summary>Import settings from JSON text. Returns false if invalid.</summary>
    public bool TryImport(string json)
    {
        try
        {
            var parsed = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
            if (parsed is null) return false;
            Apply(parsed);
            return true;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Import failed: {ex.Message}");
            return false;
        }
    }
}



