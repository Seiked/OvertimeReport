using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contracts.Overtime;
using Core.Models.Overtime;
using Moq;

namespace Test.Mock
{
    public class MockUserRepository
    {
        public static Mock<IUserRepository> UserRepositoryMock(List<UserT> data)
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetAllUser()).ReturnsAsync(() =>
                 {
                     return data;
                 });
            return mockRepo;
        }
    }
}