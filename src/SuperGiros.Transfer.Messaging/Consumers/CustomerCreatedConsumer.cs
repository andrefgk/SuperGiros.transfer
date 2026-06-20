using MassTransit;
using Microsoft.Extensions.Logging;
using SuperGiros.Transfer.Messaging.Events;

namespace SuperGiros.Transfer.Messaging.Consumers
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreatedMessage>
    {
        private readonly ILogger<CustomerCreatedConsumer> _logger;
        public CustomerCreatedConsumer(ILogger<CustomerCreatedConsumer> logger) => _logger = logger;

        public Task Consume(ConsumeContext<CustomerCreatedMessage> context)
        {
            var m = context.Message;
            _logger.LogInformation(
                "[EVENT RECIBIDO] CustomerCreated | Id:{Id} | {Nombre} {Apellido} | Tel:{Celular}",
                m.Id, m.Nombre, m.ApellidoPaterno, m.Celular);
            return Task.CompletedTask;
        }
    }
}
