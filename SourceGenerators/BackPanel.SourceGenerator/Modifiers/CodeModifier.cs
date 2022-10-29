using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BackPanel.SourceGenerator.Modifiers;

public class CodeModifier
{
    private readonly string _mappingProfilePath;
    private readonly string _dbContextPath;
    private readonly string _roleModelPath;
    private readonly string _roleDtoPath;
    private readonly string _roleDtoRequest;
    private readonly string _applicationDiPath;
    private readonly string _model;
    private string _dbContextModifiedCode = "";

    public CodeModifier(string model)
    {
        _model = model;
        _mappingProfilePath = Path.Combine(AppSettings.WorkingDirectory, AppSettings.MappingProfilePath);
        _dbContextPath = Path.Combine(AppSettings.WorkingDirectory, AppSettings.DbContextPath);
        _roleModelPath = Path.Combine(AppSettings.WorkingDirectory, AppSettings.EntitiesRelativePath, "Role.cs");
        _roleDtoPath = Path.Combine(AppSettings.WorkingDirectory, AppSettings.DtosRelativePath, "RoleDto.cs");
        _roleDtoRequest = Path.Combine(AppSettings.WorkingDirectory, AppSettings.DtosRequestsRelativePath,
            "RoleDtoRequest.cs");
        _applicationDiPath = Path.Combine(AppSettings.WorkingDirectory,
            AppSettings.ApplicationProjectRelativePath, "DI", "RegisterWithDependencyInjection.cs");
    }

    public async Task AddServiceToDiFile()
    {
        var models = Utils.PluralizeWords(_model);
        if (!File.Exists(_applicationDiPath))
            throw new Exception("the RegisterWithDependencyInjection.cs file not found");
        var fileContent = await File.ReadAllTextAsync(_applicationDiPath);
        var syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
        var root = syntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var methodSyntax = classSyntax.Members.OfType<MethodDeclarationSyntax>().First();
        var mappingStatement =
            SyntaxFactory.ParseStatement($"services.AddScoped<I{models}Service, {models}Service>();");
        var newMethodSyntax = methodSyntax!.AddBodyStatements(mappingStatement);
        root = root.ReplaceNode(methodSyntax, newMethodSyntax).NormalizeWhitespace();
        await File.WriteAllTextAsync(_applicationDiPath, root.GetText().ToString());
    }

    public async Task AppendToMappingProfile(string suffix)
    {
        if (suffix != "Dto" && suffix != "DtoRequest")
            throw new Exception("suffix should be either Dto or DtoRequest");
        if (!File.Exists(_mappingProfilePath))
            throw new Exception("the MappingProfile.cs file not found");
        var fileContent = await File.ReadAllTextAsync(_mappingProfilePath);
        var syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
        var root = syntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var constructorSyntax = classSyntax.Members.OfType<ConstructorDeclarationSyntax>().First();
        var statement = SyntaxFactory.ParseStatement($"CreateMap<{_model}{suffix}, {_model}>().ReverseMap();");
        var newConstructorSyntax = constructorSyntax!.AddBodyStatements(statement);
        root = root.ReplaceNode(constructorSyntax, newConstructorSyntax).NormalizeWhitespace();
        await File.WriteAllTextAsync(_mappingProfilePath, root.GetText().ToString());
    }

    public async Task AddDbSetToDbContext(string? model = null)
    {
        if (model == null)
            model = _model;
        
        // add Main Entity To DbSet
        await GenerateDbContextModifiedCode(model);
        // save To File 
        using (var sw = new StreamWriter(_dbContextPath))
        {
            await sw.WriteAsync(_dbContextModifiedCode);
            sw.Close();
        }
    }

    private async Task GenerateDbContextModifiedCode(string model,bool readFromFile = true)
    {
        if (!File.Exists(_dbContextPath))
            throw new Exception("the target DbContext file not found");
        var content = (readFromFile) ? await File.ReadAllTextAsync(_dbContextPath) : _dbContextModifiedCode;
        var syntaxTree = CSharpSyntaxTree.ParseText(content);
        var root = syntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var newProp =
            SyntaxFactory.ParseMemberDeclaration(
                $"public DbSet<{model}> {Utils.PluralizeWords(model)} => Set<{model}>();");
        var updatedClassSyntax = classSyntax.AddMembers(newProp!);
        root = root.ReplaceNode(classSyntax, updatedClassSyntax).NormalizeWhitespace();
        _dbContextModifiedCode = root.GetText().ToString();
        // add DbSets for any inner or related Entities
        var modelPath = Path.Combine(AppSettings.WorkingDirectory,
            AppSettings.EntitiesRelativePath, $"{model}.cs");
        var props = await Utils.ExtractPropsFromModel(modelPath);
        await AddInnerDbSetsToDbContext(props);
    }

    private async Task AddInnerDbSetsToDbContext(IList<PropertyDeclarationSyntax> properties)
    {
        foreach (var prop in properties)
        {
            var type = prop.Type.ToString();
            var pureType = Utils.ExtractType(type);
            var isEntity = Utils.CheckIfTypeIsEntity(pureType);
            var isSameType = pureType == _model;
            if (isEntity && !isSameType)
            {
                await GenerateDbContextModifiedCode(pureType,false);
            }
        }
    }

    public async Task AddPermissionsEntityToRole()
    {
        if (!File.Exists(_roleModelPath))
            throw new Exception("the target Role.cs entity file not found");
        var content = await File.ReadAllTextAsync(_roleModelPath);
        var syntaxTree = CSharpSyntaxTree.ParseText(content);
        var root = syntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var newProp =
            SyntaxFactory.ParseMemberDeclaration(
                $"public Permission? {Utils.PluralizeWords(_model)}Permissions {{get; set; }} \n");
        var updatedClassSyntax = classSyntax.AddMembers(newProp!);
        root = root.ReplaceNode(classSyntax, updatedClassSyntax).NormalizeWhitespace();
        await File.WriteAllTextAsync(_roleModelPath, root.GetText().ToString());
    }

    public async Task AddPermissionsDtoToRoleDto()
    {
        if (!File.Exists(_roleDtoPath))
            throw new Exception("the target RoleDto.cs Dto file not found");
        var content = await File.ReadAllTextAsync(_roleDtoPath);
        var syntaxTree = CSharpSyntaxTree.ParseText(content);
        var root = syntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var newProp =
            SyntaxFactory.ParseMemberDeclaration(
                $"public PermissionDto? {Utils.PluralizeWords(_model)}Permissions {{get; set; }} \n");
        var updatedClassSyntax = classSyntax.AddMembers(newProp!);
        root = root.ReplaceNode(classSyntax, updatedClassSyntax).NormalizeWhitespace();
        await File.WriteAllTextAsync(_roleDtoPath, root.GetText().ToString());
    }

    public async Task AddPermissionsDtoToRoleDtoRequest()
    {
        if (!File.Exists(_roleDtoRequest))
            throw new Exception("the target RoleDtoRequest.cs DtoRequest file not found");
        var content = await File.ReadAllTextAsync(_roleDtoRequest);
        var syntaxTree = CSharpSyntaxTree.ParseText(content);
        var root = syntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var newProp =
            SyntaxFactory.ParseMemberDeclaration(
                $"public PermissionDto? {Utils.PluralizeWords(_model)}Permissions {{get; set; }} \n");
        var updatedClassSyntax = classSyntax.AddMembers(newProp!);
        root = root.ReplaceNode(classSyntax, updatedClassSyntax).NormalizeWhitespace();
        await File.WriteAllTextAsync(_roleDtoRequest, root.GetText().ToString());
    }
}