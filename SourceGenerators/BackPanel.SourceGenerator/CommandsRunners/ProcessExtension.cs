using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BackPanel.SourceGenerator.CommandsRunners;
public static class ProcessExtensions
{
    public static Task<bool> WaitForEndAsync(this Process process)
    {
        TaskCompletionSource<bool> tcs = new();

        process.EnableRaisingEvents = true;
        process.Exited += (_, __) => tcs.TrySetResult(true);

        if (process.HasExited)
        {
            tcs.TrySetResult(true);
        }

        return tcs.Task;
    }
}