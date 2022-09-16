using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ApiController<Message, MessageDto,MessageDtoRequest, IMessageService>
{
    private readonly IRolesService _roleService;
    private readonly INotificationService _notificationService;

    public MessagesController(IMessageService service, IUriService uriSerivce, IRolesService roleService, INotificationService notificationService) : base(service, uriSerivce)
    {
        _roleService = roleService;
        _notificationService = notificationService;
    }

    public override string PermissionTitle => "MessagesPermissions";

    // [Permission(false, PermissionTypes.CREATE)]
    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> PostAsync([FromBody] MessageDtoRequest body){
        // Push Notifications
        var notification = new NotificationDto(){
            Title = "New Message",
            Message ="There is a new Message submitted, please check messages page",
            Module = "MESSAGES",
            Action = "CREATE",
            Url = "/dashboard/messages",
            CreatedAt = DateTime.Now,
            LastUpdate = DateTime.Now,
        };
        await _notificationService.BroadCastNotification(notification,"admin");
        return await base.PostAsync(body);
    }
        
}