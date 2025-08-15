using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class PasswordResetAdminCommandHandler : PasswordResetCommandHandlerBase<Admin, PasswordResetCommandBase<Admin>>
    {
        public PasswordResetAdminCommandHandler(IRepositoryBase<Admin> repository) : base(repository)
        {
        }
    }
}
