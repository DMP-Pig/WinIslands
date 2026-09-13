using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Brushes = System.Windows.Media.Brushes;

namespace WinIslands.UI;

/// <summary>
/// 跑马灯附加属性：文本超宽时自动横向无缝滚动（先停顿再滚动，匀速流畅，不截断）。
/// 应用于 TextBlock / KaraokeTextBlock。用法：local:Marquee.IsEnabled="True"。
/// 文本宽度用 FormattedText 测量（不换行，KarokeTextBlock 按 KaraokeText 全文测宽），
/// 滚到文本完全移出可视区后无缝跳回开头，视觉无跳变。纯本地渲染，不联网。
/// </summary>
public static class Marquee
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(Marquee), new PropertyMetadata(false, OnIsEnabledChanged));
    public static void SetIsEnabled(DependencyObject o, bool v) => o.SetValue(IsEnabledProperty, v);
    public static bool GetIsEnabled(DependencyObject o) => (bool)o.GetValue(IsEnabledProperty);

    /// <summary>暂停滚动（true 停住当前帧，false 继续）。</summary>
    public static readonly DependencyProperty PauseProperty = DependencyProperty.RegisterAttached(
        "Pause", typeof(bool), typeof(Marquee), new PropertyMetadata(false, OnPauseChanged));
    public static void SetPause(DependencyObject o, bool v) => o.SetValue(PauseProperty, v);
    public static bool GetPause(DependencyObject o) => (bool)o.GetValue(PauseProperty);

    private const double SpeedPxPerSec = 42;    // 滚动速度（像素/秒）
    private const double GapPx = 28;            // 首尾循环间隙
    private const double InitialDelaySec = 1.0; // 首次停顿（秒）

    // 文本属性监听（TextBlock.Text / KaraokeTextBlock.KaraokeText）
    private static readonly DependencyPropertyDescriptor TextDescriptor =
        DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));
    private static readonly DependencyPropertyDescriptor KaraokeTextDescriptor =
        DependencyPropertyDescriptor.FromProperty(KaraokeTextBlock.KaraokeTextProperty, typeof(KaraokeTextBlock));

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement fe) return;
        if ((bool)e.NewValue)
        {
            fe.Loaded += OnLoaded;
            fe.Unloaded += OnUnloaded;
            fe.SizeChanged += OnSizeChanged;
            fe.IsVisibleChanged += OnVisibleChanged;
            if (d is TextBlock tb) TextDescriptor.AddValueChanged(tb, OnTextChanged);
            if (d is KaraokeTextBlock kt) KaraokeTextDescriptor.AddValueChanged(kt, OnTextChanged);
            ScheduleEvaluate(fe);
        }
        else
        {
            fe.Loaded -= OnLoaded;
            fe.Unloaded -= OnUnloaded;
            fe.SizeChanged -= OnSizeChanged;
            fe.IsVisibleChanged -= OnVisibleChanged;
            if (d is TextBlock tb) TextDescriptor.RemoveValueChanged(tb, OnTextChanged);
            if (d is KaraokeTextBlock kt) KaraokeTextDescriptor.RemoveValueChanged(kt, OnTextChanged);
            StopMarquee(fe);
        }
    }

    private static void OnPauseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement fe) return;
        if (fe.RenderTransform is not TranslateTransform tt) return;
        if ((bool)e.NewValue) tt.BeginAnimation(TranslateTransform.XProperty, null); // 停住当前帧
        else ScheduleEvaluate(fe);
    }

    private static void OnLoaded(object sender, RoutedEventArgs e) => ScheduleEvaluate((FrameworkElement)sender);
    private static void OnUnloaded(object sender, RoutedEventArgs e) => StopMarquee((FrameworkElement)sender);
    private static void OnSizeChanged(object sender, SizeChangedEventArgs e) => ScheduleEvaluate((FrameworkElement)sender);
    private static void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        => ScheduleEvaluate((FrameworkElement)sender);
    private static void OnTextChanged(object? sender, EventArgs e) => ScheduleEvaluate((FrameworkElement)sender!);

    private static void ScheduleEvaluate(FrameworkElement fe)
    {
        // 等布局完成后再测量（ActualWidth / DesiredSize 才有效）
        fe.Dispatcher.BeginInvoke(new Action(() => Evaluate(fe)), System.Windows.Threading.DispatcherPriority.Loaded);
    }

    private static void Evaluate(FrameworkElement fe)
    {
        try
        {
            if (!fe.IsLoaded || !fe.IsVisible) { StopMarquee(fe); return; }
            if (fe is not TextBlock tb) { StopMarquee(fe); return; }

            var text = tb is KaraokeTextBlock kt ? (kt.KaraokeText ?? string.Empty) : (tb.Text ?? string.Empty);
            var viewW = tb.ActualWidth;
            if (viewW <= 1 || text.Length == 0) { StopMarquee(fe); return; }

            var textW = MeasureTextWidth(tb, text);
            if (textW <= viewW + 2) { StopMarquee(fe); return; } // 不超宽则不滚动

            if (fe.RenderTransform is not TranslateTransform tt)
            {
                tt = new TranslateTransform();
                fe.RenderTransform = tt;
                fe.RenderTransformOrigin = new System.Windows.Point(0, 0.5);
            }
            if (GetPause(fe))
            {
                tt.BeginAnimation(TranslateTransform.XProperty, null);
                return;
            }

            var distance = textW + GapPx; // 滚到文本完全移出可视区（含间隙）后无缝跳回
            var dur = TimeSpan.FromSeconds(distance / SpeedPxPerSec);
            var anim = new DoubleAnimation(0, -distance, dur)
            {
                BeginTime = TimeSpan.FromSeconds(InitialDelaySec),
                RepeatBehavior = RepeatBehavior.Forever,
            };
            Timeline.SetDesiredFrameRate(anim, 120);
            tt.BeginAnimation(TranslateTransform.XProperty, anim);
        }
        catch
        {
            // 跑马灯异常不影响主流程
        }
    }

    private static void StopMarquee(FrameworkElement fe)
    {
        if (fe.RenderTransform is TranslateTransform tt)
            tt.BeginAnimation(TranslateTransform.XProperty, null);
    }

    private static double MeasureTextWidth(TextBlock tb, string text)
    {
        var ft = new FormattedText(text, CultureInfo.CurrentUICulture, tb.FlowDirection,
            new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
            tb.FontSize, tb.Foreground ?? Brushes.White,
            VisualTreeHelper.GetDpi(tb).PixelsPerDip);
        return ft.WidthIncludingTrailingWhitespace;
    }
}
