using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Overtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.DBContext.Configuration
{
    public class DetailConfiguration : IEntityTypeConfiguration<Detail>
    {
        public void Configure(EntityTypeBuilder<Detail> builder)
        {
            builder.HasData(
              new Detail() { Id = 1, Name = "Festivo Colombia" },
              new Detail() { Id = 2, Name = "Festivo Argentina" },
              new Detail() { Id = 3, Name = "Turno normal en Sede" },
              new Detail() { Id = 4, Name = "Actividad Infraestructura" },
              new Detail() { Id = 5, Name = "Movimiento programado" },
              new Detail() { Id = 6, Name = "Otro" }
            );
        }
    }
}