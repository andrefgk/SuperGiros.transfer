using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Persistence.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);

            // ✅ IDENTITY: la BD genera el ID automáticamente
            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(t => t.State)
                .IsRequired()
                .HasDefaultValue(State.Activo);

            builder.Property(t => t.Nombre)
                .IsRequired();

            builder.Property(t => t.ApellidoPaterno)
                .IsRequired();

            builder.Property(t => t.ApellidoMaterno)
                .IsRequired(false);

            builder.Property(t => t.Celular)
                .HasMaxLength(12);

            builder.Property(t => t.email)
                .IsRequired();

            builder.Property(t => t.TipoDocumento)
                .IsRequired();

            builder.Property(t => t.NroDocumento)
                .IsRequired();
        }
    }
}
