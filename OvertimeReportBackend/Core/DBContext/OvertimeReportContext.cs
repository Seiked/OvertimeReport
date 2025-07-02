using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Models.Identity;
using Core.Models.Overtime;

namespace Core.DBContext
{
    public class OvertimeReportContext : IdentityDbContext<ApplicationUser>
    {
        public OvertimeReportContext(DbContextOptions<OvertimeReportContext> options) : base(options)
        {
        }
        public DbSet<UserT> UsersT { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(OvertimeReportContext).Assembly);
        }
    }
}