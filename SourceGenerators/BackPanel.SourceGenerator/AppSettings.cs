namespace BackPanel.SourceGenerator;

public static class AppSettings
{
    public const string WorkingDirectory = @"C:\Users\hp\Desktop\Project\BackPanel";
    public const string TemplatesRelativePath = @"SourceGenerators\BackPanel.SourceGenerator\Templates";
    public const string PersistenceProjectRelativePath = @"Infrastructure\BackPanel.Persistence";
    public const string WebAppProjectRelativePath = @"Presentation\BackPanel.WebApplication";
    public const string ApplicationProjectRelativePath = @"Core\BackPanel.Application";
    public const string EntitiesRelativePath = @"Core\BackPanel.Domain\Entities";
    public const string DtosRelativePath = @"Core\BackPanel.Application\DTOs";
    public const string DtosRequestsRelativePath = @"Core\BackPanel.Application\DTOsRequests";
    public const string InterfacesRelativePath = @"Core\BackPanel.Application\Interfaces";
    public const string ServicesRelativePath = @"Core\BackPanel.Application\Services";
    public const string ControllersRelativePath = @"Presentation\BackPanel.WebApplication\Controllers";
    public const string MappingProfilePath = @$"{ApplicationProjectRelativePath}\Mapping\MappingProfile.cs";
    public const string DbContextPath = @$"{PersistenceProjectRelativePath}\Database\AppDbContext.cs";
}