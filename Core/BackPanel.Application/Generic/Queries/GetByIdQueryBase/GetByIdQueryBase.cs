using BackPanel.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Generic.Queries.GetByIdQueryBase
{
    public record GetByIdQueryBase<TDTO>(int Id) : IRequest<TDTO>
        where TDTO : DtoBase;

}

