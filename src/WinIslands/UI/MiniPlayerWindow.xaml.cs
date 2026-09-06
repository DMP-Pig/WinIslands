using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WinIslands.Services;

namespace WinIslands.UI;

/// <summary>
/// 迷你播放器：独立悬浮小窗，显示封面 / 歌名 / 歌手与播放控制。
/// 由 App 根据「迷你播放器」开关与媒体状态控制显隐；窗口本身可拖动。
/// </summary>
public partial class MiniPlayerWindow : Window
{
    private readonly IslandViewModel _vm;
    private readonly ThemeService _theme;
    private readonly SettingsService _settings;

    public MiniPlayerWindow(IslandViewModel vm, ThemeService theme, SettingsService settings)
    {
        _vm = vm;
        _theme = theme;
        _settings = settings;
        DataContext = vm;
        InitializeComponent();
        _theme.ThemeChanged += (_, _) => ApplyTheme();
        ApplyTheme();

        // 无边框窗口拖动（按钮点击已被按钮自身处理，不会触发拖动）
        MouseLeftButtonDown += (_, e) =>
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                try { DragMove(); }
                catch { /* 拖动中异常忽略 */ }
            }
        };
    }

    /// <summary>应用主题刷子（圆角液态玻璃外壳 + 文字颜色）。</summary>
    private void ApplyTheme()
    {
        Root.Background = _theme.CardBackground;
        Root.BorderBrush = _theme.CardBorder;
        Resources["MiniHoverBrush"] = _theme.ButtonHoverBrush;
        Resources["MiniTextPrimary"] = _theme.TextPrimary;
        Resources["MiniTextSecondary"] = _theme.TextSecondary;
        Resources["MiniTrackBrush"] = _theme.SliderTrackBrush;
        Resources["MiniBarBrush"] = _theme.SliderThumbBrush;
        Resources["MiniCoverBg"] = _theme.AccentBrush;
        try
        {
            WindowEffects.ApplyDarkMode(new WindowInteropHelper(this).Handle, _theme.IsDark);
        }
        catch { /* 非关键效果，忽略 */ }
    }

    /// <summary>从设置恢复上次位置；无记录时放到主屏右下角（避开灵动岛顶部区域）。</summary>
    public void PositionFromSettings()
    {
        var s = _settings.Current;
        if (s.MiniPlayerLeft is double left && s.MiniPlayerTop is double top)
        {
            Left = left;
            Top = top;
        }
        else
        {
            var screen = System.Windows.Forms.Screen.PrimaryScreen!;
            var work = screen.WorkingArea;
            var scale = ScreenHelper.GetDpiScale(screen);
            Left = (work.Right - (int)(Width * scale)) / scale - 16;
            Top = (work.Bottom - (int)(Height * scale)) / scale - 32;
        }

        // 确保窗口不跑出主屏工作区
        try
        {
            var ps = System.Windows.Forms.Screen.PrimaryScreen!;
            var wa = ps.WorkingArea;
            var sc = ScreenHelper.GetDpiScale(ps);
            Left = Math.Clamp(Left, wa.Left / sc, (wa.Right - Width) / sc);
            Top = Math.Clamp(Top, wa.Top / sc, (wa.Bottom - Height) / sc);
        }
        catch { /* 多屏切换瞬间可能异常，忽略 */ }
    }
}
