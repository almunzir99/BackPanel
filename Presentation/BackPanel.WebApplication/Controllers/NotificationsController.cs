using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs;
using BackPanel.Application.Features.Notifications.BroadcastNotification;
using BackPanel.Application.Features.Notifications.ClearNotifications;
using BackPanel.Application.Features.Notifications.DeleteNotification;
using BackPanel.Application.Features.Notifications.GetUnreadNotifications;
using BackPanel.Application.Features.Notifications.ListNotifications;
using BackPanel.Application.Features.Notifications.MarkNotificationsAsRead;
using BackPanel.Application.Features.Notifications.PushNotification;
using BackPanel.Application.Features.Notifications.ReadNotification;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackPanel.Application.Extensions;

namespace BackPanel.WebApplication.Controllers
{
    [Authorize()]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("unread")]
        public async Task<ActionResult<IList<NotificationDto>>> GetUnreadNotifications(
            int userId,
            [FromQuery] string userType,
            [FromQuery] bool autoRead = false)
        {
            var notifications = await _mediator.Send(new GetUnreadNotificationsQuery(userId, userType));

            if (autoRead && notifications.Any())
            {
                await _mediator.Send(new MarkNotificationsAsReadCommand(userId, userType));
            }

            return Ok(notifications);
        }

        [HttpGet()]
        public async Task<ActionResult<IList<NotificationDto>>> GetNotifications(
            int userId,
            [FromQuery] string userType,
            [FromQuery] PaginationFilter filter
         )
        {
            var notifications = await _mediator.Send(new ListNotificationsQuery(userId, userType, filter));

            return Ok(notifications);
        }


        [HttpPut("{id}/read")]
        public async Task<ActionResult<NotificationDto>> ReadNotification(int id)
        {
            var notification = await _mediator.Send(new ReadNotificationCommand(id));
            return Ok(notification);
        }

        [HttpPut("read-all")]
        public async Task<ActionResult> MarkAllAsRead(
            int userId,
            [FromQuery] string userType)
        {
            await _mediator.Send(new MarkNotificationsAsReadCommand(CurrentUserId, userType));
            return Ok(new { Message = "All notifications marked as read" });
        }


        [HttpDelete("clear")]
        public async Task<ActionResult> ClearNotifications(
            [FromQuery] string userType)
        {
            await _mediator.Send(new ClearNotificationsCommand(CurrentUserId, userType));
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
            get
            {
                int currentUserId = int.Parse(HttpContext.User.GetClaimValue("id"));
                return currentUserId;
            }
        }

        protected string CurrentUserType
        {
            get
            {
                string type = HttpContext.User.GetClaimValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                return type;
            }
        }
    }

}
