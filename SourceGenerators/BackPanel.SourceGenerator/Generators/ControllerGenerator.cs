namespace BackPanel.SourceGenerator.Generators;

public class ControllerGenerator
{
    private readonly string _outPutPath;
    private readonly string _templatePath;
    private readonly string _model;
    private readonly string projectName;

    public ControllerGenerator(string model, string workingDirectory, string projectName)
    {
        var modelPath = Path.Combine(
            workingDirectory,
            AppSettings.EntitiesRelativePath.Replace("ProjectName",projectName), $"{model}.cs"
        );
        _outPutPath = Path.Combine(
            workingDirectory,
            AppSettings.ControllersRelativePath.Replace("ProjectName",projectName), $"{Utils.PluralizeWords(model)}Controller.cs"
        );
        _templatePath = Path.Combine(
            workingDirectory,
            AppSettings.TemplatesRelativePath.Replace("ProjectName",projectName), "ControllerTemplate.sgt"
        );
        if (!File.Exists(modelPath))
            throw new FileNotFoundException("Model File  Not Found");
        if (!File.Exists(_templatePath))
            throw new FileNotFoundException("Template File  Not Found");
        if (File.Exists(_outPutPath))
            throw new InvalidOperationException("Controller File Already Exists");
        _model = model;
        this.projectName = projectName;
    }
    public async Task Generate()
    {
        var models = Utils.PluralizeWords(_model);
        var templateContent = await File.ReadAllTextAsync(_templatePath);
        templateContent = templateContent.Replace("@[Models]", models);
        templateContent = templateContent.Replace("@[Model]", _model);
        templateContent = templateContent.Replace("@[ProjectName]", projectName);
        await File.WriteAllTextAsync(_outPutPath, templateContent);
    }
}