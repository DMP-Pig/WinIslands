using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>iCalendar 解析：转义、时区、全天事件、提前提醒、行折叠。</summary>
public class IcsCalendarTests : IDisposable
{
    private readonly string _dir;

    public IcsCalendarTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "WinIslandsIcs-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        try { Directory.Delete(_dir, recursive: true); } catch { /* ignore */ }
    }

    private string WriteIcs(string content)
    {
        var file = Path.Combine(_dir, "cal.ics");
        File.WriteAllText(file, content);
        return file;
    }

    [Fact]
    public void Parses_Basic_Event_And_Unescapes_Summary()
    {
        var file = WriteIcs(
            "BEGIN:VCALENDAR\r\n" +
            "BEGIN:VEVENT\r\n" +
            "UID:evt1\r\n" +
            "SUMMARY:Team\\, Lunch\\; Party\\\\Great\\nNext line\r\n" +
            "DTSTART:20260829T100000\r\n" +
            "DTEND:20260829T110000\r\n" +
            "END:VEVENT\r\n" +
            "END:VCALENDAR\r\n");

        var events = IcsParser.ParseFile(file);
        var ev = Assert.Single(events);
        Assert.Equal("Team, Lunch; Party\\Great\nNext line", ev.Title);
        Assert.Equal(new DateTimeOffset(2026, 8, 29, 10, 0, 0, TimeZoneInfo.Local.GetUtcOffset(new DateTime(2026, 8, 29))), ev.Start);
        Assert.Equal(TimeSpan.FromHours(1), ev.End - ev.Start);
    }

    [Fact]
    public void Parses_Utc_And_Tzid()
    {
        var file = WriteIcs(
            "BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nUID:u1\r\nSUMMARY:UTC meeting\r\n" +
            "DTSTART:20260829T020000Z\r\nDTEND:20260829T030000Z\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n");
        var ev = Assert.Single(IcsParser.ParseFile(file));
        Assert.Equal(new DateTimeOffset(2026, 8, 29, 2, 0, 0, TimeSpan.Zero), ev.Start);

        var file2 = WriteIcs(
            "BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nUID:u2\r\nSUMMARY:TZ meeting\r\n" +
            "DTSTART;TZID=China Standard Time:20260829T140000\r\nDTEND;TZID=China Standard Time:20260829T150000\r\n" +
            "END:VEVENT\r\nEND:VCALENDAR\r\n");
        var ev2 = Assert.Single(IcsParser.ParseFile(file2));
        Assert.Equal(TimeSpan.FromHours(8), ev2.Start.Offset); // UTC+8
    }

    [Fact]
    public void Parses_AllDay_Event_And_Valarm()
    {
        var file = WriteIcs(
            "BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nUID:u3\r\nSUMMARY:All day\r\n" +
            "DTSTART;VALUE=DATE:20260829\r\nDTEND;VALUE=DATE:20260830\r\n" +
            "BEGIN:VALARM\r\nTRIGGER:-PT15M\r\nEND:VALARM\r\n" +
            "END:VEVENT\r\nEND:VCALENDAR\r\n");
        var ev = Assert.Single(IcsParser.ParseFile(file));
        Assert.Equal(new DateTime(2026, 8, 29), ev.Start.Date);
        Assert.NotNull(ev.Alarm);
        Assert.Equal(ev.Start.AddMinutes(-15), ev.Alarm);
    }

    [Fact]
    public void Folds_Continuation_Lines()
    {
        var file = WriteIcs(
            "BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nUID:u4\r\n" +
            "SUMMARY:This is a very long summary that got \r\n folded onto the next line\r\n" +
            "DTSTART:20260829T090000\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n");
        var ev = Assert.Single(IcsParser.ParseFile(file));
        Assert.Equal("This is a very long summary that got folded onto the next line", ev.Title);
    }
}
