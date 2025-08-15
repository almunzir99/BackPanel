using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Accounts
{
    [Route("api/admin-accounts")]
    public class AdminAccountController : AccountBaseController<Admin, AdminDto, AdminDtoRequest>
    {
        public override string PermissionTitle => "AdminAccountsPermissions";
        protected override string Type => "ADMIN";
        public AdminAccountController(IUriResolver uriService, IMediator mediator) : base(uriService, mediator)
        {
        }
    }
}
