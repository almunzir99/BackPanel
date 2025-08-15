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
        var dtoGen = new DtoGenerator(_options.Model, workingDirectory, projectName);
        var CQRsGen = new CQRSGenerator(_options.Model, workingDirectory, projectName);

        var dtoRequestGen = new DtoGenerator(_options.Model, workingDirectory, projectName, DtoType.DtoRequest);
        var controllerGen = new ControllerGenerator(_options.Model, workingDirectory, projectName);
        var codeModifier = new CodeModifier(_options.Model, workingDirectory, projectName);
        var dbCommandRunner = new DbCommandRunner(_options.Model, workingDirectory, projectName);
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
        if (_options.CQRS!.Value)
        {
            /* **************** Step 9: Generate Service File ********************  */
            await CQRsGen.Generate();
            Console.WriteLine("CQRS Generated Successfully");
            /* **************** Step  11: Update RegisterRequiredApplicationService  File ********************  */
        }
        if (_options.DatabaseUpdate!.Value)
        {
            /* **************** Step  10: EF Db Update ********************  */
            Console.WriteLine("Start EF DataBase Update ....");
            await dbCommandRunner.DbUpdateAsync();
            Console.WriteLine(" EF Database Update Completed Successfully");
        }
        if (_options.Controller!.Value)
        {
            /* **************** Step 11: Generate Controller File ********************  */
            await controllerGen.Generate();
            Console.WriteLine("Controller File Generated Successfully");
        }
    }
}