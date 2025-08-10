using AutoMapper;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Commands.CreateCommandBase
{
    public class CreateCommandHandlerBase<TEntity, TDTORequest, TDTO, TCommand> : IRequestHandler<TCommand, TDTO>
        where TEntity : EntityBase
        where TDTO : class
        where TCommand : CreateCommandBase<TDTORequest, TDTO>
    {
        private readonly IRepositoryBase<TEntity> _repository;
        private readonly IMapper _mapper;
        public CreateCommandHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public virtual async Task<TDTO> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var mappedItem = _mapper.Map<TEntity>(request.Request);
            await _repository.CreateAsync(mappedItem);
            await _repository.Complete();
            var result = _mapper.Map<TDTO>(mappedItem);
            return result;
        }

    }
}
