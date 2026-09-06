using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace WinIslands.Services;

/// <summary>一条待办/便签。</summary>
public sealed record TodoItem(string Id, string Text, bool Done, DateTime CreatedAt);

/// <summary>待办/便签：本机 JSON 持久化，供灵动岛组件与设置页共用。</summary>
public sealed class TodoService : IDisposable
{
    private readonly string _file;
    private readonly object _gate = new();
    private readonly List<TodoItem> _items = new();

    public TodoService()
    {
        _file = Path.Combine(AppPaths.AppDataDir, "todos.json");
        Load();
    }

    public IReadOnlyList<TodoItem> Items { get { lock (_gate) return _items.ToList(); } }

    /// <summary>组件摘要，如「2/5」。</summary>
    public string Summary
    {
        get
        {
            lock (_gate)
            {
                if (_items.Count == 0) return string.Empty;
                var done = _items.Count(i => i.Done);
                return $"{done}/{_items.Count}";
            }
        }
    }

    /// <summary>未完成的第一条（组件显示用）。</summary>
    public string FirstPending
    {
        get { lock (_gate) return _items.FirstOrDefault(i => !i.Done)?.Text ?? string.Empty; }
    }

    public event Action? Changed;

    public void Add(string text)
    {
        text = (text ?? string.Empty).Trim();
        if (text.Length == 0) return;
        lock (_gate) _items.Insert(0, new TodoItem(Guid.NewGuid().ToString("N"), text, false, DateTime.Now));
        SaveAndNotify();
    }

    public void Toggle(string id)
    {
        lock (_gate)
        {
            var i = _items.FindIndex(x => x.Id == id);
            if (i < 0) return;
            _items[i] = _items[i] with { Done = !_items[i].Done };
        }
        SaveAndNotify();
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
            var items = JsonSerializer.Deserialize<List<TodoItem>>(File.ReadAllText(_file));
            if (items is null) return;
            lock (_gate) { _items.Clear(); _items.AddRange(items); }
        }
        catch (Exception ex) { AppLogger.Warn($"Todo load: {ex.Message}"); }
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
        catch (Exception ex) { AppLogger.Warn($"Todo save: {ex.Message}"); }
    }

    public void Dispose() { }
}
