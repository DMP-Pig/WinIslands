using WinIslands.Services;

namespace WinIslands.Tests;

public class CiderParsingTests
{
    [Fact]
    public void Parses_V3_Now_Playing()
    {
        var client = new CiderClient();
        var json = """
        {
          "status": "ok",
          "data": {
            "info": {
              "name": "Like Water",
              "artistName": "Flume",
              "albumName": "Skin",
              "artwork": { "url": "https://example.com/{w}x{h}bb.jpg", "width": 600, "height": 600 },
              "durationInMillis": 193633,
              "currentPlaybackTime": 12.5,
              "isPlaying": true,
              "status": "playing",
              "hasLyrics": true
            }
          }
        }
        """;

        var snap = client.ParseV3NowPlaying(json);

        Assert.NotNull(snap);
        Assert.Equal("Like Water", snap!.Track.Title);
        Assert.Equal("Flume", snap.Track.Artist);
        Assert.Equal("Skin", snap.Track.Album);
        Assert.Equal("https://example.com/320x320bb.jpg", snap.Track.ArtworkUrl);
        Assert.Equal(PlaybackStatus.Playing, snap.Status);
        Assert.Equal(12.5, snap.PositionSeconds, 2);
        Assert.Equal(193.633, snap.DurationSeconds, 3);
        Assert.True(snap.HasVolumeControl);
        Assert.True(snap.HasLyrics);
    }

    [Fact]
    public void Does_Not_Infer_Playing_From_RemainingTime()
    {
        var client = new CiderClient();
        var json = """
        {
          "status": "ok",
          "data": {
            "info": {
              "name": "Paused Song",
              "artistName": "Artist",
              "albumName": "Album",
              "durationInMillis": 200000,
              "currentPlaybackTime": 100.0,
              "remainingTime": 100.0
            }
          }
        }
        """;

        var snap = client.ParseV3NowPlaying(json, out var statusExplicit);

        Assert.NotNull(snap);
        Assert.False(statusExplicit); // 无显式 isPlaying/status 字段
        // 暂停中的歌 remainingTime 也 > 0，绝不能因此误判为播放
        Assert.Equal(PlaybackStatus.Paused, snap!.Status);
    }
    [Fact]
    public void Parses_Empty_Now_Playing_As_Null()
    {
        var client = new CiderClient();
        Assert.Null(client.ParseV3NowPlaying("{}"));
        Assert.Null(client.ParseV3NowPlaying("{\"status\":\"ok\",\"data\":{}}"));
    }

    [Fact]
    public void Parses_Volume_Text_And_Json()
    {
        Assert.Equal(0.8, CiderClient.ParseVolume("0.8"));
        Assert.Equal(0.5, CiderClient.ParseVolume("{\"volume\":0.5}"));
        Assert.Equal(0.25, CiderClient.ParseVolume("{\"data\":{\"volume\":0.25}}"));
        Assert.Null(CiderClient.ParseVolume("abc"));
    }

    [Fact]
    public void Extracts_Lrc_From_Cider_Json()
    {
        var json = """
        {
          "status": "ok",
          "data": {
            "lyrics": [
              { "time": 1.5, "text": "first line" },
              { "time": 4.2, "text": "second line" }
            ]
          }
        }
        """;

        var lrc = CiderClient.TryExtractLrc(json);
        Assert.NotNull(lrc);
        Assert.Contains("[00:01.50]first line", lrc);
        Assert.Contains("[00:04.20]second line", lrc);
    }

    [Fact]
    public void Extracts_Plain_Lrc_Passthrough()
    {
        var text = "[00:01.00]hello";
        Assert.Equal(text, CiderClient.TryExtractLrc(text));
    }
}
