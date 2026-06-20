using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;


namespace SuperGiros.Transfer.Persistence.Seeders
{
    public class CustomerSeeder: IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasData(new Customer
            {
                Id = 1,
                Nombre = "Arely",
                ApellidoPaterno = "Mollo",
                ApellidoMaterno = "Boza",
                Celular = "+51906081076",
                email = "arely@gmail.com",
                TipoDocumento = Domain.Enums.CustomerDocumentType.DNI,
                NroDocumento = 70537513
            },
                new Customer
                {
                    Id = 2,
                    Nombre = "Luis",
                    ApellidoPaterno = "Mollo",
                    ApellidoMaterno = "Boza",
                    Celular = "+51984452340",
                    email = "luis@gmail.com",
                    TipoDocumento = Domain.Enums.CustomerDocumentType.CE,
                    NroDocumento = 60528515
                },
                new Customer
                {
                    Id = 3,
                    Nombre = "Maria",
                    ApellidoPaterno = "Huachaca",
                    ApellidoMaterno = "Armejo",
                    Celular = "+51984173852",
                    email = "maria@gmail.com",
                    TipoDocumento = Domain.Enums.CustomerDocumentType.DNI,
                    NroDocumento = 85214785
                },
                new Customer
                {
                    Id = 4,
                    Nombre = "Javier",
                    ApellidoPaterno = "Rojas",
                    ApellidoMaterno = "Borda",
                    Celular = "+51930161092",
                    email = "javier@gmail.com",
                    TipoDocumento = Domain.Enums.CustomerDocumentType.CE,
                    NroDocumento = 85214787
                },
                new Customer
                {
                    Id = 5,
                    Nombre = "Andre",
                    ApellidoPaterno = "Flores",
                    ApellidoMaterno = "Gallegos",
                    Celular = "+51901875421",
                    email = "andre@gmail.com",
                    TipoDocumento = Domain.Enums.CustomerDocumentType.DNI,
                    NroDocumento = 78541587

                },
                new Customer
                {
                    Id = 6,
                    Nombre = "Alejandro",
                    ApellidoPaterno = "Quispe",
                    ApellidoMaterno = "Bonifacio",
                    Celular = "+51987951456",
                    email = "alejandro@gmail.com",
                    TipoDocumento = Domain.Enums.CustomerDocumentType.CE,
                    NroDocumento = 78541588
                },
                new Customer
                {
                    Id = 7,
                    Nombre = "Martin",
                    ApellidoPaterno = "Rojas",
                    ApellidoMaterno = "Chumbe",
                    Celular = "+51985741234",
                    email = "martin@gmail.com",
                    TipoDocumento = Domain.Enums.CustomerDocumentType.DNI,
                    NroDocumento = 78965895                    
                }
            );
        }
    }
}
