// See https://aka.ms/new-console-template for more information
using BackPanel.SourceGenerator;
using CommandLine;
using Microsoft.Extensions.Configuration;

await Parser.Default.ParseArguments<CommandOptions>(args).WithParsedAsync(async o =>
{

    IConfiguration config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
    if (o.Model == null)
    {
        o = new CommandOptions
        {
            Model = PromptForInput("Enter model (Required): "),
            Dto = PromptForBool("Include DTO? (yes/no): "),
            DtoRequest = PromptForBool("Include DTO Request? (yes/no): "),
            DbContext = PromptForBool("Include DbContext? (yes/no): "),
            Permission = PromptForBool("Include Permission? (yes/no): "),
            CQRS = PromptForBool("Include CQRS? (yes/no): "),
            DatabaseUpdate = PromptForBool("Include Database Update? (yes/no): "),
            Controller = PromptForBool("Include Controller? (yes/no): "),
        };

        Console.WriteLine();
    }

    var workingDirectory = config["WorkingDirectory"];
    var projectName = config["ProjectName"];
    var generator = new Generator(o, workingDirectory!, projectName!);
    await generator.GenerateAsync();
    Console.WriteLine("Press any key To Exist ...");
    Console.ReadLine();
});


static string PromptForInput(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine();
}

static bool PromptForBool(string prompt)
{
    Console.Write(prompt);
    string input = Console.ReadLine()?.Trim().ToLower();
    return input == "yes" || input == "y";
}