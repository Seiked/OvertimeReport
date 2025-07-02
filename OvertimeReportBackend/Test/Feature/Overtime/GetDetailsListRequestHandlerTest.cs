using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Contracts.Overtime;
using Core.Feature.Overtime.GetDetailsList;
using Core.MappingProfile;
using Core.Models.Overtime;
using Moq;
using Shouldly;
using Test.Mock;

namespace Test.Feature.Overtime
{
    public class GetDetailsListRequestHandlerTest
    {
        private readonly IMock<IDetailRepository> _mockDetailRepository;
        private readonly IMapper _mapper;

        public GetDetailsListRequestHandlerTest()
        {
            var data = new List<Detail>(){
                new(){
                    Id = 1,
                    Name="test1",
                },
                  new(){
                    Id = 2,
                    Name="test2",
                }
            };
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<OvertimeProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _mockDetailRepository = MockDetailRepository.MockDetail(data);
        }

        [Fact]
        public async Task GetDetailsListRequest_SuccessAsync()
        {
            var handler = new GetDetailsListRequestHandler(_mapper, _mockDetailRepository.Object);
            var result = await handler.Handle(new GetDetailsListRequest(), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            result.Data.Count.ShouldBe(2);
        }
    }
}