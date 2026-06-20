using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetOffice
{
    public class GetOfficeResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal MontoDiario { get; set; }
        public int NumeroClientes { get; set; }
        public decimal Saldo { get; set; }
        public OfficeStatus Estado { get; set; }
    }
}
