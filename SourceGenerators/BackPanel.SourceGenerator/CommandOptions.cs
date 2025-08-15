using CommandLine;

namespace BackPanel.SourceGenerator;

public class CommandOptions
{
    [Option('m', "model", Required = false)]
    public string? Model { get; set; }

    [Option('d', "dto", Required = false)]
    public bool? Dto { get; set; } = false;

    [Option('q', "dtoRequest", Required = false)]
    public bool? DtoRequest { get; set; } = false;

    [Option('b', "dbcontext", Required = false)]
    public bool? DbContext { get; set; } = false;

    [Option('e', "Permission", Required = false)]
    public bool? Permission { get; set; } = false;

    [Option('s', "cqrs", Required = false)]
    public bool? CQRS { get; set; } = false;

    [Option('u', "dbUpdate", Required = false)]
    public bool? DatabaseUpdate { get; set; } = false;

    [Option('c', "controller", Required = false)]
    public bool? Controller { get; set; } = false;
}