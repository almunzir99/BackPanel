using AutoMapper;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Queries.ExportToExcelQueryBase
{
    public class ExportToExcelQueryHandlerBase<TEntity, TDTO, TQuery> : IRequestHandler<TQuery, byte[]>
        where TEntity : EntityBase
        where TDTO : class
        where TQuery : ExportToExcelQueryBase<TEntity>
    {
        protected readonly IRepositoryBase<TEntity> _repository;
        protected readonly IMapper mapper;
        public ExportToExcelQueryHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            this.mapper = mapper;
        }

        public virtual async Task<byte[]> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.ListAsync();
            return DataExportHelper<TEntity>.ExportToExcel(data);
        }
    }
}
