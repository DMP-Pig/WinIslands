using WinIslands.UI;

namespace WinIslands.Tests;

/// <summary>
/// 位置守卫：播放器偶发上报 0 / 过期位置时，不得把进度和歌词打回开头。
/// </summary>
public class PositionGuardTests
{
    [Theory]
    // 正常前进：立即采用
    [InlineData(120.0, 118.0, false, true)]
    [InlineData(121.5, 118.0, false, true)]
    // 轻微回退（≤2s，例如时钟校正）：立即采用
    [InlineData(30.0, 32.0, false, true)]
    [InlineData(30.0, 30.0, false, true)]
    // 播放到中途时瞬间上报 0：判定为过期读数，忽略（不跳回开头）
    [InlineData(0.0, 120.0, false, false)]
    [InlineData(0.5, 200.0, false, false)]
    // 明显回退（非 0）：不立即采用（交给调用方超时后判定为真正 seek/重播）
    [InlineData(5.0, 60.0, false, false)]
    [InlineData(30.0, 120.0, false, false)]
    // 用户拖拽进度条期间：一律采用
    [InlineData(0.0, 120.0, true, true)]
    [InlineData(5.0, 60.0, true, true)]
    // 曲目最开头上报 0：正常采用（current=0）
    [InlineData(0.0, 0.0, false, true)]
    // 已播 8s 时上报 0：明显回退 → 不立即采用，交给超时判定
    [InlineData(0.0, 8.0, false, false)]
    public void Adopts_Or_Ignores_Reported_Position(double reported, double current, bool seeking, bool expectedAdopt)
    {
        Assert.Equal(expectedAdopt, IslandViewModel.ShouldAdoptReportedPosition(reported, current, seeking));
    }
}
