namespace BackPanel.SourceGenerator;

public static class AppSettings
{
    // Project name should be replace with project name from appsettings
    public const string TemplatesRelativePath ="SourceGenerators/ProjectName.SourceGenerator/Templates";
    public const string PersistenceProjectRelativePath ="Infrastructure/ProjectName.Persistence";
    public const string WebAppProjectRelativePath ="Presentation/ProjectName.WebApplication";
    public const string ApplicationProjectRelativePath ="Core/ProjectName.Application";
    public const string EntitiesRelativePath ="Core/ProjectName.Domain/Entities";
    public const string DtosRelativePath ="Core/ProjectName.Application/DTOs";
    public const string DtosRequestsRelativePath ="Core/ProjectName.Application/DTOsRequests";
    public const string InterfacesRelativePath ="Core/ProjectName.Application/Interfaces";
    public const string ServicesRelativePath ="Core/ProjectName.Application/Services";
    public const string ControllersRelativePath ="Presentation/ProjectName.WebApplication/Controllers";
    public const string MappingProfilePath =$"{ApplicationProjectRelativePath}/Mapping/MappingProfile.cs";
    public const string DbContextPath =$"{PersistenceProjectRelativePath}/Database/AppDbContext.cs";
}