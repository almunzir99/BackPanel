using System.Diagnostics;
namespace BackPanel.SourceGenerator.CommandsRunners;

public static class ProcessAsyncHelper
{
    public static async Task ExecuteShellCommand(string workingDirectory, string command)
    {
        string shell = await GetShellAsync();
        ProcessStartInfo processInfo = new()
        {
            FileName = shell,
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory
        };

        using Process process = new();
        process.StartInfo = processInfo;
        process.Start();

        // Asynchronously wait for the process to exit
        await process.WaitForExitAsync();

        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
    }
    static async Task<string> GetShellAsync()
    {
        if (OperatingSystem.IsWindows())
        {
            return "cmd.exe";
        }
        else if (OperatingSystem.IsMacOS())
        {
            const string whichCommand = "which bash";
            string bashPath = await ExecuteShellCommandAsync(whichCommand);
            return bashPath.Trim();
        }
        else
        {
            return "/bin/sh";
        }
    }
    static async Task<string> ExecuteShellCommandAsync(string command)
    {
        ProcessStartInfo processInfo = new()
        {
            FileName = "/bin/bash", // Use the Bash executable
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = new();
        process.StartInfo = processInfo;
        process.Start();

        return await process.StandardOutput.ReadToEndAsync();
    }
}