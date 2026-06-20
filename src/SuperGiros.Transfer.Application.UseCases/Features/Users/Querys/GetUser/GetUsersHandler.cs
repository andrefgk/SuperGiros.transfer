using MediatR;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Querys.GetUser
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        private readonly IApplicationDbContext _context;
        public GetUsersHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken ct)
        {
            return await _context.users.ToListAsync(ct);
        }
    }
}
