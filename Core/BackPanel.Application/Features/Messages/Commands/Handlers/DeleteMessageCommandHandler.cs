using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Messages.Commands.Handlers;
public class DeleteMessageCommandHander : DeleteCommandHandlerBase<Message, DeleteCommandBase<Message>>
{
    public DeleteMessageCommandHander(IRepositoryBase<Message> repository) : base(repository)
    {
    }
}