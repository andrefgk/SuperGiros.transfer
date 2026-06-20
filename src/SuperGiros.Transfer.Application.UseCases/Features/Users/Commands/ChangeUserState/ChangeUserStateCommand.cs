using MediatR;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.ChangeUserState
{
    public class ChangeUserStateCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public State NewState { get; set; }
    }
}
