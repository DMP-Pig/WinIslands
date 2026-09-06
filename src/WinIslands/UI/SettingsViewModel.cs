using WinIslands.Services;

namespace WinIslands.UI;

public sealed record EnumOption<T>(T Value, string Display);

/// <summary>组件设置的一行：名称 + 空闲/播放两列勾选。</summary>
/// <summary>媒体程序配置行：是否启用 + 顺序。</summary>
public sealed class MediaAppRow : ObservableObject
{
    private readonly Action<MediaAppRow> _onChanged;
    public string Key { get; }
    public string Name { get; }
    private bool _enabled;
    public bool Enabled
    {
        get => _enabled;
        set { if (Set(ref _enabled, value)) _onChanged(this); }
    }
    public MediaAppRow(string key, string name, bool enabled, Action<MediaAppRow> onChanged)
    {
        Key = key; Name = name; _enabled = enabled; _onChanged = onChanged;
    }
}
/// <summary>顺序条里的一个组件（含歌曲信息）。</summary>
public sealed class OrderItem : ObservableObject
{
    private readonly string _nameKey;
    public string Key { get; }
    public string Name => Localization.Get(_nameKey);
    public OrderItem(string key, string nameKey) { Key = key; _nameKey = nameKey; }
    public void RefreshName() => OnPropertyChanged(nameof(Name));
}
public sealed class ComponentRow : ObservableObject
{
    private readonly string _nameKey;
    private readonly ComponentFlags _c;
    private readonly Func<ComponentFlags, bool> _idleGet;
    private readonly Action<ComponentFlags, bool> _idleSet;
    private readonly Func<ComponentFlags, bool> _playGet;
    private readonly Action<ComponentFlags, bool> _playSet;
    private readonly Func<AppSettings> _settingsGet;
    private readonly Action<ComponentRow> _onChanged;

    public string Key { get; }

    public ComponentRow(string key, string nameKey, ComponentFlags c,
        Func<ComponentFlags, bool> idleGet, Action<ComponentFlags, bool> idleSet,
        Func<ComponentFlags, bool> playGet, Action<ComponentFlags, bool> playSet,
        Func<AppSettings> settingsGet, Action<ComponentRow>? onChanged = null)
    {
        Key = key; _nameKey = nameKey; _c = c;
        _idleGet = idleGet; _idleSet = idleSet;
        _playGet = playGet; _playSet = playSet;
        _settingsGet = settingsGet;
        _onChanged = onChanged ?? (_ => { });
    }


    public string Name => Localization.Get(_nameKey);

    public bool Idle { get => _idleGet(_c); set { _idleSet(_c, value); OnPropertyChanged(); _onChanged(this); } }
    public bool Playing { get => _playGet(_c); set { _playSet(_c, value); OnPropertyChanged(); _onChanged(this); } }

    /// <summary>该组件是否支持图标定制（有默认图标才支持）。</summary>
    public bool SupportsIcon => ComponentIcons.SupportsIcon(Key);

    /// <summary>组件显示图标（写入用户自定义图标字典；清空则恢复默认）。</summary>
    public string Icon
    {
        get
        {
            var icons = _settingsGet().ComponentIcons;
            return icons is not null && icons.TryGetValue(Key, out var v) ? v.Trim() : ComponentIcons.Default(Key);
        }
        set
        {
            var icons = _settingsGet().ComponentIcons;
            if (icons is null) return;
            var trimmed = (value ?? string.Empty).Trim();
            if (trimmed.Length == 0) icons.Remove(Key);
            else icons[Key] = trimmed;
            OnPropertyChanged();
            _onChanged(this);
        }
    }

    /// <summary>图标输入提示（默认字形说明）。</summary>
    public string IconPlaceholder => SupportsIcon ? ComponentIcons.Default(Key) : string.Empty;

    public void RefreshName() => OnPropertyChanged(nameof(Name));
}

/// <summary>规则引擎设置页的一行（直接编辑 Working.Rules 里的 AppRule，便于即时生效）。</summary>
public sealed class RuleRow : ObservableObject
{
    private readonly AppRule _rule;

    public AppRule Rule => _rule;
    public IReadOnlyList<EnumOption<RuleCondition>> ConditionOptions { get; } = new[]
    {
        new EnumOption<RuleCondition>(RuleCondition.Always, Localization.Get("Rules_Cond_Always")),
        new EnumOption<RuleCondition>(RuleCondition.NoMedia, Localization.Get("Rules_Cond_NoMedia")),
        new EnumOption<RuleCondition>(RuleCondition.MediaPlaying, Localization.Get("Rules_Cond_MediaPlaying")),
        new EnumOption<RuleCondition>(RuleCondition.TimeRange, Localization.Get("Rules_Cond_TimeRange")),
        new EnumOption<RuleCondition>(RuleCondition.AppPlaying, Localization.Get("Rules_Cond_AppPlaying")),
    };
    public IReadOnlyList<EnumOption<RuleAction>> ActionOptions { get; } = new[]
    {
        new EnumOption<RuleAction>(RuleAction.Hide, Localization.Get("Rules_Act_Hide")),
        new EnumOption<RuleAction>(RuleAction.Collapse, Localization.Get("Rules_Act_Collapse")),
        new EnumOption<RuleAction>(RuleAction.ForceShow, Localization.Get("Rules_Act_ForceShow")),
    };
    public IReadOnlyList<int> Hours { get; } = Enumerable.Range(0, 24).ToList();

    public RuleRow(AppRule rule) => _rule = rule;

    public string Name { get => _rule.Name; set { _rule.Name = value; OnPropertyChanged(); } }
    public bool Enabled { get => _rule.Enabled; set { _rule.Enabled = value; OnPropertyChanged(); } }
    public RuleCondition Condition
    {
        get => _rule.Condition;
        set
        {
            if (_rule.Condition == value) return;
            _rule.Condition = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsTimeRange));
            OnPropertyChanged(nameof(IsAppMatch));
        }
    }
    public bool IsTimeRange => Condition == RuleCondition.TimeRange;
    public bool IsAppMatch => Condition == RuleCondition.AppPlaying;
    public int StartHour { get => _rule.StartHour; set { _rule.StartHour = value; OnPropertyChanged(); } }
    public int EndHour { get => _rule.EndHour; set { _rule.EndHour = value; OnPropertyChanged(); } }
    public string AppMatch { get => _rule.AppMatch; set { _rule.AppMatch = value; OnPropertyChanged(); } }
    public RuleAction Action { get => _rule.Action; set { _rule.Action = value; OnPropertyChanged(); } }
}

/// <summary>快捷操作设置行：勾选显示 + ↑↓ 调整顺序（顺序即显示顺序）。</summary>
public sealed class QuickActionRow : ObservableObject
{
    private readonly SettingsViewModel _owner;
    public string Key { get; }
    public string Label { get; }

    public QuickActionRow(SettingsViewModel owner, string key)
    {
        _owner = owner;
        Key = key;
        Label = Localization.Get("QA_" + key);
    }

    /// <summary>是否勾选显示（同步到 Working.QuickActionsEnabled）。</summary>
    public bool IsChecked
    {
        get => _owner.Working.QuickActionsShown?.Contains(Key) == true;
        set
        {
            var list = _owner.Working.QuickActionsShown ??= new List<string>();
            if (value && !list.Contains(Key)) list.Add(Key);
            else if (!value) list.Remove(Key);
            OnPropertyChanged();
            _owner.NotifyQuickActionRowsChanged();
        }
    }

    public void MoveUp() => _owner.MoveQuickAction(Key, -1);
    public void MoveDown() => _owner.MoveQuickAction(Key, 1);
    public void RefreshName() => OnPropertyChanged(nameof(Label));
}

/// <summary>View model for the settings window. Edits a working copy, saves on demand.</summary>
public sealed class SettingsViewModel : ObservableObject
{
    private readonly SettingsService _service;

    public SettingsViewModel(SettingsService service, MediaAppRegistry? registry = null)
    {
        _service = service;
        Working = service.Current.Clone();
        Language = Working.Language;

        ThemeOptions = new[]
        {
            new EnumOption<ThemeMode>(ThemeMode.Auto, Localization.Get("Appearance_ThemeAuto")),
            new EnumOption<ThemeMode>(ThemeMode.Light, Localization.Get("Appearance_ThemeLight")),
            new EnumOption<ThemeMode>(ThemeMode.Dark, Localization.Get("Appearance_ThemeDark")),
        };
        PositionOptions = new[]
        {
            new EnumOption<IslandPosition>(IslandPosition.Center, Localization.Get("Appearance_PositionCenter")),
            new EnumOption<IslandPosition>(IslandPosition.Right, Localization.Get("Appearance_PositionRight")),
        };
        MonitorOptions = new[]
        {
            new EnumOption<MonitorSelection>(MonitorSelection.Primary, Localization.Get("Appearance_MonitorPrimary")),
            new EnumOption<MonitorSelection>(MonitorSelection.All, Localization.Get("Appearance_MonitorAll")),
            new EnumOption<MonitorSelection>(MonitorSelection.Index, Localization.Get("Appearance_MonitorIndex")),
        };
        ThemePresetOptions = new[]
        {
            new EnumOption<string>("Default", Localization.Get("Appearance_PresetDefault")),
            new EnumOption<string>("Ocean", Localization.Get("Appearance_PresetOcean")),
            new EnumOption<string>("Forest", Localization.Get("Appearance_PresetForest")),
            new EnumOption<string>("Sunset", Localization.Get("Appearance_PresetSunset")),
            new EnumOption<string>("Neon", Localization.Get("Appearance_PresetNeon")),
            new EnumOption<string>("Mono", Localization.Get("Appearance_PresetMono")),
            new EnumOption<string>("Grape", Localization.Get("Appearance_PresetGrape")),
            new EnumOption<string>("Sky", Localization.Get("Appearance_PresetSky")),
            new EnumOption<string>("Rose", Localization.Get("Appearance_PresetRose")),
            new EnumOption<string>("Amber", Localization.Get("Appearance_PresetAmber")),
            new EnumOption<string>("Lime", Localization.Get("Appearance_PresetLime")),
            new EnumOption<string>("Teal", Localization.Get("Appearance_PresetTeal")),
            new EnumOption<string>("Lavender", Localization.Get("Appearance_PresetLavender")),
            new EnumOption<string>("Crimson", Localization.Get("Appearance_PresetCrimson")),
            new EnumOption<string>("Midnight", Localization.Get("Appearance_PresetMidnight")),
            new EnumOption<string>("Coffee", Localization.Get("Appearance_PresetCoffee")),
            new EnumOption<string>("Sakura", Localization.Get("Appearance_PresetSakura")),
            new EnumOption<string>("Aurora", Localization.Get("Appearance_PresetAurora")),
            new EnumOption<string>("Custom", Localization.Get("Appearance_PresetCustom")),
        };
        WaveStyleOptions = new[]
        {
            new EnumOption<string>("Bars", Localization.Get("Wave_StyleBars")),
            new EnumOption<string>("Spectrum", Localization.Get("Wave_StyleSpectrum")),
            new EnumOption<string>("Ring", Localization.Get("Wave_StyleRing")),
            new EnumOption<string>("Particles", Localization.Get("Wave_StyleParticles")),
        };
        LyricsSourceOptions = new[]
        {
            new EnumOption<string>("Auto", Localization.Get("Lyrics_SrcAuto")),
            new EnumOption<string>("Local", Localization.Get("Lyrics_SrcLocal")),
            new EnumOption<string>("Amll", Localization.Get("Lyrics_SrcAmll")),
            new EnumOption<string>("Cider", Localization.Get("Lyrics_SrcCider")),
            new EnumOption<string>("Online", Localization.Get("Lyrics_SrcOnline")),
        };
        PushAnimationStyleOptions = new[]
        {
            new EnumOption<string>("Spring", Localization.Get("Appearance_PushAnimSpring")),
            new EnumOption<string>("Fade", Localization.Get("Appearance_PushAnimFade")),
            new EnumOption<string>("Slide", Localization.Get("Appearance_PushAnimSlide")),
            new EnumOption<string>("Scale", Localization.Get("Appearance_PushAnimScale")),
        };
        AnimationStyleOptions = new[]
        {
            new EnumOption<string>("Spring", Localization.Get("Appearance_AnimSpring")),
            new EnumOption<string>("Soft", Localization.Get("Appearance_AnimSoft")),
            new EnumOption<string>("Elastic", Localization.Get("Appearance_AnimElastic")),
            new EnumOption<string>("Fade", Localization.Get("Appearance_AnimFade")),
        };
        _components = BuildComponents(Working.Components);
        _orderItems = new List<OrderItem>();
        RebuildOrderItems();
        _mediaAppRows = BuildMediaApps(Working.MediaApps, registry);
        _ruleRows = (Working.Rules ?? new List<AppRule>()).Select(r => new RuleRow(r)).ToList();
        RebuildQuickActionRows();
        Localization.LanguageChanged += (_, _) =>
        {
            foreach (var r in Components) r.RefreshName();
            foreach (var o in OrderItems) o.RefreshName();
            foreach (var q in _quickActionRows) q.RefreshName();
        };

        PresetColors = new[]
        {
            "#6C5CE7", "#5B8DEF", "#00B894", "#E17055", "#E84393", "#FDCB6E",
            "#00CEC9", "#A29BFE", "#FD79A8", "#55EFC4", "#74B9FF", "#DFE6E9",
        };
    }

    public AppSettings Working { get; }

    public string Language
    {
        get => Working.Language;
        set
        {
            Working.Language = value;
            OnPropertyChanged();
        }
    }

    /// <summary>勿扰白名单（逗号分隔的 exe 名 / 应用名文本）；白名单来源不受勿扰影响。</summary>
    public string DnDAllowlistText
    {
        get => string.Join(", ", Working.DnDAllowlist);
        set
        {
            Working.DnDAllowlist = (value ?? string.Empty)
                .Split(new[] { ',', '，', ';', '；' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .Distinct(System.StringComparer.OrdinalIgnoreCase)
                .ToList();
            OnPropertyChanged();
        }
    }

    /// <summary>来电提醒检测的应用（进程名，逗号分隔）。</summary>
    public string CallNotifyAppsText
    {
        get => string.Join(", ", Working.CallNotifyApps ?? new List<string>());
        set
        {
            Working.CallNotifyApps = (value ?? string.Empty)
                .Split(new[] { ',', '，', ';', '；' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .Distinct(System.StringComparer.OrdinalIgnoreCase)
                .ToList();
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<EnumOption<ThemeMode>> ThemeOptions { get; }
    public IReadOnlyList<EnumOption<IslandPosition>> PositionOptions { get; }
    public IReadOnlyList<EnumOption<MonitorSelection>> MonitorOptions { get; }
    public IReadOnlyList<EnumOption<string>> ThemePresetOptions { get; } = Array.Empty<EnumOption<string>>();
    public IReadOnlyList<EnumOption<string>> AnimationStyleOptions { get; } = Array.Empty<EnumOption<string>>();
    public IReadOnlyList<EnumOption<string>> PushAnimationStyleOptions { get; } = Array.Empty<EnumOption<string>>();
    public IReadOnlyList<EnumOption<string>> LyricsSourceOptions { get; } = Array.Empty<EnumOption<string>>();
    public IReadOnlyList<EnumOption<string>> WaveStyleOptions { get; } = Array.Empty<EnumOption<string>>();
    public IReadOnlyList<string> PresetColors { get; }
    public IReadOnlyList<ComponentRow> Components => _components;
    public IReadOnlyList<OrderItem> OrderItems => _orderItems;
    public IReadOnlyList<MediaAppRow> MediaAppRows => _mediaAppRows;
    private List<RuleRow> _ruleRows = new();
    public IReadOnlyList<RuleRow> RuleRows => _ruleRows;

    private List<QuickActionRow> _quickActionRows = new();
    /// <summary>快捷操作设置行（全部可用操作，顺序可调，勾选控制是否显示）。</summary>
    public IReadOnlyList<QuickActionRow> QuickActionRows => _quickActionRows;

    private static readonly string[] AllQuickActions =
    {
        "Lock", "Mute", "PlayPause", "Screenshot", "Settings",
        "Desktop", "TaskManager", "Calculator", "Sleep", "VolumeUp", "VolumeDown",
    };

    private void RebuildQuickActionRows()
    {
        var order = Working.QuickActions ??= new List<string>();
        foreach (var k in AllQuickActions) if (!order.Contains(k)) order.Add(k);
        Working.QuickActions = order;
        Working.QuickActionsShown ??= new List<string>();
        _quickActionRows = order.Select(k => new QuickActionRow(this, k)).ToList();
        OnPropertyChanged(nameof(QuickActionRows));
    }

    /// <summary>在设置列表中上移/下移一个操作（顺序即灵动岛显示顺序）。</summary>
    public void MoveQuickAction(string key, int delta)
    {
        var list = Working.QuickActions ??= new List<string>();
        var idx = list.IndexOf(key);
        var target = idx + delta;
        if (idx < 0 || target < 0 || target >= list.Count) return;
        list.RemoveAt(idx);
        list.Insert(target, key);
        RebuildQuickActionRows();
    }

    /// <summary>快捷操作行勾选变化时通知列表刷新。</summary>
    public void NotifyQuickActionRowsChanged() => OnPropertyChanged(nameof(QuickActionRows));

    /// <summary>添加一条默认规则。</summary>
    public void AddRule()
    {
        var rule = new AppRule { Name = Localization.Get("Rules_NewName"), Condition = RuleCondition.TimeRange };
        Working.Rules ??= new List<AppRule>();
        Working.Rules.Add(rule);
        _ruleRows.Add(new RuleRow(rule));
        OnPropertyChanged(nameof(RuleRows));
    }

    /// <summary>删除一条规则。</summary>
    public void RemoveRule(RuleRow row)
    {
        if (row is null || !_ruleRows.Remove(row)) return;
        Working.Rules?.Remove(row.Rule);
        OnPropertyChanged(nameof(RuleRows));
    }

    private List<MediaAppRow> _mediaAppRows = new();

    private List<ComponentRow> _components = new();
    private List<OrderItem> _orderItems = new();

    private List<ComponentRow> BuildComponents(ComponentFlags c)
    {
        Func<AppSettings> sget = () => Working;
        return new()
        {
        new("Time", "Comp_Time", c, x => x.TimeWhenIdle, (x, v) => x.TimeWhenIdle = v, x => x.TimeWhenPlaying, (x, v) => x.TimeWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Weather", "Comp_Weather", c, x => x.WeatherWhenIdle, (x, v) => x.WeatherWhenIdle = v, x => x.WeatherWhenPlaying, (x, v) => x.WeatherWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Date", "Comp_Date", c, x => x.DateWhenIdle, (x, v) => x.DateWhenIdle = v, x => x.DateWhenPlaying, (x, v) => x.DateWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Cpu", "Comp_Cpu", c, x => x.CpuWhenIdle, (x, v) => x.CpuWhenIdle = v, x => x.CpuWhenPlaying, (x, v) => x.CpuWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Ram", "Comp_Ram", c, x => x.RamWhenIdle, (x, v) => x.RamWhenIdle = v, x => x.RamWhenPlaying, (x, v) => x.RamWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Gpu", "Comp_Gpu", c, x => x.GpuWhenIdle, (x, v) => x.GpuWhenIdle = v, x => x.GpuWhenPlaying, (x, v) => x.GpuWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Mic", "Comp_Mic", c, x => x.MicWhenIdle, (x, v) => x.MicWhenIdle = v, x => x.MicWhenPlaying, (x, v) => x.MicWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Cam", "Comp_Cam", c, x => x.CamWhenIdle, (x, v) => x.CamWhenIdle = v, x => x.CamWhenPlaying, (x, v) => x.CamWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Net", "Comp_Net", c, x => x.NetWhenIdle, (x, v) => x.NetWhenIdle = v, x => x.NetWhenPlaying, (x, v) => x.NetWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Battery", "Comp_Battery", c, x => x.BatteryWhenIdle, (x, v) => x.BatteryWhenIdle = v, x => x.BatteryWhenPlaying, (x, v) => x.BatteryWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("CapsLock", "Comp_CapsLock", c, x => x.CapsLockWhenIdle, (x, v) => x.CapsLockWhenIdle = v, x => x.CapsLockWhenPlaying, (x, v) => x.CapsLockWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Clipboard", "Comp_Clipboard", c, x => x.ClipboardWhenIdle, (x, v) => x.ClipboardWhenIdle = v, x => x.ClipboardWhenPlaying, (x, v) => x.ClipboardWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Todo", "Comp_Todo", c, x => x.TodoWhenIdle, (x, v) => x.TodoWhenIdle = v, x => x.TodoWhenPlaying, (x, v) => x.TodoWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Timer", "Comp_Timer", c, x => x.TimerWhenIdle, (x, v) => x.TimerWhenIdle = v, x => x.TimerWhenPlaying, (x, v) => x.TimerWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Schedule", "Comp_Schedule", c, x => x.ScheduleWhenIdle, (x, v) => x.ScheduleWhenIdle = v, x => x.ScheduleWhenPlaying, (x, v) => x.ScheduleWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Holiday", "Comp_Holiday", c, x => x.HolidayWhenIdle, (x, v) => x.HolidayWhenIdle = v, x => x.HolidayWhenPlaying, (x, v) => x.HolidayWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Meeting", "Comp_Meeting", c, x => x.MeetingWhenIdle, (x, v) => x.MeetingWhenIdle = v, x => x.MeetingWhenPlaying, (x, v) => x.MeetingWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("Disk", "Comp_Disk", c, x => x.DiskWhenIdle, (x, v) => x.DiskWhenIdle = v, x => x.DiskWhenPlaying, (x, v) => x.DiskWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("InputMethod", "Comp_InputMethod", c, x => x.InputMethodWhenIdle, (x, v) => x.InputMethodWhenIdle = v, x => x.InputMethodWhenPlaying, (x, v) => x.InputMethodWhenPlaying = v, sget, _ => RebuildOrderItems()),
        new("QuickToggles", "Comp_QuickToggles", c, x => x.QuickTogglesWhenIdle, (x, v) => x.QuickTogglesWhenIdle = v, x => x.QuickTogglesWhenPlaying, (x, v) => x.QuickTogglesWhenPlaying = v, sget, _ => RebuildOrderItems()),
        };
    }

    private static readonly (string Key, string NameKey)[] OrderDefs =
    {
        ("Time", "Comp_Time"),
        ("Weather", "Comp_Weather"),
        ("Date", "Comp_Date"),
        ("Cpu", "Comp_Cpu"),
        ("Ram", "Comp_Ram"),
        ("Gpu", "Comp_Gpu"),
        ("Mic", "Comp_Mic"),
        ("Cam", "Comp_Cam"),
        ("Net", "Comp_Net"),
        ("Battery", "Comp_Battery"),
        ("CapsLock", "Comp_CapsLock"),
        ("Clipboard", "Comp_Clipboard"),
        ("Todo", "Comp_Todo"),
        ("Timer", "Comp_Timer"),
        ("Schedule", "Comp_Schedule"),
        ("Holiday", "Comp_Holiday"),
        ("Meeting", "Comp_Meeting"),
        ("Disk", "Comp_Disk"),
        ("InputMethod", "Comp_InputMethod"),
        ("QuickToggles", "Comp_QuickToggles"),
        ("Song", "Comp_Song"),
    };

    /// <summary>
    /// 重建「拖动顺序」条：只保留已勾选（空闲或播放任一勾选）的组件；
    /// 没有勾选行的组件（如 Song 歌曲信息）始终显示。顺序沿用 WidgetOrder。
    /// </summary>
    private void RebuildOrderItems()
    {
        var keys = (Working.WidgetOrder ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        foreach (var d in OrderDefs) if (!keys.Contains(d.Key)) keys.Add(d.Key);

        var used = new HashSet<string>();
        var result = new List<OrderItem>();
        foreach (var k in keys)
        {
            var d = Array.Find(OrderDefs, x => x.Key == k);
            if (d.Key is null || !used.Add(d.Key)) continue;
            var row = _components.FirstOrDefault(c => c.Key == k);
            // 无勾选行的组件（Song）始终显示；其余只有被勾选才显示
            if (row is null || row.Idle || row.Playing)
                result.Add(new OrderItem(d.Key, d.NameKey));
        }
        _orderItems = result;
        OnPropertyChanged(nameof(OrderItems));
    }

    private List<MediaAppRow> BuildMediaApps(List<MediaAppEntry>? saved, MediaAppRegistry? registry)
    {
        var result = new List<MediaAppRow>();
        void Add(string key, string name, bool enabled)
        {
            if (result.Any(r => r.Key == key)) return;
            result.Add(new MediaAppRow(key, name, enabled, _ => SyncMediaApps()));
        }

        // 先按已保存的顺序/启用
        if (saved is not null)
            foreach (var e in saved) Add(e.Key, e.Key, e.Enabled);
        // 再补上运行中发现的程序（默认启用）
        if (registry is not null)
            foreach (var (key, name) in registry.Known) Add(key, name, true);
        return result;
    }

    private void SyncMediaApps()
    {
        Working.MediaApps = _mediaAppRows
            .Select(r => new MediaAppEntry { Key = r.Key, Enabled = r.Enabled })
            .ToList();
    }

    /// <summary>调整媒体程序顺序（优先级）。</summary>
    public void MoveMediaApp(MediaAppRow row, int delta)
    {
        var i = _mediaAppRows.IndexOf(row);
        var j = i + delta;
        if (i < 0 || j < 0 || j >= _mediaAppRows.Count) return;
        // 必须返回新 List 实例，ItemsSource 引用变化才会触发 UI 刷新
        var list = new List<MediaAppRow>(_mediaAppRows);
        (list[i], list[j]) = (list[j], list[i]);
        _mediaAppRows = list;
        OnPropertyChanged(nameof(MediaAppRows));
        SyncMediaApps();
    }

    public void MoveOrderItemTo(OrderItem item, int newIndex)
    {
        var i = _orderItems.IndexOf(item);
        if (i < 0) return;
        newIndex = Math.Clamp(newIndex, 0, _orderItems.Count - 1);
        if (i == newIndex) return;
        // 必须返回新 List 实例，ItemsSource 引用变化才会触发 UI 刷新（顺序条可见变化）
        var list = new List<OrderItem>(_orderItems);
        list.RemoveAt(i);
        list.Insert(newIndex, item);
        _orderItems = list;
        OnPropertyChanged(nameof(OrderItems));

        // 把新顺序写回 WidgetOrder：已显示项按新顺序在前，未勾选项按原顺序补在后面，
        // 这样取消勾选再勾选后仍能记住相对位置。
        var shown = _orderItems.Select(x => x.Key).ToList();
        var hidden = _components.Where(r => !r.Idle && !r.Playing).Select(r => r.Key)
            .Concat(OrderDefs.Select(d => d.Key)).Distinct()
            .Where(k => !shown.Contains(k)).ToList();
        Working.WidgetOrder = string.Join(",", shown.Concat(hidden));
    }

    public void MoveComponentTo(ComponentRow row, int newIndex)
    {
        var i = _components.IndexOf(row);
        if (i < 0) return;
        newIndex = Math.Clamp(newIndex, 0, _components.Count - 1);
        if (i == newIndex) return;
        _components.RemoveAt(i);
        _components.Insert(newIndex, row);
        OnPropertyChanged(nameof(Components));
        Working.WidgetOrder = string.Join(",", _components.Select(x => x.Key));
    }

    public void Save()
    {
        // 位置/偏移/显示器变化时清除手动拖动位置，让默认定位规则重新生效
        var prev = _service.Current;
        if (prev.Position != Working.Position ||
            Math.Abs(prev.OffsetX - Working.OffsetX) > 0.001 ||
            Math.Abs(prev.OffsetY - Working.OffsetY) > 0.001 ||
            prev.Monitor != Working.Monitor ||
            prev.MonitorIndex != Working.MonitorIndex)
        {
            Working.IslandManualLeft = null;
            Working.IslandManualTop = null;
        }
        _service.Apply(Working);
        Localization.CurrentLanguage = Working.Language;
    }
}

