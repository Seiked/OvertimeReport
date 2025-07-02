using System.Net;
using AutoMapper;
using Core.Contracts.Email;
using Core.Contracts.Identity;
using Core.Contracts.Overtime;
using Core.Helpers;
using Core.Models.Identity;
using Core.Models.Overtime;
using Core.Models.Response;
using MediatR;
using Microsoft.Extensions.Options;

namespace Core.Feature.Overtime.CreateReport
{
    public class CreateReportRequestHandler : IRequestHandler<CreateReportRequest, Response<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IReportRepository _reportRepository;
        private readonly IDetailRepository _detailRepository;
        private readonly LeaderSetting _leaderSettings;

        public CreateReportRequestHandler(IMapper mapper, IUserService userService, IReportRepository reportRepository, IDetailRepository detailRepository, IOptions<LeaderSetting> leaderSettings)
        {
            _mapper = mapper;
            _userService = userService;
            _reportRepository = reportRepository;
            _detailRepository = detailRepository;
            _leaderSettings = leaderSettings.Value;
        }

        public async Task<Response<Unit>> Handle(CreateReportRequest request, CancellationToken cancellationToken)
        {

            var response = new Response<Unit>();
            var hoursValidation = HelpersUtils.ValidateHours(request.CreateReportDto.InitialHour, request.CreateReportDto.FinalHour);
            if (!hoursValidation)
            {
                response.Errors.Add("Las fechas ingresadas no son correctas, por favor valide la información.");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                return response;
            }
            if (request.CreateReportDto.NoveltyDate.Date > DateTime.Today)
            {
                response.Errors.Add("La fecha de reporte no debe ser futura, por favor valide la información.");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                return response;
            }
            if (DateTime.Today.AddDays(-2) > request.CreateReportDto.NoveltyDate.Date)
            {
                response.Errors.Add("Recuerda que el plazo máximo de reporte es de 2 días.");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                return response;
            }
            var detailList = await _detailRepository.GetDetails();
            var detailExist = detailList.Any(x => x.Id == request.CreateReportDto.DetailId);
            if (!detailExist)
            {
                response.Errors.Add("Detail Id no existe.");
                response.StatusCode = HttpStatusCode.NotFound;
                response.Success = false;
                return response;
            }
            var report = _mapper.Map<Report>(request.CreateReportDto);
            report.ApplicationUserId = _userService.UserId;
            report.StatusId = 1;
            await _reportRepository.Create(report);

            response.StatusCode = HttpStatusCode.OK;
            response.Success = true;
            response.Data = Unit.Value;
            return response;
        }
    }
}