using System.Runtime.InteropServices;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Microsoft.Win32;

namespace WinIslands.UI;

/// <summary>
/// Windows visual effects for the island window:
///  * Acrylic / blur-behind via SetWindowCompositionAttribute (Win10 + Win11)
///  * Immersive dark mode via DwmSetWindowAttribute
///  * True rounded window region via SetWindowRgn so the blur follows the capsule shape
/// </summary>
public static class WindowEffects
{
    // ── SetWindowCompositionAttribute (undocumented, works on Win10/11) ──
    private const int WCA_ACCENT_POLICY = 19;
    private const int ACCENT_ENABLE_ACRYLICBLURBEHIND = 4;
    private const int ACCENT_ENABLE_BLURBEHIND = 3;

    [StructLayout(LayoutKind.Sequential)]
    private struct AccentPolicy
    {
        public int AccentState;
        public int AccentFlags;
        public int GradientColor; // 0xAABBGGRR
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WindowCompositionAttributeData
    {
        public int Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    [DllImport("user32.dll")]
    private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    // ── DWM ──
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
    private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

    // ── Region (CreateRoundRectRgn lives in gdi32.dll on modern Windows) ──
    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int widthEllipse, int heightEllipse);

    [DllImport("user32.dll")]
    private static extern int SetWindowRgn(IntPtr hwnd, IntPtr hRgn, bool redraw);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    /// <summary>Apply acrylic blur-behind with a tint color.</summary>
    public static void ApplyAcrylic(IntPtr hwnd, Color tint, double opacity)
    {
        try
        {
            // GradientColor format: 0xAABBGGRR (alpha first)
            var alpha = (int)(Math.Clamp(opacity, 0, 1) * 255);
            var argb = (alpha << 24)
                       | (tint.B << 16)
                       | (tint.G << 8)
                       | tint.R;

            var accent = new AccentPolicy
            {
                AccentState = ACCENT_ENABLE_ACRYLICBLURBEHIND,
                AccentFlags = 2,
                GradientColor = argb,
            };
            var data = new WindowCompositionAttributeData
            {
                Attribute = WCA_ACCENT_POLICY,
                Data = Marshal.AllocHGlobal(Marshal.SizeOf<AccentPolicy>()),
                SizeOfData = Marshal.SizeOf<AccentPolicy>(),
            };
            try
            {
                Marshal.StructureToPtr(accent, data.Data, false);
                _ = SetWindowCompositionAttribute(hwnd, ref data);
            }
            finally
            {
                Marshal.FreeHGlobal(data.Data);
            }
        }
        catch (Exception ex)
        {
            Services.AppLogger.Warn($"ApplyAcrylic failed: {ex.Message}");
        }
    }

    public static void ApplyDarkMode(IntPtr hwnd, bool dark)
    {
        try
        {
            var value = dark ? 1 : 0;
            _ = DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
        }
        catch (Exception ex)
        {
            Services.AppLogger.Warn($"ApplyDarkMode failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Clip the window to a rounded rectangle at (x,y) with the given size so the
    /// acrylic blur and hit-testing follow the island card exactly.
    /// </summary>
    public static void SetRoundedRegion(IntPtr hwnd, int x, int y, int width, int height, int radius)
    {
        try
        {
            var hrgn = CreateRoundRectRgn(x, y, x + width + 1, y + height + 1, Math.Max(1, radius), Math.Max(1, radius));
            if (hrgn == IntPtr.Zero) return;
            if (SetWindowRgn(hwnd, hrgn, true) == 0)
                DeleteObject(hrgn); // only delete when the system did not take ownership
        }
        catch (Exception ex)
        {
            Services.AppLogger.Warn($"SetRoundedRegion failed: {ex.Message}");
        }
    }
}

/// <summary>Reads the current Windows light/dark app theme.</summary>
public static class ThemeHelper
{
    private const string PersonalizeKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

    public static bool IsSystemDark()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(PersonalizeKey);
            var value = key?.GetValue("AppsUseLightTheme");
            return value is int i && i == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Parse a #RRGGBB / #AARRGGBB hex string into a Color.</summary>
    public static Color ParseColor(string hex, Color fallback)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(hex)) return fallback;
            var h = hex.Trim().TrimStart('#');
            if (h.Length == 6)
                return Color.FromRgb(
                    Convert.ToByte(h[..2], 16),
                    Convert.ToByte(h[2..4], 16),
                    Convert.ToByte(h[4..6], 16));
            if (h.Length == 8)
                return Color.FromArgb(
                    Convert.ToByte(h[..2], 16),
                    Convert.ToByte(h[2..4], 16),
                    Convert.ToByte(h[4..6], 16),
                    Convert.ToByte(h[6..8], 16));
        }
        catch { /* fall through */ }

        return fallback;
    }
}



