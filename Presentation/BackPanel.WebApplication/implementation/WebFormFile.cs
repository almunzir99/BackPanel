using BackPanel.FilesManager.Interfaces;

namespace BackPanel.WebApplication.implementation;

public class WebFormFile : IWebFormFile
{
    private readonly IFormFile _file;
    public string FileName { get; set; }

    public WebFormFile(IFormFile file, string fileName)
    {
        _file = file;
        FileName = fileName;
    }


    public async Task CopyToAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _file.CopyToAsync(stream, cancellationToken);
    }
}