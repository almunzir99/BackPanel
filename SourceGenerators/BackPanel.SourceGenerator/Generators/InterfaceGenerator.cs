namespace BackPanel.SourceGenerator.Generators;

public class InterfaceGenerator
{
    private readonly string _outPutPath;
    private readonly string _templatePath;
    private readonly string _model;

    public InterfaceGenerator(string model)
    {
        var modelPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.EntitiesRelativePath, $"{model}.cs"
        );
        _outPutPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.InterfacesRelativePath, $"I{Utils.PluralizeWords(model)}Service.cs"
        );
        _templatePath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.TemplatesRelativePath, $"InterfaceTemplate.sgt"
        );
        
        if (!File.Exists(modelPath))
            throw new FileNotFoundException("Model File  Not Found");
        if (!File.Exists(_templatePath))
            throw new FileNotFoundException("Template File  Not Found");
        if (File.Exists(_outPutPath))
            throw new InvalidOperationException("Interface File Already Exists");
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