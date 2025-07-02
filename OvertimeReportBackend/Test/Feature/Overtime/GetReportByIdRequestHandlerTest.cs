using System.Net;
using AutoMapper;
using Core.Contracts.Overtime;
using Core.Feature.Overtime.GetReportById;
using Core.MappingProfile;
using Core.Models.Overtime;
using Core.Models.Response;
using Moq;
using Shouldly;
using Test.Mock;

namespace Test.Feature.Overtime
{
    public class GetReportByIdRequestHandlerTest
    {
        private IMock<IReportRepository> _mockReportRepository;
        private readonly IMapper _mapper;

        public GetReportByIdRequestHandlerTest()
        {
            var report = new List<Report>()
            {
                new()
                {
                    InitialHour = "10:30",
                    FinalHour = "11:30",
                    DetailId = 1,
                    Headquarter = "test1",
                    ApplicationUserId = "bf0fb3a8-010e-40a3-822e-c0431f8e3a07",
                    StatusId=1,
                    NoveltyDate=DateTime.Now
                }
            };
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<OvertimeProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _mockReportRepository = MockReportRepository.MockReport(report);
        }
        [Fact]
        public async Task GetReportByIdRequest_SuccessAsync()
        {
            var handler = new GetReportByIdRequestHandler(_mockReportRepository.Object, _mapper);
            var result = await handler.Handle(new GetReportByIdRequest(1), CancellationToken.None);
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task GetReportByIdRequest_Fail_IdNotExits()
        {
            var report = new List<Report>()
            {
                null
            };
            _mockReportRepository = MockReportRepository.MockReport(report);
            var handler = new GetReportByIdRequestHandler(_mockReportRepository.Object, _mapper);
            var result = await handler.Handle(new GetReportByIdRequest(1), CancellationToken.None);
            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            result.Success.ShouldBeFalse();
        }
    }
}