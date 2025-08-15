using AutoMapper;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Common.Queries.Handlers
{
    public abstract class ExportToPDFQueryHandlerBase<TEntity, TDTO, TQuery> : IRequestHandler<TQuery, byte[]>
        where TEntity : EntityBase
        where TDTO : class
        where TQuery : ExportToPDFQueryBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public ExportToPDFQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public virtual async Task<byte[]> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var data = await Repository.ListAsync();
            return DataExportHelper<TEntity>.ExportToExcel(data);
        }
    }
}
