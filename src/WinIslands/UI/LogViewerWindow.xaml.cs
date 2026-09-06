using System;
using System.IO;
using System.Linq;
using System.Windows;
using WinIslands.Services;

namespace WinIslands.UI;

/// <summary>日志查看窗口：读取今天的日志文件，显示最近若干行。</summary>
public partial class LogViewerWindow : Window
{
    public LogViewerWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => Refresh();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e) => Refresh();

    private void Refresh()
    {
        try
        {
            var day = DateTime.Now.ToString("yyyyMMdd");
            var path = Path.Combine(AppPaths.LogsDir, $"app-{day}.log");
            if (!File.Exists(path))
            {
                LogText.Text = "（暂无日志）";
                FileLabel.Text = path;
                return;
            }
            var lines = File.ReadLines(path).TakeLast(600).ToList();
            LogText.Text = string.Join(Environment.NewLine, lines);
            FileLabel.Text = path;
            Scroller.ScrollToEnd();
        }
        catch (Exception ex)
        {
            LogText.Text = "读取日志失败：" + ex.Message;
        }
    }
}
