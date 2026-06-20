using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Persistence.Seeders
{
    public class UserSeeder : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            /* Hasheo automático de las credenciales iniciales
            string adminHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            string userHash = BCrypt.Net.BCrypt.HashPassword("user123");*/

            const string adminHash = "$2a$11$iq30d.w1ZCDg2LU3VtpisuCG7kaLbFNE8nmiSLHZB5r3GSM3HpSBC";
            const string userHash = "$2a$11$dfmFjF0uGcwaG5N/t9uzneSBeVA5cgCY6K1onyHqo5mHgUcXSrR2K";

                    builder.HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = adminHash,
                    Role = "Admin",
                    State = State.Activo,
                    Created = new DateTime(2026, 5, 3),
                    CreatedBy = "System"
                },
                new User
                {
                    Id = 2,
                    Username = "usuario",
                    PasswordHash = userHash,
                    Role = "Usuario",
                    State = State.Activo,
                    Created = new DateTime(2026, 5, 3),
                    CreatedBy = "System"
                }
            );
        }
    }
}
