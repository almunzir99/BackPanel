using BackPanel.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Features.Dashboard.Queries
{
    public record GetDashboardCountersQuery : IRequest<CountersDto>;
    public class GetDashboardCountersQueryHandler : IRequestHandler<GetDashboardCountersQuery, CountersDto>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetDashboardCountersQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<CountersDto> Handle(GetDashboardCountersQuery request, CancellationToken cancellationToken)
        {
            var counters = new CountersDto
            {
                Admins = await unitOfWork.AdminsRepository.GetTotalRecords(),
                Messages = await unitOfWork.MessagesRepository.GetTotalRecords(),
                Roles = await unitOfWork.RolesRepository.GetTotalRecords()
            };
            return counters;
        }
    }
}
