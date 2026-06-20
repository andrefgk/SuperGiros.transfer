using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomer
{
    public class GetCustomerResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public string Celular { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public CustomerDocumentType TipoDocumento { get; set; }
        public int NroDocumento { get; set; }
        public State State { get; set; } // ✅ Agregado
    }
}
