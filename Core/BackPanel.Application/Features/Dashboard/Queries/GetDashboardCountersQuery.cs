using BackPanel.Application.Interfaces;
using MediatR;

namespace BackPanel.Application.Features.Dashboard.Queries
{
    public record GetDashboardCountersQuery : IRequest<CountersDto>;
    public class GetDashboardCountersQueryHandler : IRequestHandler<GetDashboardCountersQuery, CountersDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public GetDashboardCountersQueryHandler(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<CountersDto> Handle(GetDashboardCountersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync();
            var counters = new CountersDto
            {
                Users = users.Count,
                Messages = await _unitOfWork.MessagesRepository.GetTotalRecords(),
                Roles = await _unitOfWork.RolesRepository.GetTotalRecords()
            };
            return counters;
        }
    }
}