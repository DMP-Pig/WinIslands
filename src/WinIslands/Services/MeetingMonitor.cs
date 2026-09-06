using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WinIslands.Services;

/// <summary>
/// 开会静音助手：通过前台窗口标题 / 进程名检测是否处于会议中
/// （Microsoft Teams、Zoom、腾讯会议、钉钉、飞书、Webex、Slack、Discord、Google Meet 等）。
/// 纯本机启发式检测，不联网、不上报任何数据。供「会议中自动勿扰」与灵动岛「会议中」组件使用。
/// </summary>
public static class MeetingMonitor
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint pid);

    /// <summary>是否正在会议中（前台窗口匹配会议特征）。</summary>
    public static bool IsInMeeting { get; private set; }

    /// <summary>检测到的会议软件/窗口显示名（未检测到为空字符串）。</summary>
    public static string AppName { get; private set; } = string.Empty;

    /// <summary>会议状态变化事件（参数：是否进入会议）。</summary>
    public static event EventHandler<bool>? StateChanged;

    /// <summary>内置会议客户端进程名（不含 .exe，大小写不敏感）。</summary>
    private static readonly HashSet<string> MeetingProcesses = new(StringComparer.OrdinalIgnoreCase)
    {
        "teams", "ms-teams", "msteams", "zoom", "wemeetapp", "wemeet", "dingtalk",
        "feishu", "lark", "webexmta", "webex", "slack", "discord", "skype", "skypeapp",
        "gotomeeting", "bluejeans", "meet",
    };

    /// <summary>内置窗口标题关键词（小写，包含匹配）。</summary>
    private static readonly string[] MeetingTitleKeywords =
    {
        "meeting", "会议", "腾讯会议", "zoom", "teams", "钉钉", "飞书", "webex", "瞩目",
        "google meet", "conference", "线上课堂",
    };

    private static string _customKeywords = string.Empty;
    private static string[] _customList = Array.Empty<string>();

    /// <summary>
    /// 更新自定义关键词（逗号/分号分隔；留空 = 仅用内置列表）。
    /// 只有关键词真正变化时才重新拆分，避免每帧重复分配。
    /// </summary>
    public static void SetCustomKeywords(string keywords)
    {
        var k = keywords ?? string.Empty;
        if (string.Equals(k, _customKeywords, StringComparison.Ordinal)) return;
        _customKeywords = k;
        _customList = k
            .Split(new[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => x.Length > 0)
            .ToArray();
    }

    /// <summary>检测当前前台窗口并更新会议状态。返回状态是否发生变化（供 UI 决定是否重建组件）。</summary>
    public static bool Check()
    {
        try
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero) return SetState(false, string.Empty);

            var len = GetWindowTextLength(hwnd);
            var sb = new StringBuilder(Math.Max(1, len + 1));
            GetWindowText(hwnd, sb, sb.Capacity);
            var title = sb.ToString();

            uint pid = 0;
            GetWindowThreadProcessId(hwnd, out pid);
            string? procName = null;
            try { using var p = Process.GetProcessById((int)pid); procName = p.ProcessName; }
            catch { /* 进程已退出等 */ }

            // 1) 进程名匹配（会议客户端最可靠）
            if (procName is not null && MeetingProcesses.Contains(procName))
                return SetState(true, TitleFromProcess(procName, title));

            // 2) 自定义关键词（窗口标题，优先级高于内置，便于用户精确控制）
            foreach (var key in _customList)
            {
                if (title.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                    return SetState(true, string.IsNullOrWhiteSpace(title) ? key : title.Trim());
            }

            // 3) 内置标题关键词
            foreach (var key in MeetingTitleKeywords)
            {
                if (title.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                    return SetState(true, string.IsNullOrWhiteSpace(title) ? key : title.Trim());
            }

            return SetState(false, string.Empty);
        }
        catch
        {
            return SetState(false, string.Empty);
        }
    }

    private static bool SetState(bool inMeeting, string app)
    {
        var changed = inMeeting != IsInMeeting || !string.Equals(app, AppName, StringComparison.Ordinal);
        IsInMeeting = inMeeting;
        AppName = app;
        if (changed) StateChanged?.Invoke(null, inMeeting);
        return changed;
    }

    /// <summary>进程名 → 显示名；窗口标题非空时优先用窗口标题（可能含具体会议名）。</summary>
    private static string TitleFromProcess(string proc, string title)
    {
        if (!string.IsNullOrWhiteSpace(title)) return title.Trim();
        return proc.ToLowerInvariant() switch
        {
            "teams" or "ms-teams" or "msteams" => "Microsoft Teams",
            "zoom" => "Zoom",
            "wemeetapp" or "wemeet" => "腾讯会议",
            "dingtalk" => "钉钉",
            "feishu" or "lark" => "飞书",
            "webexmta" or "webex" => "Webex",
            "slack" => "Slack",
            "discord" => "Discord",
            "skype" or "skypeapp" => "Skype",
            _ => proc,
        };
    }
}
