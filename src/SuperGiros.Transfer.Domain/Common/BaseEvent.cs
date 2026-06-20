namespace SuperGiros.Transfer.Domain.Common
{
    public abstract class BaseEvent
    {
        // Guid: ID de letras y numeros unico
        public Guid MessageID { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
