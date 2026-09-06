using WinIslands.Services;

namespace WinIslands.Tests;

/// <summary>验证码识别测试（15 验证码提示）。</summary>
public class VerificationCodeDetectorTests
{
    [Fact]
    public void Extracts_Chinese_Code_After_Keyword()
    {
        Assert.True(VerificationCodeDetector.TryExtract("【某某银行】验证码为 123456，请勿泄露。", out var code));
        Assert.Equal("123456", code);
    }

    [Fact]
    public void Extracts_English_Code_After_Keyword()
    {
        Assert.True(VerificationCodeDetector.TryExtract("Your verification code is 882761", out var code));
        Assert.Equal("882761", code);
    }

    [Fact]
    public void Extracts_Code_Before_Chinese_Keyword()
    {
        Assert.True(VerificationCodeDetector.TryExtract("886677 为您的登录验证码，5 分钟内有效", out var code));
        Assert.Equal("886677", code);
    }

    [Fact]
    public void Does_Not_Flag_Ordinary_Numbers()
    {
        Assert.False(VerificationCodeDetector.TryExtract("今天买了 3 个苹果，共 45 元", out _));
    }
}
