using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Services;

public class MessagesService : ServiceBase<Message,MessageDto,MessageDtoRequest>, IMessageService
{
    public MessagesService(IMapper mapper, IRepositoryBase<Message> repository, IRepositoryBase<Admin> adminsRepository) : base(mapper, repository, adminsRepository)
    {
    }
}