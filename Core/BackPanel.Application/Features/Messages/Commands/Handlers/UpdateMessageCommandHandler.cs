using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Messages.Commands.Handlers;
public class UpdateMessageCommandHander : UpdateCommandHandlerBase<Message, MessageDtoRequest, MessageDto, UpdateCommandBase<MessageDtoRequest, MessageDto>>
{
    public UpdateMessageCommandHander(IRepositoryBase<Message> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}