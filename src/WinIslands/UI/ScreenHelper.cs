using System.Runtime.InteropServices;
using System.Windows;
using Point = System.Windows.Point;
using WinIslands.Services;

namespace WinIslands.UI;

/// <summary>
/// Multi-monitor + DPI helpers. WinForms <see cref="System.Windows.Forms.Screen"/> coordinates
/// are physical pixels; WPF window positions are DIPs (1/96"). Conversion uses each monitor's
/// real DPI (GetDpiForMonitor) so the island is placed correctly on mixed-DPI setups (120/150/200%).
/// </summary>
public static class ScreenHelper
{
    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromPoint(POINT pt, uint flags);

    [DllImport("shcore.dll")]
    private static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X; public int Y; }

    private const uint MONITOR_DEFAULTTONEAREST = 2;
    private const int MDT_EFFECTIVE_DPI = 0;

    /// <summary>Resolve which monitors the island should appear on per settings.</summary>
    public static IReadOnlyList<System.Windows.Forms.Screen> ResolveScreens(AppSettings s)
    {
        var all = System.Windows.Forms.Screen.AllScreens;
        if (all.Length == 0) return new[] { System.Windows.Forms.Screen.PrimaryScreen! };

        return s.Monitor switch
        {
            MonitorSelection.Primary => new[] { System.Windows.Forms.Screen.PrimaryScreen! },
            MonitorSelection.Index => new[] { all[Math.Clamp(s.MonitorIndex, 0, all.Length - 1)] },
            MonitorSelection.All => all.ToList(),
            _ => new[] { System.Windows.Forms.Screen.PrimaryScreen! },
        };
    }

    /// <summary>Physical pixels per DIP for a monitor (e.g. 1.5 at 150%).</summary>
    public static double GetDpiScale(System.Windows.Forms.Screen screen)
    {
        try
        {
            var b = screen.Bounds;
            var pt = new POINT { X = b.X + b.Width / 2, Y = b.Y + b.Height / 2 };
            var hmon = MonitorFromPoint(pt, MONITOR_DEFAULTTONEAREST);
            if (hmon == IntPtr.Zero) return 1.0;
            if (GetDpiForMonitor(hmon, MDT_EFFECTIVE_DPI, out var dpiX, out _) != 0) return 1.0;
            return dpiX / 96.0;
        }
        catch
        {
            return 1.0;
        }
    }

    /// <summary>DIP bounds of a monitor's full area.</summary>
    public static Rect DpiBounds(System.Windows.Forms.Screen screen)
    {
        var scale = GetDpiScale(screen);
        var b = screen.Bounds;
        return new Rect(b.X / scale, b.Y / scale, b.Width / scale, b.Height / scale);
    }

    /// <summary>DIP bounds of a monitor's work area (excludes taskbar).</summary>
    public static Rect DpiWorkArea(System.Windows.Forms.Screen screen)
    {
        var scale = GetDpiScale(screen);
        var b = screen.WorkingArea;
        return new Rect(b.X / scale, b.Y / scale, b.Width / scale, b.Height / scale);
    }

    /// <summary>Compute the window's Left/Top (DIPs) for the given monitor + position + size.</summary>
    public static Point ComputePosition(System.Windows.Forms.Screen screen, IslandPosition position,
        double width, double height, double offsetX, double offsetY)
    {
        var work = DpiWorkArea(screen);
        var x = position == IslandPosition.Right
            ? work.Right - width - offsetX
            : work.Left + (work.Width - width) / 2 + offsetX;
        var y = work.Top + offsetY;
        return new Point(x, y);
    }

    /// <summary>DPI scale for a window (DIPs per physical pixel at the window's monitor).</summary>
    public static double WindowDpiScale(Window window)
    {
        try
        {
            var dpi = System.Windows.Media.VisualTreeHelper.GetDpi(window);
            return dpi.DpiScaleX;
        }
        catch
        {
            return 1.0;
        }
    }
}

