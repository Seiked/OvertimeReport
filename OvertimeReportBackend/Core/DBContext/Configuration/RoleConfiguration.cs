using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.DBContext.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
              new IdentityRole
              {
                  Id = "d2b1023d-1be6-4b1e-8dc5-f3c123a79c55",
                  Name = "TeamMember",
                  NormalizedName = "TEAMMEMBER"
              },
                new IdentityRole
                {
                    Id = "86ece1b6-ee1c-4ad6-9714-3a7d3ae79445",
                    Name = "Leader",
                    NormalizedName = "LEADER"
                }
            );
        }
    }
}