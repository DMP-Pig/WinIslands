using System.Windows.Threading;

using System.IO;

namespace WinIslands.Services;

/// <summary>
/// Central media state machine. Polls providers (Cider API > SMTC > window title),
/// resolves artwork to local files, tracks the active source and publishes unified
/// <see cref="MediaSnapshot"/> updates to the UI thread.
/// </summary>
public sealed class MediaCoordinator : IDisposable
{
    private readonly SettingsService _settings;
    private readonly SmtcMediaProvider _smtc;
    private readonly CiderMediaProvider _cider;
    private readonly WindowTitleMediaProvider _title;
    private readonly Dispatcher _dispatcher;
    private readonly CancellationTokenSource _cts = new();
    private readonly SemaphoreSlim _tickLock = new(1, 1);
    private System.Threading.Timer? _timer;
    private int _tick;
    private double? _lastSystemVolume;

    public MediaCoordinator(SettingsService settings, SmtcMediaProvider smtc, CiderMediaProvider cider,
        WindowTitleMediaProvider title, Dispatcher dispatcher)
    {
        _settings = settings;
        _smtc = smtc;
        _cider = cider;
        _title = title;
        _dispatcher = dispatcher;
    }

    public MediaSnapshot? Current { get; private set; }

    /// <summary>Raised on the UI thread whenever the current snapshot changes.</summary>
    public event EventHandler<MediaSnapshot>? SnapshotChanged;

    /// <summary>Raised on the UI thread when there is no active media anymore.</summary>
    public event EventHandler? MediaEnded;
    /// <summary>可用媒体会话列表变化（多播放器选择器）。</summary>
    public event EventHandler? SessionsChanged;


    public void Start()
    {
        _ = _smtc.StartAsync(_cts.Token);
        _smtc.SessionsChanged += (_, _) => PublishSessions();
        _timer = new System.Threading.Timer(_ => _ = TickAsync(), null, TimeSpan.FromMilliseconds(400), TimeSpan.FromSeconds(1));
    }

    private async Task TickAsync()
    {
        if (!await _tickLock.WaitAsync(0)) return; // don't pile up ticks
        try
        {
            _tick++;

            // 1) Cider API (explicit preference for Cider)
            await _cider.EnsureConnectedAsync();
            var snapshot = await _cider.GetSnapshotAsync();

            // 2) SMTC global session（已有缓存曲目时走轻量路径，避免每秒重取媒体属性）
            if (snapshot is null)
            {
                await _smtc.PushAsync(useCachedTrack: true);
                // 无活跃会话时把快照视为 null：媒体应用退出后立即清除，不再保留旧曲目
                snapshot = _smtc.HasActiveSession ? _smtc.LastSnapshot : null;
            }

            // 3) Window-title fallback（无媒体时每 5 秒扫一次，降低空闲 CPU）
            if (snapshot is null && (_tick % 5 == 0))
            {
                snapshot = _title.GetSnapshot();
            }

            if (snapshot is null)
            {
                if (Current is not null)
                {
                    Current = null;
                    PublishEnd();
                }

                return;
            }

            // Normalize: drop sessions that are really stopped/closed.
            if (snapshot.Status is PlaybackStatus.Closed or PlaybackStatus.Stopped)
            {
                if (Current is not null && Current.Source == snapshot.Source)
                {
                    Current = null;
                    PublishEnd();
                }

                return;
            }

            // Resolve artwork: prefer a local cached file for remote URLs.
            snapshot = await ResolveArtworkAsync(snapshot);

            // Attach volume info.
            snapshot = await AttachVolumeAsync(snapshot);

            Current = snapshot;
            Publish(snapshot);
        }
        catch (Exception ex)
        {
            AppLogger.Error("MediaCoordinator tick failed", ex);
        }
        finally
        {
            _tickLock.Release();
        }
    }

    private async Task<MediaSnapshot> ResolveArtworkAsync(MediaSnapshot snapshot)
    {
        if (string.IsNullOrEmpty(snapshot.Track.ArtworkUrl) || !string.IsNullOrEmpty(snapshot.Track.ArtworkPath))
            return snapshot;

        var key = ArtworkCache.CacheKey("art", snapshot.Track.SourceAppId, snapshot.Track.Title, snapshot.Track.Artist, snapshot.Track.Album);
        var existing = Directory.EnumerateFiles(AppPaths.ThumbCacheDir, $"{key}.*").FirstOrDefault();
        var path = existing ?? await ArtworkCache.DownloadAsync(snapshot.Track.ArtworkUrl, key, _cts.Token);
        if (string.IsNullOrEmpty(path)) return snapshot;

        return snapshot with
        {
            Track = snapshot.Track with { ArtworkPath = path, ArtworkUrl = string.Empty },
        };
    }

    private async Task<MediaSnapshot> AttachVolumeAsync(MediaSnapshot snapshot)
    {
        if (snapshot.Source == MediaSourceKind.Cider)
        {
            var v = await _cider.GetVolumeAsync();
            return snapshot with { Volume = v, HasVolumeControl = v is not null };
        }

        if (_settings.Current.UseSystemVolume)
        {
            // Poll the system volume a few times per second at most.
            if (_tick % 3 == 0) _lastSystemVolume = SystemVolume.GetVolume();
            return snapshot with { Volume = _lastSystemVolume, HasVolumeControl = _lastSystemVolume is not null };
        }

        return snapshot with { Volume = null, HasVolumeControl = false };
    }

    private void Publish(MediaSnapshot snapshot)
    {
        if (_dispatcher.CheckAccess())
        {
            SnapshotChanged?.Invoke(this, snapshot);
        }
        else
        {
            _dispatcher.BeginInvoke(() => SnapshotChanged?.Invoke(this, snapshot));
        }
    }

    private void PublishEnd()
    {
        if (_dispatcher.CheckAccess())
        {
            MediaEnded?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            _dispatcher.BeginInvoke(() => MediaEnded?.Invoke(this, EventArgs.Empty));
        }
    }

    // ── Control (routed to the active source) ──────────────────

    public async Task<bool> TogglePlayPauseAsync()
    {
        if (Current?.Source == MediaSourceKind.Cider) return await _cider.TogglePlayPauseAsync();
        if (Current?.Source == MediaSourceKind.Smtc) return await _smtc.TogglePlayPauseAsync();
        return false;
    }

    public async Task<bool> PlayAsync()
    {
        if (Current?.Source == MediaSourceKind.Cider) return await _cider.PlayAsync();
        if (Current?.Source == MediaSourceKind.Smtc) return await _smtc.PlayAsync();
        return false;
    }

    public async Task<bool> PauseAsync()
    {
        if (Current?.Source == MediaSourceKind.Cider) return await _cider.PauseAsync();
        if (Current?.Source == MediaSourceKind.Smtc) return await _smtc.PauseAsync();
        return false;
    }
    public async Task<bool> NextAsync()
    {
        if (Current?.Source == MediaSourceKind.Cider) return await _cider.NextAsync();
        if (Current?.Source == MediaSourceKind.Smtc) return await _smtc.NextAsync();
        return false;
    }

    public async Task<bool> PreviousAsync()
    {
        if (Current?.Source == MediaSourceKind.Cider) return await _cider.PreviousAsync();
        if (Current?.Source == MediaSourceKind.Smtc) return await _smtc.PreviousAsync();
        return false;
    }

    public async Task<bool> SeekAsync(double seconds)
    {
        if (Current?.Source == MediaSourceKind.Cider) return await _cider.SeekAsync(seconds);
        if (Current?.Source == MediaSourceKind.Smtc) return await _smtc.SeekAsync(seconds);
        return false;
    }

    public async Task<bool> SetVolumeAsync(double volume01)
    {
        if (Current?.Source == MediaSourceKind.Cider) return await _cider.SetVolumeAsync(volume01);
        if (_settings.Current.UseSystemVolume)
        {
            SystemVolume.SetVolume(volume01);
            _lastSystemVolume = Math.Clamp(volume01, 0, 1);
            return true;
        }

        return false;
    }

    /// <summary>Force an immediate refresh (e.g. after user interaction).</summary>
    public Task RefreshNowAsync() => TickAsync();


    /// <summary>当前可用媒体会话（SMTC 全部 + Cider 伪会话优先），供多播放器选择器。</summary>
    public IReadOnlyList<MediaSessionInfo> GetAvailableSessions()
    {
        var list = _smtc.GetSessions().ToList();
        // Cider 已连接时作为最高优先级伪会话（允许用户切回 Cider）
        if (_cider.IsEnabled && _cider.Client.IsConnected)
        {
            var isCurrent = Current?.Source == MediaSourceKind.Cider;
            list.Insert(0, new MediaSessionInfo("Cider", "Cider",
                isCurrent ? PlaybackStatus.Playing : PlaybackStatus.Opened, isCurrent));
        }
        return list;
    }

    /// <summary>切换到指定媒体会话（多播放器选择器）。</summary>
    public async Task<bool> SwitchSessionAsync(string appId)
    {
        if (string.IsNullOrWhiteSpace(appId)) return false;
        if (string.Equals(appId, "Cider", StringComparison.OrdinalIgnoreCase))
        {
            // Cider 始终是最高优先级来源：立即刷新一次即可生效
            await RefreshNowAsync();
            return _cider.IsEnabled && _cider.Client.IsConnected;
        }

        var ok = _smtc.SwitchSession(appId);
        if (ok) await RefreshNowAsync();
        return ok;
    }

    private void PublishSessions()
    {
        if (_dispatcher.CheckAccess())
            SessionsChanged?.Invoke(this, EventArgs.Empty);
        else
            _dispatcher.BeginInvoke(() => SessionsChanged?.Invoke(this, EventArgs.Empty));
    }

    public void Dispose()
    {
        _cts.Cancel();
        _timer?.Dispose();
        _smtc.Dispose();
        _cider.Dispose();
    }
}




