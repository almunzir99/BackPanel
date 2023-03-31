using BackPanel.TranslationEditor.Model;
using Newtonsoft.Json.Linq;

namespace BackPanel.TranslationEditor.Interfaces;

public interface ITranslationEditorService
{
    Task CreateLanguage(string code);
    void DeleteLanguage(string code);
    Task<JObject> GetLanguage(string code);
    IList<string> GetLanguagesList();
    Task CreateParentNode(string title);
    Task DeleteParentNode(string title);
    Task UpdateParentNode(string oldTitle, string newTitle);
    Task CreateNode(NodeBody body);
    Task UpdateNode(NodeBody body);
    Task DeleteNode(string parent, string title);
    Task<JObject> GetTranslationTree();
}