using MediatR;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeCommand : IRequest<bool>
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
