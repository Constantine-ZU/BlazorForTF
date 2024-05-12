using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class SarService
{
    public async Task RunSar(string command, Action<string> onOutputReceived, Action<string> onErrorReceived, CancellationToken cancellationToken)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "/usr/bin/sudo",
            Arguments = command,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = null;

        try
        {
            process = new Process { StartInfo = psi };

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    onOutputReceived?.Invoke(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    onErrorReceived?.Invoke(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Here we wait for the process to exit, while observing the cancellation token
            await Task.Run(() => process.WaitForExit(), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();  // Ensure the process is killed if the operation is cancelled
            }
            throw;  // Re-throw the exception to handle it in the calling method
        }
        finally
        {
            process?.Dispose(); // Ensure process resources are cleaned up
        }
    }

}
