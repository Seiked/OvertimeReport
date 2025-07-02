using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contracts.Overtime;
using Core.Models.Overtime;
using Moq;

namespace Test.Mock
{
    public class MockDetailRepository
    {
        public static Mock<IDetailRepository> MockDetail(List<Detail> data)
        {
            var mockRepo = new Mock<IDetailRepository>();
            mockRepo.Setup(x => x.GetDetails()).Returns(Task.FromResult(data));
            return mockRepo;
        }
    }
}