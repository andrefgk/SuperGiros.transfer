namespace SuperGiros.Transfer.Messaging.Events
{
    public record CustomerCreatedMessage
    {
        public int     Id              { get; init; }
        public string  Nombre          { get; init; } = string.Empty;
        public string  ApellidoPaterno { get; init; } = string.Empty;
        public string? ApellidoMaterno { get; init; }
        public string  Celular         { get; init; } = string.Empty;
        public string  Email           { get; init; } = string.Empty;
        public DateTime OcurridoEn    { get; init; } = DateTime.UtcNow;
    }
}
