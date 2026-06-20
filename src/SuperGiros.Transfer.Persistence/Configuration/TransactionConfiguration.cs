using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Persistence.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd(); // ✅ IDENTITY

            builder.Property(t => t.State)
                .IsRequired()
                .HasDefaultValue(State.Activo);

            builder.Property(t => t.Monto)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(t => t.Sede)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.TipoMovimiento)
                .IsRequired();

            builder.Property(t => t.FechaRealizacion)
                .IsRequired();
        }
    }
}
