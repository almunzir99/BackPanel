using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Authentication.Commands.AuthenticateBase
{
    public record AuthenticateCommandBase<TEntity, TDTO>(AuthenticationModel Model) : IRequest<TDTO>
        where TEntity : UserEntityBase
        where TDTO : UserDtoBase;
}
