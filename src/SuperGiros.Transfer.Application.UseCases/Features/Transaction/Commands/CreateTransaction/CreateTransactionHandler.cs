using AutoMapper;
using MediatR;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Messaging.Events;
using SuperGiros.Transfer.Messaging.Publishers;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CreateTransaction
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDBContext;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        public CreateTransactionHandler(IApplicationDbContext applicationDBContext, IMapper mapper, IEventPublisher eventPublisher)
        {
            _applicationDBContext = applicationDBContext;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = _mapper.Map<SuperGiros.Transfer.Domain.Entities.Transaction>(request);
            await _applicationDBContext.transactions.AddAsync(transaction, cancellationToken);

            if (await _applicationDBContext.SaveChangesAsync(cancellationToken) > 0)
            {
                // Publicar evento event-driven
                await _eventPublisher.PublishTransactionCreatedAsync(new TransactionCreatedMessage
                {
                    Id               = transaction.Id,
                    AccountId        = transaction.AccountId,
                    TipoMovimiento   = transaction.TipoMovimiento.ToString(),
                    Monto            = transaction.Monto,
                    Moneda           = transaction.Moneda,
                    Sede             = transaction.Sede,
                    FechaRealizacion = transaction.FechaRealizacion,
                    OcurridoEn       = DateTime.UtcNow
                }, cancellationToken);
                return true;
            }
            return false;
        }
    }
}
