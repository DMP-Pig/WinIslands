using System.Collections.Generic;

namespace WinIslands.UI;

/// <summary>
/// 组件自定义图标：默认图标库（Segoe MDL2 Assets 字体字符）。
/// 用户在「设置 → 组件」页面可为每个组件指定任意图标（emoji 或 MDL2 字符），
/// 运行时读取 AppSettings.ComponentIcons（Key=组件 Kind，Value=图标字符）。
/// 没有默认图标的组件（Time / Weather / Date / QuickToggles / Song）不支持图标定制。
/// </summary>
public static class ComponentIcons
{
    /// <summary>组件默认图标（Key=组件 Kind，Value=字体字符）。</summary>
    public static readonly IReadOnlyDictionary<string, string> Defaults = new Dictionary<string, string>
    {
        ["Cpu"] = "",
        ["Gpu"] = "",
        ["Mic"] = "",
        ["Cam"] = "",
        ["Ram"] = "",
        ["Net"] = "",
        ["Battery"] = "",
        ["CapsLock"] = "",
        ["Clipboard"] = "",
        ["Todo"] = "",
        ["Timer"] = "",
        ["Schedule"] = "",
        ["Holiday"] = "",
        ["Meeting"] = "",
        ["Disk"] = "",
        ["InputMethod"] = "",
        ["Song"] = "",
        ["ScreenCap"] = "",
        ["Recording"] = "",
        ["VolumeTemp"] = "",
        ["Volume"] = "",
        ["Usage"] = "",
        ["FileTransfer"] = "",
        ["FileCopy"] = "",
        ["Download"] = "",
    };

    /// <summary>该组件是否支持图标定制（有默认图标才支持）。</summary>
    public static bool SupportsIcon(string kind) => Defaults.ContainsKey(kind);

    /// <summary>该组件的默认图标；无则返回空串。</summary>
    public static string Default(string kind) => Defaults.TryGetValue(kind, out var v) ? v : string.Empty;

    /// <summary>解析组件图标：优先用户自定义，否则默认；都没有返回空串。</summary>
    public static string Resolve(string kind, IDictionary<string, string>? custom)
    {
        if (custom is not null && custom.TryGetValue(kind, out var v)) return v.Trim();
        return Default(kind);
    }
}
