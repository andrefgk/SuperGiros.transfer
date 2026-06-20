using MassTransit;
using SuperGiros.Transfer.Messaging.Events;

namespace SuperGiros.Transfer.Messaging.Publishers
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IBus _bus;
        public EventPublisher(IBus bus) => _bus = bus;

        public Task PublishTransactionCreatedAsync(TransactionCreatedMessage message, CancellationToken ct = default)
            => _bus.Publish(message, ct);

        public Task PublishCustomerCreatedAsync(CustomerCreatedMessage message, CancellationToken ct = default)
            => _bus.Publish(message, ct);

        public Task PublishTransactionCanceledAsync(TransactionCanceledMessage message, CancellationToken ct = default)
            => _bus.Publish(message, ct);
    }
}
