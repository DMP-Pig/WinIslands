using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>通知历史 / 勿扰白名单 / 设置深拷贝测试（批次B：通知中心一页化、勿扰白名单、通知折叠、一键处理）。</summary>
public class NotificationTests : IDisposable
{
    private readonly string _origAppData;

    public NotificationTests()
    {
        _origAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(Path.GetTempPath(), "WinIslandsNotifyTests-" + Guid.NewGuid().ToString("N"));
        Environment.SetEnvironmentVariable("WINISLANDS_APPDATA", dir);
        Directory.CreateDirectory(dir);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("WINISLANDS_APPDATA", null);
        Environment.SetEnvironmentVariable("APPDATA", _origAppData);
    }

    private static NotificationHistoryService NewHistory() => new();

    [Fact]
    public void History_Add_Carries_Source_And_Unread()
    {
        using var h = NewHistory();
        h.Add("title", "body", "\uE702", "Bluetooth");
        Assert.Single(h.Entries);
        Assert.Equal("Bluetooth", h.Entries[0].Source);
        Assert.Equal(1, h.Entries.Count(e => !e.Read));
        Assert.False(h.Entries[0].Read);
    }

    [Fact]
    public void History_MarkReadMatching_Turns_Only_Matching_Read()
    {
        using var h = NewHistory();
        h.Add("t1", "b1", "g", "QQ.exe");
        h.Add("t2", "b2", "g", "QQ.exe");
        h.MarkReadMatching("t1", "b1");
        Assert.Equal(1, h.Entries.Count(e => !e.Read));
        Assert.True(h.Entries.First(e => e.Title == "t1").Read);
        Assert.False(h.Entries.First(e => e.Title == "t2").Read);
    }

    [Fact]
    public void History_MarkAllRead_And_Remove()
    {
        using var h = NewHistory();
        h.Add("a", "b", "g");
        h.Add("c", "d", "g");
        h.MarkAllRead();
        Assert.Equal(0, h.Entries.Count(e => !e.Read));
        var first = h.Entries[0];
        h.Remove(first);
        Assert.Single(h.Entries);
    }

    [Fact]
    public void Dnd_Allowlist_Overrides_Manual_Dnd()
    {
        var s = new AppSettings { DoNotDisturbManual = true };
        s.DnDAllowlist.Add("qq.exe");
        // 白名单内来源（大小写不敏感）不受勿扰影响
        Assert.False(DoNotDisturb.IsActive(s, "QQ.exe"));
        // 非白名单来源仍被勿扰拦截
        Assert.True(DoNotDisturb.IsActive(s, "WeChat.exe"));
        // 来源为空时按普通勿扰判定
        Assert.True(DoNotDisturb.IsActive(s, null));
    }

    [Fact]
    public void Dnd_Allowlist_CaseInsensitive_And_Trim()
    {
        var s = new AppSettings { DoNotDisturbManual = true };
        s.DnDAllowlist.Add("  WeChat.exe  ");
        Assert.False(DoNotDisturb.IsActive(s, "wechat.EXE"));
    }

    [Fact]
    public void Clone_DeepCopies_DnDAllowlist()
    {
        var s = new AppSettings();
        s.DnDAllowlist.Add("QQ.exe");
        var c = s.Clone();
        c.DnDAllowlist.Add("WeChat.exe");
        Assert.Single(s.DnDAllowlist);
        Assert.Equal(2, c.DnDAllowlist.Count);
    }

    [Fact]
    public void Settings_DnDAllowlist_Roundtrip()
    {
        var svc = new SettingsService();
        svc.Current.DnDAllowlist.Add("QQ.exe");
        svc.Current.NotifyFoldEnabled = true;
        svc.Save();
        var svc2 = new SettingsService();
        Assert.Equal("QQ.exe", svc2.Current.DnDAllowlist[0]);
        Assert.True(svc2.Current.NotifyFoldEnabled);
    }
}
