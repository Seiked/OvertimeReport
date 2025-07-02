
using Core.Contracts.Overtime;
using Core.DBContext;
using Core.Models.Overtime;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OvertimeReportContext _context;

        public UserRepository(OvertimeReportContext context)
        {
            _context = context;
        }

        public async Task<List<UserT>> GetAllUser()
        {
            return await _context.UsersT.ToListAsync();
        }
    }
}