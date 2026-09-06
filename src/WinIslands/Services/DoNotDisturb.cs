using System;

namespace WinIslands.Services;

/// <summary>勿扰模式判定：手动开关或时间段自动生效。</summary>
public static class DoNotDisturb
{
    /// <summary>是否处于勿扰：手动开关或时间段自动生效；白名单来源（10 勿扰白名单）不受勿扰影响。</summary>
    public static bool IsActive(AppSettings s) => IsActive(s, null);

    public static bool IsActive(AppSettings s, string? source)
    {
        if (IsAllowlisted(s, source)) return false;
        if (s.DoNotDisturbManual) return true;
        // 开会静音助手：检测到会议时自动勿扰（仅本机前台窗口检测，不联网）
        if (s.MeetingAssistantEnabled && s.MeetingAutoDnd && MeetingMonitor.IsInMeeting) return true;
        if (!s.DoNotDisturbEnabled) return false;
        var start = TimeOfDay(s.DoNotDisturbStartHour, s.DoNotDisturbStartMinute);
        var end = TimeOfDay(s.DoNotDisturbEndHour, s.DoNotDisturbEndMinute);
        if (start == end) return false;
        if (IsAllowlisted(s, source)) return false;
        var now = DateTime.Now.TimeOfDay;
        // 分钟级判断；跨天（start > end）时采用“现在 >= start 或 现在 < end”
        return start < end ? now >= start && now < end : now >= start || now < end;
    }

    /// <summary>把「小时 + 分钟」合成 TimeSpan（越界值夹紧到合法范围）。</summary>
    private static TimeSpan TimeOfDay(int hour, int minute)
    {
        var h = Math.Clamp(hour, 0, 23);
        var m = Math.Clamp(minute, 0, 59);
        return TimeSpan.FromHours(h) + TimeSpan.FromMinutes(m);
    }

    /// <summary>来源（exe 名 / 应用显示名，大小写不敏感）是否在勿扰白名单内。</summary>
    private static bool IsAllowlisted(AppSettings s, string? source)
    {
        if (string.IsNullOrWhiteSpace(source)) return false;
        foreach (var item in s.DnDAllowlist)
        {
            var t = item?.Trim();
            if (string.IsNullOrEmpty(t)) continue;
            if (string.Equals(t, source, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }
}
