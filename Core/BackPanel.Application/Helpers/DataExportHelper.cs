using System.Text;
using ClosedXML.Excel;
using DinkToPdf;
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

    public static Byte[] ExportToPdf(IList<T> data, string stylePath)
    {
        var htmlTable = GenerateHtmlTable(data);
        var name = GetName();
        var htmlContent = @$"
        <html>
            <head>
            </head>
            <body>
                {htmlTable}
            </body>
        </html>
    ";
        var converter = new SynchronizedConverter(new PdfTools());
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings =
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4Plus,
            },
            Objects =
            {
                new ObjectSettings()
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = stylePath },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                }
            }
        };
        var content = converter.Convert(doc);
        return content;
    }

    private static string GenerateHtmlTable(IList<T> data)
    {
        var columns = GetColumns();
        var name = GetName();
        var sp = new StringBuilder();
        sp.Append(@"
          <table>
                <tr>
    ");
        foreach (var col in columns)
        {
            sp.AppendLine($@"<th style= ""padding:10px"">{col}</th>");
        }

        sp.AppendLine("</tr>");
        foreach (var row in data)
        {
            sp.AppendLine("<tr>");
            foreach (var col in columns)
            {
                var cellData = GetPropValue(row!, col);
                sp.AppendLine($@"<td style= ""padding:10px""> {cellData} </td>");
            }

            sp.AppendLine("</tr>");
        }

        sp.AppendLine("</table>");
        return sp.ToString();
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