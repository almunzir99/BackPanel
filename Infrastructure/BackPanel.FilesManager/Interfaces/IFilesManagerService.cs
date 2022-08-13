using BackPanel.FilesManager.Models;

namespace BackPanel.FilesManager.Interfaces;

public interface IFilesManagerService
{
    void CreateDirectory(string path, string directoryName);
    Task<IList<FileModel>> UploadMultiFiles(string path, IList<IWebFormFile> files);
    Task<FileModel> UploadSingleFile(string path, IWebFormFile file);
    IList<DirectoryModel> GetAllDirectories(string path);
    DirectoryModel GetDirectory(string path);

    IList<FileModel> GetAllFiles(string path);
    void DeleteFile(string path, string fileName);
    void DeleteDirectory(string path, string directoryName);
    void RenameDirectory(string path,string dirName,string newDirName);
    void RenameFile(string path,string fileName,string newFileName );
    void MoveDirectory(string oldPath, string newPath, string directoryName);
    void MoveFile(string oldPath, string newPath, string fileName);
    public bool FileExists(string path);
}