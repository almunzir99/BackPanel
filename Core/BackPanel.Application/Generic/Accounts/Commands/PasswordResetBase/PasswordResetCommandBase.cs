using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Authentication.Commands.PasswordResetBase
{
    public record PasswordResetCommandBase<TEntity>(int Id, PasswordRecoveryRequest Request) : IRequest<bool>
        where TEntity : UserEntityBase;
}