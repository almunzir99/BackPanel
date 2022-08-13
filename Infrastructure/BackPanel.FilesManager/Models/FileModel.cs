namespace BackPanel.FilesManager.Models;

public class FileModel
{
    public string Title { get; set; }
    public string Uri { get; set; }
    public string ParentDirectory { get; set; }
    public string ContentType { get; set; }
    public double Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
    public string Path { get; set; }
    public FileModel(string title,
        string uri,
        string parentDirectory,
        string contentType,
        double size,
        string path,
        DateTime createdAt,
        DateTime lastUpdate)
    {
        Title = title;
        Uri = uri;
        ParentDirectory = parentDirectory;
        ContentType = contentType;
        Size = size;
        CreatedAt = createdAt;
        LastUpdate = lastUpdate;
        Path = path;
    }
}