using AutoMapper;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Commands.CreateBulkBase
{
    public abstract class CreateBulkCommandHandlerBase<TEntity, TDTORequest, TCommand> : IRequestHandler<TCommand>
        where TEntity : EntityBase
        where TCommand : CreateBulkCommandBase<TDTORequest>
    {
        protected readonly IRepositoryBase<TEntity> Repository;
        protected readonly IMapper Mapper;
        public CreateBulkCommandHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public virtual async Task Handle(TCommand request, CancellationToken cancellationToken)
        {
            var mappedItems = Mapper.Map<List<TDTORequest>, List<TEntity>>(request.data);
            await Repository.CreateBulkAsync(mappedItems);
            await Repository.Complete();
        }
    }
}
