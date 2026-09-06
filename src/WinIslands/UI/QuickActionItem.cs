namespace WinIslands.UI;

/// <summary>一条快捷操作（展开卡片底部按钮）：键 + 图标字形 + 提示文本。</summary>
public sealed record QuickActionItem(string Key, string Glyph, string ToolTip);
