using System.Net;
using AutoMapper;
using Core.Contracts.Overtime;
using Core.Helpers;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.GetReportById
{
    public class GetReportByIdRequestHandler : IRequestHandler<GetReportByIdRequest, Response<GetReportByIdDto>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        public GetReportByIdRequestHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<Response<GetReportByIdDto>> Handle(GetReportByIdRequest request, CancellationToken cancellationToken)
        {
            var response = new Response<GetReportByIdDto>();
            var report = await _reportRepository.GetById(request.Id);
            if (report == null)
            {
                response.Success = false;
                response.Errors.Add($"El reporte no existe");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            var reportDto = _mapper.Map<GetReportByIdDto>(report);
            var country = "Colombia";
            reportDto.Hour = await HelpersUtils.CalculateHourAsync(report.InitialHour, report.FinalHour, report.NoveltyDate, country);
            response.Success = true;
            response.Data = reportDto;
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}