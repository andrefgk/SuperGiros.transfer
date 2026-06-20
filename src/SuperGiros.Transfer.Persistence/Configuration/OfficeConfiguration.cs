using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Persistence.Configuration
{
    public class OfficeConfiguration : IEntityTypeConfiguration<Offices>
    {
        public void Configure(EntityTypeBuilder<Offices> builder)
        {
            builder.ToTable("Offices");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd(); // ✅ IDENTITY

            builder.Property(x => x.Estado)
                .IsRequired()
                .HasDefaultValue(OfficeStatus.Activa);

            builder.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Ubicacion)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.MontoDiario)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.NumeroClientes)
                .IsRequired();

            builder.Property(x => x.Saldo)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
