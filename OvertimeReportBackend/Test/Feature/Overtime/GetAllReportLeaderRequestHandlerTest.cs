using System.Net;
using AutoMapper;
using Core.Contracts.Overtime;
using Core.MappingProfile;
using Core.Models.Overtime;
using Moq;
using Shouldly;
using Test.Mock;

namespace Core.Feature.Overtime.GetAllReportLeader
{
    public class GetAllReportLeaderRequestHandlerTest
    {
        private IMock<IReportRepository> _mockReportRepository;
        private readonly IMapper _mapper;
        public GetAllReportLeaderRequestHandlerTest()
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
                    StatusId=1
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
        public async Task GetAllReportLeaderRequest_SuccessAsync()
        {
            var queryReport = new QueryReportLeader();
            var handler = new GetAllReportLeaderRequestHandler(_mockReportRepository.Object, _mapper);
            var result = await handler.Handle(new GetAllReportLeaderRequest(queryReport), CancellationToken.None);
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.Success.ShouldBeTrue();
        }
    }
}