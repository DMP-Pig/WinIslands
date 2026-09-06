using System.Windows;
using System.Windows.Media.Animation;

namespace WinIslands.UI;

/// <summary>
/// iOS 风格阻尼弹簧缓动：开始时快速加速、接近目标时减速并带轻微过冲回弹，
/// 速度全程非线性（不再匀速、不生硬）。
/// 公式：x(t) = 1 - e^(-ζ·ω0·t) · (cos(ωd·t) + (ζ·ω0/ωd)·sin(ωd·t))
/// </summary>
public sealed class SpringEase : Freezable, IEasingFunction
{
    /// <summary>阻尼系数，越大回弹越少、越“肉”。</summary>
    public double Damping { get; set; } = 12;

    /// <summary>刚度，越大动画越快。</summary>
    public double Stiffness { get; set; } = 200;

    /// <summary>质量，越大越慢。</summary>
    public double Mass { get; set; } = 1;

    protected override Freezable CreateInstanceCore() =>
        new SpringEase { Damping = Damping, Stiffness = Stiffness, Mass = Mass };

    public double Ease(double normalizedTime)
    {
        var t = Math.Max(0.0, Math.Min(1.0, normalizedTime)) * 1.7; // 让振荡在动画时长内完成一次多
        var omega0 = Math.Sqrt(Stiffness / Mass);
        var zeta = Damping / (2 * Math.Sqrt(Stiffness * Mass));
        var omegaD = omega0 * Math.Sqrt(1 - zeta * zeta);
        var decay = Math.Exp(-zeta * omega0 * t);
        var value = 1 - decay * (Math.Cos(omegaD * t) + (zeta * omega0 / omegaD) * Math.Sin(omegaD * t));
        return value;
    }
}

/// <summary>
/// 柔和弹簧（Soft 动效皮肤，33 动效皮肤）：阻尼更大、刚度更低，回弹更少、收尾更软，
/// 适合需要“更丝滑、少弹跳”的动效皮肤。
/// </summary>
public sealed class SoftSpringEase : Freezable, IEasingFunction
{
    /// <summary>阻尼系数，越大回弹越少、越“肉”。</summary>
    public double Damping { get; set; } = 16;

    /// <summary>刚度，越大动画越快。</summary>
    public double Stiffness { get; set; } = 150;

    /// <summary>质量，越大越慢。</summary>
    public double Mass { get; set; } = 1;

    protected override Freezable CreateInstanceCore() =>
        new SoftSpringEase { Damping = Damping, Stiffness = Stiffness, Mass = Mass };

    public double Ease(double normalizedTime) =>
        new SpringEase { Damping = Damping, Stiffness = Stiffness, Mass = Mass }.Ease(normalizedTime);
}