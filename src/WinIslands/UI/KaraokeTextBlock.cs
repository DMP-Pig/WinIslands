using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using WinIslands.Services;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace WinIslands.UI;

/// <summary>
/// 逐字卡拉OK歌词控件（带平滑过渡动画，120fps）。
/// 两种模式：
///  1. 逐字模式（有 <see cref="Words"/>，来自 AMLL TTML）：每个字/词按各自独立起止时间
///     从左到右点亮（字间带交叉过渡的缓动曲线，动画连贯不顿挫），控件内部按墙钟在两次位置更新之间连续推进；
///  2. 整行均分模式（无 Words，兜底）：按 <see cref="HighlightFraction"/> 比例把字符均匀点亮。
/// 暂停/启动恢复时保持「暂停时刻」的高亮不动；播放中换句时从 0 开始，第一个字先不亮。
/// </summary>
public class KaraokeTextBlock : TextBlock
{
    public static readonly DependencyProperty KaraokeTextProperty =
        DependencyProperty.Register(nameof(KaraokeText), typeof(string), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, OnRenderPropsChanged));

    /// <summary>目标高亮比例 0..1（连续值；仅在无逐字时间轴时使用）。</summary>
    public static readonly DependencyProperty HighlightFractionProperty =
        DependencyProperty.Register(nameof(HighlightFraction), typeof(double), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(0.0, OnRenderPropsChanged));

    /// <summary>逐字时间轴（每字/词的绝对起止秒）。非空时启用真正的逐字卡拉OK。</summary>
    public static readonly DependencyProperty WordsProperty =
        DependencyProperty.Register(nameof(Words), typeof(IReadOnlyList<TtmlWord>), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(null, OnRenderPropsChanged));

    /// <summary>当前播放位置（秒，绝对时间，含歌词偏移；由 ViewModel 以约 5Hz 更新，控件内部按墙钟按显示器刷新率连续推进）。</summary>
    public static readonly DependencyProperty PositionSecondsProperty =
        DependencyProperty.Register(nameof(PositionSeconds), typeof(double), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(0.0, (d, e) => ((KaraokeTextBlock)d).OnPositionChanged((double)e.NewValue)));

    public static readonly DependencyProperty HighlightBrushProperty =
        DependencyProperty.Register(nameof(HighlightBrush), typeof(Brush), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(Brushes.White, OnRenderPropsChanged));

    public static readonly DependencyProperty BaseBrushProperty =
        DependencyProperty.Register(nameof(BaseBrush), typeof(Brush), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(Brushes.Gray, OnRenderPropsChanged));

    /// <summary>是否正在播放：播放中逐字推进；暂停/启动恢复时保持暂停时刻的高亮。</summary>
    public static readonly DependencyProperty IsPlayingProperty =
        DependencyProperty.Register(nameof(IsPlaying), typeof(bool), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(false, OnRenderPropsChanged));
    /// <summary>卡拉OK推进速度倍率（1.0 = 标准，0.5~2.0；仅影响播放中的推进速度）。</summary>
    public static readonly DependencyProperty KaraokeSpeedProperty =
        DependencyProperty.Register(nameof(KaraokeSpeed), typeof(double), typeof(KaraokeTextBlock),
            new FrameworkPropertyMetadata(1.0, OnRenderPropsChanged));

    private bool _renderingSubscribed;     // CompositionTarget.Rendering 已挂接
    private double _lastTickTime;          // 上一帧时间（秒），用于帧率无关平滑
    private readonly Stopwatch _tickClock = Stopwatch.StartNew();
    private double _currentFraction;   // 当前已点亮比例（0..1，整行均分模式平滑推进）
    private double _targetFraction;    // 目标比例（0..1，来自 HighlightFraction）
    private string _lastText = string.Empty;
    // 整行均分模式：缓存 3 个 Run（同一句内只更新 Foreground，仅重绘、不触发布局，换句才重建）
    private Run? _litRun;
    private Run? _blendRun;
    private Run? _restRun;

    // 逐字模式状态
    private IReadOnlyList<TtmlWord> _words = Array.Empty<TtmlWord>();
    private IReadOnlyList<TtmlWord> _renderedWords = Array.Empty<TtmlWord>();
    private readonly List<Run> _wordRuns = new();
    private bool _hasWords;
    private double _posBase;            // 最近一次来自 ViewModel 的位置（秒）
    private DateTime _posBaseTimeUtc;   // 该位置对应的墙钟时刻

    public KaraokeTextBlock()
    {
        // 120fps：使用 CompositionTarget.Rendering（跟随显示器刷新率），不再用 DispatcherTimer
        IsVisibleChanged += (_, _) =>
        {
            if (!IsVisible) { StopAnimation(); return; }
            if (_hasWords && IsPlaying)
            {
                _posBaseTimeUtc = DateTime.UtcNow; // 重新可见：立即校准墙钟基准（避免位置跳变）
                if (!_renderingSubscribed) StartAnimation();
            }
            else RefreshTarget();
        };
    }

    public string KaraokeText
    {
        get => (string)GetValue(KaraokeTextProperty);
        set => SetValue(KaraokeTextProperty, value);
    }

    public double HighlightFraction
    {
        get => (double)GetValue(HighlightFractionProperty);
        set => SetValue(HighlightFractionProperty, value);
    }

    public IReadOnlyList<TtmlWord>? Words
    {
        get => (IReadOnlyList<TtmlWord>?)GetValue(WordsProperty);
        set => SetValue(WordsProperty, value);
    }

    public double PositionSeconds
    {
        get => (double)GetValue(PositionSecondsProperty);
        set => SetValue(PositionSecondsProperty, value);
    }

    public Brush HighlightBrush
    {
        get => (Brush)GetValue(HighlightBrushProperty);
        set => SetValue(HighlightBrushProperty, value);
    }

    public Brush BaseBrush
    {
        get => (Brush)GetValue(BaseBrushProperty);
        set => SetValue(BaseBrushProperty, value);
    }

    public bool IsPlaying
    {
        get => (bool)GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, value);
    }

    /// <summary>卡拉OK推进速度倍率（1.0 = 标准）。</summary>
    public double KaraokeSpeed
    {
        get => (double)GetValue(KaraokeSpeedProperty);
        set => SetValue(KaraokeSpeedProperty, value);
    }

    private void OnPositionChanged(double pos)
    {
        _posBase = pos;
        _posBaseTimeUtc = DateTime.UtcNow;
        // 位置更新可能来自 seek/恢复：立即重绘一次；隐藏时不启动定时器（避免空转耗 CPU）
        if (!_hasWords || !IsVisible) return;
        if (IsPlaying)
        {
            // 仅当播放位置落在这句的逐字时间轴范围内才需要动画；
            // 完全未开始/已结束的行渲染一次后停止（展开列表每行都是本控件，避免动画风暴）
            if (NeedsAnimation(pos))
            {
                if (!_renderingSubscribed) StartAnimation();
            }
            else
            {
                StopAnimation();
                RenderWords(pos);
            }
        }
        else
        {
            RenderWords(_posBase);
            StopAnimation();
        }
    }

    private static void OnRenderPropsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((KaraokeTextBlock)d).RefreshTarget();

    private void RefreshTarget()
    {
        var words = (IReadOnlyList<TtmlWord>?)GetValue(WordsProperty);
        _hasWords = words is { Count: > 0 };
        if (_hasWords && !ReferenceEquals(words, _words))
        {
            _words = words!;
            _renderedWords = Array.Empty<TtmlWord>(); // 强制重建 Run
            _wordRuns.Clear();
        }

        if (_hasWords)
        {
            // 逐字模式：播放中由 CompositionTarget.Rendering 按墙钟连续推进；暂停/启动恢复直接按最近位置渲染
            if (IsPlaying)
            {
                if (NeedsAnimation(_posBase))
                {
                    if (!_renderingSubscribed) StartAnimation();
                }
                else
                {
                    StopAnimation();
                    RenderWords(_posBase);
                }
                return;
            }

            StopAnimation();
            RenderWords(_posBase);
            return;
        }

        var text = KaraokeText ?? string.Empty;
        _targetFraction = Math.Clamp(HighlightFraction, 0, 1);

        // 换行时从 0 开始：新句第一个字保持未点亮，随进度从左到右平滑点亮。
        if (!string.Equals(text, _lastText, StringComparison.Ordinal))
        {
            _lastText = text;
            if (IsPlaying)
            {
                _currentFraction = 0; // 播放中换句：从 0 开始，第一个字先不亮
                StartAnimation();
            }
            else
            {
                _currentFraction = _targetFraction; // 暂停/启动恢复换行：直接显示目标高亮
                StopAnimation();
            }
            Render();
            return;
        }

        if (Math.Abs(_currentFraction - _targetFraction) < 0.002)
        {
            _currentFraction = _targetFraction;
            StopAnimation();
            Render();
            return;
        }

        if (!_renderingSubscribed) StartAnimation();
    }

    /// <summary>挂接 CompositionTarget.Rendering（跟随显示器刷新率，120Hz 显示器上 120fps）。</summary>
    private void StartAnimation()
    {
        if (_renderingSubscribed) return;
        _renderingSubscribed = true;
        _lastTickTime = _tickClock.Elapsed.TotalSeconds;
        CompositionTarget.Rendering += OnRenderingFrame;
    }

    /// <summary>摘除 CompositionTarget.Rendering。</summary>
    private void StopAnimation()
    {
        if (!_renderingSubscribed) return;
        _renderingSubscribed = false;
        CompositionTarget.Rendering -= OnRenderingFrame;
    }

    private void OnRenderingFrame(object? sender, EventArgs e) => TickAnimation();

    private void TickAnimation()
    {
        if (_hasWords)
        {
            if (IsPlaying)
            {
                // 两次 ViewModel 位置更新之间按墙钟连续推进 → 帧率自适应丝滑，不“一动一停”
                // 按真实时间插值（不乘速度倍率）：ViewModel 每 200ms 用真实播放位置校正一次，
                // 若在此处乘倍率会产生「先超前、再被拉回」的每 200ms 回跳，看起来卡顿。
                // 「高亮更快」改为在 RenderWords 内缩放每个字的进度（见 speedScale），效果相同但不回跳。
                var pos = _posBase + (DateTime.UtcNow - _posBaseTimeUtc).TotalSeconds;
                RenderWords(pos);
                // 该行已全部点亮/尚未开始：静态即可，停止动画（避免列表里多行同时空转）
                if (!NeedsAnimation(pos)) StopAnimation();
            }
            else
            {
                StopAnimation();
                RenderWords(_posBase);
            }
            return;
        }

        // 整行均分模式：缓动逼近（差距大时走得快、接近时变慢）
        var nowTick = _tickClock.Elapsed.TotalSeconds;
        var dtTick = Math.Min(0.05, Math.Max(0.001, nowTick - _lastTickTime));
        _lastTickTime = nowTick;
        // 帧率无关指数平滑：rate=42 在 60fps 下等效于旧的 0.5 系数，120fps 下自动适配
        var lerpAlpha = 1.0 - Math.Exp(-dtTick * 42.0);
        _currentFraction += (_targetFraction - _currentFraction) * lerpAlpha;
        if (Math.Abs(_currentFraction - _targetFraction) < 0.002)
        {
            _currentFraction = _targetFraction;
            StopAnimation();
        }
        Render();
    }

    private void RenderWords(double pos)
    {
        var text = KaraokeText ?? string.Empty;
        if (_words.Count == 0) return;

        // 换句/首次时重建每个字的 Run（文本变化才触发布局）；随后仅更新颜色（只重绘不布局）
        if (!ReferenceEquals(_renderedWords, _words) || _wordRuns.Count != _words.Count)
        {
            _wordRuns.Clear();
            for (var i = 0; i < _words.Count; i++)
            {
                _wordRuns.Add(new Run { Text = _words[i].Text });
            }
            Inlines.Clear();
            foreach (var r in _wordRuns) Inlines.Add(r);
            _renderedWords = _words;
        }

        var hl = ToColor(HighlightBrush) ?? System.Windows.Media.Colors.White;
        var bs = ToColor(BaseBrush) ?? System.Windows.Media.Colors.Gray;

        // 字间交叉过渡：后续字在其开始前约 45ms 提前起笔，前一字在结束后同样微延收笔，
        // 两段缓动曲线首尾重叠 → 高亮像光带一样从左到右“流动”，不会在字边界停一下再动一下；
        // 句首第一个字不提前，保证换句时第一个字保持未点亮。
        const double leadSeconds = 0.045;
        // 卡拉OK速度倍率：作用在每个字的填充进度上（而非时间轴），因此不会与位置校正互相拉扯。
        var speedScale = Math.Clamp(KaraokeSpeed <= 0 ? 1.0 : KaraokeSpeed, 0.2, 3.0);
        for (var i = 0; i < _wordRuns.Count && i < _words.Count; i++)
        {
            var w = _words[i];
            var dur = Math.Max(w.DurationSec, 0.001);
            var lead = i > 0 ? Math.Min(leadSeconds, dur * 0.5) : 0.0;
            var raw = (pos - (w.BeginSec - lead)) / (dur + lead) * speedScale;
            var frac = SmoothStep(raw); // ease-in-out：起笔/收笔有加减速，匀速的机械感消失
            var c = Lerp(bs, hl, frac);
            // 只在颜色字节值真正变化时才新建画刷（高帧率下多数帧的色差不足 1 字节），
            // 避免每帧分配 SolidColorBrush 造成 GC 抖动而掉帧。
            if (_wordRuns[i].Foreground is not System.Windows.Media.SolidColorBrush prev || !ColorEqual(prev.Color, c))
                _wordRuns[i].Foreground = Frozen(new System.Windows.Media.SolidColorBrush(c));
        }
    }

    private void Render()
    {
        var text = KaraokeText ?? string.Empty;

        // 空文本：清空 Inlines（同时释放缓存的 Run）
        if (text.Length == 0)
        {
            if (Inlines.Count > 0)
            {
                Inlines.Clear();
                _litRun = _blendRun = _restRun = null;
            }
            return;
        }

        var f = Math.Clamp(_currentFraction, 0, 1);
        var hl = ToColor(HighlightBrush) ?? System.Windows.Media.Colors.White;
        var bs = ToColor(BaseBrush) ?? System.Windows.Media.Colors.Gray;

        // 按字符着色（而非二维渐变）：换行时高亮按阅读顺序从左到右逐行流动
        var len = text.Length;
        var litChars = Math.Min((int)Math.Floor(f * len), len);
        var blend = f * len - litChars;
        if (litChars >= len) blend = 1;

        if (_litRun is null || !string.Equals(_lastText, text, StringComparison.Ordinal))
        {
            _lastText = text;
            _litRun = new Run();
            _blendRun = new Run();
            _restRun = new Run();
            Inlines.Clear();
            Inlines.Add(_litRun);
            Inlines.Add(_blendRun);
            Inlines.Add(_restRun);
        }

        _litRun.Text = litChars > 0 ? text.Substring(0, litChars) : string.Empty;
        _litRun.Foreground = Frozen(new System.Windows.Media.SolidColorBrush(hl));

        _blendRun!.Text = litChars < len ? text[litChars].ToString() : string.Empty;
        _blendRun.Foreground = litChars < len
            ? Frozen(new System.Windows.Media.SolidColorBrush(Lerp(bs, hl, Math.Clamp(blend, 0, 1))))
            : Frozen(new System.Windows.Media.SolidColorBrush(bs));

        _restRun!.Text = litChars + 1 < len ? text.Substring(litChars + 1) : string.Empty;
        _restRun.Foreground = Frozen(new System.Windows.Media.SolidColorBrush(bs));
    }

    /// <summary>播放位置是否落在本句某个字的起止区间内（该行是否处于正在点亮的状态）。</summary>
    private bool NeedsAnimation(double pos)
    {
        var speed = Math.Clamp(KaraokeSpeed <= 0 ? 1.0 : KaraokeSpeed, 0.2, 3.0);
        foreach (var w in _words)
        {
            if (pos < w.BeginSec) continue;                    // 尚未开始：静态即可
            if ((pos - w.BeginSec) / w.DurationSec * speed < 1) return true; // 仍在点亮：需要动画
        }
        return false;
    }

    private static System.Windows.Media.SolidColorBrush Frozen(System.Windows.Media.SolidColorBrush b) { b.Freeze(); return b; }

    /// <summary>ease-in-out 缓动（smoothstep）：起笔慢→中段快→收笔慢，配合字间交叉过渡实现丝滑连贯的逐字推进。</summary>
    private static double SmoothStep(double t)
    {
        t = Math.Clamp(t, 0, 1);
        return t * t * (3 - 2 * t);
    }

    private static System.Windows.Media.Color Lerp(System.Windows.Media.Color a, System.Windows.Media.Color b, double t)
        => System.Windows.Media.Color.FromArgb(
            (byte)(a.A + (b.A - a.A) * t),
            (byte)(a.R + (b.R - a.R) * t),
            (byte)(a.G + (b.G - a.G) * t),
            (byte)(a.B + (b.B - a.B) * t));

    private static System.Windows.Media.Color? ToColor(Brush? brush)
        => (brush as SolidColorBrush)?.Color;

    /// <summary>比较两个颜色是否完全一致（避免为不足 1 字节的色差重复分配画刷）。</summary>
    private static bool ColorEqual(System.Windows.Media.Color a, System.Windows.Media.Color b)
        => a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
}
