using MediatR;
using SuperGiros.Transfer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Querys.GetUser
{
    public class GetUsersQuery : IRequest<List<User>>
    {

    }
}
