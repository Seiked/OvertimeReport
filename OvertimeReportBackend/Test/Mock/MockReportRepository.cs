using Core.Contracts.Overtime;
using Core.Feature.Common;
using Core.Models.Overtime;
using Moq;

namespace Test.Mock
{
    public class MockReportRepository
    {
        public static Mock<IReportRepository> MockReport(List<Report> data)
        {
            var mockRepo = new Mock<IReportRepository>();
            mockRepo.Setup(x => x.Create(It.IsAny<Report>())).Returns((Report newData) =>
            {
                data.Add(newData);
                return Task.CompletedTask;
            });
            mockRepo.Setup(x => x.GetAll(It.IsAny<QueryReport>())).Returns((QueryReport query) =>
            {
                return Task.FromResult(data);
            });
            mockRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                return Task.FromResult(data[0]);
            });
            return mockRepo;
        }
    }
}