using SuperGiros.Transfer.Domain.Common;

namespace SuperGiros.Transfer.Domain.Events
{
    public class CustomerCanceledEvent: BaseEvent
    {
        public int Id { get; set; }
    }
}
