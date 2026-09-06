using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WinIslands.UI;

/// <summary>WPF 未内置 CornerRadius 的补间动画，这里提供一个最小实现，用于卡片圆角平滑过渡。
/// 用法与 DoubleAnimation 一致：From/To/EasingFunction，支持 HoldEnd 保持终点值。</summary>
public sealed class CornerRadiusAnimation : AnimationTimeline
{
    public CornerRadiusAnimation() { }

    public CornerRadiusAnimation(CornerRadius toValue, Duration duration)
    {
        To = toValue;
        Duration = duration;
    }

    public override Type TargetPropertyType => typeof(CornerRadius);

    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register(nameof(From), typeof(CornerRadius?), typeof(CornerRadiusAnimation));
    public CornerRadius? From
    {
        get => (CornerRadius?)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register(nameof(To), typeof(CornerRadius?), typeof(CornerRadiusAnimation));
    public CornerRadius? To
    {
        get => (CornerRadius?)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public static readonly DependencyProperty EasingFunctionProperty =
        DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(CornerRadiusAnimation));
    public IEasingFunction? EasingFunction
    {
        get => (IEasingFunction?)GetValue(EasingFunctionProperty);
        set => SetValue(EasingFunctionProperty, value);
    }

    protected override Freezable CreateInstanceCore() => new CornerRadiusAnimation();

    public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
    {
        if (animationClock is null || animationClock.CurrentProgress is not double progress)
            return defaultDestinationValue;
        var from = From ?? (CornerRadius)defaultOriginValue;
        var to = To ?? (CornerRadius)defaultDestinationValue;
        if (progress <= 0.0) return from;
        if (progress >= 1.0) return to;
        double t = EasingFunction?.Ease(progress) ?? progress;
        return new CornerRadius(
            from.TopLeft + (to.TopLeft - from.TopLeft) * t,
            from.TopRight + (to.TopRight - from.TopRight) * t,
            from.BottomRight + (to.BottomRight - from.BottomRight) * t,
            from.BottomLeft + (to.BottomLeft - from.BottomLeft) * t);
    }
}
