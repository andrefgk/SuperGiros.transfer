using SuperGiros.Transfer.Messaging.Events;

namespace SuperGiros.Transfer.Messaging.Publishers
{
    public interface IEventPublisher
    {
        Task PublishTransactionCreatedAsync(TransactionCreatedMessage message, CancellationToken ct = default);
        Task PublishCustomerCreatedAsync(CustomerCreatedMessage message, CancellationToken ct = default);
        Task PublishTransactionCanceledAsync(TransactionCanceledMessage message, CancellationToken ct = default);
    }
}
