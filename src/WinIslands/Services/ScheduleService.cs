using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Threading;

namespace WinIslands.Services;

/// <summary>一条日程。</summary>
public sealed record ScheduleItem(string Id, string Title, DateTime When);

/// <summary>
/// 日程提醒：本机 JSON 持久化。每 20 秒检查下一个日程，
/// 到达时间触发提醒（由上层弹通知），组件可显示「距离下一日程 xx 分钟」。
/// </summary>
public sealed class ScheduleService : IDisposable
{
    private readonly string _file;
    private readonly object _gate = new();
    private readonly List<ScheduleItem> _items = new();
    private readonly DispatcherTimer _timer;
    private readonly HashSet<string> _fired = new();

    public ScheduleService()
    {
        _file = Path.Combine(AppPaths.AppDataDir, "schedules.json");
        Load();
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(20) };
        _timer.Tick += (_, _) => Check();
        _timer.Start();
        Check();
    }

    /// <summary>按需启停后台轮询：仅当日程组件显示或需要到点提醒时运行，避免空闲空转。</summary>
    public void SetPollingEnabled(bool enabled)
    {
        if (enabled) { _timer.Start(); Check(); }
        else _timer.Stop();
    }

    public IReadOnlyList<ScheduleItem> Items { get { lock (_gate) return _items.OrderBy(i => i.When).ToList(); } }

    /// <summary>下一个未到期的日程；没有则 null。</summary>
    public ScheduleItem? Next
    {
        get
        {
            var now = DateTime.Now;
            lock (_gate) return _items.Where(i => i.When > now).OrderBy(i => i.When).FirstOrDefault();
        }
    }

    /// <summary>组件摘要：「14:30 会议」或「27 分钟后」等。</summary>
    public string Summary
    {
        get
        {
            var n = Next;
            if (n is null) return string.Empty;
            var diff = n.When - DateTime.Now;
            if (diff.TotalHours >= 1) return $"{n.When:HH:mm} {n.Title}";
            var mins = Math.Max(1, (int)diff.TotalMinutes);
            return $"{mins} 分钟后 · {n.Title}";
        }
    }

    public event Action? Changed;
    /// <summary>日程到点（参数为日程）。</summary>
    public event Action<ScheduleItem>? Reminder;

    public void Add(string title, DateTime when)
    {
        title = (title ?? string.Empty).Trim();
        if (title.Length == 0 || when <= DateTime.Now) return;
        lock (_gate) _items.Add(new ScheduleItem(Guid.NewGuid().ToString("N"), title, when));
        SaveAndNotify();
        Check();
    }

    public void Remove(string id)
    {
        lock (_gate) _items.RemoveAll(x => x.Id == id);
        SaveAndNotify();
    }

    public void Clear()
    {
        lock (_gate) _items.Clear();
        SaveAndNotify();
    }

    private void Check()
    {
        var now = DateTime.Now;
        List<ScheduleItem> due;
        lock (_gate)
        {
            due = _items.Where(i => i.When <= now && _fired.Add(i.Id)).ToList();
        }
        foreach (var d in due) Reminder?.Invoke(d);
    }

    private void SaveAndNotify()
    {
        SaveCore();
        Changed?.Invoke();
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(_file)) return;
            var items = JsonSerializer.Deserialize<List<ScheduleItem>>(File.ReadAllText(_file));
            if (items is null) return;
            lock (_gate) { _items.Clear(); _items.AddRange(items); }
        }
        catch (Exception ex) { AppLogger.Warn($"Schedule load: {ex.Message}"); }
    }

    private void SaveCore()
    {
        try
        {
            AppPaths.EnsureDirectories();
            var json = JsonSerializer.Serialize(_items);
            var tmp = _file + ".tmp";
            File.WriteAllText(tmp, json);
            File.Move(tmp, _file, overwrite: true);
        }
        catch (Exception ex) { AppLogger.Warn($"Schedule save: {ex.Message}"); }
    }

    public void Dispose() => _timer.Stop();
}
