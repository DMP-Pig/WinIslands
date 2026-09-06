namespace WinIslands.Services;

/// <summary>
/// Session-level wrapper around <see cref="CiderClient"/>: owns connection lifecycle
/// (reconnect, backoff) and surfaces snapshots. When Cider is not reachable it simply
/// returns null so the coordinator falls back to SMTC / window-title.
/// </summary>
public sealed class CiderMediaProvider : IDisposable
{
    private readonly SettingsService _settings;
    private readonly CancellationTokenSource _cts = new();
    private readonly SemaphoreSlim _connectLock = new(1, 1);
    private DateTime _lastConnectAttempt;
    private bool _reconnecting;

    public CiderClient Client { get; } = new();

    public CiderMediaProvider(SettingsService settings) => _settings = settings;

    public bool IsEnabled => _settings.Current.CiderEnabled;

    /// <summary>媒体程序列表里若禁用 Cider（Cider.exe），则不检测。</summary>
    private bool IsEnabledByMediaApps()
    {
        var list = _settings.Current.MediaApps;
        if (list is null || list.Count == 0) return true;
        foreach (var e in list)
            if (string.Equals(e.Key, "Cider.exe", StringComparison.OrdinalIgnoreCase)) return e.Enabled;
        return true;
    }

    /// <summary>Try to connect (once per few seconds at most).</summary>
    public async Task EnsureConnectedAsync()
    {
        if (!IsEnabled || Client.IsConnected) return;
        if (_reconnecting) return;
        if (DateTime.UtcNow - _lastConnectAttempt < TimeSpan.FromSeconds(5)) return;

        await _connectLock.WaitAsync();
        try
        {
            if (Client.IsConnected) return;
            _reconnecting = true;
            _lastConnectAttempt = DateTime.UtcNow;
            var s = _settings.Current;
            // 用户未手动填写 Token 时，自动从 Cider 配置读取（零配置）
            var token = !string.IsNullOrWhiteSpace(s.CiderToken)
                ? s.CiderToken
                : CiderTokenAutoDetect.TryGetToken() ?? string.Empty;
            Client.SetToken(token);
            await Client.ConnectAsync(s.CiderPort, _cts.Token);
        }
        finally
        {
            _reconnecting = false;
            _connectLock.Release();
        }
    }

    public async Task<MediaSnapshot?> GetSnapshotAsync()
    {
        if (!IsEnabled || !IsEnabledByMediaApps() || !Client.IsConnected) return null;

        var snap = await Client.GetSnapshotAsync(_cts.Token);
        if (snap is null)
        {
            // Connection may have dropped; force a re-probe on next tick.
            Client.MarkDisconnected();
            await EnsureConnectedAsync();
            return null;
        }

        return snap;
    }

    public Task<bool> TogglePlayPauseAsync() => Client.TogglePlayPauseAsync(_cts.Token);
    public Task<bool> PlayAsync() => Client.PlayAsync(_cts.Token);
    public Task<bool> PauseAsync() => Client.PauseAsync(_cts.Token);
    public Task<bool> NextAsync() => Client.NextAsync(_cts.Token);
    public Task<bool> PreviousAsync() => Client.PreviousAsync(_cts.Token);
    public Task<bool> SeekAsync(double seconds) => Client.SeekAsync(seconds, _cts.Token);
    public Task<double?> GetVolumeAsync() => Client.GetVolumeAsync(_cts.Token);
    public Task<bool> SetVolumeAsync(double v) => Client.SetVolumeAsync(v, _cts.Token);
    public Task<string?> GetLyricsAsync() => Client.GetLyricsAsync(null, _cts.Token);

    public void Dispose() => _cts.Cancel();
}

