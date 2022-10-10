using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BackPanel.SourceGenerator.Generators;

public class DtoGenerator
{
    private readonly string _modelPath;
    private readonly string _outPutPath;
    private readonly string _templatePath;
    private readonly string _model;

    public DtoGenerator(string model)
    {
        _modelPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.EntitiesRelativePath, $"{model}.cs"
            );
        _outPutPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.DtosRelativePath, $"{model}Dto.cs"
            );
        _templatePath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.TemplatesRelativePath, $"DtoTemplate.sgt"
        );
        
        if (!File.Exists(_modelPath))
            throw new FileNotFoundException("Model File  Not Found");
        if (!File.Exists(_templatePath))
            throw new FileNotFoundException("Template File  Not Found");
        if (File.Exists(_outPutPath))
            throw new InvalidOperationException("Dto File Already Exists");
        _model = model;
        
    }

    public async Task Generate()
    {
        var templateContent = await File.ReadAllTextAsync(_templatePath);
        var props = await ExtractPropsFromModel();
        templateContent = templateContent.Replace("@[Model]", _model);
        templateContent = templateContent.Replace("@[Props]", props);
        await File.WriteAllTextAsync(_outPutPath,templateContent);
    }

    private async Task<string> ExtractPropsFromModel()
    {
        var modelContent = await File.ReadAllTextAsync(_modelPath);
        var modelSyntaxTree = CSharpSyntaxTree.ParseText(modelContent);
        var modelRoot = modelSyntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = modelRoot.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var modelPropsList = classSyntax.Members.Select(c => c.ToFullString());
        var stringBuilder = new StringBuilder();
        var modelProps = stringBuilder.AppendJoin<string>("", modelPropsList);
        return modelProps.ToString();
    }
}