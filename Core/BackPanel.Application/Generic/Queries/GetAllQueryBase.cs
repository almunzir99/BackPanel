using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.Extensions;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Queries
{
    public class GetAllQueryBase<TDTO>(ListFilter Filter) : IRequest<IList<TDTO>>
    where TDTO : DtoBase
    {
    }
    public class GetAllQueryHandlerBase<TEntity, TDTO, TQuery> : IRequestHandler<TQuery, IList<TDTO>>
    where TEntity : EntityBase
    where TDTO : DtoBase
    where TQuery : GetAllQueryBase<TDTO>
    {
        private readonly IRepositoryBase<TEntity> _repository;
        private readonly IMapper _mapper;
        public GetAllQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public virtual async Task<IList<TDTO>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var list = _repository.List();
            var query = list.Select(c => _mapper.Map<TDTO>(c));
            var result = await query.ToListAsync();
            var total = result.Count;
            // // Apply Order
            result = result.OrderByProperty(orderBy, ascending).ToList();
            // Apply search Expressions
            if (expressions != null)
            {
                foreach (var expression in expressions)
                {
                    var lambda = ExpressionBuilder.BuildComparisonExpression<TDto>(expression.PropName!, expression.Operator, expression.PropValue!);
                    result = result.Where(lambda.Compile()).ToList();
                }
            }
            // apply unified search Expressions
            var expr = GetUnifiedSearchExpression(search);
            var func = expr?.Compile();
            result = result.Where(func ?? (c => false)).ToList();
            // Apply Pagination
            result = result
                .Skip((validFilter.PageIndex - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();
            return (result, total);
        }
    }
    }
}
