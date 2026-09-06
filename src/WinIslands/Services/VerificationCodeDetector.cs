using System.Text.RegularExpressions;

namespace WinIslands.Services;

/// <summary>
/// 验证码识别（15 验证码提示）：从剪贴板文本（如短信）中提取 4-8 位数字验证码。
/// 只在与验证码相关的关键词附近查找，避免把普通数字误判为验证码。
/// </summary>
public static class VerificationCodeDetector
{
    private static readonly Regex[] Patterns =
    {
        // 中文：验证码/校验码/动态码/安全码 之后跟数字（如「验证码为 123456」）
        new(@"(?:验证码|校验码|动态码|安全码)[^0-9]{0,6}(?<code>\d{4,8})", RegexOptions.Compiled),
        // 数字在关键词之前（如「123456 为您的登录验证码」）
        new(@"(?<code>\d{4,8})[^0-9]{0,8}(?:验证码|校验码|动态码|安全码)", RegexOptions.Compiled),
        // 英文：verification code / security code / sms code 之后跟数字
        new(@"(?:verification|security|captcha|sms)\s+code[^0-9]{0,8}(?<code>\d{4,8})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase),
        // 短信兜底：行内含「短信/sms」且附近有独立数字
        new(@"(?:短信|sms)[^0-9]{0,20}(?<code>\d{4,8})", RegexOptions.Compiled | RegexOptions.IgnoreCase),
    };

    /// <summary>尝试从文本中提取验证码；找不到则返回 false（code 置空）。</summary>
    public static bool TryExtract(string? text, out string code)
    {
        code = string.Empty;
        if (string.IsNullOrWhiteSpace(text)) return false;
        foreach (var re in Patterns)
        {
            var m = re.Match(text);
            if (m.Success)
            {
                code = m.Groups["code"].Value;
                return true;
            }
        }
        return false;
    }
}
