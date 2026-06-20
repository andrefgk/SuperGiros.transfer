using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
