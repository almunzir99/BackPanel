// See https://aka.ms/new-console-template for more information
using BackPanel.SourceGenerator;
using CommandLine;

await Parser.Default.ParseArguments<CommandOptions>(args).WithParsedAsync(async o =>
{
    var generator = new Generator(o);
    await generator.GenerateAsync();
    Console.WriteLine("Press any key To Exist ...");
    Console.ReadLine();
});
