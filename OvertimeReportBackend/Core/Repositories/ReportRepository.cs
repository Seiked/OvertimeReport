using Core.Contracts.Overtime;
using Core.DBContext;
using Core.Feature.Common;
using Core.Models.Overtime;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly OvertimeReportContext _context;

        public ReportRepository(OvertimeReportContext context)
        {
            _context = context;
        }
        public async Task Create(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public Task<List<Report>> GetAll(QueryReport query)
        {
            var reportQuery = _context.Reports
            .Include(r => r.ApplicationUser)
            .Include(r => r.Status)
            .Include(r => r.Detail)
            .AsQueryable();

            if (query.CreationDate != DateTime.MinValue)
            {
                reportQuery = reportQuery.Where(r => r.CreationDate.Date == query.CreationDate.Date);
            }
            if (!string.IsNullOrEmpty(query.Status))
            {
                reportQuery = reportQuery.Where(r => r.Status.Name == query.Status);
            }
            if (!string.IsNullOrEmpty(query.Solver))
            {
                reportQuery = reportQuery.Where(r => r.ApplicationUser.Name.Contains(query.Solver) || r.ApplicationUser.SolId.Contains(query.Solver));
            }
            return reportQuery.ToListAsync();
        }

        public Task<Report> GetById(int id)
        {
            return _context.Reports
            .Include(r => r.ApplicationUser)
            .Include(r => r.Status)
            .Include(r => r.Detail)
            .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateReport(Report data)
        {
            _context.Reports.Update(data);
            await _context.SaveChangesAsync();
        }
    }
}