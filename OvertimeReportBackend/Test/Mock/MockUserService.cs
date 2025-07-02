using Moq;
using Core.Contracts.Identity;

namespace Test.Mock
{
    public class MockUserService
    {
        public static Mock<IUserService> UserServiceMock()
        {
            var mockRepo = new Mock<IUserService>();
            mockRepo.Setup(x => x.UserId).Returns("bf0fb3a8-010e-40a3-822e-c0431f8e3a07");
            mockRepo.Setup(x => x.Name).Returns("test name");
            return mockRepo;
        }
    }
}