using System.Windows;
using System.Windows.Media.Animation;

namespace WinIslands.UI;

/// <summary>
/// iOS 风格阻尼弹簧缓冲：开始时快速加速、接近目标时减速并带轻微过冲回弹；
/// 速度全程非线性（不再均速、不生硬）。
/// 公式：x(t) = 1 - e^(-ζ·ω0·t) · (cos(ωd·t) + (ζ·ω0/ωd)·sin(ωd·t))
/// </summary>
public sealed class SpringEase : Freezable, IEasingFunction
{
    /// <summary>阻尼系数，越大回弹越少、越“软”。</summary>
    public double Damping { get; set; } = 12;

    /// <summary>刚度，越大动画越快。</summary>
    public double Stiffness { get; set; } = 200;

    /// <summary>质量，越大越慢。</summary>
    public double Mass { get; set; } = 1;

    // 预计算缓存：同一实例多次调用免重复计算二级量
    private double _omega0Cache = -1, _zetaCache = -1, _omegaDCache = -1;
    private double _lastD = -1, _lastK = -1, _lastM = -1;

    protected override Freezable CreateInstanceCore() =>
        new SpringEase { Damping = Damping, Stiffness = Stiffness, Mass = Mass };

    public double Ease(double normalizedTime)
    {
        var t = Math.Max(0.0, Math.Min(1.0, normalizedTime)) * 1.7; // 让振荡在动画时长内完成一次多周
        // 缓存当前参数的推导结果，避免每帧重复计算（仅在参数变化时重算）
        if (_lastD != Damping || _lastK != Stiffness || _lastM != Mass)
        {
            _omega0Cache = Math.Sqrt(Stiffness / Mass);
            _zetaCache = Damping / (2 * Math.Sqrt(Stiffness * Mass));
            var z2 = 1 - _zetaCache * _zetaCache;
            _omegaDCache = _omega0Cache * Math.Sqrt(z2 > 0 ? z2 : 0.0001);
            _lastD = Damping; _lastK = Stiffness; _lastM = Mass;
        }
        var decay = Math.Exp(-_zetaCache * _omega0Cache * t);
        var value = 1 - decay * (Math.Cos(_omegaDCache * t) + (_zetaCache * _omega0Cache / _omegaDCache) * Math.Sin(_omegaDCache * t));
        return value;
    }
}

/// <summary>
/// 柔和弹簧（Soft 动效的轮、3 动效的轮）：阻尼更大、刚度更低，回弹更少、收尾更软，
/// 适合需要“更丝滑、少弹跳”的动效的轮。
/// </summary>
public sealed class SoftSpringEase : Freezable, IEasingFunction
{
    /// <summary>阻尼系数，越大回弹越少、越“软”。</summary>
    public double Damping { get; set; } = 16;

    /// <summary>刚度，越大动画越快。</summary>
    public double Stiffness { get; set; } = 150;

    /// <summary>质量，越大越慢。</summary>
    public double Mass { get; set; } = 1;

    // 内部缓存实例：免免 .Ease() 每次创建新实例，消除 GC 压力
    private readonly SpringEase _inner = new();

    protected override Freezable CreateInstanceCore() =>
        new SoftSpringEase { Damping = Damping, Stiffness = Stiffness, Mass = Mass };

    public double Ease(double normalizedTime)
    {
        // 同步子参到内部实例，避免每帧创建新对象
        _inner.Damping = Damping;
        _inner.Stiffness = Stiffness;
        _inner.Mass = Mass;
        return _inner.Ease(normalizedTime);
    }
}
