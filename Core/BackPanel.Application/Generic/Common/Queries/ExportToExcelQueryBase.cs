using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Common.Queries
{
    public record ExportToExcelQueryBase<TEntity> : IRequest<byte[]>
        where TEntity : class;
}
