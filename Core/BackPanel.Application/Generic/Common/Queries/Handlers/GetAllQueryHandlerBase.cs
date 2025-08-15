using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Extensions;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Application.Generic.Common.Queries.Handlers
{
    public abstract class GetAllQueryHandlerBase<TEntity, TDTO, TQuery> : IRequestHandler<TQuery, Tuple<List<TDTO>, int>>
    where TEntity : EntityBase
    where TDTO : DtoBase
    where TQuery : GetAllQueryBase<TDTO>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public GetAllQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public virtual async Task<Tuple<List<TDTO>, int>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var list = Repository.Query();
            var query = list.Select(c => Mapper.Map<TDTO>(c));
            var result = await query.ToListAsync();
            var total = result.Count;
            // Apply Order
            result = result.OrderByProperty(request.Filter.OrderBy, !request.Filter.Descending).ToList();
            // Apply search Expressions
            foreach (var expression in request.Filter.SearchExpressions)
            {
                var lambda = ExpressionBuilder.BuildComparisonExpression<TDTO>(expression.PropName!, expression.Operator, expression.PropValue!);
                result = result.Where(lambda.Compile()).ToList();
            }
            // Apply unified search Expressions
            var expr = UnifiedSearchHelper.GetUnifiedSearchExpression<TDTO>(request.Filter.Search);
            var func = expr?.Compile();
            result = result.Where(func ?? (c => false)).ToList();
            // Apply Pagination
            result = result
                .Skip((request.Filter.PaginationFilter.PageIndex - 1) * request.Filter.PaginationFilter.PageSize)
                .Take(request.Filter.PaginationFilter.PageSize).ToList();
            return Tuple.Create(result, total);
        }
    }

}
