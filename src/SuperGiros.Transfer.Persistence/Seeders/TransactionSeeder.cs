using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperGiros.Transfer.Domain.Enums;
using SuperGiros.Transfer.Domain.Entities;

namespace SuperGiros.Transfer.Persistence.Seeders
{
    internal class TransactionSeeder : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            var fechaEstatica = new DateTime(2026, 3, 19);

            builder.HasData(
                new
                {
                    Id = 1,
                    AccountId = 1001,
                    TipoMovimiento = TransactionType.Giro,
                    Monto = 1500.50M,
                    Moneda = "Sol",
                    Descripcion = "Depósito inicial de fondos",
                    Sede = "Cusco",             // <-- Agregado
                    FechaRealizacion = fechaEstatica,    // <-- Agregado
                    State = State.Activo, // <--- AGREGA ESTO
                    CreatedBy = "System",
                    Created = fechaEstatica,
                    LastModifyBy = "",                   // <-- Nombre corregido
                    LastModifify = fechaEstatica           // <-- Nombre corregido
                },
                new
                {
                    Id = 2,
                    AccountId = 1001,
                    TipoMovimiento = TransactionType.Transferencia, // Corregido el enum según tu equipo
                    Monto = 200.00M,
                    Moneda = "Sol",
                    Descripcion = "Retiro a cuenta bancaria",
                    Sede = "Mazuko",               // <-- Agregado
                    FechaRealizacion = fechaEstatica,    // <-- Agregado
                    State = State.Activo, // <--- AGREGA ESTO
                    CreatedBy = "System",
                    Created = fechaEstatica,
                    LastModifyBy = "",                   // <-- Nombre corregido
                    LastModifify = fechaEstatica           // <-- Nombre corregido
                }
            );
        }
    }
}
