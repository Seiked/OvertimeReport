using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Contracts.Identity;
using Core.Contracts.Overtime;
using Core.Feature.Overtime.UpdateRejectReport;
using Core.MappingProfile;
using Core.Models.Overtime;
using Moq;
using Shouldly;
using Test.Mock;
using Xunit;

namespace Test.Feature.Overtime
{
    public class UpdateRejectReportRequestHandlerTest
    {
        private IMock<IReportRepository> _mockReportRepository;
        private readonly IMapper _mapper;
        private readonly IMock<IUserService> _mockUserService;
        public UpdateRejectReportRequestHandlerTest()
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
            _mockUserService = MockUserService.UserServiceMock();
            _mockReportRepository = MockReportRepository.MockReport(report);
        }
        [Fact]
        public async Task UpdateRejectReportRequestHandler_SuccessAsync()
        {
            var updateDto = new UpdateRejectReportDto()
            {
                ApprovalComment = "test"
            };
            var handler = new UpdateRejectReportRequestHandler(_mockReportRepository.Object, _mapper, _mockUserService.Object);
            var result = await handler.Handle(new UpdateRejectReportRequest(1, updateDto), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            result.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task UpdateRejectReportRequestHandler_Fail_Id_NotExist()
        {
            var updateDto = new UpdateRejectReportDto()
            {
                ApprovalComment = "test"
            };
            var report = new List<Report>()
            {
               null
            };
            _mockReportRepository = MockReportRepository.MockReport(report);
            var handler = new UpdateRejectReportRequestHandler(_mockReportRepository.Object, _mapper, _mockUserService.Object);
            var result = await handler.Handle(new UpdateRejectReportRequest(1, updateDto), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
            result.Success.ShouldBeFalse();
            result.Errors[0].ShouldBe("El reporte no existe");
        }
        [Fact]
        public async Task UpdateRejectReportRequestHandler_Fail_CantBeApproved()
        {
            var updateDto = new UpdateRejectReportDto()
            {
                ApprovalComment = "test"
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
                    StatusId=2
                }
            };
            _mockReportRepository = MockReportRepository.MockReport(report);
            var handler = new UpdateRejectReportRequestHandler(_mockReportRepository.Object, _mapper, _mockUserService.Object);
            var result = await handler.Handle(new UpdateRejectReportRequest(1, updateDto), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
            result.Success.ShouldBeFalse();
            result.Errors[0].ShouldBe("El reporte no se puede rechazar");
        }
    }
}