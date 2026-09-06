using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WinIslands.Services;

namespace WinIslands.UI;

/// <summary>可选独立悬浮歌词小窗。
/// #5 增强：支持不透明度调节与锁定（锁定后不可拖动且 WS_EX_TRANSPARENT 鼠标穿透，不挡操作）。</summary>
public partial class LyricsWindow : Window, INotifyPropertyChanged
{
    private readonly IslandViewModel _vm;
    private readonly SettingsService _settings;
    public double LyricsFontSize => Math.Clamp(_settings.Current.LyricFontSize, 9, 28);
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Raise(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TRANSPARENT = 0x00000020;

    public LyricsWindow(IslandViewModel vm, SettingsService settings)
    {
        _vm = vm;
        _settings = settings;
        DataContext = vm;
        InitializeComponent();
        // 默认点击可拖动；锁定状态下禁止拖动
        MouseLeftButtonDown += (_, e) =>
        {
            if (_settings.Current.LyricsWindowLocked) return;
            if (e.ButtonState == MouseButtonState.Pressed) DragMove();
        };
        SourceInitialized += (_, _) => ApplyWindowStyles();
        ApplySettings();
    }

    /// <summary>根据当前设置应用不透明度与锁定/穿透状态（设置变更时由 App 调用）。</summary>
    public void ApplySettings()
    {
        try
        {
            Opacity = Math.Clamp(_settings.Current.LyricsWindowOpacity, 0.3, 1.0);
            Raise(nameof(LyricsFontSize));
            ApplyWindowStyles();
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"Lyrics window settings: {ex.Message}");
        }
    }

    private void ApplyWindowStyles()
    {
        try
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == IntPtr.Zero) return;
            var ex = GetWindowLong(hwnd, GWL_EXSTYLE);
            var locked = _settings.Current.LyricsWindowLocked;
            var target = locked ? (ex | WS_EX_TRANSPARENT) : (ex & ~WS_EX_TRANSPARENT);
            if (target != ex) SetWindowLong(hwnd, GWL_EXSTYLE, target);
        }
        catch { /* 句柄未就绪时忽略 */ }
    }

    /// <summary>位置在主屏幕底部中央附近。</summary>
    public void PositionNearBottom()
    {
        var screen = System.Windows.Forms.Screen.PrimaryScreen!;
        var work = screen.WorkingArea;
        var scale = ScreenHelper.GetDpiScale(screen);
        Left = (work.X + work.Width / 2) / scale - ActualWidth / 2;
        Top = (work.Y + work.Height - (int)(64 * scale)) / scale - ActualHeight;
    }
}
