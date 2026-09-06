using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinIslands.Services;

/// <summary>
/// 一个按钮动作：
///   url     -> 用默认浏览器/系统打开 Value（URL 或文件）
///   launch  -> 启动 Value 指定的程序（可含参数）
///   command -> 在本地执行 Value 作为命令行（cmd /c；仅本机回环 API，可配 Token，请仅信任可信推送方）
///   notify  -> 通知推送方处理（推送方需自行注册回呼；暂未实现，留作扩展）
/// </summary>
public sealed class IslandPushButton
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = "";

    [JsonPropertyName("action")]
    public string Action { get; set; } = "url"; // url | launch | notify

    [JsonPropertyName("value")]
    public string Value { get; set; } = "";
}

/// <summary>
/// 输入框：用户在上岛卡片填写文字并提交，按推送方配置的 action 执行（默认 notify 回传推送方，
/// 由推送方通过 WebSocket push_input 事件自行处理）。
/// </summary>
public sealed class IslandPushInput
{
    /// <summary>输入框占位提示（可选）。</summary>
    [JsonPropertyName("placeholder")]
    public string Placeholder { get; set; } = "";

    /// <summary>输入框初始值（可选，预填文字）。</summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = "";

    /// <summary>提交按钮文字（可选，默认「提交」）。</summary>
    [JsonPropertyName("submit_label")]
    public string SubmitLabel { get; set; } = "";

    /// <summary>提交后执行的动作；默认 notify（把用户输入作为 value 回传推送方）。</summary>
    [JsonPropertyName("action")]
    public string Action { get; set; } = "notify"; // url | launch | notify
}

/// <summary>
/// 第三方软件通过本地上岛 API 推送的“灵动岛卡片”。
/// 字段均为可选：WinIslands 会用设置里的全局默认补齐（显示时长、点击行为）。
/// </summary>
public sealed class IslandPush
{
    /// <summary>推送方自定义 ID，用于更新或移除同一条推送。</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    /// <summary>标题（必填，显示在紧凑态与展开态）。</summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    /// <summary>正文详情（可选，展开态显示）。</summary>
    [JsonPropertyName("body")]
    public string Body { get; set; } = "";

    /// <summary>图标：Segoe MDL2 Assets 字形代码（如 "\uE8D6"）或 emoji/文本（可选）。</summary>
    [JsonPropertyName("icon")]
    public string Icon { get; set; } = "";

    /// <summary>进度 0..1（可选，展开态显示进度条）。</summary>
    [JsonPropertyName("progress")]
    public double? Progress { get; set; }

    /// <summary>显示时长（秒）。留空则用 WinIslands 设置的全局默认时长。</summary>
    [JsonPropertyName("duration_seconds")]
    public int? DurationSeconds { get; set; }

    /// <summary>操作按钮（可选）。</summary>
    [JsonPropertyName("buttons")]
    public List<IslandPushButton>? Buttons { get; set; }
    /// <summary>内容类型：info（默认）/ success / warning / error，用于提示色。</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    /// <summary>优先级：high / normal / low（默认 normal）。多条推送并存时，高优先级显示在前。</summary>
    [JsonPropertyName("priority")]
    public string Priority { get; set; } = "";

    /// <summary>副标题（可选，标题下方小字）。</summary>
    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; } = "";

    /// <summary>自定义强调色（#RRGGBB / #AARRGGBB，可选），覆盖类型默认色。</summary>
    [JsonPropertyName("accent")]
    public string Accent { get; set; } = "";

    /// <summary>卡片主题：dark / light / auto（默认 auto 跟随应用明暗主题）。</summary>
    [JsonPropertyName("theme")]
    public string Theme { get; set; } = "";

    /// <summary>整卡点击回跳（可选）：url 打开链接/文件，launch 启动程序。</summary>
    [JsonPropertyName("click")]
    public IslandPushButton? Click { get; set; }

    /// <summary>输入框（可选）：用户输入文字提交后按 Input.Action 执行（默认 notify 回传推送方）。</summary>
    [JsonPropertyName("input")]
    public IslandPushInput? Input { get; set; }

    /// <summary>排序用优先级数值（不入 JSON）：high=2 / normal=1 / low=0。</summary>
    [JsonIgnore]
    public int PriorityRank => (Priority ?? "").Trim().ToLowerInvariant() switch
    {
        "high" => 2,
        "low" => 0,
        _ => 1,
    };

    /// <summary>服务端计算的过期时间（UTC），客户端推送时忽略。</summary>
    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    // ── v3 扩展：图片 / 动态进度 / 心跳 ─────────────────────────

    /// <summary>图片：data URI（data:image/png;base64,...）或 http(s) 链接（可选，展开态右侧显示）。</summary>
    [JsonPropertyName("image")]
    public string Image { get; set; } = "";

    /// <summary>动态进度起始值 0..1（配合 progress_duration_seconds 自动推进；默认 0）。</summary>
    [JsonPropertyName("progress_from")]
    public double? ProgressFrom { get; set; }

    /// <summary>动态进度结束值 0..1（默认 1）。</summary>
    [JsonPropertyName("progress_to")]
    public double? ProgressTo { get; set; }

    /// <summary>动态进度持续时间（秒）：设置后进度条从 progress_from 自动推进到 progress_to，推送方无需反复更新。</summary>
    [JsonPropertyName("progress_duration_seconds")]
    public int? ProgressDurationSeconds { get; set; }

    /// <summary>心跳间隔（秒）：推送方需周期性以同 id 更新续期；超过 2 倍间隔未续期自动移除。</summary>
    [JsonPropertyName("heartbeat_seconds")]
    public int? HeartbeatSeconds { get; set; }

    /// <summary>动态进度锚点（服务端在每次完整更新时重置，不入 JSON）。</summary>
    [JsonIgnore]
    internal DateTime? ProgressAnchorUtc { get; set; }

    /// <summary>最近一次收到推送/心跳的时间（UTC，服务端维护，不入 JSON）。</summary>
    [JsonIgnore]
    internal DateTime LastSeenUtc { get; set; } = DateTime.UtcNow;

    /// <summary>有效进度：普通 progress 原样返回；配置了动态进度段时按经过时间线性插值（0..1）。</summary>
    [JsonIgnore]
    public double? EffectiveProgress
    {
        get
        {
            if (ProgressDurationSeconds is int dur && dur > 0 && ProgressAnchorUtc is DateTime anchor)
            {
                var t = (DateTime.UtcNow - anchor).TotalSeconds / dur;
                var from = Math.Clamp(ProgressFrom ?? 0, 0, 1);
                var to = Math.Clamp(ProgressTo ?? 1, 0, 1);
                var v = t <= 0 ? from : (t >= 1 ? to : from + (to - from) * t);
                return Math.Clamp(v, 0, 1);
            }
            return Progress;
        }
    }
}
