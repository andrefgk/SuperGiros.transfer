using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Para cambio de contraseña
        public string Role { get; set; } = string.Empty;
    }
}
