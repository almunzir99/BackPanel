using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BackPanel.Application.Helpers;

public static class DataExportHelper<T>
{
    public static Byte[] ExportToExcel(IList<T> data)
    {
        var columns = GetColumns();
        var name = GetName();
        using var workBook = new XLWorkbook();
        var worksheet = workBook.Worksheets.Add(name);
        var currentRow = 1;
        foreach (var column in columns)
        {
            var index = columns.IndexOf(column);
            index++;
            var xlCell = worksheet.Cell(currentRow, index);
            xlCell.Value = column;
            xlCell.Style.Font.Bold = true;
            xlCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBEBE0");
            xlCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            xlCell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#D4D4D4");
            worksheet.Column(index).AdjustToContents();
        }

        for (int i = 0; i < data.Count(); i++)
        {
            var row = data[i];
            for (int j = 0; j < columns.Count; j++)
            {
                var column = columns[j];
                var cellData = GetPropValue(row!, column);
                var xlCell = worksheet.Cell(i + 2, j + 1);
                xlCell.Value = cellData == null ? "" : cellData.ToString();
                if (cellData != null && cellData.ToString()!.Length < 100)
                {
                    worksheet.Column(j + 1).AdjustToContents();
                }
            }
        }

        using var stream = new MemoryStream();
        workBook.SaveAs(stream);
        var content = stream.ToArray();
        return content;
    }

    private static IList<String> GetColumns()
    {
        Type type = typeof(T);
        var properties = type.GetProperties().Where(c => c.PropertyType.IsPrimitive ||
                                                         c.PropertyType == typeof(String)
                                                         || c.PropertyType == typeof(DateTime)).ToList();
        var columns = properties.Select(c => c.Name).ToList();
        return columns;
    }

    private static string GetName()
    {
        Type type = typeof(T);
        return type.Name;
    }

    private static object? GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName)?.GetValue(src, null);
    }
}