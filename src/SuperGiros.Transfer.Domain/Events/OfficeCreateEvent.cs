using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Events
{
    public class OfficeCreateEvent
    {
        public Guid OfficeId { get; set; }
        public string Nombre { get; set; }
    }
}
