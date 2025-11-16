using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;
using BackPanel.SMTP.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class PasswordRecoveryRequestAdminCommandHandler : PasswordRecoveryRequestCommandHandlerBase<Admin, PasswordRecoveryRequestCommandBase<Admin>>
    {
        public PasswordRecoveryRequestAdminCommandHandler(IRepositoryBase<Admin> repository, IMemoryCache memoryCache, IPathProvider pathProvider, IConfiguration configuration, ISmtpService smtpService) : base(repository, memoryCache, pathProvider, configuration, smtpService)
        {
        }
    }
}
