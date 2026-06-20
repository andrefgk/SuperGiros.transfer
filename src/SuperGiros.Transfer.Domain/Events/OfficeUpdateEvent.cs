using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Events
{
    public class OfficeUpdateEvent
    {
        public Guid OfficeId { get; set; }
    }
}
