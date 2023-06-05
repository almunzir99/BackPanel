using System.Diagnostics;

namespace BackPanel.SourceGenerator.CommandsRunners;

public class DbCommandRunner
{
    private readonly string _model;
    private readonly string workingDirectory;
    private readonly string projectName;

    public DbCommandRunner(string model, string workingDirectory,string projectName)
    {
        _model = model;
        this.workingDirectory = workingDirectory;
        this.projectName = projectName;
    }

    public async Task MigrateAsync(string? message = null)
    {
        _ = message ?? $"Create{Utils.PluralizeWords(_model)}Table";
         string command = "dotnet ef migrations add {migrateMessage} " +
                      $" -s {AppSettings.WebAppProjectRelativePath.Replace("ProjectName",projectName)} -p {AppSettings.PersistenceProjectRelativePath.Replace("ProjectName",projectName)}";
        await ProcessAsyncHelper.ExecuteShellCommand(workingDirectory, OperatingSystem.IsWindows() ? $"/k {command}" : command);
    }
    public async Task DbUpdateAsync()
    {
         string command = "dotnet ef database update" +
                      $" -s {AppSettings.WebAppProjectRelativePath.Replace("ProjectName",projectName)} -p {AppSettings.PersistenceProjectRelativePath.Replace("ProjectName",projectName)}";
        await ProcessAsyncHelper.ExecuteShellCommand(workingDirectory, OperatingSystem.IsWindows() ? $"/k {command}" : command);
    }
}