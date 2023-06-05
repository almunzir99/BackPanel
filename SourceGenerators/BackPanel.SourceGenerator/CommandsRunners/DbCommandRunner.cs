using System.Diagnostics;

namespace BackPanel.SourceGenerator.CommandsRunners;

public class DbCommandRunner
{
    private readonly string _model;

    public DbCommandRunner(string model)
    {
        _model = model;
    }

    public async Task MigrateAsync(string? message = null)
    {
        _ = message ?? $"create{Utils.PluralizeWords(_model)}Table";
        const string command = "dotnet ef migrations add {migrateMessage} "+
                      $" -s {AppSettings.WebAppProjectRelativePath} -p {AppSettings.PersistenceProjectRelativePath}";
       await ProcessAsyncHelper.ExecuteShellCommand(AppSettings.WorkingDirectory, "cmd", $"/k {command}", 20000);
    }
    public static async Task DbUpdateAsync()
    {
        const string command = "dotnet ef database update"+
                      $" -s {AppSettings.WebAppProjectRelativePath} -p {AppSettings.PersistenceProjectRelativePath}";
        await ProcessAsyncHelper.ExecuteShellCommand(AppSettings.WorkingDirectory, "cmd", $"/k {command}", 20000);
    }
}