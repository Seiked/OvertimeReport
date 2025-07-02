using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Models.Identity;
using System.Security.Policy;
using Core.Models.Overtime;

namespace Core.DBContext.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserT>
    {
        public void Configure(EntityTypeBuilder<UserT> builder)
        {

            builder.HasData(
               new UserT
               {
                   Id = 1,
                   SolId = "OVERT12",
                   Name = "Alex Medina",
                   PositionTittle = "TEAMMEMBER",
                   Office = "Medellin",
                   Email = "AMedina@overtime.com"

               },
                new UserT
                {
                    Id = 2,
                    SolId = "OVERT13",
                    Name = "Juan Torres",
                    PositionTittle = "TEAMMEMBER",
                    Office = "Medellin",
                    Email = "Juantorres@overtime.com"

                },
                new UserT
                {
                    Id = 3,
                    SolId = "OVERT123",
                    Name = "Sebastian Villa",
                    PositionTittle = "LEADER",
                    Office = "Medellin",
                    Email = "sebastianvillanv@gmail.com"

                }
            );
        }
    }
}