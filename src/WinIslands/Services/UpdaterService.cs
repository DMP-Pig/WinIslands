using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinIslands.Services;

/// <summary>
/// 自更新检查：访问 GitHub Releases 最新版，与当前版本比较。
/// 仅在你手动开启（或点击“立即检查”）时联网，不上报任何数据。
/// </summary>
public sealed class UpdaterService
{
    /// <summary>GitHub 仓库标识（账号改名前/后两个名称都可用，按顺序尝试）。</summary>
    private static readonly string[] RepoCandidates = { "JudeKwong/WinIslands", "DMP-Pig/WinIslands" };

    /// <summary>最近一次成功解析到的仓库，用于拼接回退链接。</summary>
    public static string Repo { get; private set; } = RepoCandidates[0];
    private readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(10) };

    public event Action<string, string>? NewVersionAvailable; // version, url

    /// <summary>检查最新版本；返回 true 表示存在新版本。任何网络异常都安全返回 false。</summary>
    public async Task<bool> CheckAsync()
    {
        // 逐个候选仓库尝试：账号改名后旧名会 404，新名可用；两者都试可避免更新检查静默失效。
        foreach (var repo in RepoCandidates)
        {
            try
            {
                var json = await _http.GetStringAsync($"https://api.github.com/repos/{repo}/releases/latest");
                using var doc = JsonDocument.Parse(json);
                var tag = doc.RootElement.TryGetProperty("tag_name", out var t) ? t.GetString() : "";
                var url = doc.RootElement.TryGetProperty("html_url", out var u) ? u.GetString() : "";
                if (string.IsNullOrWhiteSpace(tag)) continue;
                var latest = ParseVersion(tag);
                var current = ParseVersion(CurrentVersion());
                if (latest <= 0 || current <= 0) continue;
                Repo = repo; // 记录可用仓库，回退链接使用它
                if (latest > current)
                {
                    NewVersionAvailable?.Invoke(tag, url ?? $"https://github.com/{repo}/releases");
                    return true;
                }
                return false; // 已确认最新版本，无需再试其他候选
            }
            catch (Exception ex)
            {
                AppLogger.Debug($"Update check failed for {repo}: {ex.Message}");
            }
        }
        return false;
    }

    /// <summary>把 1.2.3 或 1.2.3-beta1 解析为数值（忽略预发布后缀）。</summary>
    private static long ParseVersion(string tag)
    {
        var t = (tag ?? string.Empty).Trim().TrimStart('v', 'V');
        var dash = t.IndexOf('-');
        if (dash > 0) t = t.Substring(0, dash);
        var parts = t.Split('.');
        long v = 0;
        for (var i = 0; i < 3 && i < parts.Length; i++)
        {
            if (!long.TryParse(parts[i], out var n)) return 0;
            v = v * 1000 + n;
        }
        return v;
    }

    public static string CurrentVersion()
        => typeof(UpdaterService).Assembly.GetName().Version?.ToString(3) ?? "0.0.0";
}
