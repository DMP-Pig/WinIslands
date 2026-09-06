using System;
using System.Collections.Generic;
using System.Linq;

namespace WinIslands.Services;

/// <summary>规则触发条件。</summary>
public enum RuleCondition
{
    Always = 0,       // 始终
    NoMedia = 1,      // 无媒体播放时
    MediaPlaying = 2, // 正在播放媒体时
    TimeRange = 3,    // 指定时间段内（支持跨天）
    AppPlaying = 4,   // 指定媒体程序正在播放时
}

/// <summary>规则动作。</summary>
public enum RuleAction
{
    Hide = 0,      // 隐藏灵动岛
    Collapse = 1,  // 强制收起（仅紧凑胶囊）
    ForceShow = 2, // 强制显示
}

/// <summary>一条显示规则：条件满足时执行动作。所有配置保存在本地 JSON。</summary>
public sealed class AppRule
{
    public bool Enabled { get; set; } = true;
    public string Name { get; set; } = "";
    public RuleCondition Condition { get; set; } = RuleCondition.Always;
    public int StartHour { get; set; } = 22;  // TimeRange：开始小时 0-23
    public int EndHour { get; set; } = 8;     // TimeRange：结束小时 0-23（跨天）
    public string AppMatch { get; set; } = ""; // AppPlaying：媒体应用标识（大小写不敏感，支持包含匹配，如 Spotify / Cider）
    public RuleAction Action { get; set; } = RuleAction.Hide;
}

/// <summary>规则求值结果。</summary>
public sealed record RuleEval(bool ForceHide, bool ForceCollapse, bool ForceShow)
{
    public static readonly RuleEval None = new(false, false, false);
}

/// <summary>
/// 条件规则引擎：根据当前状态（是否播放媒体、当前媒体程序、时间）求值所有启用的规则。
/// 纯静态函数，便于单元测试。
/// </summary>
public static class RuleEngine
{
    /// <summary>求值所有启用的规则，返回最终动作（多个规则叠加）。</summary>
    public static RuleEval Evaluate(AppSettings? s, bool hasMedia, string? mediaAppId)
    {
        if (s?.Rules is not { Count: > 0 }) return RuleEval.None;
        bool hide = false, collapse = false, show = false;
        foreach (var r in s.Rules)
        {
            if (r is null || !r.Enabled) continue;
            if (!Matches(r, hasMedia, mediaAppId)) continue;
            switch (r.Action)
            {
                case RuleAction.Hide: hide = true; break;
                case RuleAction.Collapse: collapse = true; break;
                case RuleAction.ForceShow: show = true; break;
            }
        }
        return new RuleEval(hide, collapse, show);
    }

    /// <summary>判断一条规则的条件是否满足（独立方法，便于测试）。</summary>
    public static bool Matches(AppRule? r, bool hasMedia, string? mediaAppId)
    {
        if (r is null) return false;
        return r.Condition switch
        {
            RuleCondition.NoMedia => !hasMedia,
            RuleCondition.MediaPlaying => hasMedia,
            RuleCondition.TimeRange => InTimeRange(r.StartHour, r.EndHour),
            RuleCondition.AppPlaying => hasMedia && !string.IsNullOrWhiteSpace(r.AppMatch)
                && mediaAppId is not null
                && mediaAppId.Contains(r.AppMatch, StringComparison.OrdinalIgnoreCase),
            _ => true, // Always 及其它
        };
    }

    /// <summary>时间段判断（支持跨天，如 22:00-08:00；相同小时视为该整点小时）。</summary>
    public static bool InTimeRange(int startHour, int endHour, int? nowHour = null)
    {
        startHour = Math.Clamp(startHour, 0, 23);
        endHour = Math.Clamp(endHour, 0, 23);
        var now = nowHour ?? DateTime.Now.Hour;
        if (startHour < endHour) return now >= startHour && now < endHour;
        if (startHour > endHour) return now >= startHour || now < endHour;
        return now == startHour;
    }
}
