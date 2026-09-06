using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace WinIslands.Services;

/// <summary>一条日历事件（来自 .ics）。</summary>
public sealed record CalendarEvent(
    string Id, string Title, DateTimeOffset Start, DateTimeOffset End, DateTimeOffset? Alarm);

/// <summary>
/// iCalendar (.ics) 解析器 + 提醒服务。
/// 解析 .ics 文件中的 VEVENT（标题 / 开始 / 结束 / VALARM 提前提醒），
/// 每 30 秒重新读取文件并按「提前提醒时间」触发提醒，避免跨天/长时间运行后遗漏。
/// 支持：
///   - DTSTART/DTEND 的 UTC（Z）、本地时间、TZID 参数、VALUE=DATE 全天事件
///   - VALARM TRIGGER 负偏移（-PTnW/M/H/S、-PnDTnHnMnS）
///   - 行折叠（CRLF 后以空格/制表符开头的续行）
/// 不做：RRULE 重复规则（只提醒出现的单次事件）。
/// 纯本地解析，不联网。
/// </summary>
public sealed class CalendarService : IDisposable
{
    private readonly DispatcherTimer _timer;
    private readonly HashSet<string> _fired = new();
    private List<CalendarEvent> _events = new();
    private string? _lastFile;
    private DateTime _lastWrite;

    public CalendarService()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
        _timer.Tick += (_, _) => Refresh();
        _timer.Start();
    }

    /// <summary>按需启停后台轮询：总开关关闭或未配置 .ics 路径时停止 30 秒定时器，避免空转。</summary>
    public void SetEnabled(bool enabled)
    {
        if (enabled) { _timer.Start(); Refresh(); }
        else _timer.Stop();
    }

    /// <summary>解析出的全部事件（按开始时间排序）。</summary>
    public IReadOnlyList<CalendarEvent> Events => _events;

    /// <summary>下一个尚未结束的事件；没有则 null。</summary>
    public CalendarEvent? Next
    {
        get
        {
            var now = DateTimeOffset.Now;
            return _events.Where(e => e.End > now).OrderBy(e => e.Start).FirstOrDefault();
        }
    }

    /// <summary>组件摘要：「14:30 会议」/「还有 1 小时 · 会议」等；无事件返回空。</summary>
    public string Summary
    {
        get
        {
            var n = Next;
            if (n is null) return string.Empty;
            var diff = n.Start - DateTimeOffset.Now;
            if (diff.TotalHours >= 1) return $"{n.Start.LocalDateTime:HH:mm} {n.Title}";
            var mins = Math.Max(1, (int)Math.Ceiling(diff.TotalMinutes));
            return $"{mins} 分钟后 · {n.Title}";
        }
    }

    /// <summary>组件摘要已变化（供 UI 刷新）。</summary>
    public event Action? Changed;

    /// <summary>事件到点（提前提醒，参数为事件）。</summary>
    public event Action<CalendarEvent>? Reminder;

    /// <summary>重新读取 .ics 文件并检查提醒（调用方设置变化时也可主动调用）。</summary>
    public void Refresh(string? path = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            var fi = new FileInfo(path);
            if (!fi.Exists) { _events = new List<CalendarEvent>(); RaiseChanged(); return; }

            // 文件未变化则只做提醒检查（避免每 30 秒重复解析）
            if (_lastFile != path || fi.LastWriteTimeUtc != _lastWrite)
            {
                _lastFile = path;
                _lastWrite = fi.LastWriteTimeUtc;
                _events = IcsParser.ParseFile(path);
                RaiseChanged();
            }

            CheckReminders();
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Calendar refresh failed: {ex.Message}");
        }
    }

    private void CheckReminders()
    {
        var now = DateTimeOffset.Now;
        List<CalendarEvent> due = new();
        foreach (var e in _events)
        {
            var alarm = e.Alarm ?? e.Start;
            if (alarm <= now && now < e.End && _fired.Add(e.Id))
                due.Add(e);
        }
        foreach (var d in due) Reminder?.Invoke(d);
    }

    private void RaiseChanged() => Changed?.Invoke();

    /// <summary>用于调试/诊断：格式化当前事件列表。</summary>
    public string Dump()
    {
        var sb = new System.Text.StringBuilder();
        foreach (var e in _events)
            sb.AppendLine($"{e.Start.LocalDateTime:yyyy-MM-dd HH:mm} ~ {e.End.LocalDateTime:HH:mm}  {e.Title}");
        return sb.ToString();
    }

    public void Dispose() => _timer.Stop();
}

/// <summary>极简 iCalendar VEVENT 解析器（不依赖第三方库）。</summary>
public static class IcsParser
{
    public static List<CalendarEvent> ParseFile(string path)
    {
        var result = new List<CalendarEvent>();
        string[] lines;
        try { lines = File.ReadAllLines(path); }
        catch { return result; }

        // 1) 合并折叠行：以空格/制表符开头的行是上一行的续行
        var unfolded = new List<string>(lines.Length);
        foreach (var raw in lines)
        {
            var line = raw.TrimEnd('\r');
            if (line.Length == 0) continue;
            if ((line[0] == ' ' || line[0] == '\t') && unfolded.Count > 0)
                unfolded[unfolded.Count - 1] += line.Substring(1);
            else
                unfolded.Add(line);
        }

        string? curSummary = null, curUid = null, curStart = null, curEnd = null;
        string? startTz = null, endTz = null;
        bool startDate = false, endDate = false;
        TimeSpan? curAlarm = null;
        bool inEvent = false;

        foreach (var line in unfolded)
        {
            if (line.StartsWith("BEGIN:VEVENT", StringComparison.OrdinalIgnoreCase))
            {
                curSummary = null; curUid = null; curStart = null; curEnd = null;
                startTz = null; endTz = null; startDate = false; endDate = false;
                curAlarm = null; inEvent = true;
                continue;
            }
            if (line.StartsWith("END:VEVENT", StringComparison.OrdinalIgnoreCase))
            {
                if (inEvent)
                {
                    var ev = BuildEvent(curUid, curSummary, curStart, startTz, startDate,
                        curEnd, endTz, endDate, curAlarm);
                    if (ev is not null) result.Add(ev);
                    inEvent = false;
                }
                continue;
            }
            if (!inEvent) continue;

            // VALARM：解析 TRIGGER 的负偏移，作为提前提醒时间
            if (line.StartsWith("BEGIN:VALARM", StringComparison.OrdinalIgnoreCase))
            {
                // 后续行在同一个循环里处理，TRIGGER 会单独命中
            }
            else if (line.StartsWith("TRIGGER", StringComparison.OrdinalIgnoreCase))
            {
                var value = ContentOf(line);
                curAlarm = TryParseTrigger(value);
            }
            else if (line.StartsWith("UID", StringComparison.OrdinalIgnoreCase))
            {
                curUid = Unescape(ContentOf(line));
            }
            else if (line.StartsWith("SUMMARY", StringComparison.OrdinalIgnoreCase))
            {
                curSummary = Unescape(ContentOf(line));
            }
            else if (line.StartsWith("DTSTART", StringComparison.OrdinalIgnoreCase))
            {
                ParseDateTime(line, out curStart, out startTz, out startDate);
            }
            else if (line.StartsWith("DTEND", StringComparison.OrdinalIgnoreCase))
            {
                ParseDateTime(line, out curEnd, out endTz, out endDate);
            }
        }

        return result.OrderBy(e => e.Start).ToList();
    }

    private static CalendarEvent? BuildEvent(string? uid, string? summary, string? start, string? startTz, bool startDate,
        string? end, string? endTz, bool endDate, TimeSpan? alarmOffset)
    {
        var s = ParseDateTimeValue(start, startTz, startDate);
        if (s is null) return null;
        var e = ParseDateTimeValue(end, endTz, endDate);
        if (e is null) e = s.Value.AddHours(1); // 无 DTEND：默认 1 小时
        var startOut = startDate ? new DateTimeOffset(s.Value.Date, TimeZoneInfo.Local.GetUtcOffset(s.Value.Date)) : s.Value;
        var endOut = endDate ? new DateTimeOffset(e.Value.Date, TimeZoneInfo.Local.GetUtcOffset(e.Value.Date)) : e.Value;
        if (endOut <= startOut) endOut = startOut.AddHours(1);
        var id = (string.IsNullOrWhiteSpace(uid) ? Guid.NewGuid().ToString("N") : uid) + "\u0001" + startOut.ToString("o");
        var alarm = alarmOffset.HasValue ? startOut.Add(alarmOffset.Value) : (DateTimeOffset?)null;
        return new CalendarEvent(id, string.IsNullOrWhiteSpace(summary) ? "(无标题)" : summary, startOut, endOut, alarm);
    }

    private static void ParseDateTime(string line, out string? value, out string? tzid, out bool isDate)
    {
        value = ContentOf(line);
        tzid = null; isDate = false;
        var colon = line.IndexOf(':');
        var head = colon >= 0 ? line.Substring(0, colon) : line;
        var paramsPart = head.Contains(';') ? head.Substring(head.IndexOf(';')) : string.Empty;
        // 例如 ;TZID=Asia/Shanghai 或 ;VALUE=DATE
        foreach (var p in paramsPart.Split(';'))
        {
            var kv = p.Split('=');
            if (kv.Length != 2) continue;
            if (string.Equals(kv[0].Trim(), "TZID", StringComparison.OrdinalIgnoreCase)) tzid = kv[1].Trim();
            if (string.Equals(kv[0].Trim(), "VALUE", StringComparison.OrdinalIgnoreCase)
                && string.Equals(kv[1].Trim(), "DATE", StringComparison.OrdinalIgnoreCase)) isDate = true;
        }
    }

    private static DateTimeOffset? ParseDateTimeValue(string? raw, string? tzid, bool isDate)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        try
        {
            if (isDate && raw.Length >= 8)
            {
                var y = int.Parse(raw.Substring(0, 4));
                var mo = int.Parse(raw.Substring(4, 2));
                var d = int.Parse(raw.Substring(6, 2));
                return new DateTimeOffset(new DateTime(y, mo, d), TimeZoneInfo.Local.GetUtcOffset(new DateTime(y, mo, d)));
            }

            var s = raw.Trim();
            if (s.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            {
                var dt = DateTime.ParseExact(s.Substring(0, s.Length - 1),
                    new[] { "yyyyMMdd'T'HHmmss", "yyyyMMdd'T'HHmm" }, CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                return new DateTimeOffset(dt, TimeSpan.Zero);
            }

            var dtLocal = DateTime.ParseExact(s,
                new[] { "yyyyMMdd'T'HHmmss", "yyyyMMdd'T'HHmm" }, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal);
            if (!string.IsNullOrWhiteSpace(tzid))
            {
                try
                {
                    var tz = TimeZoneInfo.FindSystemTimeZoneById(tzid);
                    return new DateTimeOffset(dtLocal, tz.GetUtcOffset(dtLocal));
                }
                catch { /* 未知 TZID：按本地时间 */ }
            }
            return new DateTimeOffset(dtLocal, TimeZoneInfo.Local.GetUtcOffset(dtLocal));
        }
        catch { return null; }
    }

    /// <summary>解析 VALARM 的 TRIGGER 值：-PT15M / -P1DT2H / PT0S（0 表示事件开始时）。返回相对事件开始的偏移。</summary>
    private static TimeSpan? TryParseTrigger(string value)
    {
        var v = value.Trim();
        // 绝对值（TRIGGER;VALUE=DATE-TIME:...）暂不支持，按开始时间提醒
        if (v.Contains(',')) return null;
        if (!v.StartsWith("-", StringComparison.Ordinal) && !v.StartsWith("+", StringComparison.Ordinal))
            return null; // 只有负偏移（提前）/0 有意义
        var negative = v.StartsWith("-", StringComparison.Ordinal);
        var body = v.TrimStart('-', '+');
        if (body.Length < 2) return null;
        // 形如 P1DT2H 的复合时长：拆成 天+时分秒 处理
        var sign = negative ? -1.0 : 1.0;
        var txt = body.TrimStart("P".ToCharArray());
        double days = 0, hours = 0, mins = 0, secs = 0;
        var num = new System.Text.StringBuilder();
        foreach (var ch in txt)
        {
            if (char.IsDigit(ch) || ch == '.') { num.Append(ch); continue; }
            if (num.Length == 0) continue;
            var val = double.TryParse(num.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var d) ? d : 0;
            switch (char.ToUpperInvariant(ch))
            {
                case 'W': days += val * 7; break;
                case 'D': days += val; break;
                case 'H': hours += val; break;
                case 'M': mins += val; break;
                case 'S': secs += val; break;
            }
            num.Clear();
        }
        if (days == 0 && hours == 0 && mins == 0 && secs == 0) return TimeSpan.Zero;
        var span = TimeSpan.FromDays(days) + TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(mins) + TimeSpan.FromSeconds(secs);
        return sign < 0 ? -span : span; // 负 = 提前提醒
    }

    private static string ContentOf(string line)
    {
        var colon = line.IndexOf(':');
        return colon >= 0 ? line.Substring(colon + 1) : line;
    }

    private static string Unescape(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        // ICS 转义：\\ → \、\n → 换行、\, → 逗号、\; → 分号。
        // 必须逐个字符扫描：先用占位符方案会丢掉后续的 \n/\,/\; 替换（旧实现实为无效操作）。
        var sb = new System.Text.StringBuilder(s.Length);
        for (var i = 0; i < s.Length; i++)
        {
            if (s[i] == '\\' && i + 1 < s.Length)
            {
                switch (s[i + 1])
                {
                    case 'n':
                    case 'N': sb.Append('\n'); i++; break;
                    case ',': sb.Append(','); i++; break;
                    case ';': sb.Append(';'); i++; break;
                    case '\\': sb.Append('\\'); i++; break;
                    default: sb.Append(s[i]); break; // 未知转义保留原样
                }
            }
            else
            {
                sb.Append(s[i]);
            }
        }
        return sb.ToString();
    }
}
