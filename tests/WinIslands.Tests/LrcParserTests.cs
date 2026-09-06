using WinIslands.Services;

namespace WinIslands.Tests;

public class LrcParserTests
{
    [Fact]
    public void Parses_Basic_Timestamps()
    {
        var lrc = "[00:12.00]Hello world\n[00:15.50]Second line";
        var doc = LrcParser.Parse(lrc);

        Assert.False(doc.IsEmpty);
        Assert.Equal(2, doc.Lines.Count);
        Assert.Equal("Hello world", doc.Lines[0].Text);
        Assert.Equal(TimeSpan.FromSeconds(12), doc.Lines[0].Time);
        Assert.Equal(TimeSpan.FromSeconds(15.5), doc.Lines[1].Time);
    }

    [Fact]
    public void Parses_Multiple_Timestamps_Per_Line()
    {
        var lrc = "[00:01.00][00:10.00]Repeat me";
        var doc = LrcParser.Parse(lrc);

        Assert.Equal(2, doc.Lines.Count);
        Assert.All(doc.Lines, l => Assert.Equal("Repeat me", l.Text));
    }

    [Fact]
    public void Parses_Hour_Format()
    {
        var lrc = "[01:02:03.50]Long track";
        var doc = LrcParser.Parse(lrc);

        Assert.Single(doc.Lines);
        Assert.Equal(TimeSpan.FromHours(1) + TimeSpan.FromMinutes(2) + TimeSpan.FromSeconds(3) + TimeSpan.FromMilliseconds(500),
            doc.Lines[0].Time);
    }

    [Fact]
    public void Parses_Metadata_And_Offset()
    {
        var lrc = "[ti:Title]\n[ar:Artist]\n[al:Album]\n[offset:500]\n[00:01.00]line";
        var doc = LrcParser.Parse(lrc);

        Assert.Equal("Title", doc.Title);
        Assert.Equal("Artist", doc.Artist);
        Assert.Equal("Album", doc.Album);
        Assert.Equal(500, doc.OffsetMs);
        Assert.Single(doc.Lines);
    }

    [Fact]
    public void IndexAt_Finds_Active_Line()
    {
        var lrc = "[00:01.00]one\n[00:05.00]two\n[00:10.00]three";
        var doc = LrcParser.Parse(lrc);

        Assert.Equal(-1, doc.IndexAt(TimeSpan.Zero));
        Assert.Equal(0, doc.IndexAt(TimeSpan.FromSeconds(1)));
        Assert.Equal(1, doc.IndexAt(TimeSpan.FromSeconds(5)));
        Assert.Equal(2, doc.IndexAt(TimeSpan.FromSeconds(30)));
    }

    [Fact]
    public void Empty_Input_Is_Empty()
    {
        Assert.True(LrcParser.Parse("").IsEmpty);
        Assert.True(LrcParser.Parse("   \n  ").IsEmpty);
        Assert.True(LrcParser.Parse("plain text without timestamps").IsEmpty);
    }

    [Fact]
    public void PairLines_Merges_Adjacent_Translation()
    {
        LyricLine[] lines =
        {
            new LyricLine(TimeSpan.FromSeconds(1), "Hello"),
            new LyricLine(TimeSpan.FromMilliseconds(1150), "你好"),
            new LyricLine(TimeSpan.FromSeconds(5), "World"),
        };

        var pairs = LrcParser.PairLines(lines, TimeSpan.FromMilliseconds(250), true);

        Assert.Equal(2, pairs.Count);
        Assert.True(pairs[0].HasTranslation);
        Assert.Equal("你好", pairs[0].Translation);
        Assert.Equal("Hello", pairs[0].Main.Text);
        Assert.False(pairs[1].HasTranslation);
        Assert.Equal("World", pairs[1].Main.Text);
    }

    [Fact]
    public void PairLines_Disabled_Or_Far_Apart_Keeps_Every_Line()
    {
        LyricLine[] lines =
        {
            new LyricLine(TimeSpan.FromSeconds(1), "one"),
            new LyricLine(TimeSpan.FromSeconds(10), "two"),
            new LyricLine(TimeSpan.FromSeconds(11), "two"),
        };

        var disabled = LrcParser.PairLines(lines, TimeSpan.FromMilliseconds(250), false);
        Assert.Equal(3, disabled.Count);
        Assert.All(disabled, p => Assert.False(p.HasTranslation));

        var enabled = LrcParser.PairLines(lines, TimeSpan.FromMilliseconds(250), true);
        Assert.Equal(3, enabled.Count);
        Assert.All(enabled, p => Assert.False(p.HasTranslation));
    }
}
