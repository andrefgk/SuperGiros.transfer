using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CancelTransaction
{
    public class CancelTransactionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
