using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetTransaction
{
    public sealed record GetTransactionQuery : IRequest<GetTransactionResponseDto>
    {
        public int Id { get; set; }
    }
}
