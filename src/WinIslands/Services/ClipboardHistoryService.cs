using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;

namespace WinIslands.Services;

/// <summary>一条剪贴板记录。</summary>
public sealed record ClipboardEntry(DateTime Time, string Text)
{
    public string TextPreview => Text.Length > 40 ? Text.Substring(0, 40) + "…" : Text;
}

/// <summary>
/// 剪贴板历史：轮询系统剪贴板文本（间隔 900ms，仅启用时运行），
/// 去重并保留最近 N 条，持久化到本机 JSON。内容绝不上传。
/// </summary>
public sealed class ClipboardHistoryService : IDisposable
{
    private const string DefaultFile = "clipboard-history.json";
    private readonly string _file;
    private readonly DispatcherTimer _timer;
    private readonly List<ClipboardEntry> _entries = new();
    private string _last = string.Empty;
    private bool _enabled;    // 是否记录剪贴板历史
    private bool _polling;     // 独立轮询开关（复制提示不需要历史记录也能检测复制）

    public ClipboardHistoryService()
    {
        _file = Path.Combine(AppPaths.AppDataDir, DefaultFile);
        Load();
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(900) };
        _timer.Tick += (_, _) => Poll();
    }

    /// <summary>最新在前。</summary>
    public IReadOnlyList<ClipboardEntry> Entries
    {
        get { lock (_entries) return _entries.ToList(); }
    }

    /// <summary>组件显示用的简短摘要（如：剪贴板 · 3 条）。</summary>
    public string Summary
    {
        get { lock (_entries) return _entries.Count == 0 ? string.Empty : _entries.Count.ToString(); }
    }

    public event Action? Changed;

    /// <summary>检测到新复制的文本（无论是否记录历史都会触发，供「已复制/验证码/复制进度」提示使用）。</summary>
    public event Action<ClipboardEntry>? EntryAdded;

    /// <summary>保留条数上限（由设置同步，默认 15）。</summary>
    public int MaxEntries { get; set; } = 15;

    public void SetEnabled(bool enabled)
    {
        _enabled = enabled;
        UpdatePolling();
        if (enabled)
        {
            BaselineClipboard();
            Poll();
        }
    }

    /// <summary>设置是否轮询剪贴板（与历史记录解耦：复制提示开启时也需轮询）。</summary>
    public void SetPolling(bool polling)
    {
        _polling = polling;
        if (polling) BaselineClipboard();
        UpdatePolling();
    }

    /// <summary>
    /// 启动基线：把剪贴板当前已有内容视为「已知」，避免应用启动/开启轮询时
    /// 把启动前就已存在的剪贴板内容误判为新复制，弹出多余的「已复制」提示。
    /// 仅当尚未建立基线（_last 为空）时读取一次；之后真正的复制仍会正常触发。
    /// </summary>
    private void BaselineClipboard()
    {
        try
        {
            var text = System.Windows.Clipboard.ContainsText() ? (System.Windows.Clipboard.GetText() ?? string.Empty) : string.Empty;
            var baseline = ComputeBaseline(_last, text);
            if (baseline is not null) _last = baseline;
        }
        catch
        {
            // 剪贴板被占用等：忽略，保持空基线（首次真实复制仍会正常触发）
        }
    }

    /// <summary>
    /// 基线判定（纯逻辑，可测试）：仅当尚未建立基线且剪贴板当前有合法文本时返回新基线，
    /// 否则返回 null（保持原值）。防止启动时把已有内容误判为新复制。
    /// </summary>
    internal static string? ComputeBaseline(string currentLast, string clipboardText)
    {
        if (currentLast.Length > 0) return null;
        if (string.IsNullOrWhiteSpace(clipboardText) || clipboardText.Length > 20000) return null;
        return clipboardText;
    }

    private void UpdatePolling()
    {
        if (_enabled || _polling)
        {
            if (!_timer.IsEnabled) _timer.Start();
        }
        else if (_timer.IsEnabled)
        {
            _timer.Stop();
        }
    }

    public void CopyToClipboard(string text)
    {
        try
        {
            System.Windows.Clipboard.SetText(text ?? string.Empty);
            _last = text ?? string.Empty;
        }
        catch (Exception ex) { AppLogger.Warn($"Clipboard set failed: {ex.Message}"); }
    }

    public void Clear()
    {
        lock (_entries) { _entries.Clear(); SaveCore(); }
        Changed?.Invoke();
    }

    private void Poll()
    {
        if (!_enabled && !_polling) return;
        try
        {
            if (!System.Windows.Clipboard.ContainsText()) return;
            var text = System.Windows.Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(text) || text.Length > 20000) return;
            if (text == _last) return;
            _last = text;
            var entry = new ClipboardEntry(DateTime.Now, text);
            if (_enabled)
            {
                lock (_entries)
                {
                    _entries.RemoveAll(e => e.Text == text);
                    _entries.Insert(0, entry);
                    var max = Math.Clamp(MaxEntries, 3, 200);
                    while (_entries.Count > max) _entries.RemoveAt(_entries.Count - 1);
                    SaveCore();
                }
                Changed?.Invoke();
            }
            EntryAdded?.Invoke(entry);
        }
        catch (Exception ex)
        {
            AppLogger.Debug($"Clipboard poll: {ex.Message}");
        }
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(_file)) return;
            var items = JsonSerializer.Deserialize<List<ClipboardEntry>>(File.ReadAllText(_file));
            if (items is null) return;
            lock (_entries) { _entries.Clear(); _entries.AddRange(items.Where(e => !string.IsNullOrWhiteSpace(e.Text))); }
        }
        catch (Exception ex) { AppLogger.Warn($"Clipboard history load: {ex.Message}"); }
    }

    private void SaveCore()
    {
        try
        {
            AppPaths.EnsureDirectories();
            var json = JsonSerializer.Serialize(_entries);
            var tmp = _file + ".tmp";
            File.WriteAllText(tmp, json);
            File.Move(tmp, _file, overwrite: true);
        }
        catch (Exception ex) { AppLogger.Warn($"Clipboard history save: {ex.Message}"); }
    }

    public void Dispose() => _timer.Stop();
}
