using AutoMapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Messages.Commands.Handlers;
public class CreateBulkMessageCommandHander : CreateBulkCommandHandlerBase<Message, MessageDtoRequest, CreateBulkCommandBase<MessageDtoRequest>>
{
    public CreateBulkMessageCommandHander(IRepositoryBase<Message> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}