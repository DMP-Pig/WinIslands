using System.IO;
using System.Text.Json;

namespace WinIslands.Services;

/// <summary>
/// 轻量播放状态持久化（%APPDATA%\WinIslands\state.json）：
/// 保存最近一次「曲目 + 位置 + 状态」，供下次启动恢复。
/// 解决暂停后退出再打开时，因 Cider/SMTC 位置到达慢而先显示 0、再跳回暂停点的问题。
/// </summary>
public sealed class PlaybackStateStore
{
    public string? TrackKey { get; set; }
    public double PositionSeconds { get; set; }
    public string? Status { get; set; } // "Playing" / "Paused"
    public DateTime SavedAtUtc { get; set; }

    public static string FilePath => Path.Combine(AppPaths.AppDataDir, "state.json");

    public static PlaybackStateStore? Load()
    {
        try
        {
            if (!File.Exists(FilePath)) return null;
            var json = File.ReadAllText(FilePath);
            var state = JsonSerializer.Deserialize<PlaybackStateStore>(json);
            if (state is null || string.IsNullOrEmpty(state.TrackKey) || state.PositionSeconds <= 0) return null;
            // 过期超过 1 小时不恢复（避免旧曲目位置串台）
            if (DateTime.UtcNow - state.SavedAtUtc > TimeSpan.FromHours(1)) return null;
            return state;
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Playback state load failed: {ex.Message}");
            return null;
        }
    }

    public void Save()
    {
        try
        {
            SavedAtUtc = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Playback state save failed: {ex.Message}");
        }
    }
}
