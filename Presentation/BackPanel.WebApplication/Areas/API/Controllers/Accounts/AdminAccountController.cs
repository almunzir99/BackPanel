using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Accounts
{
    [Authorize]
    [ApiController]
    [Route("api/admin-accounts")]
    public class AdminAccountController : UserAccountControllerBase<AppUserDto, AppUserDtoRequest>
    {
        private readonly IUserService _userService;
        protected override IUserService UserService => _userService;

        public AdminAccountController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
