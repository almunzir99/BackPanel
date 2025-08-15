using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Messages.Commands.Handlers;
public class CreateMessageCommandHander : CreateCommandHandlerBase<Message, MessageDtoRequest, MessageDto, CreateCommandBase<MessageDtoRequest, MessageDto>>
{
    public CreateMessageCommandHander(IRepositoryBase<Message> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}