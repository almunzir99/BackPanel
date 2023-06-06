namespace BackPanel.Domain.Attributes;
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class IgnoreExportAttribute : Attribute
{
}