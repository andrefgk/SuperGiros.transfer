using SuperGiros.Transfer.Domain.Common;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
namespace SuperGiros.Transfer.Domain.Entities
{
    public class Offices : BaseAuditableEntity
    {
        public string Nombre { get; set; }

        public string Ubicacion { get; set; }

        public decimal MontoDiario { get; set; }

        public int NumeroClientes { get; set; }

        public decimal Saldo { get; set; }

        public OfficeStatus Estado { get; set; }
    }
}
