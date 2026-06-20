using MediatR;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string Celular { get; set; }
        public string email { get; set; }
        public CustomerDocumentType TipoDocumento { get; set; }
        public int NroDocumento { get; set; }
    }
}
