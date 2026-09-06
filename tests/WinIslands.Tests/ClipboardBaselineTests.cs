using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>
/// 剪贴板启动基线：应用启动时不得把「启动前就已存在的剪贴板内容」误判为新复制
/// 而弹出多余的「已复制」提示；建立基线后首次真实复制仍必须正常触发。
/// </summary>
public class ClipboardBaselineTests
{
    [Fact]
    public void Empty_Clipboard_Keeps_Empty_Baseline()
    {
        Assert.Null(ClipboardHistoryService.ComputeBaseline("", ""));
        Assert.Null(ClipboardHistoryService.ComputeBaseline("", "   "));
    }

    [Fact]
    public void PreExisting_Clipboard_Becomes_Baseline_Only_Once()
    {
        // 启动时剪贴板已有内容 → 作为基线，避免首轮轮询误报
        var b1 = ClipboardHistoryService.ComputeBaseline("", @"E:\some\path");
        Assert.Equal(@"E:\some\path", b1);

        // 基线已建立（_last 非空）→ 不再改变
        Assert.Null(ClipboardHistoryService.ComputeBaseline(@"E:\some\path", "new text"));
    }

    [Fact]
    public void Oversized_Clipboard_Ignored_For_Baseline()
    {
        Assert.Null(ClipboardHistoryService.ComputeBaseline("", new string('x', 20001)));
    }

    [Fact]
    public void Real_New_Copy_After_Baseline_Is_Not_Swallowed()
    {
        // 基线是旧内容；之后用户复制了新文本 → 轮询应能识别（这里验证基线判定不会吞掉它：
        // ComputeBaseline 返回 null 表示保持旧基线，轮询随后用新文本与旧基线比对并触发）
        Assert.Null(ClipboardHistoryService.ComputeBaseline("old", "brand new"));
    }
}
