using Core.Feature.Common;
using Core.Models.Overtime;

namespace Core.Contracts.Overtime
{
    public interface IReportRepository
    {
        public Task Create(Report report);
        public Task<List<Report>> GetAll(QueryReport query);
        public Task<Report> GetById(int id);
        public Task UpdateReport(Report report);
    }
}