using AutoMapper;
using MediatR;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var user = _mapper.Map<User>(request);
            // El interceptor se encargará del PasswordHash y el State.Active
            _context.users.Add(user);
            await _context.SaveChangesAsync(ct);
            return user.Id;
        }
    }
}
