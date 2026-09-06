using System.IO.Pipes;
using System.Text;

namespace WinIslands.Services;

/// <summary>
/// Enforces a single running instance. The first instance owns a named mutex and serves
/// a small named pipe; later instances send "show" through the pipe (so clicking the exe
/// again reveals the island) and then exit.
/// </summary>
public sealed class SingleInstance : IDisposable
{
    private const string MutexName = @"Local\WinIslands_SingleInstance";
    private const string PipeName = "WinIslands";

    private Mutex? _mutex;
    private CancellationTokenSource _cts = new();

    /// <summary>True when this process is the first (owner) instance.</summary>
    public bool IsFirstInstance { get; private set; }

    /// <summary>Raised when another instance asked us to show the island.</summary>
    public event EventHandler? ShowRequested;

    public bool TryAcquire()
    {
        _mutex = new Mutex(initiallyOwned: true, MutexName, out var createdNew);
        IsFirstInstance = createdNew;
        if (!createdNew)
        {
            NotifyFirstInstance();
            _mutex.Dispose();
            _mutex = null;
            return false;
        }

        _ = ServePipeAsync();
        return true;
    }

    private static void NotifyFirstInstance()
    {
        try
        {
            using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out, PipeOptions.Asynchronous);
            client.Connect(1000);
            var bytes = Encoding.UTF8.GetBytes("show");
            client.Write(bytes, 0, bytes.Length);
        }
        catch
        {
            // First instance may not be listening yet; nothing else to do.
        }
    }

    private async Task ServePipeAsync()
    {
        while (!_cts.IsCancellationRequested)
        {
            try
            {
                await using var server = new NamedPipeServerStream(PipeName, PipeDirection.In, 1,
                    PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                await server.WaitForConnectionAsync(_cts.Token);
                var buffer = new byte[16];
                _ = await server.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
                ShowRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch
            {
                await Task.Delay(200, CancellationToken.None);
            }
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        _mutex?.Dispose();
        _mutex = null;
    }
}
