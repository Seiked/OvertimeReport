using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Overtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.DBContext.Configuration
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasData(
              new Status() { Id = 1, Name = "Pendiente" },
              new Status() { Id = 2, Name = "Aprobado" },
              new Status() { Id = 3, Name = "Rechazado" }
            );
        }
    }
}