using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Generic.Accounts.Commands.UpdateProfileBase
{
    public record UpdateProfileCommandBase<TDTORequest, TDTO>(int Id, TDTORequest Request) : IRequest<TDTO>
        where TDTORequest : class
        where TDTO : UserDtoBase;
}
