using SuperGiros.Transfer.Domain.Common;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Entities
{
    public class Transaction : BaseAuditableEntity
    {
        public int AccountId { get; set; }
        public TransactionType TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string Descripcion { get; set; }

        // --- Nuevos campos agregados para cumplir tus requerimientos ---
        public string Sede { get; set; }
        public DateTime FechaRealizacion { get; set; }

        // Constructor vacio requerido por Entity Framework
        protected Transaction() { }

        // Método de Creación y Validación Básica (Actualizado)
        public static Transaction Create(int accountId, TransactionType tipoMovimiento, decimal monto,
                                         string moneda, string descripcion, string sede, DateTime fechaRealizacion)
        {
            // Validaciones de negocio
            if (monto <= 0)
            {
                throw new ArgumentException("El monto de la transacción debe ser mayor a cero.", nameof(monto));
            }

            if (string.IsNullOrWhiteSpace(moneda))
            {
                throw new ArgumentException("Debe especificar la moneda (ej. USD, PEN).", nameof(moneda));
            }

            // Nueva validación para la Sede
            if (string.IsNullOrWhiteSpace(sede))
            {
                throw new ArgumentException("Debe especificar la sede donde se realiza la operación.", nameof(sede));
            }

            // Instancia de la entidad
            var transaction = new Transaction
            {
                AccountId = accountId,
                TipoMovimiento = tipoMovimiento,
                Monto = monto,
                Moneda = moneda,
                Descripcion = descripcion,
                Sede = sede,
                FechaRealizacion = fechaRealizacion
            };

            return transaction;
        }
    }
}
