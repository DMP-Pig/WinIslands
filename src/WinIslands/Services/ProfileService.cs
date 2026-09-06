using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WinIslands.Services;

/// <summary>
/// 配置档案：把整套设置另存为命名档案（profiles 目录），随时切换。
/// 「Default」始终存在；删除当前档案会回到 Default。
/// </summary>
public sealed class ProfileService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly SettingsService _settings;
    private readonly string _dir;

    public ProfileService(SettingsService settings)
    {
        _settings = settings;
        _dir = Path.Combine(AppPaths.AppDataDir, "profiles");
        Directory.CreateDirectory(_dir);
    }

    public string ActiveProfile => _settings.Current.ActiveProfile;

    /// <summary>所有档案名（不含扩展名），Default 始终在列表首位。</summary>
    public IReadOnlyList<string> List()
    {
        var names = Directory.Exists(_dir)
            ? Directory.GetFiles(_dir, "*.json").Select(Path.GetFileNameWithoutExtension)
                .Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => n!).OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList()
            : new List<string>();
        if (!names.Contains("Default", StringComparer.OrdinalIgnoreCase))
            names.Insert(0, "Default");
        return names;
    }

    /// <summary>把当前设置另存为档案并切到该档案。</summary>
    public void SaveCurrentAs(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        name = Sanitize(name);
        var path = Path.Combine(_dir, name + ".json");
        var clone = _settings.Current.Clone();
        clone.ActiveProfile = name;
        File.WriteAllText(path, JsonSerializer.Serialize(clone, JsonOptions));
        _settings.Update(s => s.ActiveProfile = name);
    }

    /// <summary>切换到指定档案（不存在则忽略）。</summary>
    public void Load(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;
        var path = Path.Combine(_dir, Sanitize(name) + ".json");
        if (!File.Exists(path)) return;
        try
        {
            var parsed = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path), JsonOptions);
            if (parsed is null) return;
            parsed.ActiveProfile = Sanitize(name);
            _settings.Apply(parsed);
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Profile load failed: {ex.Message}");
        }
    }

    /// <summary>删除档案；若删除的是当前档案，自动切回 Default 并重置。</summary>
    public void Delete(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || string.Equals(name, "Default", StringComparison.OrdinalIgnoreCase)) return;
        var path = Path.Combine(_dir, Sanitize(name) + ".json");
        try { if (File.Exists(path)) File.Delete(path); } catch (Exception ex) { AppLogger.Warn($"Profile delete: {ex.Message}"); }
        if (string.Equals(_settings.Current.ActiveProfile, name, StringComparison.OrdinalIgnoreCase))
            _settings.Update(s => s.ActiveProfile = "Default");
    }

    private static string Sanitize(string name)
        => string.Concat((name ?? "").Trim().Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
}
