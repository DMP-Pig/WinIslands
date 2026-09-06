using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>规则引擎：时间段判断 / 条件匹配 / 多规则叠加求值（批次38：规则引擎）。</summary>
public class RuleEngineTests
{
    // ---- InTimeRange ----

    [Theory]
    [InlineData(22, 8, 23, true)]   // 跨天：22:00-08:00，23 点命中
    [InlineData(22, 8, 2, true)]    // 跨天：凌晨 2 点命中
    [InlineData(22, 8, 12, false)]  // 跨天：中午 12 点不命中
    [InlineData(9, 18, 12, true)]   // 普通范围：12 点命中
    [InlineData(9, 18, 8, false)]   // 普通范围：8 点不命中
    [InlineData(9, 18, 18, false)]  // 普通范围：结束时刻不命中（半开区间）
    [InlineData(7, 7, 7, true)]     // 相同小时：视为该整点小时，命中
    [InlineData(7, 7, 8, false)]    // 相同小时：其他小时不命中
    public void InTimeRange_Handles_Ranges(int start, int end, int now, bool expected)
        => Assert.Equal(expected, RuleEngine.InTimeRange(start, end, now));

    [Fact]
    public void InTimeRange_Clamps_Out_Of_Range_Hours()
    {
        Assert.True(RuleEngine.InTimeRange(24, -1, 23));   // clamp 后为 23-0（跨天），23 点命中
        Assert.False(RuleEngine.InTimeRange(24, -1, 12));  // clamp 后为 23-0（跨天），12 点不命中
    }

    // ---- Matches ----

    [Fact]
    public void Matches_Always_Is_True()
    {
        var rule = new AppRule { Condition = RuleCondition.Always };
        Assert.True(RuleEngine.Matches(rule, hasMedia: false, mediaAppId: null));
        Assert.True(RuleEngine.Matches(rule, hasMedia: true, mediaAppId: "Spotify.exe"));
    }

    [Fact]
    public void Matches_NoMedia_And_MediaPlaying()
    {
        var noMedia = new AppRule { Condition = RuleCondition.NoMedia };
        var playing = new AppRule { Condition = RuleCondition.MediaPlaying };
        Assert.True(RuleEngine.Matches(noMedia, hasMedia: false, mediaAppId: null));
        Assert.False(RuleEngine.Matches(noMedia, hasMedia: true, mediaAppId: "QQMusic.exe"));
        Assert.True(RuleEngine.Matches(playing, hasMedia: true, mediaAppId: "QQMusic.exe"));
        Assert.False(RuleEngine.Matches(playing, hasMedia: false, mediaAppId: null));
    }

    [Fact]
    public void Matches_AppPlaying_Is_CaseInsensitive_Contains()
    {
        var rule = new AppRule { Condition = RuleCondition.AppPlaying, AppMatch = "spotify" };
        Assert.True(RuleEngine.Matches(rule, hasMedia: true, mediaAppId: "Spotify.exe"));
        Assert.True(RuleEngine.Matches(rule, hasMedia: true, mediaAppId: "Cider - Spotify Radio"));
        Assert.False(RuleEngine.Matches(rule, hasMedia: true, mediaAppId: "QQMusic.exe"));
        Assert.False(RuleEngine.Matches(rule, hasMedia: false, mediaAppId: null)); // 无媒体不命中
        Assert.False(RuleEngine.Matches(rule, hasMedia: true, mediaAppId: null));  // 空匹配文本
    }

    [Fact]
    public void Matches_Null_Rule_Is_False()
    {
        Assert.False(RuleEngine.Matches(null, hasMedia: true, mediaAppId: "x"));
    }

    // ---- Evaluate ----

    [Fact]
    public void Evaluate_NoRules_Returns_None()
    {
        Assert.Equal(RuleEval.None, RuleEngine.Evaluate(null, hasMedia: false, mediaAppId: null));
        Assert.Equal(RuleEval.None, RuleEngine.Evaluate(new AppSettings(), hasMedia: false, mediaAppId: null));
    }

    [Fact]
    public void Evaluate_Disabled_Rules_Are_Skipped()
    {
        var s = new AppSettings();
        s.Rules.Add(new AppRule { Enabled = false, Condition = RuleCondition.Always, Action = RuleAction.Hide });
        Assert.Equal(RuleEval.None, RuleEngine.Evaluate(s, hasMedia: true, mediaAppId: "x"));
    }

    [Fact]
    public void Evaluate_Stacked_Rules_Combine_Flags()
    {
        var s = new AppSettings();
        s.Rules.Add(new AppRule { Condition = RuleCondition.MediaPlaying, Action = RuleAction.Hide });
        s.Rules.Add(new AppRule { Condition = RuleCondition.Always, Action = RuleAction.Collapse });
        s.Rules.Add(new AppRule { Condition = RuleCondition.AppPlaying, AppMatch = "cider", Action = RuleAction.ForceShow });
        var ev = RuleEngine.Evaluate(s, hasMedia: true, mediaAppId: "Cider.exe");
        Assert.True(ev.ForceHide);
        Assert.True(ev.ForceCollapse);
        Assert.True(ev.ForceShow);

        var ev2 = RuleEngine.Evaluate(s, hasMedia: false, mediaAppId: null);
        Assert.False(ev2.ForceHide);     // MediaPlaying 条件不满足
        Assert.True(ev2.ForceCollapse);  // Always 条件始终满足
        Assert.False(ev2.ForceShow);     // AppPlaying 条件不满足
    }
}
