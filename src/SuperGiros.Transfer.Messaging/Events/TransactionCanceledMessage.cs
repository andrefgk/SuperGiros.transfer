namespace SuperGiros.Transfer.Messaging.Events
{
    public record TransactionCanceledMessage
    {
        public int      Id          { get; init; }
        public DateTime OcurridoEn { get; init; } = DateTime.UtcNow;
    }
}
