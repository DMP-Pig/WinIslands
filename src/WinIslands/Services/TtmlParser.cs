using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WinIslands.Services;

/// <summary>一个逐字时间片：文本 + 绝对起止秒（来自 TTML &lt;span&gt;）。</summary>
public sealed record TtmlWord(string Text, double BeginSec, double EndSec)
{
    public double DurationSec => Math.Max(0.001, EndSec - BeginSec);
}

/// <summary>TTML 中的一行（&lt;p&gt; 元素）：行文本 + 该行内逐字时间轴。</summary>
public sealed record TtmlLine(string Text, double BeginSec, double EndSec, IReadOnlyList<TtmlWord> Words)
{
    public bool HasWords => Words.Count > 0;
}

/// <summary>解析后的 TTML 逐字歌词文档（Apple Music 风格）。</summary>
public sealed class TtmlDocument
{
    public IReadOnlyList<TtmlLine> Lines { get; init; } = Array.Empty<TtmlLine>();
    public bool IsEmpty => Lines.Count == 0;
}

/// <summary>
/// 解析 Apple Music 风格 TTML 逐字歌词 XML（AMLL TTML DataBase 使用该格式）。
/// 结构：&lt;tt&gt;&lt;body&gt;&lt;div&gt;&lt;p begin end&gt;&lt;span begin end&gt;字&lt;/span&gt;…&lt;/p&gt;…
/// 每个 &lt;span&gt; 通常是一个字/词（带独立起止时间）；<c>ttm:role="x-translation"</c> /
/// <c>x-roman</c> 的 span 是翻译/音译，不参与主句逐字高亮。
/// 解析失败或格式不兼容时返回空文档（调用方优雅降级为逐句高亮）。
/// </summary>
public static class TtmlParser
{
    private static readonly XNamespace Tt = "http://www.w3.org/ns/ttml";
    private static readonly XNamespace Ttm = "http://www.w3.org/ns/ttml#metadata";

    private static readonly Regex TimeRegex = new(
        @"^(?:(?<h>\d+):)?(?<m>\d{1,2}):(?<s>\d{1,2})(?:[.,](?<f>\d{1,4}))?$",
        RegexOptions.Compiled);

    public static TtmlDocument Parse(string ttmlXml)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ttmlXml)) return new TtmlDocument();

            XDocument doc;
            try { doc = XDocument.Parse(ttmlXml); }
            catch { return new TtmlDocument(); }

            var body = doc.Root?.Element(Tt + "body") ?? doc.Root?.Element("body");
            if (body is null) return new TtmlDocument();

            var lines = new List<TtmlLine>();
            foreach (var p in body.Descendants().Where(e => e.Name.LocalName == "p"))
            {
                var line = ParseLine(p);
                if (line is not null) lines.Add(line);
            }

            // 时间轴可能乱序，按行开始时间排序（与 LRC 索引保持一致）
            lines.Sort((a, b) => a.BeginSec.CompareTo(b.BeginSec));
            return new TtmlDocument { Lines = lines };
        }
        catch
        {
            return new TtmlDocument();
        }
    }

    private static TtmlLine? ParseLine(XElement p)
    {
        var pBegin = ParseTimeAttr(p, "begin");
        var pEnd = ParseTimeAttr(p, "end");
        if (pBegin is null) return null;

        var words = new List<TtmlWord>();

        // 1) <p> 的直接文本（无 span 包裹），作为整个词条
        var direct = string.Concat(p.Nodes().OfType<XText>().Select(t => t.Value)).Trim();
        if (direct.Length > 0)
        {
            words.Add(new TtmlWord(direct, pBegin.Value, pEnd ?? pBegin.Value + 1.0));
        }

        // 2) 叶子 <span>（不含子 span），按文档顺序收集；跳过翻译/音译角色
        foreach (var span in p.Descendants().Where(e => e.Name.LocalName == "span" && !e.Elements().Any(c => c.Name.LocalName == "span")))
        {
            var role = (string?)span.Attribute(Ttm + "role");
            if (role is "x-translation" or "x-roman") continue;

            var text = span.Value.Trim();
            if (text.Length == 0) continue;

            var begin = ParseTimeAttr(span, "begin") ?? pBegin.Value;
            var end = ParseTimeAttr(span, "end") ?? pEnd ?? begin + 1.0;
            words.Add(new TtmlWord(text, begin, Math.Max(begin + 0.001, end)));
        }

        if (words.Count == 0) return null;

        var lineText = string.Concat(words.Select(w => w.Text));
        return new TtmlLine(lineText, pBegin.Value, pEnd ?? pBegin.Value + 1.0, words);
    }

    private static double? ParseTimeAttr(XElement el, string name)
    {
        var raw = (string?)el.Attribute(name);
        return string.IsNullOrWhiteSpace(raw) ? null : ParseClock(raw);
    }

    /// <summary>解析 TTML 时钟格式：mm:ss.fff / hh:mm:ss.fff（也兼容 1:02:03.4）。</summary>
    internal static double? ParseClock(string raw)
    {
        raw = raw.Trim();
        if (raw.EndsWith("s", StringComparison.OrdinalIgnoreCase) && double.TryParse(raw[..^1], NumberStyles.Float, CultureInfo.InvariantCulture, out var secs))
            return secs;

        var m = TimeRegex.Match(raw);
        if (!m.Success) return null;

        var hours = m.Groups["h"].Success ? double.Parse(m.Groups["h"].Value, CultureInfo.InvariantCulture) : 0;
        var minutes = double.Parse(m.Groups["m"].Value, CultureInfo.InvariantCulture);
        var seconds = double.Parse(m.Groups["s"].Value, CultureInfo.InvariantCulture);
        var frac = m.Groups["f"].Success ? m.Groups["f"].Value : string.Empty;
        var ms = frac.Length == 0 ? 0 : double.Parse("0." + frac, CultureInfo.InvariantCulture);
        return hours * 3600 + minutes * 60 + seconds + ms;
    }
}
