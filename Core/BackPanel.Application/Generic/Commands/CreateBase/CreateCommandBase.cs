using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Commands.CreateBase
{
    public record CreateCommandBase<TDTORequest, TDTO>(TDTORequest Request) : IRequest<TDTO>;
}
