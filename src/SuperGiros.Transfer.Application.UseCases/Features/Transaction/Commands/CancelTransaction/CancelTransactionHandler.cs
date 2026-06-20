using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Domain.Enums;
using SuperGiros.Transfer.Messaging.Events;
using SuperGiros.Transfer.Messaging.Publishers;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CancelTransaction
{
    public class CancelTransactionHandler : IRequestHandler<CancelTransactionCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IEventPublisher _eventPublisher;

        public CancelTransactionHandler(IApplicationDbContext applicationDbContext, IEventPublisher eventPublisher)
        {
            _applicationDbContext = applicationDbContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(CancelTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _applicationDbContext.transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (transaction is null)
                throw new Exception($"Transaction con Id {request.Id} no existe");

            // SOFT DELETE: cambia estado a Inactivo (NO elimina el registro)
            transaction.State = State.Inactivo;
            _applicationDbContext.transactions.Update(transaction);

            if (await _applicationDbContext.SaveChangeAsync(cancellationToken) > 0)
            {
                await _eventPublisher.PublishTransactionCanceledAsync(new TransactionCanceledMessage
                {
                    Id         = transaction.Id,
                    OcurridoEn = DateTime.UtcNow
                }, cancellationToken);
                return true;
            }
            return false;
        }
    }
}
