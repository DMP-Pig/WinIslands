using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>上岛推送：优先级排序 / 同 id 更新位置不变 / 移除后位置重排（批次E：消息队列优先级）。</summary>
public class IslandPushTests : IDisposable
{
    private readonly string _origAppData;

    public IslandPushTests()
    {
        // 重定向到临时目录，避免触碰真实配置文件
        _origAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(Path.GetTempPath(), "WinIslandsPushTests-" + Guid.NewGuid().ToString("N"));
        Environment.SetEnvironmentVariable("WINISLANDS_APPDATA", dir);
        Directory.CreateDirectory(dir);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("WINISLANDS_APPDATA", null);
        Environment.SetEnvironmentVariable("APPDATA", _origAppData);
    }

    private static IslandApiServer NewServer()
    {
        var svc = new SettingsService();
        svc.Current.IslandApiDefaultDuration = 60;
        return new IslandApiServer(svc);
    }

    private static IslandPush Push(string id, string priority = "", string title = "t")
        => new() { Id = id, Title = title, Priority = priority };

    [Fact]
    public void PriorityRank_Maps_High_Normal_Low()
    {
        Assert.Equal(2, Push("h", "high").PriorityRank);
        Assert.Equal(1, Push("n", "normal").PriorityRank);
        Assert.Equal(1, Push("e", "").PriorityRank);
        Assert.Equal(0, Push("l", "low").PriorityRank);
    }

    [Fact]
    public void ActivePushes_Sorts_By_Priority_Then_InsertionOrder()
    {
        using var server = NewServer();
        server.AddOrUpdate(Push("low1", "low"));
        server.AddOrUpdate(Push("high1", "high"));
        server.AddOrUpdate(Push("norm1", "normal"));
        server.AddOrUpdate(Push("low2", "low"));
        server.AddOrUpdate(Push("high2", "high"));

        var ids = server.ActivePushes.Select(p => p.Id).ToList();
        Assert.Equal(new[] { "high1", "high2", "norm1", "low1", "low2" }, ids);
    }

    [Fact]
    public void SameId_Update_Keeps_Position_And_Expiry()
    {
        using var server = NewServer();
        server.AddOrUpdate(Push("a"));
        var b = server.AddOrUpdate(Push("b"));
        server.AddOrUpdate(Push("c"));
        var bExpiry = b.ExpiresAt;

        var updated = server.AddOrUpdate(new IslandPush { Id = "b", Title = "b-updated", Body = "new" });

        Assert.Equal("b-updated", updated.Title);
        Assert.Equal(bExpiry, updated.ExpiresAt); // 同 id 更新保留原过期时间

        var ids = server.ActivePushes.Select(p => p.Id).ToList();
        Assert.Equal(new[] { "a", "b", "c" }, ids); // 队列位置不变
        Assert.Equal(2, ids.IndexOf("b") + 1);      // position（1 基）= 2
    }

    [Fact]
    public void Theme_And_Command_Action_Serialize()
    {
        // #17 推送主题 + #16 command 动作的 JSON 往返
        var push = System.Text.Json.JsonSerializer.Deserialize<IslandPush>(
            """{"id":"t1","title":"T","theme":"dark","buttons":[{"label":"Run","action":"command","value":"notepad.exe"}]}""");
        Assert.NotNull(push);
        Assert.Equal("dark", push!.Theme);
        Assert.Single(push.Buttons!);
        Assert.Equal("command", push.Buttons[0].Action);
        Assert.Equal("notepad.exe", push.Buttons[0].Value);
    }

    [Fact]
    public void Remove_Reorders_Remaining_Positions()
    {
        using var server = NewServer();
        server.AddOrUpdate(Push("a"));
        server.AddOrUpdate(Push("b"));
        server.AddOrUpdate(Push("c"));
        server.Remove("b");

        var ids = server.ActivePushes.Select(p => p.Id).ToList();
        Assert.Equal(new[] { "a", "c" }, ids);
    }
}
