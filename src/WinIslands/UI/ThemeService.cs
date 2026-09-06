using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Brushes = System.Windows.Media.Brushes;
using WinIslands.Services;

namespace WinIslands.UI;

/// <summary>
/// Computes the brush palette for the island from settings (theme mode, accent color,
/// skin preset, opacity) and re-applies it when settings or the OS theme change.
/// </summary>
public sealed class ThemeService
{
    private AppSettings _settings = new();

    public bool IsDark { get; private set; } = true;

    /// <summary>定时明暗切换：非 null 时以该值覆盖系统跟随（仅 Theme=Auto 时由 App 设置）。</summary>
    public bool? OverrideDark { get; set; }

    public Brush CardBackground { get; private set; } = Brushes.Transparent;
    public Brush CardBorder { get; private set; } = Brushes.Transparent;
    public Brush TextPrimary { get; private set; } = Brushes.White;
    public Brush TextSecondary { get; private set; } = Brushes.Gray;
    public Brush AccentBrush { get; private set; } = Brushes.Transparent;
    public Brush AccentBorderBrush { get; private set; } = Brushes.Transparent;
    public Brush ButtonHoverBrush { get; private set; } = Brushes.Transparent;
    public Brush SliderTrackBrush { get; private set; } = Brushes.Gray;
    public Brush SliderThumbBrush { get; private set; } = Brushes.White;
    public Color AccentColor { get; private set; } = Colors.Indigo;
    public Color TintColor { get; private set; } = Color.FromRgb(0x1E, 0x1E, 0x2E);
    public double Opacity { get; private set; } = 0.92;

    public event EventHandler? ThemeChanged;

    /// <summary>皮肤预设表：强调色 + 深/浅两种液态玻璃底色（Hex）。</summary>
    private static readonly (string Key, string Accent, string TintDark, string TintLight)[] Skins =
    {
        ("Ocean",    "#5B8DEF", "#0A1B33", "#E6F0FF"),
        ("Forest",   "#00B894", "#0A2118", "#E4FBF3"),
        ("Sunset",   "#E17055", "#2A120E", "#FDEBE5"),
        ("Neon",     "#E84393", "#2A0A1E", "#FDE7F2"),
        ("Mono",     "#8A8A93", "#15151A", "#F0F0F2"),
        ("Grape",    "#A29BFE", "#16102E", "#EFEDFF"),
        ("Sky",      "#38BDF8", "#0A1E2E", "#E4F4FD"),
        ("Rose",     "#FB7185", "#2E0F16", "#FDECEF"),
        ("Amber",    "#F59E0B", "#2A1D08", "#FEF3E2"),
        ("Lime",     "#84CC16", "#1C260A", "#F2FBE3"),
        ("Teal",     "#14B8A6", "#0A2422", "#E2FBF8"),
        ("Lavender", "#C4B5FD", "#1E1830", "#F2EDFF"),
        ("Crimson",  "#DC2626", "#281010", "#FDEBEB"),
        ("Midnight", "#6366F1", "#101028", "#E9E9FD"),
        ("Coffee",   "#B08968", "#221812", "#F5EDE5"),
        ("Sakura",   "#F9A8D4", "#2A1422", "#FDEDF7"),
        ("Aurora",   "#34D399", "#0A2518", "#E3FBF0"),
    };

    /// <summary>解析 #RRGGBB / #AARRGGBB / RRGGBB；无效时返回 fallback。</summary>
    private static Color ParseHex(string? hex, Color fallback)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(hex)) return fallback;
            var h = hex.Trim().TrimStart('#');
            if (h.Length == 6)
                return Color.FromRgb(Convert.ToByte(h.Substring(0, 2), 16),
                                     Convert.ToByte(h.Substring(2, 2), 16),
                                     Convert.ToByte(h.Substring(4, 2), 16));
            if (h.Length == 8)
                return Color.FromArgb(Convert.ToByte(h.Substring(0, 2), 16),
                                      Convert.ToByte(h.Substring(2, 2), 16),
                                      Convert.ToByte(h.Substring(4, 2), 16),
                                      Convert.ToByte(h.Substring(6, 2), 16));
        }
        catch { /* 无效输入 -> fallback */ }
        return fallback;
    }

    /// <summary>壁纸主色缓存：路径不变则不重复解码（壁纸文件常达数 MB，避免每次重读）。</summary>
    private static (string Path, Color Color)? _wallpaperCache;

    /// <summary>读取当前壁纸并缩放到 1x1 取平均主色；不可用（纯色/幻灯片/路径无效）时返回 null。</summary>
    private static Color? GetWallpaperColor()
    {
        try
        {
            // WPF 不提供壁纸路径，从注册表读取（纯本地，不联网）
            var path = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallPaper", "") as string;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return null;
            if (_wallpaperCache?.Path == path) return _wallpaperCache.Value.Color;
            var bmp = new BitmapImage(new Uri(path));
            bmp.Freeze();
            var rt = new RenderTargetBitmap(1, 1, 96, 96, PixelFormats.Pbgra32);
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
                dc.DrawImage(bmp, new Rect(0, 0, 1, 1));
            rt.Render(dv);
            var px = new byte[4];
            rt.CopyPixels(px, 4, 0);
            var c = Color.FromRgb(px[2], px[1], px[0]); // Pbgra32 内存序：B G R A
            _wallpaperCache = (path, c);
            return c;
        }
        catch
        {
            // 任何读取/解码异常都回退原主题色，绝不影响主流程
            return null;
        }
    }

    public void Apply(AppSettings settings)
    {
        _settings = settings;
        var dark = settings.Theme switch
        {
            ThemeMode.Light => false,
            ThemeMode.Dark => true,
            _ => OverrideDark ?? ThemeHelper.IsSystemDark(),
        };
        IsDark = dark;
        Opacity = Math.Clamp(settings.Opacity, 0.3, 1.0);
        AccentColor = ThemeHelper.ParseColor(settings.AccentColor, Color.FromRgb(0x6C, 0x5C, 0xE7));

        // 主题预设（皮肤）：覆盖强调色与背景色调；Default 使用用户自定义强调色 + 默认底色
        var preset = settings.ThemePreset;
        var skin = Skins.FirstOrDefault(s => s.Key == preset);
        bool tintResolved = false;
        if (skin != default)
        {
            AccentColor = ThemeHelper.ParseColor(skin.Accent, AccentColor);
            TintColor = ThemeHelper.ParseColor(dark ? skin.TintDark : skin.TintLight, TintColor);
            tintResolved = true;
        }
        else if (preset == "Custom")
        {
            // 自定义皮肤：强调色用用户主题色，背景色用 ThemeTint（留空则回退明暗默认底色）
            TintColor = ParseHex(settings.ThemeTint, TintColor);
            tintResolved = !string.IsNullOrWhiteSpace(settings.ThemeTint);
        }
        // Default（或未知预设）：强调色保持用户自定义，背景色用下面的明暗默认值
        // 壁纸取色（可选）：开启后壁纸主色覆盖强调色（含皮肤预设），失败则保持原色
        if (settings.WallpaperThemeColorEnabled)
        {
            var wall = GetWallpaperColor();
            if (wall is Color wc) AccentColor = wc;
        }

        if (dark)
        {
            if (!tintResolved) TintColor = Color.FromRgb(0x16, 0x16, 0x22);
            TextPrimary = new SolidColorBrush(Color.FromArgb(235, 245, 245, 250));
            TextSecondary = new SolidColorBrush(Color.FromArgb(170, 245, 245, 250));
            CardBorder = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));
            ButtonHoverBrush = new SolidColorBrush(Color.FromArgb(36, 255, 255, 255));
            SliderTrackBrush = new SolidColorBrush(Color.FromArgb(120, 255, 255, 255));
            SliderThumbBrush = new SolidColorBrush(Color.FromArgb(235, 255, 255, 255));
        }
        else
        {
            if (!tintResolved) TintColor = Color.FromRgb(0xF2, 0xF2, 0xF6);
            TextPrimary = new SolidColorBrush(Color.FromArgb(230, 25, 25, 32));
            TextSecondary = new SolidColorBrush(Color.FromArgb(150, 25, 25, 32));
            CardBorder = new SolidColorBrush(Color.FromArgb(70, 255, 255, 255));
            ButtonHoverBrush = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
            SliderTrackBrush = new SolidColorBrush(Color.FromArgb(110, 0, 0, 0));
            SliderThumbBrush = new SolidColorBrush(Color.FromArgb(230, 25, 25, 32));
        }

        var accent = Color.FromArgb((byte)(dark ? 255 : 235), AccentColor.R, AccentColor.G, AccentColor.B);
        AccentBrush = new SolidColorBrush(accent);
        AccentBorderBrush = new SolidColorBrush(Color.FromArgb(160, AccentColor.R, AccentColor.G, AccentColor.B));

        // iOS 风格液态玻璃胶囊：紧凑态更通透（88%），展开态由 GlassLayer 叠加变实（≈97%）
        // 用户 Opacity 设置（0.3~1.0）整体缩放基础透明度，深色取色仍来自 TintColor
        var compactAlpha = (byte)Math.Round(224 * Opacity);
        CardBackground = new SolidColorBrush(Color.FromArgb(compactAlpha, TintColor.R, TintColor.G, TintColor.B));

        FreezeAll();
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void FreezeAll()
    {
        foreach (var b in new[] { CardBackground, CardBorder, TextPrimary, TextSecondary, AccentBrush, AccentBorderBrush, ButtonHoverBrush, SliderTrackBrush, SliderThumbBrush })
            b.Freeze();
    }
}

