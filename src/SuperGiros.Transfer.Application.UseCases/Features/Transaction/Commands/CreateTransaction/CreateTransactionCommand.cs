using MediatR;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<bool>
    {
        public int AccountId { get; set; }
        public TransactionType TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string Descripcion { get; set; }

        // --- Nuevos campos agregados para cumplir tus requerimientos ---
        public string Sede { get; set; }
        public DateTime FechaRealizacion { get; set; }

    }
}
