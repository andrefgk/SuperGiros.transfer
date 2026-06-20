using SuperGiros.Transfer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Trader"; // Admin o Usuario
    }
}
