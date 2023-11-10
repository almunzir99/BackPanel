using System.ComponentModel.DataAnnotations;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.TranslationEditor.Interfaces;
using BackPanel.TranslationEditor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BackPanel.WebApplication.Controllers;

[Route("api/translations")]
public class TranslationsEditorController : ControllerBase
{
    private readonly ITranslationEditorService _service;

    public TranslationsEditorController(ITranslationEditorService service)
    {
        _service = service;
    }

    [HttpGet("languages")]
    public IActionResult GetLanguages()
    {

        var result = _service.GetLanguagesList();
        var response = new Response<IList<string>>(data: result, message: "Data Retrieved successfully");
        return Ok(response);
    }
    [HttpGet("tree")]
    public async Task<IActionResult> GetTranslationTree()
    {

        var result = await _service.GetTranslationTree();
        var response = new Response<JObject>(data: result, message: "Data Retrieved successfully");
        return Ok(response);
    }

    [HttpPost("languages/new")]
    public async Task<IActionResult> CreateLanguage([Required][FromQuery] string code)
    {

        await _service.CreateLanguage(code);
        var response = new Response<String>(data: null, message: "Language File Created successfully");
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpGet("languages/{code}")]
    public async Task<IActionResult> GetLanguage(string code)
    {

        var result = await _service.GetLanguage(code);
        var response = new Response<JObject>(data: result, message: "Language File Retrieved successfully");
        return Ok(response);
    }
    [HttpGet("languages-files/{code}")]
    public async Task<IActionResult> GetLanguageFiles(string code)
    {

        var result = await _service.GetLanguage(code);
        return Ok(result);
    }

    [HttpPost("parents/new")]
    public async Task<IActionResult> CreateParentNode([Required][FromQuery] string title)
    {

        await _service.CreateParentNode(title);
        var response = new Response<string>(data: null, message: "Parent Node Created successfully");
        return Ok(response);
    }

    [HttpPost("nodes/new")]
    public async Task<IActionResult> CreateNode([FromBody] NodeBody body)
    {

        await _service.CreateNode(body);
        var response = new Response<String>(data: null, message: "Node Created successfully");
        return Ok(response);
    }

    [HttpPut("parents/update")]
    public async Task<IActionResult> UpdateParentNode([Required][FromQuery] string oldTitle,
        [Required][FromQuery] string newTitle)
    {

        await _service.UpdateParentNode(oldTitle, newTitle);
        var response = new Response<String>(data: null, message: "Parent Node Updated successfully");
        return Ok(response);
    }

    [HttpPut("nodes/update")]
    public async Task<IActionResult> UpdateNode([FromBody] NodeBody body)
    {

        await _service.CreateNode(body);
        var response = new Response<String>(data: null, message: "Node Updated successfully");
        return Ok(response);
    }
    [HttpDelete("langauges/delete/{code}")]
    public IActionResult DeleteLanguage(string code)
    {

        _service.DeleteLanguage(code);
        var response = new Response<String>(data: null, message: "Language  deleted successfully");
        return Ok(response);
    }
    [HttpDelete("parents/delete")]
    public async Task<IActionResult> DeleteParentNode([Required][FromQuery] string title)
    {

        await _service.DeleteParentNode(title);
        var response = new Response<String>(data: null, message: "Parent Node deleted successfully");
        return Ok(response);
    }

    [HttpDelete("nodes/delete")]
    public async Task<IActionResult> DeleteNode([Required][FromQuery] string parent, [Required][FromQuery] string title)
    {

        await _service.DeleteNode(parent, title);
        var response = new Response<String>(data: null, message: "Node deleted successfully");
        return Ok(response);
    }
}