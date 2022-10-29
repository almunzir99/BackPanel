using System.Text;
using System.Text.RegularExpressions;
using BackPanel.SourceGenerator.Modifiers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace BackPanel.SourceGenerator.Generators;

public class DtoGenerator
{
    private readonly string _modelPath;
    private readonly string _outPutPath;
    private readonly string _templatePath;
    private readonly string _model;
    private readonly DtoType _dtoType;
    private readonly CodeModifier _codeModifier;

    public DtoGenerator(string model, DtoType dtoType = DtoType.Dto)
    {
        _dtoType = dtoType;
        _codeModifier = new CodeModifier(model);
        _model = model;
        _modelPath = Path.Combine(
            AppSettings.WorkingDirectory,
            AppSettings.EntitiesRelativePath, $"{model}.cs"
        );
        _outPutPath = Path.Combine(
            AppSettings.WorkingDirectory,
            _dtoType == DtoType.DtoRequest ? AppSettings.DtosRequestsRelativePath : AppSettings.DtosRelativePath,
            _dtoType == DtoType.DtoRequest ? $"{model}DtoRequest.cs" : $"{model}Dto.cs"
        );
        _templatePath = Path.Combine(
            AppSettings.WorkingDirectory,
            AppSettings.TemplatesRelativePath,
            _dtoType == DtoType.DtoRequest ? "DtoRequestTemplate.sgt" : $"DtoTemplate.sgt"
        );

        if (!File.Exists(_modelPath))
            throw new FileNotFoundException("Model File  Not Found");
        if (!File.Exists(_templatePath))
            throw new FileNotFoundException("Template File  Not Found");
        if (File.Exists(_outPutPath))
            throw new InvalidOperationException("Dto File Already Exists");

         
    }

    public async Task Generate()
    {
        var stringBuilder = new StringBuilder();
        var templateContent = await File.ReadAllTextAsync(_templatePath);
        var extractedProps = await ExtractPropsFromModel();
        var usings = await Utils.ExtractUsingsFromModel(_modelPath);
          if(_dtoType == DtoType.Dto)
        {
            extractedProps = ClearPropsAttribute(extractedProps);
            usings = usings.Where(c => !c.Contains("using System.ComponentModel.DataAnnotations;") 
            && !c.Contains("using System.ComponentModel.DataAnnotations.Schema;")).ToList();
        }
        var usingsStr = stringBuilder.AppendJoin("", usings).ToString();
        var props = await BuildDtoProps(extractedProps);
        templateContent = templateContent.Replace("@[Model]", _model);
        templateContent = templateContent.Replace("@[Props]", props);
        templateContent = templateContent.Replace("@[Usings]", usingsStr);
        var formattedCode = Utils.FormatCodeWithRoslyn(templateContent);
        await File.WriteAllTextAsync(_outPutPath, formattedCode);
        await _codeModifier.AppendToMappingProfile(_dtoType != DtoType.DtoRequest ? "DtoRequest" : "Dto");
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
    private IList<PropertyDeclarationSyntax> ClearPropsAttribute(IList<PropertyDeclarationSyntax> props)
     {
        var newPropList = props.Select(c =>{
            var newProp = c;
            foreach (var attributeList in c.AttributeLists)
            {
                newProp = c.RemoveNode(attributeList,SyntaxRemoveOptions.KeepNoTrivia);
            }
            return newProp ?? c;
        }).ToList();
        return newPropList;
     }
    private async Task<string> BuildDtoProps(IList<PropertyDeclarationSyntax> modelPropsList)
    {
        var stringBuilder = new StringBuilder();
        foreach (var prop in modelPropsList)
        {
            var type = prop.Type.ToString();
            var pureType = Utils.ExtractType(type);
            var isEntity = Utils.CheckIfTypeIsEntity(pureType);
            var isSameType = pureType == _model;
            if (isEntity && !isSameType)
            {
                await GeneratePropDto(pureType);
            }
        }

        var modelPropsListStrings = NormalizeDtoProps(modelPropsList);
        var modelProps = stringBuilder.AppendJoin("", modelPropsListStrings);
        return modelProps.ToString();
    }

    private IList<string> NormalizeDtoProps(IList<PropertyDeclarationSyntax> modelPropsList)
    {
        var propsListStrings = modelPropsList.Select(c =>
        {
            var propStr = c.ToFullString();
            var type = c.Type.ToString();
            var pureType = Utils.ExtractType(type);
            var isEntity = Utils.CheckIfTypeIsEntity(pureType);
            if (isEntity && propStr.Contains($"{type}"))
            {
                var newPropStr = "";
                var regex = new Regex(@"(\w+) (\w+\<)?(\w+)(\>?)\?? (\w+) ({ get; set;? }?) ?(\=)? ?(new)? ?(\w+\<)?(\w+)?(\>?)\??(\(\))?(;)?");
                if (regex.Match(propStr).Success)
                {
                    var groups = regex.Match(propStr).Groups;
                    for (int i = 1; i < groups.Count; i++)
                    {
                        if (i == 3|| ( i == 10 && !string.IsNullOrEmpty(groups[10].Value.Trim())))
                        {
                            var typeSuffix = _dtoType == DtoType.DtoRequest ? "DtoRequest" : "Dto";
                            newPropStr +=
                                $"{groups[i].Value.Replace("?", "")}{typeSuffix}?" + " ";
                        }
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
             _dtoType == DtoType.DtoRequest ? AppSettings.DtosRequestsRelativePath : AppSettings.DtosRelativePath
        ));
        var found = false;
        foreach (var dto in dtos)
        {
            var name = Path.GetFileNameWithoutExtension(dto);
            var dtoFile = _dtoType == DtoType.DtoRequest ? $"{model}DtoRequest" : $"{model}Dto";
            if (name == dtoFile)
            {
                found = true;
                break;
            }
        }
        if (found)
            return;
        var generator = new DtoGenerator(model, _dtoType);
        await generator.Generate();
    }
}