using MediatR;
using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Querys.GetUserByUsername
{
    public record GetUserByUsernameQuery(string Username) : IRequest<User?>;
}
