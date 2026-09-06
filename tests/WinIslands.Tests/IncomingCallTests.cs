using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>来电提醒：通话窗口标题识别与应用名规范化。</summary>
public class IncomingCallTests
{
    [Theory]
    // 来电语义
    [InlineData("邀请你进行语音通话", CallKind.Incoming)]
    [InlineData("正在邀请你视频通话", CallKind.Incoming)]
    [InlineData("inviting you to a voice call", CallKind.Incoming)]
    [InlineData("Incoming call from Alice", CallKind.Incoming)]
    // 通话中语义
    [InlineData("语音通话", CallKind.Active)]
    [InlineData("视频通话中", CallKind.Active)]
    [InlineData("正在通话", CallKind.Active)]
    [InlineData("voice call with Bob", CallKind.Active)]
    [InlineData("Video Call", CallKind.Active)]
    // 非通话窗口
    [InlineData("微信", CallKind.None)]
    [InlineData("QQ", CallKind.None)]
    [InlineData("文件传输助手", CallKind.None)]
    [InlineData("", CallKind.None)]
    [InlineData(null, CallKind.None)]
    public void Classifies_Call_Titles(string? title, CallKind expected)
    {
        Assert.Equal(expected, IncomingCallMonitor.ClassifyTitle(title));
    }

    [Fact]
    public void Normalizes_App_Names()
    {
        var apps = IncomingCallMonitor.NormalizeApps(new[] { "Weixin.exe", " WeChat ", "qq.exe", "  " });
        Assert.Equal(new[] { "qq", "wechat", "weixin" }, apps.OrderBy(x => x).ToArray());
    }

    [Fact]
    public void Empty_Apps_Fall_Back_To_Defaults()
    {
        var apps = IncomingCallMonitor.NormalizeApps(new List<string>());
        Assert.Contains("weixin", apps);
        Assert.Contains("qq", apps);
    }
}
