using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinIslands.Services;

/// <summary>
/// 输入法状态：读取当前前台窗口使用的键盘布局（中/英 + 输入法名），
/// 并可切换到中/英另一侧布局（PostMessage WM_INPUTLANGCHANGEREQUEST，与系统切换行为一致）。
/// 全部走 Win32 API，纯本地，无任何联网。
/// </summary>
public static class InputMethodMonitor
{
    private const int LangChinese = 0x04;          // LANG_CHINESE（简体/繁体均属中文）
    private const uint WmInputLangChangeRequest = 0x0050;
    private const int InputLangChangeSysCharset = 0x0001;

    [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    [DllImport("user32.dll")] private static extern IntPtr GetKeyboardLayout(uint idThread);
    [DllImport("user32.dll")] private static extern int GetKeyboardLayoutList(int nBuff, [Out] IntPtr[]? lpList);
    [DllImport("user32.dll")] private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    [DllImport("imm32.dll", CharSet = CharSet.Unicode)] private static extern int ImmGetDescription(IntPtr hkl, StringBuilder lpszDescription, int uBufLen);

    /// <summary>当前前台窗口的键盘布局（HKL），失败返回 Zero。</summary>
    private static IntPtr CurrentHkl()
    {
        try
        {
            var h = GetForegroundWindow();
            if (h == IntPtr.Zero) return IntPtr.Zero;
            var tid = GetWindowThreadProcessId(h, out _);
            return GetKeyboardLayout(tid);
        }
        catch { return IntPtr.Zero; }
    }

    private static bool IsHklChinese(IntPtr hkl)
    {
        if (hkl == IntPtr.Zero) return false;
        var locale = (int)((long)hkl & 0xFFFF); // HKL 低 16 位 = 语言 ID（LCID）
        return (locale & 0xFF) == LangChinese;
    }

    /// <summary>当前是否中文输入法。</summary>
    public static bool IsChinese()
    {
        try { return IsHklChinese(CurrentHkl()); }
        catch { return false; }
    }

    /// <summary>输入法显示文本，如「中 · 微软拼音」/「英 · English (US)」。失败返回空串。</summary>
    public static string GetStatusText()
    {
        try
        {
            var hkl = CurrentHkl();
            if (hkl == IntPtr.Zero) return string.Empty;
            var isChinese = IsHklChinese(hkl);
            if (isChinese)
            {
                var ime = ImeDescription(hkl);
                return ime.Length > 0 ? "中 · " + ime : "中";
            }
            // 英文（或西文）布局通常没有 IME 描述，用布局语言名兜底
            var localeName = LocaleDisplayName((int)((long)hkl & 0xFFFF));
            return localeName.Length > 0 ? "英 · " + localeName : "英";
        }
        catch { return string.Empty; }
    }

    /// <summary>切换到与当前相反的中/英布局（只切换语言大类，不影响输入法选择）。返回是否成功。</summary>
    public static bool ToggleChineseEnglish()
    {
        try
        {
            var h = GetForegroundWindow();
            if (h == IntPtr.Zero) return false;
            var cur = CurrentHkl();
            var curIsChinese = IsHklChinese(cur);
            var n = GetKeyboardLayoutList(0, null);
            if (n <= 1) return false; // 只有一个布局，无从切换
            var list = new IntPtr[n];
            GetKeyboardLayoutList(n, list);
            IntPtr target = IntPtr.Zero;
            foreach (var hkl in list)
            {
                if (hkl != cur && IsHklChinese(hkl) != curIsChinese) { target = hkl; break; }
            }
            if (target == IntPtr.Zero) return false;
            return PostMessage(h, WmInputLangChangeRequest, (IntPtr)InputLangChangeSysCharset, target);
        }
        catch { return false; }
    }

    /// <summary>IME 描述（如「微软拼音」）；非中文布局通常为空。</summary>
    private static string ImeDescription(IntPtr hkl)
    {
        try
        {
            var sb = new StringBuilder(256);
            return ImmGetDescription(hkl, sb, sb.Capacity) > 0 ? sb.ToString() : string.Empty;
        }
        catch { return string.Empty; }
    }

    /// <summary>语言 ID → 地区缩写（如 0x0409 → "US"；失败返回空串）。</summary>
    private static string LocaleDisplayName(int locale)
    {
        try
        {
            var code = System.Globalization.CultureInfo.GetCultureInfo(locale);
            var region = new System.Globalization.RegionInfo(code.LCID).TwoLetterISORegionName;
            return string.IsNullOrEmpty(region) ? code.ThreeLetterWindowsLanguageName : region.ToUpperInvariant();
        }
        catch { return string.Empty; }
    }
}
