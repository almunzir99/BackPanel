using Pluralize.NET.Core;

namespace BackPanel.SourceGenerator;

public static class Utils
{
    public static string PluralizeWords(string word)
    {
        var pluralizer = new Pluralizer();
        return pluralizer.Pluralize(word);
    }
}