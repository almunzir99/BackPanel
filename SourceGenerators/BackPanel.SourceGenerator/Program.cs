// See https://aka.ms/new-console-template for more information


using BackPanel.SourceGenerator;
using BackPanel.SourceGenerator.CommandsRunners;
using BackPanel.SourceGenerator.Generators;
using BackPanel.SourceGenerator.Modifiers;
using CommandLine;


await Parser.Default.ParseArguments<CommandOptions>(args).WithParsedAsync(async o =>
{
    await Generate(o);
    Console.WriteLine("Press any key To Exist ...");
    Console.ReadLine();
});

async Task Generate(CommandOptions options)
{
    if (options.Model == null)
        throw new NullReferenceException("model parameter shouldn't be null");
    var dtoGen = new DtoGenerator(options.Model);
    var dtoRequestGen = new DtoRequestGenerator(options.Model);
    var interfaceGen = new InterfaceGenerator(options.Model);
    var serviceGen = new ServiceGenerator(options.Model);
    var controllerGen = new ControllerGenerator(options.Model);
    var codeModifier = new CodeModifier(options.Model);
    var dbCommandRunner = new DbCommandRunner(options.Model);
    /* **************** Step 1: Generate Dto File ********************  */
    await  dtoGen.Generate();
    Console.WriteLine("Dto File Generated Successfully");
    /* **************** Step 2: Generate Dto Request File ********************  */
    await  dtoRequestGen.Generate();
    Console.WriteLine("Dto Request File Generated Successfully");
    /* **************** Step 3: Generate Interface File ********************  */
    await  interfaceGen.Generate();
    Console.WriteLine("Interface File Generated Successfully");
    /* **************** Step 4: Generate Service File ********************  */
    await  serviceGen.Generate();
    Console.WriteLine("Service File Generated Successfully");
    /* **************** Step 5: Generate Controller File ********************  */
    await  controllerGen.Generate();
    Console.WriteLine("Service File Generated Successfully");
    /* **************** Step 6: Update MappingProfile  File ********************  */
    await  codeModifier.AppendToMappingProfile();
    Console.WriteLine("MappingProfile  updated Successfully");
    /* **************** Step 7: Update DbContext  File ********************  */
    await  codeModifier.AddDbSetToDbContext();
    Console.WriteLine("DbContext updated Successfully");
    /* **************** Step 8: Update Role Entity  File ********************  */
    await  codeModifier.AddPermissionsEntityToRole();
    Console.WriteLine("Role.cs Entity updated Successfully");
    /* **************** Step  9: Update Role Dto  File ********************  */
    await  codeModifier.AddPermissionsDtoToRoleDto();
    Console.WriteLine("RoleDto.cs updated Successfully");
    /* **************** Step  10: Update Role Dto Request  File ********************  */
    await  codeModifier.AddPermissionsDtoToRoleDtoRequest();
    Console.WriteLine("RoleDtoRequest.cs updated Successfully");
    /* **************** Step  11: Update RegisterRequiredApplicationService  File ********************  */
    await  codeModifier.AddServiceToDiFile();
    Console.WriteLine("RegisterRequiredApplicationService.cs updated Successfully");
    /* **************** Step  12: EF Migration ********************  */
    Console.WriteLine("Start EF Migrating Process ....");
    await dbCommandRunner.MigrateAsync();
    Console.WriteLine(" EF Migrating Completed Successfully");
    /* **************** Step  13: EF Db Update ********************  */
    Console.WriteLine("Start EF DataBase Update ....");
    await dbCommandRunner.DbUpdateAsync();
    Console.WriteLine(" EF Database Update Completed Successfully");
}