using MediatR;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Querys.GetUserByUsername
{
    public class GetUserByUsernameHandler : IRequestHandler<GetUserByUsernameQuery, User?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByUsernameHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            // Usamos el repositorio que ya tienes programado
            return await _userRepository.GetByUsernameAsync(request.Username);
        }
    }
}
