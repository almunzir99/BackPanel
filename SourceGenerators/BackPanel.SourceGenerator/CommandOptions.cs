using CommandLine;

namespace BackPanel.SourceGenerator;

public class CommandOptions
{
    [Option('m', "model", Required = false)]
    public string? Model { get; set; } = "Test";

    [Option('d', "dto", Required = false)]
    public bool? Dto { get; set; } = true;

    [Option('q', "dtoRequest", Required = false)]
    public bool? DtoRequest { get; set; } = true;

    [Option('b', "dbcontext", Required = false)]
    public bool? DbContext { get; set; } = true;

    [Option('e', "Permission", Required = false)]
    public bool? Permission { get; set; } = true;

    [Option('s', "service", Required = false)]
    public bool? Service { get; set; } = false;

    [Option('u', "dbUpdate", Required = false)]
    public bool? DatabaseUpdate { get; set; } = false;

    [Option('c', "controller", Required = false)]
    public bool? Controller { get; set; } = true;
    

}