using System.Net;
using AutoMapper;
using Core.Contracts.Identity;
using Core.Contracts.Overtime;
using Core.Models.Overtime;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.UpdateApproveReport
{
    public class UpdateApprovalReportRequestHandler : IRequestHandler<UpdateApprovalReportRequest, Response<Unit>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UpdateApprovalReportRequestHandler(IReportRepository reportRepository, IMapper mapper, IUserService userService)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Response<Unit>> Handle(UpdateApprovalReportRequest request, CancellationToken cancellationToken)
        {
            var response = new Response<Unit>();
            var report = await _reportRepository.GetById(request.Id);
            if (report == null)
            {
                response.Success = false;
                response.Errors.Add($"El reporte no existe");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            if (report.StatusId != 1)
            {
                response.Success = false;
                response.Errors.Add($"El reporte no se puede aprobar");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            _mapper.Map(request.UpdateApprovalReportDto, report);
            report.ApprovalSolver = $"{_userService.SolId} - {_userService.Name}";
            await _reportRepository.UpdateReport(report);
            response.Success = true;
            response.Data = Unit.Value;
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}