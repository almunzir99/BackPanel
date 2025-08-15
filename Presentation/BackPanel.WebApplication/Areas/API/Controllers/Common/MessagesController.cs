using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Features.Notifications.Commands;
using BackPanel.Application.Interfaces;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[ApiController]
[Route("api/messages")]
public class MessagesController : ApiController<Message, MessageDto, MessageDtoRequest>
{
    public MessagesController(IUriResolver uriService, IMediator mediator) : base(uriService, mediator)
    {
    }

    public override string PermissionTitle => "MessagesPermissions";

    [Permission(false, PermissionTypes.CREATE)]
    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> PostAsync([FromBody] MessageDtoRequest body)
    {
        // Push Notifications
        var notification = new NotificationDto()
        {
            Title = "New Message",
            Message = "There is a new Message submitted, please check messages page",
            Module = "MESSAGES",
            Action = "CREATE",
            Url = "/dashboard/messages",
            CreatedAt = DateTime.Now,
            LastUpdate = DateTime.Now,
        };
        await Mediator.Send(new BroadcastNotificationCommand(notification, "admin"));
        return await base.PostAsync(body);
    }
}