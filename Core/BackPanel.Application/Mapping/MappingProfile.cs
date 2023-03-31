using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RoleDto, Role>().ReverseMap();
        CreateMap<RoleDtoRequest, Role>().ReverseMap();
        CreateMap<PermissionDto, Permission>().ReverseMap();
        CreateMap<AdminDto, Admin>().ReverseMap();
        CreateMap<AdminDtoRequest, Admin>().ReverseMap();
        CreateMap<MessageDto, Message>().ReverseMap();
        CreateMap<MessageDtoRequest, Message>().ReverseMap();
        CreateMap<AdminDto, Admin>().ReverseMap();
        CreateMap<ActivityDto, Activity>().ReverseMap();
        CreateMap<NotificationDto, Notification>().ReverseMap();
    }
}