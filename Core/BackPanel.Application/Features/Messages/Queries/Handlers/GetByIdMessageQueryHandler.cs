using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Generic.Common.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Messages.Queries.Handlers;
public class GetByIdMessageQueryHandler : GetByIdQueryHandlerBase<Message, MessageDto, GetByIdQueryBase<MessageDto>>
{
    public GetByIdMessageQueryHandler(IRepositoryBase<Message> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}