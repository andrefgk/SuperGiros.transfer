using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.ChangeUserState
{
    public class ChangeUserStateHandler : IRequestHandler<ChangeUserStateCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ChangeUserStateHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ChangeUserStateCommand request, CancellationToken cancellationToken)
        {
            // Buscamos al usuario ignorando filtros para poder reactivarlo si es necesario
            var user = await _context.users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null) return false;

            // Actualizamos el estado
            user.State = request.NewState;

            _context.users.Update(user);

            // Guardamos cambios (esto activará tu Interceptor de Auditoría)
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
