using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pluralize.NET.Core;

namespace BackPanel.SourceGenerator;

public static class Utils
{
    public static string PluralizeWords(string word)
    {
        var pluralizer = new Pluralizer();
        return pluralizer.Pluralize(word);
    }
    public static bool CheckIfTypeIsEntity(string entity)
    {
        var isEntity = false;
        var path = Path.Combine(
            AppSettings.WorkingDirectory,
            AppSettings.EntitiesRelativePath
        );
        var entities = Directory.GetFiles(path);
        foreach (var fEntity in entities)
        {
            var name = Path.GetFileNameWithoutExtension(fEntity);
            if (name == entity)
            {
                isEntity = true;
                break;
            }
        }

        return isEntity;
    }
    // Pure Type Extraction Form Complex Type Like Generics
    public static string ExtractType(string type)
    {
        var target = type;
        var genericRegex = new Regex(@"([\w.]+)\<([\w.]+)\>\??");
        var match = genericRegex.Match(target);
        if (match.Success)
        {
            target = match.Groups[2].Value;
        }

        target = target.Replace("?", "");
        return target;
    }

    public static string FormatCodeWithRoslyn(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetRoot().NormalizeWhitespace();
        var formatted = root.ToFullString();
        return formatted;
    }
    public static async Task<IList<PropertyDeclarationSyntax>> ExtractPropsFromModel(string modelPath)
    {
        var modelContent = await File.ReadAllTextAsync(modelPath);
        var modelSyntaxTree = CSharpSyntaxTree.ParseText(modelContent);
        var modelRoot = modelSyntaxTree.GetCompilationUnitRoot();
        var namespaceSyntax = modelRoot.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();
        var classSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        var modelPropsList = classSyntax.Members.OfType<PropertyDeclarationSyntax>().ToList();
        return modelPropsList;
    }
    public static async Task<IList<String>> ExtractUsingsFromModel(string modelPath)
    {
        var modelContent = await File.ReadAllTextAsync(modelPath);
        var modelSyntaxTree = CSharpSyntaxTree.ParseText(modelContent);
        var modelRoot = modelSyntaxTree.GetCompilationUnitRoot();
        var usings = modelRoot.Usings.Select(c => c.ToFullString()).ToList();
        return usings;
    }
}