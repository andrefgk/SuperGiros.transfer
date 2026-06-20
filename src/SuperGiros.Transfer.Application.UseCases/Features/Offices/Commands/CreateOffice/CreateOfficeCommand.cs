using MediatR;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CreateOffice
{
    public class CreateOfficeCommand : IRequest<int>
    {
        public string Nombre { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal MontoDiario { get; set; }
        public int NumeroClientes { get; set; }
        public decimal Saldo { get; set; }
        public OfficeStatus Estado { get; set; }
    }
}
