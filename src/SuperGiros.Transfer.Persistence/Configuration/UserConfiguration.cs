using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasMaxLength(20)
                .IsRequired();

            // Filtro Global: Solo trae registros con State = 1 (Active)
            builder.HasQueryFilter(x => x.State == State.Activo);
        }
    }
}
