using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace WinIslands.UI;

/// <summary>
/// iOS 风格阻尼弹簧缓冲：开始时快速加速、接近目标时减速并带轻微过冲回弹；
/// 速度全程非线性（不再均速、不生硬）。
/// 公式：x(t) = 1 - e^(-zeta*omega0*t) * (cos(omegaD*t) + (zeta*omega0/omegaD)*sin(omegaD*t))
/// </summary>
public sealed class SpringEase : Freezable, IEasingFunction
{
    public double Damping { get; set; } = 12;
    public double Stiffness { get; set; } = 200;
    public double Mass { get; set; } = 1;

    private double _omega0Cache = -1, _zetaCache = -1, _omegaDCache = -1;
    private double _lastD = -1, _lastK = -1, _lastM = -1;

    protected override Freezable CreateInstanceCore() =>
        new SpringEase { Damping = Damping, Stiffness = Stiffness, Mass = Mass };

    public double Ease(double normalizedTime)
    {
        var t = Math.Max(0.0, Math.Min(1.0, normalizedTime)) * 1.7;
        if (_lastD != Damping || _lastK != Stiffness || _lastM != Mass)
        {
            _omega0Cache = Math.Sqrt(Stiffness / Mass);
            _zetaCache = Damping / (2 * Math.Sqrt(Stiffness * Mass));
            var z2 = 1 - _zetaCache * _zetaCache;
            _omegaDCache = _omega0Cache * Math.Sqrt(z2 > 0 ? z2 : 0.0001);
            _lastD = Damping; _lastK = Stiffness; _lastM = Mass;
        }
        var decay = Math.Exp(-_zetaCache * _omega0Cache * t);
        return 1 - decay * (Math.Cos(_omegaDCache * t) + (_zetaCache * _omega0Cache / _omegaDCache) * Math.Sin(_omegaDCache * t));
    }
}

/// <summary>
/// 柔和弹簧（Soft 动效的轮、3 动效的轮）：阻尼更大、刚度更低，回弹更少、收尾更软。
/// </summary>
public sealed class SoftSpringEase : Freezable, IEasingFunction
{
    public double Damping { get; set; } = 16;
    public double Stiffness { get; set; } = 150;
    public double Mass { get; set; } = 1;

    private double _omega0 = -1, _zeta = -1, _omegaD = -1;
    private double _lastD2 = -1, _lastK2 = -1, _lastM2 = -1;

    protected override Freezable CreateInstanceCore() =>
        new SoftSpringEase { Damping = Damping, Stiffness = Stiffness, Mass = Mass };

    public double Ease(double normalizedTime)
    {
        var t = Math.Max(0.0, Math.Min(1.0, normalizedTime)) * 1.7;
        if (_lastD2 != Damping || _lastK2 != Stiffness || _lastM2 != Mass)
        {
            _omega0 = Math.Sqrt(Stiffness / Mass);
            _zeta = Damping / (2 * Math.Sqrt(Stiffness * Mass));
            var z2 = 1 - _zeta * _zeta;
            _omegaD = _omega0 * Math.Sqrt(z2 > 0 ? z2 : 0.0001);
            _lastD2 = Damping; _lastK2 = Stiffness; _lastM2 = Mass;
        }
        var decay = Math.Exp(-_zeta * _omega0 * t);
        return 1 - decay * (Math.Cos(_omegaD * t) + (_zeta * _omega0 / _omegaD) * Math.Sin(_omegaD * t));
    }
}