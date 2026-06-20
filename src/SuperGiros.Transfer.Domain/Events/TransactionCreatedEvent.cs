using System;
using System.Collections.Generic;
using System.Text;
using SuperGiros.Transfer.Domain.Common;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Domain.Events
{
    public class TransactionCreatedEvent : BaseEvent
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public TransactionType TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string Sede { get; set; }
        public DateTime FechaRealizacion { get; set; }
    }
}
