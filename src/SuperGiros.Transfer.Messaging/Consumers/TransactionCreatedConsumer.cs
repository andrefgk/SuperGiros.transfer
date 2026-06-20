using MassTransit;
using Microsoft.Extensions.Logging;
using SuperGiros.Transfer.Messaging.Events;

namespace SuperGiros.Transfer.Messaging.Consumers
{
    public class TransactionCreatedConsumer : IConsumer<TransactionCreatedMessage>
    {
        private readonly ILogger<TransactionCreatedConsumer> _logger;
        public TransactionCreatedConsumer(ILogger<TransactionCreatedConsumer> logger) => _logger = logger;

        public Task Consume(ConsumeContext<TransactionCreatedMessage> context)
        {
            var m = context.Message;
            _logger.LogInformation(
                "[EVENT RECIBIDO] TransactionCreated | Id:{Id} | Cuenta:{AccountId} | {Tipo} | S/ {Monto} {Moneda} | Sede:{Sede}",
                m.Id, m.AccountId, m.TipoMovimiento, m.Monto, m.Moneda, m.Sede);
            return Task.CompletedTask;
        }
    }
}
