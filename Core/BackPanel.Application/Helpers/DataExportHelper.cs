using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BackPanel.Application.Static;
using BackPanel.Domain.Attributes;
using ClosedXML.Excel;
using PuppeteerSharp;

namespace BackPanel.Application.Helpers;
public static class DataExportHelper<T>
{
    public static Byte[] ExportToExcel(IList<T> data)
    {
        var columns = GetColumns();
        var name = GetName();
        using var workBook = new XLWorkbook();
        var worksheet = workBook.Worksheets.Add(name);
        const int currentRow = 1;
        foreach (var column in columns)
        {
            var index = columns.IndexOf(column);
            index++;
            var xlCell = worksheet.Cell(currentRow, index);
            xlCell.Value = ToSentenceCase(column);
            xlCell.Style.Font.Bold = true;
            xlCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBEBE0");
            xlCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            xlCell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#D4D4D4");
            worksheet.Column(index).AdjustToContents();
        }

        for (int i = 0; i < data.Count; i++)
        {
            var row = data[i];
            for (int j = 0; j < columns.Count; j++)
            {
                var column = columns[j];
                var cellData = GetPropValue(row!, column);
                var xlCell = worksheet.Cell(i + 2, j + 1);
                xlCell.Value = cellData == null ? "" : cellData.ToString();
                if (cellData?.ToString()!.Length < 100)
                {
                    worksheet.Column(j + 1).Width = 20;
                }
            }
        }

        using var stream = new MemoryStream();
        workBook.SaveAs(stream);
        return stream.ToArray();
    }

    public static async Task<byte[]> ExportToPdfAsync(IList<T> data, string baseUrl, Dictionary<string, string>? summary = null)
    {
        var companyInfo = StaticData.CompanyInfo;
        var htmlTable = GenerateHtmlTable(data);
        var stylePath = Path.Combine(baseUrl, "Assets", "Styles", "styles.css");
        var logo = Path.Combine(baseUrl,"Assets","logo.svg");
        var summaryTable = (summary == null) ? "" : GenerateSummaryTable(summary!);
        var name = GetName();
        var htmlHeader = @$" <div class=""header table w100"">
        <div class=""info-section cell"">
            <div class=""table"">
                <div class=""info cell"">
                    {companyInfo?.CompanyName} <br>
                    {companyInfo?.Address} <br>
                    {companyInfo?.PhoneNumber} <br>
                    {companyInfo?.Email} <br>
                </div>
            </div>
        </div>
        <div class=""cell"">
            <img src=""{logo}"" class=""logo cell"">
        </div>
        <div class=""cell"">
            <h2>
                Report Of {name} Table
            </h2>
            <div>
                <span class=""label"">Issued At : </span> <span class=""value"">{DateTime.Now}</span>
            </div>
            <div>
                <span class=""label"">Total Items : </span> <span class=""value"">{data.Count}</span>
            </div>
        </div>
    </div>";
        _ = GetName();
        var htmlContent = @$"
        <html>
            <head>
                 <link rel='stylesheet' href='{stylePath}'>
            </head>
            <body>
                {htmlHeader}
                {summaryTable}
                {htmlTable}
            </body>
        </html>
    ";
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args = new string[]{"--no-sandbox",
        "--disable-web-security"}
        });
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(htmlContent,new NavigationOptions(){
            WaitUntil = new WaitUntilNavigation[] {
                 WaitUntilNavigation.Networkidle2
            }
        });
        return await page.PdfDataAsync();
    }

    private static string GenerateHtmlTable(IList<T> data)
    {
        var columns = GetColumns();
        var sp = new StringBuilder();
        sp.Append(@"
          <table>
                <tr>
    ");
        foreach (var col in columns)
        {
            sp.Append(@"<th style= ""padding:10px"">").Append(ToSentenceCase(col)).AppendLine("</th>");
        }

        sp.AppendLine("</tr>");
        foreach (var row in data)
        {
            sp.AppendLine("<tr>");
            foreach (var col in columns)
            {
                var cellData = GetPropValue(row!, col);
                sp.Append(@"<td style= ""padding:10px""> ").Append(cellData).AppendLine(" </td>");
            }

            sp.AppendLine("</tr>");
        }

        sp.AppendLine("</table>");
        return sp.ToString();
    }
    private static string GenerateSummaryTable(Dictionary<string, string> summary)
    {
        var summaryList = summary.ToList();
        var sp = new StringBuilder();
        sp.Append(@"
          <table>
        <thead>
            <tr>
                <th colspan='4'>
                    Summary
                </th>
            </tr>
        </thead>
        ");
        sp.Append("<tbody>");
        var size = 0;
        while (size < summaryList.Count)
        {
            var cellsList = summaryList.Skip(size).Take(2);
            sp.Append("<tr>");
            foreach (var item in cellsList)
            {
                sp.Append("<td class='light p-5'>");
                sp.Append(item.Key).Append(" :");
                sp.Append("</td>");
                sp.Append("<td class='dark p-5'>");
                sp.Append(item.Value);
                sp.Append("</td>");
            }
            sp.Append("</tr>");
            size += summaryList.Count - size < 2 ? summaryList.Count : 2;
        }
        sp.Append("</tbody>");
        sp.Append("</table>");
        return sp.ToString();
    }

    private static IList<string> GetColumns()
    {
        Type type = typeof(T);
        var properties = type.GetProperties().Where(c => (c.PropertyType.IsPrimitive ||
                                                         c.PropertyType == typeof(string)
                                                         || c.PropertyType == typeof(DateTime) || c.PropertyType == typeof(DateTime?))
                                                         && !c.GetCustomAttributes().Any(c => c.GetType() == typeof(Domain.Attributes.IgnoreExportAttribute)))
                                                         .ToList();
        var columns = properties.ConvertAll(c => c.Name);
        return columns;
    }

    private static string GetName()
    {
        Type type = typeof(T);
        return ToSentenceCase(type.Name.Replace("ExportModel", ""));
    }

    private static object? GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName)?.GetValue(src, null);
    }
    private static string ToSentenceCase(string str)
    {
        return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
    }
}