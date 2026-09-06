namespace WinIslands.Services;

/// <summary>Which provider the current media snapshot came from.</summary>
public enum MediaSourceKind
{
    None = 0,
    Smtc = 1,        // Windows global media session (SMTC)
    Cider = 2,       // Cider local HTTP API
    WindowTitle = 3, // window-title + process heuristics (fallback)
}

/// <summary>Playback status mapped from SMTC / Cider into one vocabulary.</summary>
public enum PlaybackStatus
{
    Closed = 0,
    Opened = 1,
    Changing = 2,
    Stopped = 3,
    Playing = 4,
    Paused = 5,
}

/// <summary>Immutable track metadata.</summary>
public sealed record TrackInfo(
    string Title,
    string Artist,
    string Album,
    string AlbumArtist,
    string SourceAppName,       // e.g. "Spotify"
    string SourceAppId,         // e.g. Spotify.exe.Spotify or Cider
    string ArtworkPath,         // local cached file path ("" if none)
    string ArtworkUrl,          // remote URL ("" if none / local)
    TimeSpan Duration)
{
    public bool HasArtwork => !string.IsNullOrEmpty(ArtworkPath);
}

/// <summary>
/// A point-in-time snapshot of what is playing and how. Providers produce these;
/// <see cref="MediaCoordinator"/> merges them into one current state.
/// </summary>
public sealed record MediaSnapshot
{
    public required TrackInfo Track { get; init; }
    public required MediaSourceKind Source { get; init; }
    public PlaybackStatus Status { get; init; }
    public double PositionSeconds { get; init; }
    public double DurationSeconds { get; init; }
    public double? Volume { get; init; }          // 0..1, null = unknown
    public bool CanPlayPause { get; init; } = true;
    public bool CanNext { get; init; } = true;
    public bool CanPrevious { get; init; } = true;
    public bool CanSeek { get; init; } = true;
    public bool HasVolumeControl { get; init; }
    public bool HasLyrics { get; init; }
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.Now;

    public string SourceLabel => Source switch
    {
        MediaSourceKind.Smtc => "SMTC",
        MediaSourceKind.Cider => "Cider",
        MediaSourceKind.WindowTitle => "Window",
        _ => string.Empty,
    };
}

/// <summary>Immutable helper for the coordinator: what changed in an update.</summary>
[Flags]
public enum MediaChangeKind
{
    None = 0,
    Track = 1,       // different song started
    Status = 2,      // play/pause changed
    Position = 4,    // progress tick
    Artwork = 8,
    Lyrics = 16,
    Source = 32,
    All = Track | Status | Position | Artwork | Lyrics | Source,
}

