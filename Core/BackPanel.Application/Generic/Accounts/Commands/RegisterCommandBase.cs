using BackPanel.Application.DTOs;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Accounts.Commands
{
    public record RegisterCommandBase<TDTORequest, TDTO>(TDTORequest Model) : IRequest<TDTO>
       where TDTO : UserDtoBase;
}
