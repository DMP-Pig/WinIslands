using WinIslands.Services;

namespace WinIslands.Tests;

public class WindowTitleParsingTests
{
    [Theory]
    [InlineData("Artist - Title", "Artist", "Title")]
    [InlineData("  Artist – Title  ", "Artist", "Title")]
    [InlineData("Artist — Title", "Artist", "Title")]
    [InlineData("NoDashHere", "", "NoDashHere")]
    [InlineData("Spotify - Artist - Title", "Artist", "Title")]
    public void ParseTitle_Variants(string input, string artist, string title)
    {
        var (a, t) = WindowTitleMediaProvider.ParseTitle(input);
        Assert.Equal(artist, a);
        Assert.Equal(title, t);
    }
}
