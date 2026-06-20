using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime LastModifify { get; set; }
        public string? LastModififyBy { get; set; }

        // Estado global estandarizado: Inactivo=0, Activo=1, Completado=2
        public State State { get; set; } = State.Activo;
    }
}
