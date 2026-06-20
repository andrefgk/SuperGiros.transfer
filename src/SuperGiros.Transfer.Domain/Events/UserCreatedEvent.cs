using SuperGiros.Transfer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Events
{
    public class UserCreatedEvent: BaseEvent
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int State { get; set; }
    }
}
