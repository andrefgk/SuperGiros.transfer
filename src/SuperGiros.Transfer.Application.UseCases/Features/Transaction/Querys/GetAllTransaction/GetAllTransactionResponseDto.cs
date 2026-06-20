using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetAllTransaction
{
    public class GetAllTransactionResponseDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public TransactionType TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;
        public DateTime FechaRealizacion { get; set; }
        public State State { get; set; } // ✅ Agregado
    }
}
