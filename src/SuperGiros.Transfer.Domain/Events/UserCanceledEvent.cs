using SuperGiros.Transfer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Events
{
    public class UserCanceledEvent: BaseEvent
    {
        public int Id { get; set; }
    }
}
