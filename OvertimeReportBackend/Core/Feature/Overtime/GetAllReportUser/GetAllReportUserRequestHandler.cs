using System.Net;
using AutoMapper;
using Core.Contracts.Identity;
using Core.Contracts.Overtime;
using Core.Feature.Common;
using Core.Models.Response;
using MediatR;
using Core.Helpers;

namespace Core.Feature.Overtime.GetAllReportUser
{
    public class GetAllReportUserRequestHandler : IRequestHandler<GetAllReportUserRequest, Response<List<GetAllReportUserDto>>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public GetAllReportUserRequestHandler(IReportRepository reportRepository, IUserService userService, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Response<List<GetAllReportUserDto>>> Handle(GetAllReportUserRequest request, CancellationToken cancellationToken)
        {
            var queryReport = _mapper.Map<QueryReport>(request.Query);
            var reportDtoList = new List<GetAllReportUserDto>();
            var reportList = await _reportRepository.GetAll(queryReport);
            var reportUserList = reportList.Where(x => x.ApplicationUser.Id == _userService.UserId).ToList();
            foreach (var item in reportUserList)
            {
                var country = "Colombia";
                var reportItem = new GetAllReportUserDto()
                {
                    Id = item.Id,
                    Detail = item.Detail.Name,
                    Headquarter = item.Headquarter,
                    Hour = await HelpersUtils.CalculateHourAsync(item.InitialHour, item.FinalHour, item.NoveltyDate, country),
                    ReportDate = item.CreationDate.ToShortDateString(),
                    Status = item.Status.Name,
                    ApprovalDate = item.ApprovalDate.ToShortDateString()
                };
                reportDtoList.Add(reportItem);
            }
            var response = new Response<List<GetAllReportUserDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = reportDtoList
            };
            return response;
        }
    }
}