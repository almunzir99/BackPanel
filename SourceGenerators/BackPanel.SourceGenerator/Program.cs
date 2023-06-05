// See https://aka.ms/new-console-template for more information
using BackPanel.SourceGenerator;
using CommandLine;
using Microsoft.Extensions.Configuration;

await Parser.Default.ParseArguments<CommandOptions>(args).WithParsedAsync(async o =>
{
     IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    var workingDirectory = config["WorkingDirectory"];
    var projectName = config["ProjectName"];
    var generator = new Generator(o,workingDirectory!,projectName!);
    await generator.GenerateAsync();
    Console.WriteLine("Press any key To Exist ...");
    Console.ReadLine();
});
