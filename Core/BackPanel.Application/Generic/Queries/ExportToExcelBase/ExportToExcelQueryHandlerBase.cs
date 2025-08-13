using AutoMapper;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Queries.ExportToExcelBase
{
    public abstract class ExportToExcelQueryHandlerBase<TEntity, TDTO, TQuery> : IRequestHandler<TQuery, byte[]>
        where TEntity : EntityBase
        where TDTO : class
        where TQuery : ExportToExcelQueryBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public ExportToExcelQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            this.Mapper = mapper;
        }

        public virtual async Task<byte[]> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var data = await Repository.ListAsync();
            return DataExportHelper<TEntity>.ExportToExcel(data);
        }
    }
}
