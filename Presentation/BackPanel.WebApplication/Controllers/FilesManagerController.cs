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
        try
        {
            List<IWebFormFile> webFiles =
                new(files.Select(file => new WebFormFile(file, file.FileName)).ToList());
            var result = await _fileService.UploadMultiFiles(path ?? "", webFiles);
            return Ok(new Response<IList<FileModel>>(data: result));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new[] { e.Message }));
        }
    }

    [HttpPost("directories")]
    public IActionResult CreateDirectory([Required] [FromQuery] string directoryName,
        [FromQuery] string? path = "")
    {
        try
        {
            _fileService.CreateDirectory(path: path ?? "", directoryName: directoryName);
            return Ok(new Response<string>(message: "Directory created Successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpGet("directories")]
    public IActionResult GetDirectories([FromQuery] string? path = "")
    {
        try
        {
            var result = _fileService.GetAllDirectories(path ?? "");
            return Ok(new Response<IList<DirectoryModel>>(data: result));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpGet("directories/single")]
    public IActionResult GetDirectory([FromQuery] string? path = "")
    {
        try
        {
            var result = _fileService.GetDirectory(path ?? "");
            return Ok(new Response<DirectoryModel>(data: result));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpGet("files")]
    public IActionResult GetFiles([FromQuery] string? path = "")
    {
        try
        {
            var result = _fileService.GetAllFiles(path ??  "");
            return Ok(new Response<IList<FileModel>>(data: result));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpDelete("files")]
    public IActionResult DeleteFile([Required] [FromQuery] string fileName,
        [FromQuery] string? path = "")
    {
        try
        {
            _fileService.DeleteFile(path ?? "", fileName);
            return Ok(new Response<string>(message: "File Deleted Successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpDelete("directories")]
    public IActionResult DeleteDirectory([Required] [FromQuery] string directoryName,
        [FromQuery] string? path = "")
    {
        try
        {
            _fileService.DeleteDirectory(path ?? "", directoryName);
            return Ok(new Response<string>(message: "Directory Deleted Successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpGet("directories/move")]
    public IActionResult MoveDirectory([FromQuery] string? oldPath, [FromQuery] string? newPath,
        [Required] [FromQuery] string directoryName)
    {
        try
        {
            _fileService.MoveDirectory(oldPath ?? "", newPath ?? "", directoryName);
            return Ok(new Response<string>(message: "Directory Deleted Successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpGet("directories/rename")]
    public IActionResult RenameDirectory([FromQuery] string? path, [Required] [FromQuery] string oldName,
        [Required] [FromQuery] string newName)
    {
        try
        {
            _fileService.RenameDirectory(path ??  "", oldName, newDirName: newName);
            return Ok(new Response<string>(message: "Directory Deleted Successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpGet("files/move")]
    public IActionResult MoveFile([FromQuery] string? oldPath,[FromQuery] string? newPath,
        [Required] [FromQuery] string fileName)
    {
        try
        {
            _fileService.MoveFile(oldPath ?? "", newPath ?? "", fileName);
            return Ok(new Response<string>(message: "file moved Successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }

    [HttpGet("files/rename")]
    public IActionResult RenameFile( [FromQuery] string? path, [Required] [FromQuery] string oldName,
        [Required] [FromQuery] string newName)
    {
        try
        {
            _fileService.RenameFile(path ??  "", oldName, newName);
            return Ok(new Response<string>(message: "file renamed Successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string[]>(success: false,
                errors: new [] { e.Message }));
        }
    }
}