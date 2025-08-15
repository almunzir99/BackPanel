using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Common.Queries
{
    public class GetAllQueryBase<TDTO>(ListFilter Filter) : IRequest<Tuple<List<TDTO>, int>>
    where TDTO : DtoBase
    {
        public ListFilter Filter { get; } = Filter;
    }

}
