using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Events
{
    public class OfficeCanceledEvent
    {
        public Guid OfficeId { get; set; }
    }
}
