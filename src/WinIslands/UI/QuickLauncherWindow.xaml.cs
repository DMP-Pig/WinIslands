using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WinIslands.Services;
// UseWindowsForms 的隐式 using 会引入 System.Windows.Forms，这里显式消除事件参数类型歧义
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextChangedEventArgs = System.Windows.Controls.TextChangedEventArgs;
using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using Color = System.Windows.Media.Color;

namespace WinIslands.UI;

/// <summary>
/// 快速启动器（Spotlight 风格）：Ctrl+Space 弹出，模糊搜索开始菜单程序 / 系统工具 / 网址，
/// 回车用 ShellExecute 启动。只读取本地开始菜单，不联网、不收集任何数据。
/// </summary>
public partial class QuickLauncherWindow : Window
{
    private readonly ThemeService _theme;
    private readonly List<LauncherItem> _all = new();
    private bool _suppressHide; // 展示瞬间的 Deactivated 不触发隐藏

    /// <summary>一条可启动的结果（Path 可能是 .lnk / 命令 / URL）。</summary>
    public sealed record LauncherItem(string Name, string Path, string Glyph);

    public QuickLauncherWindow(ThemeService theme)
    {
        InitializeComponent();
        _theme = theme;
        _theme.ThemeChanged += (_, _) => ApplyTheme();
        ApplyTheme();
        LoadApps();
        Localization.LanguageChanged += (_, _) => ApplyStrings();
        ApplyStrings();
    }

    private void ApplyStrings()
    {
        EmptyHint.Text = Localization.Get("Launcher_Empty");
        FootHint.Text = Localization.Get("Launcher_Foot");
        SearchBox.ToolTip = Localization.Get("Launcher_Placeholder");
    }

    private void ApplyTheme()
    {
        Root.Background = _theme.CardBackground;
        Root.BorderBrush = _theme.CardBorder;
        Foreground = _theme.TextPrimary;
        Foreground = _theme.TextPrimary;
        SearchBox.Foreground = _theme.TextPrimary;
        SearchBox.CaretBrush = _theme.TextPrimary;
        SearchShell.Background = _theme.ButtonHoverBrush;
        SearchShell.BorderBrush = _theme.CardBorder;
        if (FindResource("HighlightBrush") is SolidColorBrush hl && _theme.AccentBorderBrush is SolidColorBrush ab)
        {
            // 选中行用强调色（更醒目）
            hl.Color = Color.FromArgb(90, ab.Color.R, ab.Color.G, ab.Color.B);
        }
    }

    /// <summary>列出常用系统工具（可直接 ShellExecute 的 exe / URI）。</summary>
    private static IEnumerable<LauncherItem> SystemTools()
    {
        var tools = new (string Name, string Path, string Glyph)[]
        {
            ("Windows 设置", "ms-settings:", "\uE713"),
            ("记事本", "notepad.exe", "\uE8A5"),
            ("计算器", "calc.exe", "\uE8EF"),
            ("命令提示符", "cmd.exe", "\uE756"),
            ("控制面板", "control.exe", "\uE713"),
            ("任务管理器", "taskmgr.exe", "\uE71D"),
            ("画图", "mspaint.exe", "\uE71D"),
            ("远程桌面连接", "mstsc.exe", "\uE71D"),
        };
        foreach (var t in tools) yield return new LauncherItem(t.Name, t.Path, t.Glyph);
    }

    /// <summary>扫描开始菜单 .lnk（用户 + 公共）与系统工具。</summary>
    private void LoadApps()
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var dirs = new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Programs),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
        };
        foreach (var dir in dirs)
        {
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) continue;
            try
            {
                foreach (var lnk in Directory.EnumerateFiles(dir, "*.lnk", SearchOption.AllDirectories))
                {
                    var name = Path.GetFileNameWithoutExtension(lnk);
                    if (!seen.Add(name)) continue;
                    _all.Add(new LauncherItem(name, lnk, "\uE71D"));
                }
            }
            catch { /* 无权限目录跳过 */ }
        }
        foreach (var t in SystemTools())
            if (seen.Add(t.Name)) _all.Add(t);
    }

    /// <summary>刷新结果列表：空查询显示前 20 个；否则前缀匹配优先，其次包含匹配；网址/路径放到最前。</summary>
    private void Refilter()
    {
        var q = SearchBox.Text.Trim();
        var list = new List<LauncherItem>();
        if (q.Length == 0)
        {
            list.AddRange(_all.Take(22));
        }
        else
        {
            var lower = q.ToLowerInvariant();
            if (IsUrlLike(q)) list.Add(new LauncherItem(q, q, "\uE774")); // 直接打开网址
            var starts = _all.Where(i => i.Name.StartsWith(q, StringComparison.OrdinalIgnoreCase))
                             .OrderBy(i => i.Name.Length);
            var contains = _all.Where(i => !i.Name.StartsWith(q, StringComparison.OrdinalIgnoreCase) &&
                                           i.Name.IndexOf(lower, StringComparison.OrdinalIgnoreCase) >= 0)
                               .OrderBy(i => i.Name.Length);
            list.AddRange(starts);
            list.AddRange(contains);
            if (list.Count > 30) list = list.Take(30).ToList();
        }
        ResultList.ItemsSource = list;
        if (list.Count > 0) ResultList.SelectedIndex = 0;
        EmptyHint.Visibility = list.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private static bool IsUrlLike(string q) =>
        q.Contains("://", StringComparison.OrdinalIgnoreCase) ||
        (q.Contains('.') && !q.Contains(' ') && !q.Contains('\\') && !Path.IsPathRooted(q));

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => Refilter();

    private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Down) { MoveSelection(1); e.Handled = true; }
        else if (e.Key == Key.Up) { MoveSelection(-1); e.Handled = true; }
        else if (e.Key == Key.Enter) { LaunchSelected(); e.Handled = true; }
        else if (e.Key == Key.Escape) { Hide(); e.Handled = true; }
    }

    private void MoveSelection(int delta)
    {
        if (ResultList.Items.Count == 0) return;
        var idx = ResultList.SelectedIndex < 0 ? 0 : Math.Clamp(ResultList.SelectedIndex + delta, 0, ResultList.Items.Count - 1);
        ResultList.SelectedIndex = idx;
        ResultList.ScrollIntoView(ResultList.Items[idx]);
    }

    private void LaunchSelected()
    {
        if (ResultList.SelectedItem is LauncherItem item) { Launch(item.Path); Hide(); }
        else
        {
            var q = SearchBox.Text.Trim();
            if (q.Length > 0 && IsUrlLike(q)) { Launch(q); Hide(); }
        }
    }

    private void ResultList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ResultList.SelectedItem is LauncherItem item) { Launch(item.Path); Hide(); }
    }

    private void Launch(string path)
    {
        try
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"快速启动器启动失败: {path} ({ex.Message})");
            EmptyHint.Text = $"{Localization.Get("Launcher_Empty")}\n{path}";
        }
    }

    /// <summary>显示/隐藏（全局快捷键调用）。</summary>
    public void Toggle()
    {
        if (IsVisible) { Hide(); return; }
        SearchBox.Text = string.Empty;
        Refilter();
        ApplyStrings();
        var wa = SystemParameters.WorkArea;
        Left = wa.Left + (wa.Width - ActualWidth) / 2;
        Top = wa.Top + wa.Height * 0.16;
        _suppressHide = true;
        Show();
        Activate();
        _suppressHide = false;
        SearchBox.Focus();
    }

    protected override void OnDeactivated(EventArgs e)
    {
        base.OnDeactivated(e);
        if (!_suppressHide && IsVisible) Hide(); // 点击其它窗口自动收起
    }
}
