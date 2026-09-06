using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>Regression tests for the local .lrc matcher (spaces/case in file names).</summary>
public class LyricsServiceTests
{
    private static string NewDir(string sub) =>
        Path.Combine(Path.GetTempPath(), "WinIslandsLyricsTests-" + Guid.NewGuid().ToString("N"), sub);

    [Fact]
    public void Finds_Lrc_With_Spaces_And_Case()
    {
        // 文件名带空格/大小写，曲目信息正常（修复前按去空格模式匹配会失败）
        var dir = NewDir("Lyrics");
        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, "Demo Artist - Demo Song.lrc");
        File.WriteAllText(file, "[00:01.00]hello");

        var settings = new SettingsService { };
        settings.Current.LyricsFolder = dir;
        var svc = new LyricsService(settings, cider: null);
        var track = new TrackInfo("Demo Song", "Demo Artist", "", "", "x", "x", "", "", TimeSpan.Zero);

        Assert.Equal(file, svc.FindLocalLrc(track));
    }

    [Fact]
    public void Finds_Lrc_By_Title_Only()
    {
        var dir = NewDir("Lyrics");
        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, "songtitle.lrc");
        File.WriteAllText(file, "[00:01.00]hello");

        var settings = new SettingsService();
        settings.Current.LyricsFolder = dir;
        var svc = new LyricsService(settings, cider: null);
        var track = new TrackInfo("SongTitle", "Artist", "", "", "x", "x", "", "", TimeSpan.Zero);

        var found = svc.FindLocalLrc(track);
        Assert.True(found is not null, $"found=null lyricsFolder={settings.Current.LyricsFolder} expected={file}");
        Assert.Equal(file, found);
    }

    [Fact]
    public void Uses_Configured_Lyrics_Folder()
    {
        var dir = NewDir("custom");
        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, "Artist-Title-Album.lrc");
        File.WriteAllText(file, "[00:01.00]hello");

        var settings = new SettingsService();
        settings.Current.LyricsFolder = dir;
        var svc = new LyricsService(settings, cider: null);
        var track = new TrackInfo("Title", "Artist", "Album", "", "x", "x", "", "", TimeSpan.Zero);

        Assert.Equal(file, svc.FindLocalLrc(track));
    }

    [Fact]
    public void Returns_Null_When_No_Match()
    {
        var settings = new SettingsService();
        var svc = new LyricsService(settings, cider: null);
        var track = new TrackInfo("No Such Song Anywhere", "Nobody", "", "", "x", "x", "", "", TimeSpan.Zero);

        Assert.Null(svc.FindLocalLrc(track));
    }
}
