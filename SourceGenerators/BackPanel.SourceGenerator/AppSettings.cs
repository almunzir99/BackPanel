namespace BackPanel.SourceGenerator;

public static class AppSettings
{
    public static string WorkingDirectory = @"C:\Users\hp\Desktop\Project\BackPanel";
    public static string TemplatesRelativePath = @"SourceGenerators\BackPanel.SourceGenerator\Templates";
    public static string PersistenceProjectRelativePath = @"Infrastructure\BackPanel.Persistence";
    public static string WebAppProjectRelativePath = @"Presentation\BackPanel.WebApplication";
    public static string ApplicationProjectRelativePath = @"Core\BackPanel.Application";
    public static string EntitiesRelativePath = @"Core\BackPanel.Domain\Entities";
    public static string DtosRelativePath = @"Core\BackPanel.Application\DTOs";
    public static string DtosRequestsRelativePath = @"Core\BackPanel.Application\DTOsRequests";
    public static string InterfacesRelativePath = @"Core\BackPanel.Application\Interfaces";
    public static string ServicesRelativePath = @"Core\BackPanel.Application\Services";
    public static string ControllersRelativePath = @"Presentation\BackPanel.WebApplication\Controllers";
    public static string MappingProfilePath = @$"{ApplicationProjectRelativePath}\Mapping\MappingProfile.cs";
    public static string DbContextPath = @$"{PersistenceProjectRelativePath}\Database\AppDbContext.cs";

}