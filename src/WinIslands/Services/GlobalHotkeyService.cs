using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WinIslands.Services;

/// <summary>
/// 全局快捷键（Win32 RegisterHotKey）。
/// 通过一个不可见的 0x0 窗口接收 WM_HOTKEY，不抢占焦点。
/// 支持自定义组合键：格式如 "Ctrl+Alt+I"、"Ctrl+Shift+Space"、"Win+Alt+F1"。
/// 受设置 GlobalHotkeysEnabled 控制（默认开启）。设置变化时调用 <see cref="Apply"/> 重新注册。
/// </summary>
public sealed class GlobalHotkeyService : IDisposable
{
    public const int WM_HOTKEY = 0x0312;
    private const uint MOD_ALT = 0x0001;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const uint MOD_WIN = 0x0008;
    private const uint MOD_NOREPEAT = 0x4000; // 按住不重复触发

    private readonly Window _host;
    private HwndSource? _source;
    private IntPtr _hwnd;
    private readonly List<HotkeyBinding> _bindings = new();
    private bool _enabled = true;
    private bool _disposed;

    /// <summary>单个快捷键绑定：ID、组合键描述、原始字符串、事件。</summary>
    private sealed class HotkeyBinding
    {
        public int Id;
        public string Text = "";
        public uint Modifiers;
        public uint Vk;
        public Action? Handler;
    }

    public GlobalHotkeyService(AppSettings? settings = null)
    {
        _host = new Window
        {
            WindowStyle = WindowStyle.None,
            ResizeMode = ResizeMode.NoResize,
            ShowInTaskbar = false,
            ShowActivated = false,
            Width = 0,
            Height = 0,
            Left = -10000,
            Top = -10000,
            Opacity = 0,
            AllowsTransparency = false,
        };
        _host.SourceInitialized += (_, _) =>
        {
            _hwnd = new WindowInteropHelper(_host).Handle;
            _source = HwndSource.FromHwnd(_hwnd);
            _source?.AddHook(WndProc);
            if (settings is not null) Apply(settings);
        };
        _host.Show(); // 不可见窗口：必须 Show 一次才有 HWND，但永远不可见、不抢焦点
    }

    // ── 事件：5 个可自定义动作 ──
    public event Action? PlayPausePressed;
    public event Action? NextPressed;
    public event Action? PreviousPressed;
    public event Action? ToggleVisibilityPressed;
    /// <summary>展开 / 收起灵动岛（35 全局快捷键大全新增）。</summary>
    public event Action? ExpandPressed;
    /// <summary>打开 / 收起快速启动器（Ctrl+Space，Spotlight 风格）。</summary>
    public event Action? LauncherPressed;
    /// <summary>打开 / 收起剪贴板历史面板。</summary>
    public event Action? ClipboardPanelPressed;

    /// <summary>启用或禁用全部快捷键。</summary>
    public void SetEnabled(bool enabled)
    {
        _enabled = enabled;
        if (enabled) RegisterAll();
        else UnregisterAll();
    }

    /// <summary>按当前设置重新注册全部快捷键（设置变化时调用）。</summary>
    public void Apply(AppSettings settings)
    {
        _bindings.Clear();
        AddBinding(0xC011, settings.HotkeyPlayPause, PlayPausePressed);
        AddBinding(0xC012, settings.HotkeyNext, NextPressed);
        AddBinding(0xC013, settings.HotkeyPrev, PreviousPressed);
        AddBinding(0xC014, settings.HotkeyToggleVisible, ToggleVisibilityPressed);
        AddBinding(0xC015, settings.HotkeyExpand, ExpandPressed);
        AddBinding(0xC016, settings.QuickLauncherEnabled ? settings.HotkeyLauncher : "", LauncherPressed);
        AddBinding(0xC017, settings.ClipboardPanelEnabled ? settings.HotkeyClipboardPanel : "", ClipboardPanelPressed);
        if (_enabled) RegisterAll();
    }

    private void AddBinding(int id, string text, Action? handler)
    {
        text = (text ?? string.Empty).Trim();
        if (handler is null || string.IsNullOrWhiteSpace(text)) return;
        if (!TryParse(text, out var mods, out var vk))
        {
            AppLogger.Warn($"全局快捷键格式无法解析，已忽略：\"{text}\"");
            return;
        }
        _bindings.Add(new HotkeyBinding { Id = id, Text = text, Modifiers = mods, Vk = vk, Handler = handler });
    }

    /// <summary>
    /// 解析 "Ctrl+Alt+I" 形式的组合键。
    /// 修饰键：Ctrl / Alt / Shift / Win（可省略，但为避免误触建议至少一个）。
    /// 主键：a-z、0-9、F1-F24、Left/Right/Up/Down/Space/Enter/Tab/Esc/Home/End/PageUp/PageDown/Insert/Delete/Back/CapsLock/Comma/Period 等。
    /// </summary>
    public static bool TryParse(string text, out uint modifiers, out uint vk)
    {
        modifiers = 0;
        vk = 0;
        if (string.IsNullOrWhiteSpace(text)) return false;
        var parts = text.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0) return false;

        for (var i = 0; i < parts.Length - 1; i++)
        {
            switch (parts[i].ToLowerInvariant())
            {
                case "ctrl" or "control": modifiers |= MOD_CONTROL; break;
                case "alt": modifiers |= MOD_ALT; break;
                case "shift": modifiers |= MOD_SHIFT; break;
                case "win" or "windows" or "cmd": modifiers |= MOD_WIN; break;
                default: return false; // 未知修饰键
            }
        }

        var key = parts[^1].Trim();
        if (string.IsNullOrEmpty(key)) return false;
        if (key.Length == 1)
        {
            var ch = char.ToUpperInvariant(key[0]);
            if (ch is >= 'A' and <= 'Z') { vk = ch; return true; }
            if (ch is >= '0' and <= '9') { vk = ch; return true; }
            return false;
        }

        switch (key.ToLowerInvariant())
        {
            case "space": vk = 0x20; break;
            case "enter" or "return": vk = 0x0D; break;
            case "tab": vk = 0x09; break;
            case "esc" or "escape": vk = 0x1B; break;
            case "back" or "backspace": vk = 0x08; break;
            case "delete" or "del": vk = 0x2E; break;
            case "insert" or "ins": vk = 0x2D; break;
            case "home": vk = 0x24; break;
            case "end": vk = 0x23; break;
            case "pageup" or "pgup": vk = 0x21; break;
            case "pagedown" or "pgdn": vk = 0x22; break;
            case "left" or "←": vk = 0x25; break;
            case "right" or "→": vk = 0x27; break;
            case "up" or "↑": vk = 0x26; break;
            case "down" or "↓": vk = 0x28; break;
            case "capslock" or "caps": vk = 0x14; break;
            case "comma" or ",": vk = 0xBC; break;
            case "period" or ".": vk = 0xBE; break;
            case "semicolon" or ";": vk = 0xBA; break;
            case "minus" or "-": vk = 0xBD; break;
            case "equals" or "=": vk = 0xBB; break;
            case "slash" or "/": vk = 0xBF; break;
            case "backslash" or "\\": vk = 0xDC; break;
            case "quote" or "'": vk = 0xDE; break;
            case "grave" or "`": vk = 0xC0; break;
            case "lbracket" or "[": vk = 0xDB; break;
            case "rbracket" or "]": vk = 0xDD; break;
            default:
                if (key.StartsWith("F", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(key.AsSpan(1), NumberStyles.None, CultureInfo.InvariantCulture, out var fn) &&
                    fn is >= 1 and <= 24)
                {
                    vk = (uint)(0x70 + fn - 1);
                    break;
                }
                return false;
        }
        return vk != 0;
    }

    private void RegisterAll()
    {
        if (_disposed || _hwnd == IntPtr.Zero) return;
        foreach (var b in _bindings)
        {
            if (!RegisterHotKey(_hwnd, b.Id, b.Modifiers | MOD_NOREPEAT, b.Vk))
            {
                AppLogger.Warn($"RegisterHotKey 失败：\"{b.Text}\"（{Marshal.GetLastWin32Error()}）");
            }
        }
    }

    private void UnregisterAll()
    {
        if (_hwnd == IntPtr.Zero) return;
        foreach (var b in _bindings)
            UnregisterHotKey(_hwnd, b.Id);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg != WM_HOTKEY) return IntPtr.Zero;
        var id = wParam.ToInt32();
        foreach (var b in _bindings)
        {
            if (b.Id != id) continue;
            try { b.Handler?.Invoke(); }
            catch (Exception ex) { AppLogger.Error("Hotkey handler error", ex); }
            break;
        }
        handled = true;
        return IntPtr.Zero;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        try
        {
            UnregisterAll();
            _source?.RemoveHook(WndProc);
            _host.Close();
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"GlobalHotkeyService dispose failed: {ex.Message}");
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}