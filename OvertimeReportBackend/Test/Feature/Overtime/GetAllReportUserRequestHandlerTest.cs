using System.Net;
using AutoMapper;
using Core.Contracts.Identity;
using Core.Contracts.Overtime;
using Core.Feature.Overtime.GetAllReportUser;
using Core.MappingProfile;
using Core.Models.Overtime;
using Core.Models.Response;
using Moq;
using Shouldly;
using Test.Mock;


namespace Test.Feature.Overtime
{
    public class GetAllReportUserRequestHandlerTest
    {
        private readonly Mock<IReportRepository> _mockReportRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly IMapper _mapper;
        public GetAllReportUserRequestHandlerTest()
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
                    Detail=new(){
                        Id=1,
                        Name="detail test"
                    },
                    Status=new(){
                        Id=1,
                        Name="status test"
                    },ApplicationUser=new(){
                        Id="bf0fb3a8-010e-40a3-822e-c0431f8e3a07",
                        Name="test name",
                        Office="test office",
                    },
                    NoveltyDate=DateTime.Now,

                }
            };
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<OvertimeProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _mockUserService = MockUserService.UserServiceMock();
            _mockReportRepository = MockReportRepository.MockReport(report);
        }
        [Fact]
        public async Task GetAllReportUserRequest_SuccessAsync()
        {
            var queryReport = new QueryReportUser();
            var handler = new GetAllReportUserRequestHandler(_mockReportRepository.Object, _mockUserService.Object, _mapper);
            var result = await handler.Handle(new GetAllReportUserRequest(queryReport), CancellationToken.None);
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.Success.ShouldBeTrue();
        }
    }
}