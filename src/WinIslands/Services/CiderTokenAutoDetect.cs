using System.IO;

namespace WinIslands.Services;

/// <summary>
/// 从 Cider 的本地配置自动读取 API Token（零配置）。
/// Cider 2.x 将外部控制 Token 存在 %APPDATA%\sh.cider.dotnet\spa-config.yml 的
/// connectivity 段下。若用户已手动填写 Token，则以手动为准。
/// </summary>
public static class CiderTokenAutoDetect
{
    public static string? TryGetToken()
    {
        try
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dirs = new[] { Path.Combine(appData, "sh.cider.dotnet"), Path.Combine(appData, "Cider") };
            foreach (var dir in dirs)
            {
                foreach (var file in new[] { "spa-config.yml", "client-options.yml" })
                {
                    var path = Path.Combine(dir, file);
                    if (!File.Exists(path)) continue;
                    var token = ReadToken(path);
                    if (!string.IsNullOrEmpty(token)) return token;
                }
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Cider token auto-detect failed: {ex.Message}");
        }

        return null;
    }

    private static string? ReadToken(string path)
    {
        foreach (var line in File.ReadAllLines(path))
        {
            var t = line.Trim();
            if (!t.StartsWith("token:", StringComparison.OrdinalIgnoreCase)) continue;
            var v = t["token:".Length..].Trim().Trim('"', '\'');
            if (v.Length > 0) return v;
        }

        return null;
    }
}
