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
        var migrateMessage = message ?? $"create{Utils.PluralizeWords(_model)}Table"; 
        var command = $@"dotnet ef migrations add {migrateMessage} "+
                      $" -s {AppSettings.WebAppProjectRelativePath} -p {AppSettings.PersistenceProjectRelativePath}";
       await ProcessAsyncHelper.ExecuteShellCommand(AppSettings.WorkingDirectory, "cmd", $"/k {command}", 20000);
    }
    public async Task DbUpdateAsync()
    {
        var command = $@"dotnet ef database update"+
                      $" -s {AppSettings.WebAppProjectRelativePath} -p {AppSettings.PersistenceProjectRelativePath}";
        await ProcessAsyncHelper.ExecuteShellCommand(AppSettings.WorkingDirectory, "cmd", $"/k {command}", 20000);
    }
    
}