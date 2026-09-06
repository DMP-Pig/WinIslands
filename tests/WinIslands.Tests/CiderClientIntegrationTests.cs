using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>
/// Cider 本地 API 集成测试：用进程内 TcpListener 模拟 Cider V3 HTTP 服务，
/// 验证客户端的连接探测、快照解析、歌词获取、播放控制与音量读取。
/// 纯本地回环，不依赖 GUI / 真实 Cider。
/// </summary>
public sealed class CiderClientIntegrationTests : IDisposable
{
    private readonly MockCiderServer _server;
    private readonly CiderClient _client;

    public CiderClientIntegrationTests()
    {
        _server = new MockCiderServer();
        _client = new CiderClient(token: "test-token");
    }

    public void Dispose() => _server.Dispose();

    [Fact]
    public async Task Connects_V3_And_Reads_Snapshot_And_Lyrics()
    {
        var ok = await _client.ConnectAsync(_server.Port);
        Assert.True(ok, _client.LastError);
        Assert.True(_client.IsConnected);
        Assert.Equal(CiderApiProfile.V3, _client.Profile);

        // 快照：曲目 + 显式播放状态
        var snap = await _client.GetSnapshotAsync();
        Assert.NotNull(snap);
        Assert.Equal("Mock Song 测试", snap!.Track.Title);
        Assert.Equal("Mock Artist", snap.Track.Artist);
        Assert.Equal("Mock Album", snap.Track.Album);
        Assert.Equal(PlaybackStatus.Playing, snap.Status);
        Assert.True(snap.HasVolumeControl);

        // 歌词：模拟返回纯 LRC
        var lrc = await _client.GetLyricsAsync();
        Assert.NotNull(lrc);
        Assert.Contains("作词：林夕", lrc);
        Assert.Contains("[00:03.00]", lrc);
    }

    [Fact]
    public async Task Control_And_Volume_Endpoints_Are_Called()
    {
        await _client.ConnectAsync(_server.Port);

        Assert.True(await _client.TogglePlayPauseAsync());
        Assert.True(await _client.NextAsync());
        Assert.True(await _client.PreviousAsync());
        Assert.True(await _client.PlayAsync());
        Assert.True(await _client.PauseAsync());
        Assert.True(await _client.SeekAsync(42.5));

        var vol = await _client.GetVolumeAsync();
        Assert.Equal(0.7, vol);
        Assert.True(await _client.SetVolumeAsync(0.35));

        var posted = _server.Posts.ToArray();
        Assert.Contains("/api/v1/playback/playpause", posted);
        Assert.Contains("/api/v1/playback/next", posted);
        Assert.Contains("/api/v1/playback/previous", posted);
        Assert.Contains("/api/v1/playback/play", posted);
        Assert.Contains("/api/v1/playback/pause", posted);
        Assert.Contains("/api/v1/playback/seek", posted);
        Assert.Contains("/api/v1/playback/volume", posted);
    }

    [Fact]
    public async Task Auth_Header_Sent_When_Token_Configured()
    {
        await _client.ConnectAsync(_server.Port);
        await _client.GetSnapshotAsync();
        Assert.Contains("apptoken: test-token", _server.HeadersSeen, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>极简 HTTP 服务器（TcpListener），记录请求路径与头，模拟 Cider V3 端点。</summary>
    private sealed class MockCiderServer : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _loop;

        public int Port { get; }
        public ConcurrentQueue<string> Posts { get; } = new();
        public ConcurrentQueue<string> HeadersSeen { get; } = new();

        public MockCiderServer()
        {
            _listener = new TcpListener(IPAddress.Loopback, 0);
            _listener.Start();
            Port = ((IPEndPoint)_listener.LocalEndpoint).Port;
            _loop = Task.Run(AcceptLoop);
        }

        private async Task AcceptLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                TcpClient client;
                try { client = await _listener.AcceptTcpClientAsync(); }
                catch { break; }
                _ = Task.Run(() => Handle(client));
            }
        }

        private async Task Handle(TcpClient client)
        {
            try
            {
                using (client)
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8, false, 4096, leaveOpen: true))
                {
                    var requestLine = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(requestLine)) return;
                    var parts = requestLine.Split(' ');
                    var method = parts[0];
                    var path = parts.Length > 1 ? parts[1] : "/";
                    // 读请求头直到空行
                    string? line;
                    while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync()))
                    {
                        var idx = line.IndexOf(':');
                        if (idx > 0) HeadersSeen.Enqueue(line[..idx].Trim() + ": " + line[(idx + 1)..].Trim());
                    }
                    if (method == "POST") Posts.Enqueue(path.Split('?')[0]);

                    var body = Respond(path.Split('?')[0]);
                    var bodyBytes = Encoding.UTF8.GetBytes(body);
                    var header = "HTTP/1.1 200 OK\r\nContent-Type: application/json\r\nContent-Length: " + bodyBytes.Length + "\r\nConnection: close\r\n\r\n";
                    var headerBytes = Encoding.UTF8.GetBytes(header);
                    await stream.WriteAsync(headerBytes);
                    await stream.WriteAsync(bodyBytes);
                    await stream.FlushAsync();
                }
            }
            catch { /* 客户端断开等 */ }
        }

        private static string Respond(string path)
        {
            switch (path)
            {
                case "/api/v1/playback/active":
                    return "{}";
                case "/api/v1/playback/now-playing":
                    return """{"status":"ok","data":{"info":{"name":"Mock Song 测试","artistName":"Mock Artist","albumName":"Mock Album","durationInMillis":180000,"currentPlaybackTime":12.5,"isPlaying":true,"status":"playing","hasLyrics":true}}}""";
                case "/api/v1/playback/volume":
                    return """{"volume":0.7}""";
                case "/api/v1/lyrics/current":
                case "/api/v1/lyrics":
                    return "[00:01.00]作词：林夕\n[00:03.00]line one 第一行\n[00:06.00]line two 第二行\n";
                default:
                    return """{"ok":true}""";
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            try { _listener.Stop(); } catch { /* ignore */ }
        }
    }
}
