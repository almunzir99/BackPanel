namespace BackPanel.TranslationEditor.Model;

public class NodeBody
{
    public string? Title { get; set; }
    public string? Parent { get; set; }
    public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();

}