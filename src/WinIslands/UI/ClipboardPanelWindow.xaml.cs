using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WinIslands.Services;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using Color = System.Windows.Media.Color;

namespace WinIslands.UI;

/// <summary>
/// 剪贴板历史查看面板（Ctrl+Alt+V）：列出最近复制的文本，点击即可重新复制。
/// 数据来自本机 ClipboardHistoryService，纯本地，绝不上传。
/// </summary>
public partial class ClipboardPanelWindow : Window
{
    private readonly ThemeService _theme;
    private readonly ClipboardHistoryService _clipboard;
    private bool _suppressHide;
    private DateTime _copiedUntil = DateTime.MinValue;

    public ClipboardPanelWindow(ThemeService theme, ClipboardHistoryService clipboard)
    {
        InitializeComponent();
        _theme = theme;
        _clipboard = clipboard;
        _clipboard.Changed += () => Dispatcher.BeginInvoke(RefreshList);
        _theme.ThemeChanged += (_, _) => ApplyTheme();
        ApplyTheme();
        Localization.LanguageChanged += (_, _) => ApplyStrings();
        ApplyStrings();
        RefreshList();
    }

    private void ApplyStrings()
    {
        TitleText.Text = Localization.Get("ClipPanel_Title");
        ClearBtn.Content = Localization.Get("ClipPanel_Clear");
        EmptyHint.Text = Localization.Get("ClipPanel_Empty");
        FootHint.Text = Localization.Get("ClipPanel_Foot");
    }

    private void ApplyTheme()
    {
        Root.Background = _theme.CardBackground;
        Root.BorderBrush = _theme.CardBorder;
        Foreground = _theme.TextPrimary;
        ClearBtn.Foreground = _theme.TextPrimary;
        if (FindResource("HighlightBrush") is SolidColorBrush hl && _theme.AccentBorderBrush is SolidColorBrush ab)
            hl.Color = Color.FromArgb(90, ab.Color.R, ab.Color.G, ab.Color.B);
    }

    private void RefreshList()
    {
        var items = _clipboard.Entries
            .Select(e => new Row(e.Time.ToString("MM-dd HH:mm"), e.Text, e.TextPreview))
            .ToList();
        ResultList.ItemsSource = items;
        EmptyHint.Visibility = items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private sealed record Row(string TimeText, string Text, string TextPreview);

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        _clipboard.Clear();
        RefreshList();
    }

    private void ResultList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        CopySelected();
    }

    private void CopySelected()
    {
        if (ResultList.SelectedItem is not Row row) return;
        _clipboard.CopyToClipboard(row.Text);
        FootHint.Text = Localization.Get("ClipPanel_Copied");
        _copiedUntil = DateTime.Now.AddSeconds(1.6);
        Hide();
    }

    private void List_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) { CopySelected(); e.Handled = true; }
        else if (e.Key == Key.Escape) { Hide(); e.Handled = true; }
    }

    /// <summary>显示/收起（全局快捷键或灵动岛组件调用）。</summary>
    public void Toggle()
    {
        if (IsVisible) { Hide(); return; }
        RefreshList();
        ApplyStrings();
        var wa = SystemParameters.WorkArea;
        Left = wa.Left + wa.Width - ActualWidth - 16;
        Top = wa.Top + 16;
        _suppressHide = true;
        Show();
        Activate();
        _suppressHide = false;
        ResultList.Focus();
    }

    protected override void OnDeactivated(EventArgs e)
    {
        base.OnDeactivated(e);
        if (!_suppressHide && IsVisible)
        {
            if (DateTime.Now < _copiedUntil) return; // 刚复制完的短暂提示期间不闪退
            Hide();
        }
    }
}
