using System;
using System.Collections.Generic;
using System.Linq;

namespace WinIslands.Services;

/// <summary>
/// 记录运行中见过的媒体程序（SMTC 会话来源），供设置界面展示「媒体选择与顺序」。
/// </summary>
public sealed class MediaAppRegistry
{
    private readonly object _gate = new();
    private readonly List<(string Key, string Name)> _known = new();

    public IReadOnlyList<(string Key, string Name)> Known
    {
        get { lock (_gate) return _known.ToList(); }
    }

    public void Register(string key, string name)
    {
        if (string.IsNullOrWhiteSpace(key)) return;
        lock (_gate)
        {
            if (_known.Any(k => string.Equals(k.Key, key, StringComparison.OrdinalIgnoreCase))) return;
            _known.Add((key, string.IsNullOrWhiteSpace(name) ? key : name));
        }
    }
}
