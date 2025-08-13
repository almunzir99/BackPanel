using BackPanel.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Authentication.Commands.PasswordRecoveryRequestBase
{
    public record PasswordRecoveryRequestCommandBase<TEntity>(string Email) : IRequest<bool>
        where TEntity : UserEntityBase;
}
