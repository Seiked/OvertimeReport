using System.Net;
using AutoMapper;
using Core.Contracts.Email;
using Core.Contracts.Identity;
using Core.Contracts.Overtime;
using Core.Feature.Overtime.CreateReport;
using Core.MappingProfile;
using Core.Models.Identity;
using Core.Models.Overtime;
using Core.Models.Response;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.Mock;

namespace Test.Feature.Overtime
{
    public class CreateReportRequestHandlerTest
    {
        private readonly IMock<IUserService> _mockUserService;
        private readonly IMock<IDetailRepository> _mockDetailRepository;
        private IMock<IReportRepository> _mockReportRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<LeaderSetting> _leaderSettings;
        private CreateReportDto _createReportDto;
        public CreateReportRequestHandlerTest()
        {
            LeaderSetting setting = new()
            {
                Email = "test@test.mail",
                SolId = "SOLF11"
            };
            _leaderSettings = Options.Create(setting);
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<OvertimeProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            var detailList = new List<Detail>(){
                new(){
                    Id=1,
                    Name="test1",
                },
                new(){
                    Id=2,
                    Name="test2",
                }
            };
            var report = new List<Report>()
            {
                new()
                {
                    InitialHour = "10:30",
                    FinalHour = "11:30",
                    DetailId = 1,
                    Headquarter = "test1",
                    ApplicationUserId = "bf0fb3a8-010e-40a3-822e-c0431f8e3a07",
                    StatusId=1
                }
            };
            _mockUserService = MockUserService.UserServiceMock();
            _mockDetailRepository = MockDetailRepository.MockDetail(detailList);
            _mockReportRepository = MockReportRepository.MockReport(report);
        }

        [Fact]
        public async Task CreateReportRequest_SuccessAsync()
        {
            var createReportDto = new CreateReportDto()
            {
                NoveltyDate = DateTime.Now,
                InitialHour = "10:30",
                FinalHour = "11:30",
                DetailId = 1,
                Headquarter = "test1",
            };
            var handler = new CreateReportRequestHandler(_mapper, _mockUserService.Object, _mockReportRepository.Object, _mockDetailRepository.Object, _leaderSettings);
            var result = await handler.Handle(new CreateReportRequest(createReportDto), CancellationToken.None);
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task CreateReportRequest_Fail_InitialHour_Higher_FinalHour()
        {
            var createReportDto = new CreateReportDto()
            {
                NoveltyDate = DateTime.Now,
                InitialHour = "12:30",
                FinalHour = "11:30",
                DetailId = 1,
                Headquarter = "test1",
            };
            var handler = new CreateReportRequestHandler(_mapper, _mockUserService.Object, _mockReportRepository.Object, _mockDetailRepository.Object, _leaderSettings);
            var result = await handler.Handle(new CreateReportRequest(createReportDto), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
            result.Errors[0].ShouldBe("Las fechas ingresadas no son correctas, por favor valide la información.");
            result.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task CreateReportRequest_Fail_Date_Higher()
        {
            var createReportDto = new CreateReportDto()
            {
                NoveltyDate = DateTime.Now.AddDays(3),
                InitialHour = "10:30",
                FinalHour = "11:30",
                DetailId = 1,
                Headquarter = "test1",
            };
            var handler = new CreateReportRequestHandler(_mapper, _mockUserService.Object, _mockReportRepository.Object, _mockDetailRepository.Object, _leaderSettings);
            var result = await handler.Handle(new CreateReportRequest(createReportDto), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
            result.Errors[0].ShouldBe("La fecha de reporte no debe ser futura, por favor valide la información.");
            result.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task CreateReportRequest_Fail_Date_Lower_TwoDays()
        {
            var createReportDto = new CreateReportDto()
            {
                NoveltyDate = DateTime.Now.AddDays(-33),
                InitialHour = "10:30",
                FinalHour = "11:30",
                DetailId = 1,
                Headquarter = "test1",
            };
            var handler = new CreateReportRequestHandler(_mapper, _mockUserService.Object, _mockReportRepository.Object, _mockDetailRepository.Object, _leaderSettings);
            var result = await handler.Handle(new CreateReportRequest(createReportDto), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
            result.Errors[0].ShouldBe("Recuerda que el plazo máximo de reporte es de 2 días.");
            result.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task CreateReportRequest_Fail_Detail_NotExits()
        {
            var createReportDto = new CreateReportDto()
            {
                NoveltyDate = DateTime.Now,
                InitialHour = "10:30",
                FinalHour = "11:30",
                DetailId = 4,
                Headquarter = "test1",
            };
            var handler = new CreateReportRequestHandler(_mapper, _mockUserService.Object, _mockReportRepository.Object, _mockDetailRepository.Object, _leaderSettings);
            var result = await handler.Handle(new CreateReportRequest(createReportDto), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
            result.Errors[0].ShouldBe("Detail Id no existe.");
            result.Success.ShouldBeFalse();
        }
    }
}