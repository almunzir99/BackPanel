using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Interfaces;

public interface IMessageService : IServicesBase<Message,MessageDto,MessageDtoRequest>
{
    
}