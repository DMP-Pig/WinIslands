using System.Text.Json;
using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>配置 JSON 往返与勿扰判定。</summary>
public class SettingsAndDndTests : IDisposable
{
    private readonly string _origAppData;
    private readonly string _dir;

    public SettingsAndDndTests()
    {
        _origAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _dir = Path.Combine(Path.GetTempPath(), "WinIslandSettings-" + Guid.NewGuid().ToString("N"));
        Environment.SetEnvironmentVariable("WINISLAND_APPDATA", _dir);
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("WINISLAND_APPDATA", null);
        Environment.SetEnvironmentVariable("APPDATA", _origAppData);
        try { Directory.Delete(_dir, recursive: true); } catch { /* ignore */ }
    }

    private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() } };

    [Fact]
    public void Settings_RoundTrip_Preserves_Values()
    {
        var s = new AppSettings
        {
            Theme = ThemeMode.Dark,
            Position = IslandPosition.Right,
            Monitor = MonitorSelection.Index,
            MonitorIndex = 1,
            OffsetX = 12.5,
            IslandManualLeft = 123.4,
            IslandManualTop = null,
            CompactWidth = 388,
            CompactWidthAuto = false,
            LyricTimeOffsets = new Dictionary<string, double> { ["abc"] = -1.5 },
            ComponentIcons = new Dictionary<string, string> { ["Time"] = "\uE823" },
            DnDAllowlist = new List<string> { "QQ.exe", "Bluetooth" },
            UsageMergeItems = new List<string> { "Mic", "Cam" },
            MediaApps = new List<MediaAppEntry> { new() { Key = "Spotify.exe", Enabled = false } },
            Rules = new List<AppRule>
            {
                new() { Name = "晚上隐藏", Condition = RuleCondition.TimeRange, StartHour = 22, EndHour = 8, Action = RuleAction.Hide },
            },
            Components = new ComponentFlags { TimeWhenIdle = true, LyricsWhenPlaying = true },
        };

        var json = JsonSerializer.Serialize(s, JsonOpts);
        var back = JsonSerializer.Deserialize<AppSettings>(json, JsonOpts);

        Assert.NotNull(back);
        Assert.Equal(ThemeMode.Dark, back!.Theme);
        Assert.Equal(IslandPosition.Right, back.Position);
        Assert.Equal(MonitorSelection.Index, back.Monitor);
        Assert.Equal(1, back.MonitorIndex);
        Assert.Equal(12.5, back.OffsetX);
        Assert.Equal(123.4, back.IslandManualLeft);
        Assert.Null(back.IslandManualTop);
        Assert.Equal(388, back.CompactWidth);
        Assert.False(back.CompactWidthAuto);
        Assert.Equal(-1.5, back.LyricTimeOffsets["abc"]);
        Assert.Equal("\uE823", back.ComponentIcons["Time"]);
        Assert.Contains("QQ.exe", back.DnDAllowlist);
        Assert.Contains("Mic", back.UsageMergeItems);
        Assert.Single(back.MediaApps);
        Assert.False(back.MediaApps[0].Enabled);
        Assert.Single(back.Rules);
        Assert.Equal(RuleAction.Hide, back.Rules[0].Action);
        Assert.True(back.Components.TimeWhenIdle);
        Assert.True(back.Components.LyricsWhenPlaying);
    }

    [Fact]
    public void Missing_Fields_Use_Defaults_And_Do_Not_Crash()
    {
        // 旧配置没有新字段 → 反序列化后默认值可用（不抛 NullReference）
        var json = """{"Theme":"Auto","Position":"Center"}""";
        var s = JsonSerializer.Deserialize<AppSettings>(json, JsonOpts)!;
        Assert.NotNull(s.Components);
        Assert.Empty(s.MediaApps);
        Assert.Empty(s.Rules);
        Assert.Empty(s.LyricTimeOffsets);
        Assert.True(s.CompactWidthAuto);
        Assert.Equal(360, s.CompactWidth);
    }

    [Fact]
    public void Dnd_Manual_And_Whitelist()
    {
        var s = new AppSettings { DoNotDisturbManual = true };
        Assert.True(DoNotDisturb.IsActive(s, null));
        // 白名单来源不受手动勿扰影响
        s.DnDAllowlist.Add("Bluetooth");
        Assert.False(DoNotDisturb.IsActive(s, "Bluetooth"));
        Assert.True(DoNotDisturb.IsActive(s, "QQ.exe"));
    }

    [Fact]
    public void Dnd_Time_Range_Cross_Midnight()
    {
        // 跨天反向区间（22:00-08:00）与正向区间（09:00-18:00）的整点判定
        Assert.True(RuleEngine.InTimeRange(22, 8, nowHour: 23));
        Assert.True(RuleEngine.InTimeRange(22, 8, nowHour: 3));
        Assert.False(RuleEngine.InTimeRange(22, 8, nowHour: 12));
        Assert.True(RuleEngine.InTimeRange(9, 18, nowHour: 12));
        Assert.False(RuleEngine.InTimeRange(9, 18, nowHour: 20));
        // 相同小时：仅该整点小时命中
        Assert.True(RuleEngine.InTimeRange(20, 20, nowHour: 20));
        Assert.False(RuleEngine.InTimeRange(20, 20, nowHour: 21));
        Assert.True(RuleEngine.InTimeRange(22, 8, nowHour: 3));
        Assert.False(RuleEngine.InTimeRange(22, 8, nowHour: 12));
        Assert.True(RuleEngine.InTimeRange(9, 18, nowHour: 12));
        Assert.False(RuleEngine.InTimeRange(9, 18, nowHour: 20));
    }
}
