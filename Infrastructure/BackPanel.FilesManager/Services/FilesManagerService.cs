using BackPanel.FilesManager.Interfaces;
using BackPanel.FilesManager.Models;

namespace BackPanel.FilesManager.Services;

public class FilesManagerService : IFilesManagerService
{
    private readonly IPathProvider _pathProvider;
    private readonly string _rootPath;

    public FilesManagerService(IPathProvider pathProvider)
    {
        _pathProvider = pathProvider;
        _rootPath = Path.Combine(pathProvider.GetRootPath(), "BackPanel", "Files-Manager");
    }

    public void CreateDirectory(string path, string directoryName)
    {
        var combinedPath = Path.Combine(_rootPath, path, directoryName);
        if (!Directory.Exists(combinedPath))
            Directory.CreateDirectory(combinedPath);
    }

    public void DeleteDirectory(string path, string directoryName)
    {
        var combinedPath = Path.Combine(_rootPath, path, directoryName);
        if (Directory.Exists(combinedPath))
        {
            var files = Directory.GetFiles(combinedPath);
            var directories = Directory.GetDirectories(combinedPath);

            if (files.Length != 0)
                foreach (var file in files)
                {
                    this.DeleteFile(combinedPath, Path.GetFileName(file));
                }

            if (directories.Length != 0)
                foreach (var directory in directories)
                {
                    this.DeleteDirectory(combinedPath, Path.GetFileName(directory));
                }

            Directory.Delete(combinedPath);
        }
        else
            throw new DirectoryNotFoundException($"Directory {combinedPath} is Not Found");
    }

    public bool FileExists(string path)
    {
        var combinedPath = Path.Combine(_rootPath, path);
        return File.Exists(combinedPath);
    }

    public void DeleteFile(string path, string fileName)
    {
        var combinedPath = Path.Combine(_rootPath, path, fileName);
        if (File.Exists(combinedPath))
        {
            File.Delete(combinedPath);
        }
        else
            throw new FileNotFoundException($"File {combinedPath} is Not Found");
    }

    public IList<DirectoryModel> GetAllDirectories(string path)
    {
        var combinedPath = Path.Combine(_rootPath,
            path);
        if (!Directory.Exists(combinedPath))
            throw new DirectoryNotFoundException($"directory {path} is not found ");
        return Directory.GetDirectories(combinedPath)
            .Select(c => this.GetDirectoryModel(combinedPath, c)).ToArray();
    }

    public IList<FileModel> GetAllFiles(string path)
    {
        var combinedPath = Path.Combine(_rootPath, path);
        var relativePath = Path.GetRelativePath(_pathProvider.GetRootPath(), combinedPath);
        return Directory.GetFiles(combinedPath).Select(file =>
        {
            var fileModel = GetFileModel(relativePath, Path.GetFileName(file));
            return fileModel;
        }).ToArray();
    }

    public async Task<IList<FileModel>> UploadMultiFiles(string path, IList<IWebFormFile> files)
    {
        var listFiles = new List<FileModel>();
        foreach (var file in files)
        {
            var res = await UploadSingleFile(path, file);
            listFiles.Add(res);
        }

        return listFiles;
    }

    public async Task<FileModel> UploadSingleFile(string path, IWebFormFile file)
    {
        var fileName = System.Guid.NewGuid();
        var fileExtension = Path.GetExtension(file.FileName);
        var fileDirCombinedPath = Path.Combine(_rootPath,path);
        var combinedPath = Path.Combine(fileDirCombinedPath,
            $"{fileName}{fileExtension}");
        await using (var stream = new FileStream(combinedPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return GetFileModel(
            Path.GetRelativePath(_pathProvider.GetRootPath(), fileDirCombinedPath),$"{fileName}{fileExtension}");
    }

    private Uri GetFileUri(string fileName, string path = "")
    {
        path = (path.Equals("")) ? path : string.Concat("/", path);
        path = path.Replace("wwwroot", "");
        if (path != "/")
            path = string.Concat(path, "/");
        var uri = new Uri(string.Concat(_pathProvider.GetBaseUrl(), path, fileName));
        return uri;
    }

    private FileModel GetFileModel(string path, string fileName)
    {
        var compinedPath = Path.Combine(_pathProvider.GetRootPath(), path, fileName);
        var info = new FileInfo(compinedPath);
        var uri = this.GetFileUri(fileName, path);
        var filePath = Path.Combine(path, fileName).Replace("\\","/");
        var fileModel = new FileModel(
            title: fileName,
            uri: uri.ToString(),
            parentDirectory: path,
            contentType: Path.GetExtension(fileName),
            size: info.Length,
            path: filePath,
            createdAt: info.CreationTime,
            info.LastWriteTime);
        return fileModel;
    }

    private DirectoryModel GetDirectoryModel(string path, string name)
    {
        var combinedPath = Path.Combine(path, name);
        var directoryInfo = new DirectoryInfo(combinedPath);
        IList<FileModel> files = Directory
            .GetFiles(combinedPath)
            .Select(file =>
                this.GetFileModel($"{Path.GetRelativePath(_pathProvider.GetRootPath(), combinedPath)}", Path.GetFileName(file)))
            .ToList();
        IList<DirectoryModel> directories = Directory
            .GetDirectories(combinedPath)
            .Select(dir => this.GetDirectoryModel(combinedPath, dir))
            .ToList();
        return new DirectoryModel(
            title: Path.GetFileName(name),
            size: 2,
            parentDirectory: Path.GetFileName(path),
            files: files,
            directories: directories,
            createdAt: directoryInfo.CreationTime,
            lastUpdate: directoryInfo.LastWriteTime
        );
    }

    public DirectoryModel GetDirectory(string path)
    {
        var combinedPath = Path.Combine(_rootPath,
            path);
        if (!Directory.Exists(combinedPath))
            throw new DirectoryNotFoundException($"directory {path} is not found ");
        var fileName = Path.GetFileName(combinedPath);
        combinedPath = combinedPath.Replace(fileName, "");
        return GetDirectoryModel(combinedPath, fileName);
    }

    public void RenameDirectory(string path, string dirName, string newDirName)
    {
        var combinedPath = Path.Combine(_rootPath,
            path);
        if (!Directory.Exists(combinedPath))
            throw new DirectoryNotFoundException($"directory {path} is not found ");
        var oldPath = Path.Combine(combinedPath, dirName);
        var newPath = Path.Combine(combinedPath, newDirName);
        Directory.Move(oldPath, newPath);
    }

    public void RenameFile(string path, string fileName, string newFileName)
    {
        var combinedPath = Path.Combine(_rootPath,
            path);
        if (!Directory.Exists(combinedPath))
            throw new FileNotFoundException($"directory {path} is not found ");
        var oldPath = Path.Combine(combinedPath, fileName);
        var newPath = Path.Combine(combinedPath, newFileName);
        File.Move(oldPath, newPath);
    }

    public void MoveDirectory(string oldPath, string newPath, string directoryName)
    {
        var combinedOldPath = Path.Combine(_rootPath,
            oldPath, directoryName);
        var combinedNewPath = Path.Combine(_rootPath,
            newPath, directoryName);
        if (!Directory.Exists(combinedOldPath))
            throw new DirectoryNotFoundException($"directory {combinedOldPath} is not found ");
        if (!Directory.Exists(combinedOldPath))
            throw new DirectoryNotFoundException($"directory {combinedNewPath} is not found ");
        Directory.Move(combinedOldPath, combinedNewPath);
    }

    public void MoveFile(string oldPath, string newPath, string fileName)
    {
        var combinedOldPath = Path.Combine(_rootPath,
            oldPath, fileName);
        var combinedNewPath = Path.Combine(_rootPath,
            newPath, fileName);
        if (!File.Exists(combinedOldPath))
            throw new DirectoryNotFoundException($"directory {combinedOldPath} is not found ");
        File.Move(combinedOldPath, combinedNewPath);
    }
}