using System.Text.Json.Nodes;
using BackPanel.FilesManager.Interfaces;
using BackPanel.TranslationEditor.Interfaces;
using BackPanel.TranslationEditor.Model;
using Newtonsoft.Json.Linq;

namespace BackPanel.TranslationEditor.Services;

public class TranslationEditorService : ITranslationEditorService
{
    private readonly string _rootPath;

    public TranslationEditorService(IPathProvider pathProvider)
    {
        _rootPath = Path.Combine(pathProvider.GetRootPath(), "i18n");
    }

    public async Task CreateLanguage(string code)
    {
        var newFilePath = Path.Combine(_rootPath, $"{code}.json");
        var files = Directory.GetFiles(_rootPath);

        if (File.Exists(newFilePath))
            throw new Exception("the language file is already created");
        if (files.Length > 0)
        {
            var content = await File.ReadAllTextAsync(files[0]);
            await File.WriteAllTextAsync(newFilePath, content);
        }
        else
            await File.WriteAllTextAsync(newFilePath, "{}");
    }

    public  void DeleteLanguage(string code)
    {
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            var fileCode = Path.GetFileNameWithoutExtension(file);
            if (fileCode == code)
            {
                 File.Delete(file);
            }

        }

    }

    public async Task<JObject> GetLanguage(string code)
    {
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            var fileCode = Path.GetFileNameWithoutExtension(file);
            if (fileCode == code)
            {
                var content = await File.ReadAllTextAsync(file);
                return JObject.Parse(content);
            }

        }

        throw new Exception("there's no translation with provided code");
    }

    public IList<string> GetLanguagesList()
    {
        var languagesCodes = Directory.GetFiles(_rootPath)
            .Select(c => Path.GetFileName(c)!).ToList();
        return languagesCodes;
    }

    public async Task CreateParentNode(string title)
    {
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            var json = await File.ReadAllTextAsync(file);
            var jsonObject = JObject.Parse(json);
            jsonObject[title] = new JObject();
            await File.WriteAllTextAsync(file,jsonObject.ToString());
        }
    }

    public async Task DeleteParentNode(string title)
    {
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            var json = await File.ReadAllTextAsync(file);
            var jsonObject = JObject.Parse(json);
            var target = jsonObject[title];
            if (target == null)
                throw new Exception("the target parent node not available");
            jsonObject.Property(title)!.Remove();
            await File.WriteAllTextAsync(file,jsonObject.ToString());

        }
    }

    public async Task UpdateParentNode(string oldTitle, string newTitle)
    {
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            var json = await File.ReadAllTextAsync(file);
            var jsonObject = JObject.Parse(json);
            var target = jsonObject.GetValue(oldTitle);
            if (target == null)
                throw new Exception("the target parent node not available");
            jsonObject[newTitle] = jsonObject.GetValue(oldTitle);
            jsonObject.Property(oldTitle)!.Remove();
            await File.WriteAllTextAsync(file,jsonObject.ToString());

        }
    }

    public async Task CreateNode(NodeBody body)
    {
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            
            var code = Path.GetFileNameWithoutExtension(file);
            var json = await File.ReadAllTextAsync(file);
            var jsonObject = JObject.Parse(json);
            var target = jsonObject[body.Parent!];
            if (target == null)
                throw new Exception("the target parent node not available");
            if (body.Values.Keys.Contains(code) == false)
                target[body.Title!] = "";
            else
                target[body.Title!] = body.Values[code];
            await File.WriteAllTextAsync(file,jsonObject.ToString());
        }
    }

    public async Task UpdateNode(NodeBody body)
    {
        await this.CreateNode(body);
    }

    public async Task DeleteNode(string parent, string title)
    {
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            var json = await File.ReadAllTextAsync(file);
            var jsonObject = JObject.Parse(json);
            var target = jsonObject.GetValue(parent) as JObject;
            if (target == null)
                throw new Exception("the target parent node not available");
            if (target[title] == null)
                throw new Exception("the target  node not available");
            target.Property(title)!.Remove();
            await File.WriteAllTextAsync(file,jsonObject.ToString());

        }
    }

    public async Task<JObject> GetTranslationTree()
    {
        var resultObject = new JObject();
        foreach (var file in Directory.GetFiles(_rootPath))
        {
            var code = Path.GetFileNameWithoutExtension(file);
            var json = await File.ReadAllTextAsync(file);
            var jObject = JObject.Parse(json);
            foreach (var keyValuePair in jObject)
            {
                if (resultObject.ContainsKey(keyValuePair.Key) == false)
                    resultObject[keyValuePair.Key] = new JObject();
                if (keyValuePair.Value is JObject node)
                    foreach (var childKeyValuePair in node)
                    {
                        if (resultObject[keyValuePair.Key] != null)
                        {
                            resultObject[keyValuePair.Key]![childKeyValuePair.Key] ??= new JObject();
                            resultObject[keyValuePair.Key]![childKeyValuePair.Key]![code] = childKeyValuePair.Value;
                        }
                    }
            }
        }

        return resultObject;
    }
}