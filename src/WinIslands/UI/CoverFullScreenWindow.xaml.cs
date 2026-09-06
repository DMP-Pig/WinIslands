using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WinIslands.UI;

/// <summary>
/// #2 封面沉浸：点击展开态的大封面/大卡后，在当前显示器上弹出全屏封面预览。
/// 点击任意处 / Esc / 右键关闭（带淡入淡出动画）。
/// </summary>
public partial class CoverFullScreenWindow : Window
{
    private bool _closing;

    public CoverFullScreenWindow(ImageSource artwork, System.Windows.Forms.Screen screen)
    {
        InitializeComponent();
        Art.Source = artwork;

        // 覆盖目标显示器全屏（物理像素 → DIP 换算，兼容高 DPI/多显示器）
        var bounds = ScreenHelper.DpiBounds(screen);
        Left = bounds.X;
        Top = bounds.Y;
        Width = bounds.Width;
        Height = bounds.Height;
        Opacity = 0;

        Loaded += (_, _) =>
        {
            var inAnim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(240))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };
            BeginAnimation(OpacityProperty, inAnim);
        };
    }

    private void Root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => CloseWithAnimation();

    private void Root_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        CloseWithAnimation();
        e.Handled = true;
    }

    private void OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            CloseWithAnimation();
            e.Handled = true;
        }
    }

    /// <summary>反向淡出后关闭，避免"啪"地消失。</summary>
    private void CloseWithAnimation()
    {
        if (_closing) return;
        _closing = true;
        var outAnim = new DoubleAnimation(0, TimeSpan.FromMilliseconds(220))
        {
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn },
        };
        outAnim.Completed += (_, _) => Close();
        BeginAnimation(OpacityProperty, outAnim);
    }
}
