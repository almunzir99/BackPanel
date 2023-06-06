using BackPanel.SourceGenerator.CommandsRunners;
using BackPanel.SourceGenerator.Generators;
using BackPanel.SourceGenerator.Modifiers;

namespace BackPanel.SourceGenerator;
public class Generator
{
    private readonly CommandOptions _options;
    private readonly string projectName;
    private readonly string workingDirectory;

    public Generator(CommandOptions options, string workingDirectory, string projectName)
    {
        this.projectName = projectName;
        _options = options;
        this.workingDirectory = workingDirectory;
    }
    public async Task GenerateAsync()
    {
        if (_options.Model == null)
            throw new NullReferenceException("model parameter shouldn't be null");
        var dtoGen = new DtoGenerator(_options.Model,workingDirectory,projectName);
        var dtoRequestGen = new DtoGenerator(_options.Model,workingDirectory,projectName, DtoType.DtoRequest);
        var interfaceGen = new InterfaceGenerator(_options.Model,workingDirectory,projectName);
        var serviceGen = new ServiceGenerator(_options.Model,workingDirectory,projectName);
        var controllerGen = new ControllerGenerator(_options.Model,workingDirectory,projectName);
        var codeModifier = new CodeModifier(_options.Model,workingDirectory,projectName);
        var dbCommandRunner = new DbCommandRunner(_options.Model,workingDirectory,projectName);
        /* **************** Step 1: Generate Dto File ********************  */
        if (_options.Dto!.Value)
        {
            await dtoGen.Generate();
            Console.WriteLine("Dto File Generated Successfully");
        }
        /* **************** Step 2: Generate Dto Request File ********************  */
        if (_options.DtoRequest!.Value)
        {
            await dtoRequestGen.Generate();
            Console.WriteLine("Dto Request File Generated Successfully");
        }
        else
        {
            return;
        }

        if (!_options.Dto.Value) return;
        if (_options.DbContext!.Value)
        {
            /* **************** Step 3: Update DbContext  File ********************  */
            await codeModifier.AddDbSetToDbContext();
            Console.WriteLine("DbContext updated Successfully");
            /* **************** Step  4: EF Migration ********************  */
            Console.WriteLine("Start EF Migrating Process ....");
            await dbCommandRunner.MigrateAsync();
            Console.WriteLine(" EF Migrating Completed Successfully");
        }
        else
        {
            return;
        }

        if (_options.Permission!.Value)
        {
            /* **************** Step 5: Update Role Entity  File ********************  */
            await codeModifier.AddPermissionsEntityToRole();
            Console.WriteLine("Role.cs Entity updated Successfully");
            /* **************** Step  6: Update Role Dto  File ********************  */
            await codeModifier.AddPermissionsDtoToRoleDto();
            Console.WriteLine("RoleDto.cs updated Successfully");
            /* **************** Step  7: Update Role Dto Request  File ********************  */
            await codeModifier.AddPermissionsDtoToRoleDtoRequest();
            Console.WriteLine("RoleDtoRequest.cs updated Successfully");
            /* **************** Step  8: EF Migration ********************  */
            Console.WriteLine("Start EF Migrating Process ....");
            await dbCommandRunner.MigrateAsync($"Add{_options.Model}Permissions");
            Console.WriteLine(" EF Migrating Completed Successfully");
        }
        else
        {
            return;
        }

        if (_options.Service!.Value)
        {
            /* **************** Step 9: Generate Interface File ********************  */
            await interfaceGen.Generate();
            Console.WriteLine("Interface File Generated Successfully");
            /* **************** Step 10: Generate Service File ********************  */
            await serviceGen.Generate();
            Console.WriteLine("Service File Generated Successfully");
            /* **************** Step  11: Update RegisterRequiredApplicationService  File ********************  */
            await codeModifier.AddServiceToDiFile();
            Console.WriteLine("RegisterRequiredApplicationService.cs updated Successfully");
        }
        else
        {
            return;
        }

        if (_options.DatabaseUpdate!.Value)
        {
            /* **************** Step  13: EF Db Update ********************  */
            Console.WriteLine("Start EF DataBase Update ....");
            await dbCommandRunner.DbUpdateAsync();
            Console.WriteLine(" EF Database Update Completed Successfully");
        }
        if (_options.Controller!.Value)
        {
            /* **************** Step 5: Generate Controller File ********************  */
            await controllerGen.Generate();
            Console.WriteLine("Controller File Generated Successfully");
        }
    }
}