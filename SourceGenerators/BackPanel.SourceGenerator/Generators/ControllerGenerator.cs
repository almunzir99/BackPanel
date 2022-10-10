namespace BackPanel.SourceGenerator.Generators;

public class ControllerGenerator
{
    private readonly string _outPutPath;
    private readonly string _templatePath;
    private readonly string _model;

    public ControllerGenerator(string model)
    {
        var modelPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.EntitiesRelativePath, $"{model}.cs"
        );
        _outPutPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.ControllersRelativePath, $"{Utils.PluralizeWords(model)}Controller.cs"
        );
        _templatePath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.TemplatesRelativePath, $"ControllerTemplate.sgt"
        );
        
        if (!File.Exists(modelPath))
            throw new FileNotFoundException("Model File  Not Found");
        if (!File.Exists(_templatePath))
            throw new FileNotFoundException("Template File  Not Found");
        if (File.Exists(_outPutPath))
            throw new InvalidOperationException("Controller File Already Exists");
        _model = model;
        
    }
    public async Task Generate()
    {
        var models = Utils.PluralizeWords(_model);
        var templateContent = await File.ReadAllTextAsync(_templatePath);
        templateContent = templateContent.Replace("@[Models]", models);
        templateContent = templateContent.Replace("@[Model]", _model);
        await File.WriteAllTextAsync(_outPutPath,templateContent);
    }
}