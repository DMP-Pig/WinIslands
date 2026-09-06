using WinIslands.Services;
using WinIslands.UI;

namespace WinIslands.Tests;

/// <summary>快捷操作设置：顺序重建、勾选显示、上移/下移。</summary>
public class QuickActionsTests : IDisposable
{
    private readonly string _origAppData;
    private readonly string _dir;

    public QuickActionsTests()
    {
        _origAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _dir = Path.Combine(Path.GetTempPath(), "WinIslandsQuick-" + Guid.NewGuid().ToString("N"));
        Environment.SetEnvironmentVariable("WINISLANDS_APPDATA", _dir);
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("WINISLANDS_APPDATA", null);
        Environment.SetEnvironmentVariable("APPDATA", _origAppData);
        try { Directory.Delete(_dir, recursive: true); } catch { /* ignore */ }
    }

    [Fact]
    public void Builds_All_Actions_In_Order()
    {
        var svc = new SettingsService();
        var vm = new SettingsViewModel(svc);
        Assert.Equal(11, vm.QuickActionRows.Count);
        Assert.Equal("Lock", vm.QuickActionRows[0].Key);
        Assert.Equal("VolumeDown", vm.QuickActionRows[^1].Key);
    }

    [Fact]
    public void Toggle_Updates_Shown_List()
    {
        var svc = new SettingsService();
        var vm = new SettingsViewModel(svc);
        var row = vm.QuickActionRows.First(r => r.Key == "TaskManager");

        Assert.False(row.IsChecked); // 默认未勾选
        row.IsChecked = true;
        Assert.Contains("TaskManager", vm.Working.QuickActionsShown);
        row.IsChecked = false;
        Assert.DoesNotContain("TaskManager", vm.Working.QuickActionsShown);
    }

    [Fact]
    public void Move_Reorders_All_Action_List()
    {
        var svc = new SettingsService();
        var vm = new SettingsViewModel(svc);
        // 把 Mute 从第 2 位移到第 1 位
        vm.MoveQuickAction("Mute", -1);
        Assert.Equal("Mute", vm.QuickActionRows[0].Key);
        Assert.Equal("Lock", vm.QuickActionRows[1].Key);
    }
}
