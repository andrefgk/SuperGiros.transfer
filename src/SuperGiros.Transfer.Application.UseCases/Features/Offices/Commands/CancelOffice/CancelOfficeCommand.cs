using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CancelOffice
{
    public class CancelOfficeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

