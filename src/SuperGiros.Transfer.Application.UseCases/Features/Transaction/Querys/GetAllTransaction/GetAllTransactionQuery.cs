using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetAllTransaction
{
    public sealed record GetAllTransactionQuery : IRequest<IEnumerable<GetAllTransactionResponseDto>>
    {
    }
}
