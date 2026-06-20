using SuperGiros.Transfer.Domain.Common;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Events
{
    public class CustomerUpdatedEvent : BaseEvent
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string Celular { get; set; }
        public string email { get; set; }
        public CustomerDocumentType TipoDocumento { get; set; }
        public int NroDocumento { get; set; }
    }
}
