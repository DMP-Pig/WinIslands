using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinIslands.Services;

/// <summary>一条 RSS 订阅条目。</summary>
public sealed record RssItem(string Title, string Link, string Summary, DateTimeOffset Published);

/// <summary>一封新邮件摘要（仅邮件头，不下载正文）。</summary>
public sealed record MailHeader(string Key, string Subject, string From, string DateText);

/// <summary>
/// RSS 订阅 + 电子邮件（POP3）提醒服务，零第三方依赖：
///   - RSS：HttpClient + XDocument 解析（支持 RSS 2.0 / Atom）
///   - 邮件：TcpClient + SslStream 实现 POP3（TLS 或明文；TOP 只取邮件头，不下载正文）
/// 后台 Timer 轮询，不阻塞 UI；发现新条目通过事件抛给上层（弹通知）。
/// 首次启用只「标记已读」不通知，避免历史刷屏；去重键持久化到本地 JSON，重启不重复提醒。
/// 联网仅发生在用户开启对应开关后；不上报任何数据。
/// </summary>
public sealed class RssMailService : IDisposable
{
    private const int MaxSeen = 500;                  // 去重历史上限，防止文件无限膨胀
    private readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(12) };
    private readonly object _gate = new();
    private readonly HashSet<string> _seenRss = new(StringComparer.Ordinal);
    private readonly HashSet<string> _seenMail = new(StringComparer.Ordinal);
    private readonly string _seenFile = Path.Combine(AppPaths.AppDataDir, "rss_mail_seen.json");

    private System.Threading.Timer? _rssTimer;
    private System.Threading.Timer? _mailTimer;
    private int _rssPolling;
    private int _mailPolling;
    private bool _primedRss;    // 首次 RSS 轮询成功后置位：此前已存在的条目只标记不通知
    private bool _primedMail;   // 同上（邮件）

    // 配置快照（Configure 更新，Timer 回调读取）
    private string[] _urls = Array.Empty<string>();
    private int _rssIntervalMin = 15;
    private string _server = "";
    private int _port = 995;
    private bool _useSsl = true;
    private string _user = "";
    private string _pass = "";
    private int _mailIntervalMin = 5;

    public RssMailService() => LoadSeen();

    /// <summary>RSS 新条目（标题, 摘要, 链接）。</summary>
    public event Action<string, string, string>? RssItemReceived;

    /// <summary>新邮件（主题, 发件人, 日期/时间）。</summary>
    public event Action<string, string, string>? MailReceived;

    /// <summary>按设置启停轮询；设置变化时上层调用，即时生效。</summary>
    public void Configure(
        bool rssEnabled, string rssUrls, int rssIntervalMinutes,
        bool mailEnabled, string mailServer, int mailPort, bool mailSsl,
        string mailUser, string mailPassword, int mailCheckMinutes)
    {
        lock (_gate)
        {
            _urls = (rssUrls ?? string.Empty)
                .Split(new[] { ',', ';', '\n', '\r', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(u => u.Trim())
                .Where(u => u.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                         || u.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            _rssIntervalMin = Math.Clamp(rssIntervalMinutes, 1, 1440);
            _server = mailServer?.Trim() ?? string.Empty;
            _port = Math.Clamp(mailPort, 1, 65535);
            _useSsl = mailSsl;
            _user = mailUser ?? string.Empty;
            _pass = mailPassword ?? string.Empty;
            _mailIntervalMin = Math.Clamp(mailCheckMinutes, 1, 1440);
        }
        RestartTimers();
    }

    private void RestartTimers()
    {
        lock (_gate)
        {
            _rssTimer?.Dispose();
            _mailTimer?.Dispose();
            _rssTimer = null;
            _mailTimer = null;
            if (_urls.Length > 0)
            {
                // 稍作延迟启动，避免开机瞬间并发请求；间隔按分钟
                _rssTimer = new System.Threading.Timer(_ => _ = PollRssAsync(), null,
                    TimeSpan.FromSeconds(3), TimeSpan.FromMinutes(_rssIntervalMin));
            }
            if (!string.IsNullOrWhiteSpace(_server))
            {
                _mailTimer = new System.Threading.Timer(_ => _ = PollMailAsync(), null,
                    TimeSpan.FromSeconds(6), TimeSpan.FromMinutes(_mailIntervalMin));
            }
        }
    }

    // ── RSS ─────────────────────────────────────────────────────
    private async Task PollRssAsync()
    {
        if (Interlocked.Exchange(ref _rssPolling, 1) == 1) return; // 防重入
        try
        {
            string[] urls;
            lock (_gate) urls = _urls;
            foreach (var url in urls)
            {
                try
                {
                    using var resp = await _http.GetAsync(url).ConfigureAwait(false);
                    resp.EnsureSuccessStatusCode();
                    var xml = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var items = RssParser.Parse(xml);
                    foreach (var item in items)
                    {
                        var key = Hash($"{item.Link}\n{item.Title}\n{item.Published:O}");
                        bool isNew;
                        lock (_gate) isNew = _seenRss.Add(key);
                        if (!isNew) continue;
                        if (!_primedRss) continue; // 首次只标记，不通知历史条目
                        RssItemReceived?.Invoke(item.Title, item.Summary, item.Link);
                    }
                    _primedRss = true;
                    SaveSeen();
                }
                catch (Exception ex)
                {
                    AppLogger.Warn($"RSS 轮询失败（{url}）：{ex.Message}");
                }
            }
        }
        finally
        {
            Interlocked.Exchange(ref _rssPolling, 0);
        }
    }

    // ── 邮件（POP3）────────────────────────────────────────────
    private async Task PollMailAsync()
    {
        if (Interlocked.Exchange(ref _mailPolling, 1) == 1) return;
        try
        {
            string server, user, pass;
            int port;
            bool useSsl;
            lock (_gate) { server = _server; user = _user; pass = _pass; port = _port; useSsl = _useSsl; }
            if (string.IsNullOrWhiteSpace(server)) return;

            var headers = await Pop3Client.FetchHeadersAsync(server, port, useSsl, user, pass).ConfigureAwait(false);
            foreach (var h in headers)
            {
                bool isNew;
                lock (_gate) isNew = _seenMail.Add(h.Key);
                if (!isNew) continue;
                if (!_primedMail) continue;
                MailReceived?.Invoke(h.Subject, h.From, h.DateText);
            }
            _primedMail = true;
            SaveSeen();
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"邮件检查失败：{ex.Message}");
        }
        finally
        {
            Interlocked.Exchange(ref _mailPolling, 0);
        }
    }

    // ── 去重持久化 ─────────────────────────────────────────────
    private void LoadSeen()
    {
        try
        {
            if (!File.Exists(_seenFile)) return;
            using var doc = JsonDocument.Parse(File.ReadAllText(_seenFile));
            if (doc.RootElement.TryGetProperty("rss", out var r))
                foreach (var e in r.EnumerateArray())
                {
                    var v = e.GetString();
                    if (v is not null) _seenRss.Add(v);
                }
            if (doc.RootElement.TryGetProperty("mail", out var m))
                foreach (var e in m.EnumerateArray())
                {
                    var v = e.GetString();
                    if (v is not null) _seenMail.Add(v);
                }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"加载 RSS/邮件去重记录失败：{ex.Message}");
        }
    }

    private void SaveSeen()
    {
        try
        {
            AppPaths.EnsureDirectories();
            lock (_gate)
            {
                var obj = new
                {
                    rss = _seenRss.TakeLast(MaxSeen).ToArray(),
                    mail = _seenMail.TakeLast(MaxSeen).ToArray(),
                };
                var json = JsonSerializer.Serialize(obj);
                var tmp = _seenFile + ".tmp";
                File.WriteAllText(tmp, json);
                File.Move(tmp, _seenFile, overwrite: true);
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"保存 RSS/邮件去重记录失败：{ex.Message}");
        }
    }

    /// <summary>SHA-256 十六进制摘要，用于去重键。</summary>
    internal static string Hash(string s)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(s)));
    }

    public void Dispose()
    {
        _http.Dispose();
        lock (_gate)
        {
            _rssTimer?.Dispose();
            _mailTimer?.Dispose();
            _rssTimer = null;
            _mailTimer = null;
        }
    }
}

/// <summary>极简 RSS 2.0 / Atom 解析器（System.Xml.Linq，无第三方依赖）。</summary>
internal static class RssParser
{
    public static List<RssItem> Parse(string xml)
    {
        var result = new List<RssItem>();
        try
        {
            var doc = XDocument.Parse(xml);
            var root = doc.Root;
            if (root is null) return result;
            var ns = root.Name.Namespace;

            if (root.Name.LocalName.Equals("feed", StringComparison.OrdinalIgnoreCase))
            {
                // Atom：<feed><entry>…
                foreach (var e in root.Elements(ns + "entry"))
                {
                    var title = (string?)e.Element(ns + "title") ?? string.Empty;
                    var linkEl = e.Element(ns + "link");
                    var link = (string?)linkEl?.Attribute("href") ?? string.Empty;
                    var summary = (string?)e.Element(ns + "summary")
                                ?? (string?)e.Element(ns + "content") ?? string.Empty;
                    var updated = (string?)e.Element(ns + "updated") ?? string.Empty;
                    result.Add(new RssItem(
                        RssText.Clean(title), RssText.Clean(link),
                        RssText.Clean(summary), RssText.ParseDate(updated) ?? DateTimeOffset.Now));
                }
            }
            else
            {
                // RSS 2.0 / 1.0：<rss><channel><item>…
                foreach (var item in root.Descendants()
                             .Where(x => x.Name.LocalName.Equals("item", StringComparison.OrdinalIgnoreCase)))
                {
                    var n = item.Name.Namespace;
                    var title = (string?)item.Element(n + "title") ?? string.Empty;
                    var link = (string?)item.Element(n + "link") ?? string.Empty;
                    var desc = (string?)item.Element(n + "description")
                             ?? (string?)item.Element(n + "encoded") ?? string.Empty;
                    var pub = (string?)item.Element(n + "pubDate")
                            ?? (string?)item.Element(n + "date") ?? string.Empty;
                    result.Add(new RssItem(
                        RssText.Clean(title), RssText.Clean(link),
                        RssText.Clean(desc), RssText.ParseDate(pub) ?? DateTimeOffset.Now));
                }
            }
        }
        catch (Exception ex)
        {
            AppLogger.Warn($"RSS 解析失败：{ex.Message}");
        }
        return result;
    }
}

/// <summary>RSS 文本清洗：去 HTML 标签 / 反转义 / 截断摘要。</summary>
internal static class RssText
{
    public static string Clean(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
        var t = Regex.Replace(raw, "<br\\s*/?>", "\n", RegexOptions.IgnoreCase);
        t = Regex.Replace(t, "<[^>]+>", " ");
        t = System.Net.WebUtility.HtmlDecode(t);
        t = Regex.Replace(t, @"[ \t]+", " ");
        var lines = t.Split('\n').Select(l => l.Trim()).Where(l => l.Length > 0);
        t = string.Join(" ", lines);
        return t.Length <= 160 ? t : t.Substring(0, 157) + "…";
    }

    public static DateTimeOffset? ParseDate(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        if (DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var d))
            return d;
        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dt))
            return new DateTimeOffset(dt);
        return null;
    }
}

/// <summary>
/// 极简 POP3 客户端：TcpClient + SslStream，仅读取邮件头（TOP n 0），不下载正文。
/// 命令：USER / PASS / STAT / TOP；响应以 +OK 开头，多行以「.」结束（dot-stuffing 已处理）。
/// </summary>
internal static class Pop3Client
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(12);

    public static async Task<List<MailHeader>> FetchHeadersAsync(
        string server, int port, bool useSsl, string user, string pass)
    {
        using var cts = new CancellationTokenSource(Timeout);
        using var tcp = new TcpClient { ReceiveTimeout = 10000, SendTimeout = 10000 };
        await tcp.ConnectAsync(server, port, cts.Token).ConfigureAwait(false);

        using var raw = tcp.GetStream();
        Stream s = raw;
        SslStream? ssl = null;
        if (useSsl)
        {
            ssl = new SslStream(raw, false, (_, _, _, _) => true); // 服务器证书由系统根证书校验
            await ssl.AuthenticateAsClientAsync(server).ConfigureAwait(false);
            s = ssl;
        }
        try
        {
            using var reader = new StreamReader(s, Encoding.UTF8, false, 1024, leaveOpen: true);
            var banner = await ReadLineAsync(reader, cts.Token).ConfigureAwait(false);
            if (!banner.StartsWith("+OK", StringComparison.Ordinal))
                throw new InvalidOperationException($"POP3 欢迎信息异常：{banner}");

            RequireOk(await CmdAsync(s, reader, cts.Token, $"USER {user}").ConfigureAwait(false));
            RequireOk(await CmdAsync(s, reader, cts.Token, $"PASS {pass}").ConfigureAwait(false));

            var stat = await CmdAsync(s, reader, cts.Token, "STAT").ConfigureAwait(false);
            var parts = stat.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3 || !int.TryParse(parts[1], out var count) || count <= 0)
            {
                await QuitAsync(s, cts.Token).ConfigureAwait(false);
                return new List<MailHeader>();
            }

            var result = new List<MailHeader>();
            var take = Math.Min(count, 20); // 只看最近 20 封，避免大邮箱卡顿
            for (var i = count; i > count - take; i--)
            {
                var text = await CmdAsync(s, reader, cts.Token, $"TOP {i} 0", multiline: true).ConfigureAwait(false);
                var h = MailHeaderParser.Parse(text);
                if (h is not null) result.Add(h);
            }
            await QuitAsync(s, cts.Token).ConfigureAwait(false);
            return result;
        }
        finally
        {
            ssl?.Dispose();
        }

        static void RequireOk(string line)
        {
            if (!line.StartsWith("+OK", StringComparison.Ordinal))
                throw new InvalidOperationException($"POP3 命令失败：{line}");
        }
    }

    private static async Task<string> CmdAsync(Stream s, TextReader r, CancellationToken ct, string cmd, bool multiline = false)
    {
        var bytes = Encoding.ASCII.GetBytes(cmd + "\r\n");
        await s.WriteAsync(bytes, ct).ConfigureAwait(false);
        await s.FlushAsync(ct).ConfigureAwait(false);
        if (!multiline)
            return await ReadLineAsync(r, ct).ConfigureAwait(false);

        var first = await ReadLineAsync(r, ct).ConfigureAwait(false);
        if (!first.StartsWith("+OK", StringComparison.Ordinal)) return first;
        var lines = new List<string> { first };
        while (true)
        {
            var line = await ReadLineAsync(r, ct).ConfigureAwait(false);
            if (line is null || line == ".") break;
            if (line.StartsWith("..", StringComparison.Ordinal)) line = line.Substring(1); // dot-stuffing
            lines.Add(line);
        }
        return string.Join("\n", lines);
    }

    private static async Task<string> ReadLineAsync(TextReader r, CancellationToken ct)
    {
        var line = await r.ReadLineAsync(ct).ConfigureAwait(false);
        return line ?? string.Empty;
    }

    private static async Task QuitAsync(Stream s, CancellationToken ct)
    {
        try
        {
            var bytes = Encoding.ASCII.GetBytes("QUIT\r\n");
            await s.WriteAsync(bytes, ct).ConfigureAwait(false);
            await s.FlushAsync(ct).ConfigureAwait(false);
        }
        catch { /* 忽略退出错误 */ }
    }
}

/// <summary>解析 POP3 TOP 返回的邮件头：支持字段续行折叠、MIME 编码主题（=?utf-8?B?...?= / =?utf-8?Q?...?=）。</summary>
internal static class MailHeaderParser
{
    public static MailHeader? Parse(string text)
    {
        string? subject = null, from = null, date = null, messageId = null;
        foreach (var rawLine in text.Split('\n'))
        {
            var line = rawLine.TrimEnd('\r');
            if (line.Length == 0) break; // 头部结束（空行）
            if (line[0] == ' ' || line[0] == '\t')
            {
                // 续行：折叠到当前字段
                if (subject is not null) subject += " " + line.Trim();
                else if (from is not null) from += " " + line.Trim();
                else if (date is not null) date += " " + line.Trim();
                else if (messageId is not null) messageId += " " + line.Trim();
                continue;
            }
            var idx = line.IndexOf(':');
            if (idx <= 0) continue;
            var name = line.Substring(0, idx).Trim();
            var val = line.Substring(idx + 1).Trim();
            if (name.Equals("Subject", StringComparison.OrdinalIgnoreCase)) subject = val;
            else if (name.Equals("From", StringComparison.OrdinalIgnoreCase)) from = val;
            else if (name.Equals("Date", StringComparison.OrdinalIgnoreCase)) date = val;
            else if (name.Equals("Message-ID", StringComparison.OrdinalIgnoreCase)) messageId = val;
        }

        subject = MimeDecode.Decode(subject ?? string.Empty);
        from = MimeDecode.Decode(from ?? string.Empty);
        if (subject.Length == 0) subject = "(无主题)";
        var rawKey = !string.IsNullOrWhiteSpace(messageId)
            ? messageId
            : $"{subject}|{date}|{from}";
        var key = RssMailService.Hash(rawKey);
        return key.Length == 0 ? null : new MailHeader(key, subject, from, date ?? string.Empty);
    }
}

/// <summary>解码 MIME 编码字（RFC 2047）：=?charset?B?base64?= 与 =?charset?Q?…?=。</summary>
internal static class MimeDecode
{
    public static string Decode(string value)
    {
        if (string.IsNullOrEmpty(value) || !value.Contains("=?", StringComparison.Ordinal))
            return value;
        var sb = new StringBuilder();
        foreach (var part in value.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (part.Length > 8 && part.StartsWith("=?", StringComparison.Ordinal) && part.EndsWith("?=", StringComparison.Ordinal))
            {
                var inner = part.Substring(2, part.Length - 4);
                var chunks = inner.Split('?');
                if (chunks.Length >= 3)
                {
                    var decoded = TryDecode(chunks[0], chunks[1], chunks[2]);
                    if (decoded is not null)
                    {
                        sb.Append(decoded);
                        continue;
                    }
                }
            }
            sb.Append(part).Append(' ');
        }
        return sb.ToString().Trim();
    }

    private static string? TryDecode(string charset, string encoding, string data)
    {
        try
        {
            var enc = encoding.ToUpperInvariant();
            if (enc == "B")
            {
                var bytes = Convert.FromBase64String(data);
                return DecodeBytes(bytes, charset);
            }
            if (enc == "Q")
            {
                data = data.Replace('_', ' ');
                var bytes = new List<byte>();
                for (var i = 0; i < data.Length; i++)
                {
                    if (data[i] == '=' && i + 2 < data.Length
                        && byte.TryParse(data.Substring(i + 1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
                    {
                        bytes.Add(b);
                        i += 2;
                    }
                    else
                    {
                        bytes.AddRange(Encoding.UTF8.GetBytes(data[i].ToString()));
                    }
                }
                return DecodeBytes(bytes.ToArray(), charset);
            }
        }
        catch { /* 解析失败回落原文 */ }
        return null;
    }

    private static string DecodeBytes(byte[] bytes, string charset)
    {
        try
        {
            if (charset.Equals("utf-8", StringComparison.OrdinalIgnoreCase)
                || charset.Equals("utf8", StringComparison.OrdinalIgnoreCase))
                return Encoding.UTF8.GetString(bytes);
            if (charset.Equals("iso-8859-1", StringComparison.OrdinalIgnoreCase)
                || charset.Equals("latin1", StringComparison.OrdinalIgnoreCase))
                return Encoding.Latin1.GetString(bytes);
            // 其它字符集（GBK 等）无内置支持，回退 UTF-8（失败也不会抛异常）
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
