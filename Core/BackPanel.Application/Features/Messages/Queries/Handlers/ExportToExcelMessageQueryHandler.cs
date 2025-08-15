using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Generic.Common.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Messages.Queries.Handlers;
public class ExportToExcelMessageQueryHandler : ExportToExcelQueryHandlerBase<Message, MessageDto, ExportToExcelQueryBase<Message>>
{
    public ExportToExcelMessageQueryHandler(IRepositoryBase<Message> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}