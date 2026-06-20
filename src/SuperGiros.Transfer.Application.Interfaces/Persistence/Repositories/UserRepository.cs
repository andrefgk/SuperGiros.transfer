using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Application.Interfaces.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _context;

        public UserRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            // Este código que tienes en tu imagen está perfecto
            return await _context.users
                .FirstOrDefaultAsync(u => u.Username == username && u.State == State.Activo);
        }
    }
}
