using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackPanel.Application.Extensions;
using BackPanel.Application.Features.Notifications.Queries;
using BackPanel.Application.Features.Notifications.Commands;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common
{
    [Authorize()]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("unread")]
        public async Task<ActionResult<IList<NotificationDto>>> GetUnreadNotifications(
            [FromQuery] bool autoRead = false)
        {
            var notifications = await _mediator.Send(new GetUnreadNotificationsQuery(CurrentUserId));

            if (autoRead && notifications.Any())
                await _mediator.Send(new MarkNotificationsAsReadCommand(CurrentUserId));

            return Ok(notifications);
        }

        [HttpGet]
        public async Task<ActionResult<IList<NotificationDto>>> GetNotifications(
            [FromQuery] PaginationFilter filter)
        {
            var notifications = await _mediator.Send(new ListNotificationsQuery(CurrentUserId, filter));
            return Ok(notifications);
        }

        [HttpPut("{id}/read")]
        public async Task<ActionResult<NotificationDto>> ReadNotification(int id)
        {
            var notification = await _mediator.Send(new ReadNotificationCommand(id));
            return Ok(notification);
        }

        [HttpPut("read-all")]
        public async Task<ActionResult> MarkAllAsRead()
        {
            await _mediator.Send(new MarkNotificationsAsReadCommand(CurrentUserId));
            return Ok(new { Message = "All notifications marked as read" });
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearNotifications()
        {
            await _mediator.Send(new ClearNotificationsCommand(CurrentUserId));
            return Ok(new { Message = "All notifications cleared" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotification(int id)
        {
            await _mediator.Send(new DeleteNotificationCommand(id));
            return Ok(new { Message = "Notification deleted successfully" });
        }

        protected int CurrentUserId
        {
            get => int.Parse(HttpContext.User.GetClaimValue("id"));
        }
    }
}
