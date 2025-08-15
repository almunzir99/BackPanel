using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Messages.Commands.Handlers;
public class ToggleActiveMessageCommandHander : ToggleActiveCommandHandlerBase<Message, ToggleActiveCommandBase<Message>>
{
    public ToggleActiveMessageCommandHander(IRepositoryBase<Message> repository) : base(repository)
    {
    }
}