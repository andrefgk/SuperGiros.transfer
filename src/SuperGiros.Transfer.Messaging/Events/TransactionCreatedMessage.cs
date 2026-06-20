namespace SuperGiros.Transfer.Messaging.Events
{
    public record TransactionCreatedMessage
    {
        public int      Id               { get; init; }
        public int      AccountId        { get; init; }
        public string   TipoMovimiento   { get; init; } = string.Empty;
        public decimal  Monto            { get; init; }
        public string   Moneda           { get; init; } = string.Empty;
        public string   Sede             { get; init; } = string.Empty;
        public DateTime FechaRealizacion { get; init; }
        public DateTime OcurridoEn      { get; init; } = DateTime.UtcNow;
    }
}
