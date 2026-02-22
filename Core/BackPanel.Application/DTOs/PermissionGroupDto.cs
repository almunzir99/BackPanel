using System.Reflection;
using BackPanel.Application.Constants;

namespace BackPanel.Application.DTOs;

/// <summary>Leaf node — a single permission (e.g. "View", "Add").</summary>
public class PermissionActionDto
{
    /// <summary>Full dot-notation value stored in AspNetRoleClaims, e.g. "Administration.Admins.View"</summary>
    public string Value { get; set; } = string.Empty;
    /// <summary>Human-readable label built from the last segment, e.g. "View"</summary>
    public string Label { get; set; } = string.Empty;
}

/// <summary>Mid-level node — a module within a section (e.g. "Admins").</summary>
public class PermissionModuleDto
{
    public string Key   { get; set; } = string.Empty;  // "Admins"
    public string Label { get; set; } = string.Empty;  // "Admins"
    public List<PermissionActionDto> Actions { get; set; } = new();
}

/// <summary>Top-level node — a section (e.g. "Administration").</summary>
public class PermissionSectionDto
{
    public string Key   { get; set; } = string.Empty;  // "Administration"
    public string Label { get; set; } = string.Empty;  // "Administration"
    public List<PermissionModuleDto> Modules { get; set; } = new();
}

/// <summary>
/// Builds the nested permission tree from <see cref="PermissionsConstants"/> at runtime using reflection.
/// Any new constant added to <see cref="PermissionsConstants"/> is automatically picked up.
/// </summary>
public static class PermissionTreeBuilder
{
    public static List<PermissionSectionDto> Build()
    {
        var values = typeof(PermissionsConstants)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(string))
            .Select(f => (string)f.GetValue(null)!)
            .OrderBy(v => v);

        var sections = new Dictionary<string, PermissionSectionDto>(StringComparer.OrdinalIgnoreCase);

        foreach (var value in values)
        {
            var parts = value.Split('.');
            if (parts.Length < 3) continue; // must be at least Section.Module.Action

            var sectionKey = parts[0];
            var moduleKey  = parts[1];
            var actionLabel = string.Join(".", parts[2..]); // supports 4+ segments

            if (!sections.TryGetValue(sectionKey, out var section))
            {
                section = new PermissionSectionDto { Key = sectionKey, Label = sectionKey };
                sections[sectionKey] = section;
            }

            var module = section.Modules.FirstOrDefault(m => m.Key == moduleKey);
            if (module == null)
            {
                module = new PermissionModuleDto { Key = moduleKey, Label = moduleKey };
                section.Modules.Add(module);
            }

            module.Actions.Add(new PermissionActionDto { Value = value, Label = actionLabel });
        }

        return sections.Values.ToList();
    }
}
