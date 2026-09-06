using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WinIslands.UI.Converters;

public sealed class BoolToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var b = value is true;
        if (Invert) b = !b;
        return b ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is Visibility.Visible;
}

public sealed class NullToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; } // true = visible when null

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Invert=false：有值（非 null）时可见；Invert=true：为 null 时可见
        var visible = value is not null;
        if (Invert) visible = !visible;
        return visible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public sealed class TimeSpanToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan ts && ts >= TimeSpan.Zero)
            return ts.TotalHours >= 1 ? ts.ToString(@"h\:mm\:ss") : ts.ToString(@"m\:ss");
        if (value is double seconds) return TimeSpan.FromSeconds(Math.Max(0, seconds)).ToString(@"m\:ss");
        return "0:00";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Converts an opacity double (0..1) to a Visibility (used for fade-out in compact mode).</summary>


/// <summary>
/// Converts an icon string to the proper font family:
/// Segoe MDL2 Assets for private-use-area glyphs (U+E000-U+F8FF), otherwise Segoe UI Emoji.
/// Used by component icons so users can enter either MDL2 glyphs or emoji.
/// </summary>
public sealed class Mdl2GlyphFontConverter : IValueConverter
{
    private static readonly System.Windows.Media.FontFamily Mdl2 = new("Segoe MDL2 Assets");
    private static readonly System.Windows.Media.FontFamily Emoji = new("Segoe UI Emoji");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            var s = value?.ToString();
            if (!string.IsNullOrEmpty(s))
            {
                var cp = char.ConvertToUtf32(s, 0);
                if (cp >= 0xE000 && cp <= 0xF8FF) return Mdl2;
            }
        }
        catch { /* fall through */ }
        return Emoji;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public sealed class OpacityToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is double d && d > 0.01 ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
