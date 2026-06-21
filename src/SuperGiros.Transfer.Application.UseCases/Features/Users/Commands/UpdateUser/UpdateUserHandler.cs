using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null) return false;

            // Actualizar nombre de usuario si se proporciona
            if (!string.IsNullOrWhiteSpace(request.Username))
                user.Username = request.Username;

            // Actualizar Rol
            if (!string.IsNullOrWhiteSpace(request.Role))
                user.Role = request.Role;

            // Actualizar Contraseña: El Interceptor detectará el cambio y aplicará BCrypt
            if (!string.IsNullOrWhiteSpace(request.Password))
                user.PasswordHash = request.Password;

            _context.users.Update(user);

            // Retorna true si hubo cambios guardados con éxito
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
