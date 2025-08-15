using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class PasswordRecoveryRequestAdminCommandHandler : PasswordRecoveryRequestCommandHandlerBase<Admin, PasswordRecoveryRequestCommandBase<Admin>>
    {
        public PasswordRecoveryRequestAdminCommandHandler(IRepositoryBase<Admin> repository) : base(repository)
        {
        }
    }
}
