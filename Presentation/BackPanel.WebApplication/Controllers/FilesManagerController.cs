using System.ComponentModel.DataAnnotations;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.FilesManager.Interfaces;
using BackPanel.FilesManager.Models;
using BackPanel.WebApplication.implementation;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Controllers;

[ApiController]
[Route("api/files-manager")]
public class FilesManagerController : ControllerBase
{
    private readonly IFilesManagerService _fileService;

    public FilesManagerController(IFilesManagerService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFiles(IList<IFormFile> files,
        [FromQuery] string? path = "")
    {

        List<IWebFormFile> webFiles =
            new(files.Select(file => new WebFormFile(file, file.FileName)).ToList());
        var result = await _fileService.UploadMultiFiles(path ?? "", webFiles);
        return Ok(new Response<IList<FileModel>>(data: result));

    }

    [HttpPost("directories")]
    public IActionResult CreateDirectory([Required][FromQuery] string directoryName,
        [FromQuery] string? path = "")
    {

        _fileService.CreateDirectory(path: path ?? "", directoryName: directoryName);
        return Ok(new Response<string>(message: "Directory created Successfully"));
    }

    [HttpGet("directories")]
    public IActionResult GetDirectories([FromQuery] string? path = "")
    {

        var result = _fileService.GetAllDirectories(path ?? "");
        return Ok(new Response<IList<DirectoryModel>>(data: result));
    }

    [HttpGet("directories/single")]
    public IActionResult GetDirectory([FromQuery] string? path = "")
    {

        var result = _fileService.GetDirectory(path ?? "");
        return Ok(new Response<DirectoryModel>(data: result));
    }

    [HttpGet("files")]
    public IActionResult GetFiles([FromQuery] string? path = "")
    {

        var result = _fileService.GetAllFiles(path ?? "");
        return Ok(new Response<IList<FileModel>>(data: result));
    }

    [HttpDelete("files")]
    public IActionResult DeleteFile([Required][FromQuery] string fileName,
        [FromQuery] string? path = "")
    {

        _fileService.DeleteFile(path ?? "", fileName);
        return Ok(new Response<string>(message: "File Deleted Successfully"));
    }

    [HttpDelete("directories")]
    public IActionResult DeleteDirectory([Required][FromQuery] string directoryName,
        [FromQuery] string? path = "")
    {

        _fileService.DeleteDirectory(path ?? "", directoryName);
        return Ok(new Response<string>(message: "Directory Deleted Successfully"));
    }

    [HttpGet("directories/move")]
    public IActionResult MoveDirectory([FromQuery] string? oldPath, [FromQuery] string? newPath,
        [Required][FromQuery] string directoryName)
    {

        _fileService.MoveDirectory(oldPath ?? "", newPath ?? "", directoryName);
        return Ok(new Response<string>(message: "Directory Deleted Successfully"));
    }

    [HttpGet("directories/rename")]
    public IActionResult RenameDirectory([FromQuery] string? path, [Required][FromQuery] string oldName,
        [Required][FromQuery] string newName)
    {

        _fileService.RenameDirectory(path ?? "", oldName, newDirName: newName);
        return Ok(new Response<string>(message: "Directory Deleted Successfully"));
    }

    [HttpGet("files/move")]
    public IActionResult MoveFile([FromQuery] string? oldPath, [FromQuery] string? newPath,
        [Required][FromQuery] string fileName)
    {

        _fileService.MoveFile(oldPath ?? "", newPath ?? "", fileName);
        return Ok(new Response<string>(message: "file moved Successfully"));
    }

    [HttpGet("files/rename")]
    public IActionResult RenameFile([FromQuery] string? path, [Required][FromQuery] string oldName,
        [Required][FromQuery] string newName)
    {

        _fileService.RenameFile(path ?? "", oldName, newName);
        return Ok(new Response<string>(message: "file renamed Successfully"));
    }
}