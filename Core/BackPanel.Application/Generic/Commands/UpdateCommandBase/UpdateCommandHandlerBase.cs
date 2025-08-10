using AutoMapper;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Generic.Commands.UpdateCommandBase
{
    public class UpdateCommandHandlerBase<TEntity, TDTORequest, TDTO, TCommand> : IRequestHandler<TCommand, TDTO>
       where TEntity : EntityBase
       where TDTO : class
       where TCommand : UpdateCommandBase<TDTORequest, TDTO>
    {
        private readonly IRepositoryBase<TEntity> _repository;
        private readonly IMapper _mapper;
        public UpdateCommandHandlerBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public virtual async Task<TDTO> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var mappedItem = _mapper.Map<TDTORequest, TEntity>(request.Request);
            var result = await _repository.UpdateAsync(id, mappedItem);
            await _repository.Complete();
            return _mapper.Map<TEntity, TDTO>(result);
        }

    }

}