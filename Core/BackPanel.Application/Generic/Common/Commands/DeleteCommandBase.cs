using DocumentFormat.OpenXml.Office2010.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Common.Commands
{
    public record DeleteCommandBase<TEntity>(int Id) : IRequest;

}
