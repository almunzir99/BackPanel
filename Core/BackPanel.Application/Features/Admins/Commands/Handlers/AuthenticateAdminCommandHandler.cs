using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class AuthenticateAdminCommandHandler : AuthenticateCommandHandlerBase<Admin, AdminDto>
    {
        public AuthenticateAdminCommandHandler(IRepositoryBase<Admin> repository, IMapper mapper, IWebConfiguration webConfiguration) : base(repository, mapper, webConfiguration)
        {
        }

        protected override string UserType => "ADMIN";
    }
}
