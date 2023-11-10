using System.ComponentModel.DataAnnotations;
using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.WebApplication.implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Controllers;

public abstract class
    UserBaseController<TEntity, TDto, TDtoRequest, TService> : ApiController<TEntity, TDto, TDtoRequest, TService>
    where TEntity : UserEntityBase
    where TDto : UserDtoBase
    where TDtoRequest : UserBaseDtoRequest
    where TService : IUserBaseService<TEntity, TDto, TDtoRequest>
{
    public abstract override string PermissionTitle { get; }
    protected abstract string Type { get; }
    private readonly INotificationService _notificationService;

    protected UserBaseController(TService service, IUriService uriService, INotificationService notificationService) :
        base(service, uriService)
    {
        _notificationService = notificationService;
    }

    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public virtual async Task<IActionResult> Authenticate(AuthenticationModel model)
    {

        var user = await Service.Authenticate(model);
        var response = new Response<TDto>(data: user, message: "you logged in successfully");
        return Ok(response);

    }
    [Permission(true, PermissionTypes.CREATE)]
    [HttpPost("Register")]
    public virtual async Task<IActionResult> Register(TDtoRequest body)
    {

        var user = await Service.Register(body);
        var response = new Response<TDto>(data: user, message: "account created successfully");
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("password/recovery/request")]
    public virtual async Task<IActionResult> PasswordRecoveryRequest([Required][FromQuery] string email)
    {

        await Service.PasswordRecoveryRequest(email);
        var response = new Response<TDto>(success: true, message: "request send successfully");
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("password/recovery")]
    public virtual async Task<IActionResult> PasswordRecovery(PasswordRecoveryRequest recovery)
    {

        if (recovery.NewPassword != null)
        {
            if (recovery.Key != null)
                await Service.PasswordRecovery(recovery.Key, recovery.NewPassword);
        }

        var response = new Response<TDto>(success: true, message: "password reset successfully");
        return Ok(response);
    }

    [Authorize]
    [HttpPut("profile/password-reset")]
    public virtual async Task<IActionResult> PasswordReset(PasswordRecoveryRequest recovery)
    {
        var type = GetCurrentUserType();
        if (type != this.Type)
            return StatusCode(403);

        var id = GetCurrentUserId();
        if (recovery.OldPassword != null)
        {
            if (recovery.NewPassword != null)
                await Service.ResetPassword(id, recovery.OldPassword, recovery.NewPassword);
        }

        var response = new Response<TDto>(success: true, message: "password reset successfully");
        return Ok(response);
    }

    [Authorize]
    [HttpPut("profile/change-photo")]
    public virtual async Task<IActionResult> ChangePersonalPhoto(IFormFile file)
    {
        var type = GetCurrentUserType();
        if (type != this.Type)
            return StatusCode(403);

        var id = GetCurrentUserId();
        var webFile = new WebFormFile(file, file.FileName);
        var result = await Service.ChangePersonalPhoto(id, webFile);
        var response = new Response<string>(data: result, success: true,
    message: "personal photo updated successfully");
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("profile/update-info")]
    public virtual async Task<IActionResult> ChangePersonalPhoto(JsonPatchDocument<TEntity> body)
    {
        var type = GetCurrentUserType();
        if (type != this.Type)
            return StatusCode(403);

        var id = GetCurrentUserId();
        var result = await Service.UpdatePersonalInfo(id, body);
        var response = new Response<TDto>(data: result, success: true,
    message: "personal Information updated successfully");
        return Ok(response);
    }

    [Authorize]
    [HttpGet("profile")]
    public virtual async Task<IActionResult> GetCurrentUser()
    {
        var type = GetCurrentUserType();
        if (type != this.Type)
            return StatusCode(403);

        var id = GetCurrentUserId();
        var result = await Service.GetProfileAsync(id);
        var response = new Response<TDto>(data: result, success: true, message: "information fetched successfully");
        return Ok(response);
    }

    [HttpGet("notifications")]
    public virtual async Task<IActionResult> GetNotificationAsync([FromQuery] PaginationFilter? filter)
    {

        var userId = GetCurrentUserId();
        filter ??= new PaginationFilter();
        var result = await _notificationService.ListNotificationsAsync(userId, Type, filter);
        if (Request.Path.Value != null)
        {
            var response = PaginationHelper.CreatePagedResponse(
                result, filter, UriService, 0, Request.Path.Value
            );
            return Ok(response);
        }

        var badResponse = new Response<TDto>(success: false, message: "Registration failed, check errors below",
            errors: new[] { "Request.Path.value  == null" });
        return BadRequest(badResponse);
    }


    [HttpDelete("notifications/clear")]
    public virtual async Task<IActionResult> ClearNotifications()
    {

        var userId = GetCurrentUserId();
        await _notificationService.ClearNotificationAsync(userId, Type);
        var response = new Response<string>(message: "Notifications cleared successfully");
        return Ok(response);
    }

    [HttpDelete("notifications/{notificationId}")]
    public virtual async Task<IActionResult> DeleteNotification(int notificationId)
    {

        await _notificationService.DeleteNotificationAsync(notificationId);
        var response = new Response<string>(message: "Notification deleted successfully");
        return Ok(response);
    }

    [HttpGet("notifications/unread")]
    public virtual async Task<IActionResult> GetUnreadNotifications([FromQuery] bool autoRead = false)
    {

        var userId = GetCurrentUserId();
        var result = await _notificationService.GetUnreadNotification(userId, Type, autoRead);
        var response =
            new Response<IList<NotificationDto>>(data: result, message: "notification fetch successfully");
        return Ok(response);
    }
}