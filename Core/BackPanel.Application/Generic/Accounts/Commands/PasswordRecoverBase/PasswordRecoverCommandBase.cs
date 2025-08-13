using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Authentication.Commands.PasswordRecoverBase
{
    public record PasswordRecoverCommandBase<TEntity>(PasswordRecoveryRequest Request) : IRequest<bool>
        where TEntity : UserEntityBase;
}
