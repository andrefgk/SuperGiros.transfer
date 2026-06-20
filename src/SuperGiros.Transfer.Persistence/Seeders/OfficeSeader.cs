using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Persistence.Seeders
{
    public class OfficeSeeder : IEntityTypeConfiguration<Offices>
    {
        public void Configure(EntityTypeBuilder<Offices> builder)
        {
            builder.HasData(
                new Offices
                {
                    Id = 1,
                    Nombre = "Oficina Central",
                    Ubicacion = "Lima - Centro",
                    MontoDiario = 5000,
                    NumeroClientes = 120,
                    Saldo = 20000,
                    Estado = Domain.Enums.OfficeStatus.Activa,
                    Created = new DateTime(2026, 3, 1),
                    CreatedBy = "System",


                },
                new Offices
                {
                    Id = 2,
                    Nombre = "Oficina Norte",
                    Ubicacion = "Lima - Los Olivos",
                    MontoDiario = 3000,
                    NumeroClientes = 80,
                    Saldo = 15000,
                    Estado = Domain.Enums.OfficeStatus.Activa,
                    Created = new DateTime(2026, 3, 10),
                    CreatedBy = "System"

                }
            );
        }
    }
}
