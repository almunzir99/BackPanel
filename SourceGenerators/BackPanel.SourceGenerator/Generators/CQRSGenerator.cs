using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.SourceGenerator.Generators
{
    public class CQRSGenerator
    {
        private readonly string _commandsOutPutFolderPath;
        private readonly string _queriesOutPutFolderPath;
        private readonly string _templateFolderPath;
        private readonly string _model;
        private readonly string projectName;


        private List<string> commands = new()
        {
            "Create", "Update", "Delete","CreateBulk","ToggleActive"
        };
        private List<string> queries = new()
        {
            "GetAll","GetById","ExportToExcel","ExportToPDF"
        };
        public CQRSGenerator(string model, string workingDirectory, string projectName)
        {
            var modelPath = Path.Combine(
                workingDirectory,
                AppSettings.EntitiesRelativePath.Replace("ProjectName", projectName), $"{model}.cs"
            );
            _commandsOutPutFolderPath = Path.Combine(
                workingDirectory,
                AppSettings.CommandsHandlerRelativePath.Replace("ProjectName", projectName).Replace("FeaturePuralName", Utils.PluralizeWords(model))
            );
            _queriesOutPutFolderPath = Path.Combine(
               workingDirectory,
               AppSettings.QueriesHandlerRelativePath.Replace("ProjectName", projectName).Replace("FeaturePuralName", Utils.PluralizeWords(model))
           );
            _templateFolderPath = Path.Combine(
                workingDirectory,
                AppSettings.TemplatesRelativePath.Replace("ProjectName", projectName)
            );
            if (!File.Exists(modelPath))
                throw new FileNotFoundException("Model File  Not Found");
            if (!Directory.Exists(_templateFolderPath))
                throw new FileNotFoundException("Template Folder  Not Found");
            if (!File.Exists(_commandsOutPutFolderPath))
                Directory.CreateDirectory(_commandsOutPutFolderPath);
            if (!File.Exists(_queriesOutPutFolderPath))
                Directory.CreateDirectory(_queriesOutPutFolderPath);
            _model = model;
            this.projectName = projectName;
        }

        public async Task Generate()
        {
            var models = Utils.PluralizeWords(_model);
            foreach (var command in commands)
            {
                var templatePath = Path.Combine(_templateFolderPath, "Commands", "Handlers", $"{command}CommandHandlerTemplate.sgt");
                if (!File.Exists(templatePath))
                    throw new FileNotFoundException($"Template File Not Found: {templatePath}");
                var templateContent = await File.ReadAllTextAsync(templatePath);
                templateContent = templateContent.Replace("@[Models]", models);
                templateContent = templateContent.Replace("@[Model]", _model);
                templateContent = templateContent.Replace("@[ProjectName]", projectName);
                var outputFilePath = Path.Combine(_commandsOutPutFolderPath, $"{command}{_model}CommandHandler.cs");
                if (!File.Exists(outputFilePath))
                    File.Create(outputFilePath).Close(); // Ensure the file exists before writing
                await File.WriteAllTextAsync(outputFilePath, Utils.FormatCodeWithRoslyn(templateContent));
            }
            foreach (var query in queries)
            {
                var templatePath = Path.Combine(_templateFolderPath, "Queries", "Handlers", $"{query}QueryHandlerTemplate.sgt");
                if (!File.Exists(templatePath))
                    throw new FileNotFoundException($"Template File Not Found: {templatePath}");
                var templateContent = await File.ReadAllTextAsync(templatePath);
                templateContent = templateContent.Replace("@[Models]", models);
                templateContent = templateContent.Replace("@[Model]", _model);
                templateContent = templateContent.Replace("@[ProjectName]", projectName);
                var outputFilePath = Path.Combine(_queriesOutPutFolderPath, $"{query}{_model}QueryHandler.cs");
                if (!File.Exists(outputFilePath))
                    File.Create(outputFilePath).Close(); // Ensure the file exists before writing
                await File.WriteAllTextAsync(outputFilePath, Utils.FormatCodeWithRoslyn(templateContent));
            }

        }


    }
}