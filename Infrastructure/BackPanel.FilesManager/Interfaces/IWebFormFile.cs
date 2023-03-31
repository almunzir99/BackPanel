namespace BackPanel.FilesManager.Interfaces;

public interface IWebFormFile
{
     string FileName { get; set; }
     Task CopyToAsync(Stream stream, CancellationToken cancellationToken = default);
}