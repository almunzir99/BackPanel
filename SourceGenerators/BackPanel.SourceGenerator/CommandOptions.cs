using CommandLine;

namespace BackPanel.SourceGenerator;

public class CommandOptions
{
    [Option('m', "model", Required = false)]
    public string? Model { get; set; } = "Test";
}