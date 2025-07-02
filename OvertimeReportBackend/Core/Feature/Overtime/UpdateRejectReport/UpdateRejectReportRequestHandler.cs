using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Contracts.Identity;
using Core.Contracts.Overtime;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.UpdateRejectReport
{
    public class UpdateRejectReportRequestHandler : IRequestHandler<UpdateRejectReportRequest, Response<Unit>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UpdateRejectReportRequestHandler(IReportRepository reportRepository, IMapper mapper, IUserService userService)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Response<Unit>> Handle(UpdateRejectReportRequest request, CancellationToken cancellationToken)
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
                response.Errors.Add($"El reporte no se puede rechazar");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            _mapper.Map(request.UpdateRejectReportDto, report);
            report.ApprovalSolver = $"{_userService.SolId} - {_userService.Name}";
            await _reportRepository.UpdateReport(report);
            response.Success = true;
            response.Data = Unit.Value;
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}