using System.ComponentModel.DataAnnotations;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.TranslationEditor.Interfaces;
using BackPanel.TranslationEditor.Model;
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

    [HttpGet("tree")]
    public async Task<IActionResult> GetTranslationTree()
    {
        try
        {
            var result = await _service.GetTranslationTree();
            var response = new Response<JObject>(data: result, message: "Data Retrieved successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [HttpPost("languages/new")]
    public async Task<IActionResult> CreateLanguage([Required] [FromQuery] string code)
    {
        try
        {
            await _service.CreateLanguage(code);
            var response = new Response<String>(data: null, message: "Language File Created successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }
    [HttpGet("languages/{code}")]
    public async Task<IActionResult> GetLanguage(string code)
    {
        try
        {
            var result = await _service.GetLanguage(code);
            var response = new Response<JObject>(data: result, message: "Language File Retrieved successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [HttpPost("parents/new")]
    public async Task<IActionResult> CreateParentNode([Required] [FromQuery] string title)
    {
        try
        {
            await _service.CreateParentNode(title);
            var response = new Response<String>(data: null, message: "Parent Node Created successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [HttpPost("nodes/new")]
    public async Task<IActionResult> CreateNode([FromBody] NodeBody body)
    {
        try
        {
            await _service.CreateNode(body);
            var response = new Response<String>(data: null, message: "Node Created successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [HttpPut("parents/update")]
    public async Task<IActionResult> UpdateParentNode([Required] [FromQuery] string oldTitle,
        [Required] [FromQuery] string newTitle)
    {
        try
        {
            await _service.UpdateParentNode(oldTitle, newTitle);
            var response = new Response<String>(data: null, message: "Parent Node Updated successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [HttpPut("nodes/update")]
    public async Task<IActionResult> UpdateNode([FromBody] NodeBody body)
    {
        try
        {
            await _service.CreateNode(body);
            var response = new Response<String>(data: null, message: "Node Updated successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }
    [HttpDelete("languages/{code}")]
    public  IActionResult DeleteLanguage(string code)
    {
        try
        {
             _service.DeleteLanguage(code);
            var response = new Response<String>(data: null, message: "Language  deleted successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }
    [HttpDelete("parents/delete")]
    public async Task<IActionResult> DeleteParentNode([Required] [FromQuery] string title)
    {
        try
        {
            await _service.DeleteParentNode(title);
            var response = new Response<String>(data: null, message: "Parent Node deleted successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [HttpDelete("nodes/delete")]
    public async Task<IActionResult> DeleteNode([Required] [FromQuery] string parent,[Required] [FromQuery] string title)
    {
        try
        {
            await _service.DeleteNode(parent,title);
            var response = new Response<String>(data: null, message: "Node deleted successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response =
                new Response<String>(success: false, message: "operation failed", errors: new[] { e.Message });
            return BadRequest(response);
        }
    }
}