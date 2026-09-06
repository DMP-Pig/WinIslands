using WinIslands.Services;

namespace WinIslands.UI;

/// <summary>一行歌词视图模型：主句（逐字卡拉OK高亮）+ 可选的翻译行 + 可选的逐字时间轴。</summary>
public sealed class LyricLineViewModel : ObservableObject
{
    private readonly LyricLine _line;
    private bool _isCurrent;
    private bool _showTranslation = true;
    private double _highlightFraction;

    public LyricLineViewModel(LyricLine line, string? translation = null, IReadOnlyList<TtmlWord>? words = null)
    {
        _line = line;
        Translation = string.IsNullOrWhiteSpace(translation) ? null : translation;
        Words = words is { Count: > 0 } ? words : Array.Empty<TtmlWord>();
    }

    public string Text => _line.Text;
    public TimeSpan Time => _line.Time;

    /// <summary>翻译行文本（可能为 null）。</summary>
    public string? Translation { get; }

    /// <summary>
    /// 该行内的逐字时间轴（来自 AMLL TTML）。非空时，卡拉OK控件按每个字/词的
    /// 独立起止时间点亮，实现真正的逐字卡拉OK；为空时回退为整行均分比例。
    /// </summary>
    public IReadOnlyList<TtmlWord> Words { get; }

    /// <summary>是否显示翻译行（由「展开歌词快捷操作」的翻译开关统一控制）。</summary>
    public bool ShowTranslation
    {
        get => _showTranslation;
        set => Set(ref _showTranslation, value);
    }

    public bool HasTranslation => Translation is not null;

    public bool IsCurrent
    {
        get => _isCurrent;
        set => Set(ref _isCurrent, value);
    }

    /// <summary>整行卡拉OK：已点亮比例（0..1，连续值，平滑过渡；仅在无逐字时间轴时使用）。</summary>
    public double HighlightFraction
    {
        get => _highlightFraction;
        set => Set(ref _highlightFraction, value);
    }
}
