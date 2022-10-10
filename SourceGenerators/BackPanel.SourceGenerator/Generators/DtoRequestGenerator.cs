using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BackPanel.SourceGenerator.Generators;

public class DtoRequestGenerator
{
     private readonly string _modelPath;
    private readonly string _outPutPath;
    private readonly string _templatePath;
    private readonly string _model;

    public DtoRequestGenerator(string model)
    {
        _modelPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.EntitiesRelativePath, $"{model}.cs"
            );
        _outPutPath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.DtosRequestsRelativePath, $"{model}DtoRequest.cs"
            );
        _templatePath = Path.Combine(
            AppSettings.WorkingDirectory, 
            AppSettings.TemplatesRelativePath, $"DtoRequestTemplate.sgt"
        );
        
        if (!File.Exists(_modelPath))
            throw new FileNotFoundException("Model File  Not Found");
        if (!File.Exists(_templatePath))
            throw new FileNotFoundException("Template File  Not Found");
        if (File.Exists(_outPutPath))
            throw new InvalidOperationException("Dto Request File Already Exists");
        _model = model;
        
    }

    public async Task Generate()
    {
        var templateContent = await File.ReadAllTextAsync(_templatePath);
        var props = ExtractPropsFromModel();
        templateContent = templateContent.Replace("@[Model]", _model);
        templateContent = templateContent.Replace("@[Props]", props);
        await File.WriteAllTextAsync(_outPutPath,templateContent);
    }

    private String ExtractPropsFromModel()
    {
        var modelContent = File.ReadAllText(_modelPath);
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