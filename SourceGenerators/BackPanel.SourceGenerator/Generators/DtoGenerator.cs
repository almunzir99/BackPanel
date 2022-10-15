using System.Text;
using System.Text.RegularExpressions;
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
        var extractedProps = await ExtractPropsFromModel();
        var props = await BuildDtoProps(extractedProps);
        templateContent = templateContent.Replace("@[Model]", _model);
        templateContent = templateContent.Replace("@[Props]", props);
        var formattedCode = Utils.FormatCodeWithRoslyn(templateContent);
        await File.WriteAllTextAsync(_outPutPath, formattedCode);
    }

    private async Task<IList<PropertyDeclarationSyntax>> ExtractPropsFromModel()
    {
        var modelContent = await File.ReadAllTextAsync(_modelPath);
        var modelSyntaxTree = CSharpSyntaxTree.ParseText(modelContent);
        var modelRoot = modelSyntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = modelRoot.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var modelPropsList = classSyntax.Members.OfType<PropertyDeclarationSyntax>().ToList();
        return modelPropsList;
    }
    private async Task<string> BuildDtoProps(IList<PropertyDeclarationSyntax> modelPropsList)
    {
        var stringBuilder = new StringBuilder();
        foreach (var prop in modelPropsList)
        {
            var type = prop.Type.ToString();
            var pureType = Utils.ExtractType(type);
            var isEntity = Utils.CheckIfTypeIsEntity(pureType);
            if (isEntity)
            {
                await GeneratePropDto(pureType);
            }
        }

        var modelPropsListStrings =  NormalizeDtoProps(modelPropsList);
        var modelProps = stringBuilder.AppendJoin("", modelPropsListStrings);
        return modelProps.ToString();
    }

    private IList<string> NormalizeDtoProps(IList<PropertyDeclarationSyntax> modelPropsList)
    {
        var propsListStrings = modelPropsList.Select(c =>
        {
            var propStr = c.ToFullString();
            var type = c.Type.ToString();
            var isEntity = Utils.CheckIfTypeIsEntity(type);
            if (isEntity && propStr.Contains($"{type}"))
            {
                var newPropStr = "";
                var regex = new Regex(@"(\w+) (\w+\??) (\w+) ({ get; set;? }?)");
                if (regex.Match(propStr).Success)
                {
                    var groups = regex.Match(propStr).Groups;
                    for (int i = 1; i < groups.Count; i++)
                    {
                        if (i == 2)
                            newPropStr += $"{groups[i].Value.Replace("?", "")}Dto?" + " ";
                        else
                            newPropStr += groups[i].Value + " ";
                    }
                }

                return $"{newPropStr} \n";
            }

            return propStr;
        }).ToList();
        return propsListStrings;
    }
    private async Task GeneratePropDto(string model)
    {
        var dtos = Directory.GetFiles(Path.Combine(
            AppSettings.WorkingDirectory,
            AppSettings.DtosRelativePath
        ));
        var found = false;
        foreach (var dto in dtos)
        {
            var name = Path.GetFileNameWithoutExtension(dto);
            if (name == $"{model}Dto")
            {
                found = true;
                break;
            }
        }

        if (found)
            return;
        var options = new CommandOptions() { Model = model, DtoRequest = false };
        var generator = new Generator(options);
        await generator.GenerateAsync();
    }
}