using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contracts.Overtime;
using Core.DBContext;
using Core.Models.Overtime;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class DetailRepository : IDetailRepository
    {
        private readonly OvertimeReportContext _context;

        public DetailRepository(OvertimeReportContext context)
        {
            _context = context;
        }

        public async Task<List<Detail>> GetDetails()
        {
            return await _context.Details.ToListAsync();
        }
    }
}