using WinIslands.Services;

namespace WinIslands.Tests;

public class SettingsTests : IDisposable
{
    private readonly string _origAppData;

    public SettingsTests()
    {
        // Redirect app-data paths to a temp folder so tests don't touch the real profile.
        _origAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(Path.GetTempPath(), "WinIslandTests-" + Guid.NewGuid().ToString("N"));
        Environment.SetEnvironmentVariable("WINISLAND_APPDATA", dir);
        Directory.CreateDirectory(dir);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("WINISLAND_APPDATA", null);
        Environment.SetEnvironmentVariable("APPDATA", _origAppData);
    }

    [Fact]
    public void Save_And_Load_Roundtrip()
    {
        var svc = new SettingsService();
        svc.Current.AccentColor = "#112233";
        svc.Current.CiderPort = 9999;
        svc.Current.OnlineLyricsEnabled = true;
        svc.Save();

        var svc2 = new SettingsService();
        Assert.Equal("#112233", svc2.Current.AccentColor);
        Assert.Equal(9999, svc2.Current.CiderPort);
        Assert.True(svc2.Current.OnlineLyricsEnabled);
    }

    [Fact]
    public void Export_Then_Import_Roundtrip()
    {
        var svc = new SettingsService();
        svc.Current.Theme = ThemeMode.Dark;
        svc.Current.Position = IslandPosition.Right;
        svc.Current.OffsetY = 42;
        svc.Save();

        var json = svc.Export();
        var svc2 = new SettingsService();
        Assert.True(svc2.TryImport(json));
        Assert.Equal(ThemeMode.Dark, svc2.Current.Theme);
        Assert.Equal(IslandPosition.Right, svc2.Current.Position);
        Assert.Equal(42, svc2.Current.OffsetY);
    }

    [Fact]
    public void Import_Invalid_Json_Fails()
    {
        var svc = new SettingsService();
        Assert.False(svc.TryImport("{ not valid json "));
    }
}
