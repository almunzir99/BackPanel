using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands
{
    public record RecoverPasswordCommandBase<TEntity>(int userId, String newPassword) : IRequest
           where TEntity : UserEntityBase;
}
