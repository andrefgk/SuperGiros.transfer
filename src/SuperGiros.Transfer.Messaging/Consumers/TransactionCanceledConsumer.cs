using MassTransit;
using Microsoft.Extensions.Logging;
using SuperGiros.Transfer.Messaging.Events;

namespace SuperGiros.Transfer.Messaging.Consumers
{
    public class TransactionCanceledConsumer : IConsumer<TransactionCanceledMessage>
    {
        private readonly ILogger<TransactionCanceledConsumer> _logger;
        public TransactionCanceledConsumer(ILogger<TransactionCanceledConsumer> logger) => _logger = logger;

        public Task Consume(ConsumeContext<TransactionCanceledMessage> context)
        {
            _logger.LogInformation(
                "[EVENT RECIBIDO] TransactionCanceled | Id:{Id} | En:{Cuando}",
                context.Message.Id, context.Message.OcurridoEn);
            return Task.CompletedTask;
        }
    }
}
