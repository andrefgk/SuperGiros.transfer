using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CreateOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeHandler : IRequestHandler<UpdateOfficeCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDBContext;
        private readonly IMapper _mapper;

        public UpdateOfficeHandler(IApplicationDbContext applicationDBContext, IMapper mapper)
        {
            _applicationDBContext = applicationDBContext;
            _mapper = mapper;
        }
        public async Task<bool> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _applicationDBContext.offices.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            if (office is not null)
            {
                office.Nombre = request.Nombre;
                office.Ubicacion = request.Ubicacion;
                office.MontoDiario = request.MontoDiario;
                office.NumeroClientes = request.NumeroClientes;
                office.Saldo = request.Saldo;
                office.Estado = request.Estado;

                _applicationDBContext.offices.Update(office);

            }
            if (await _applicationDBContext.SaveChangesAsync(cancellationToken) > 0)
                return true;
            else
                return false;
        }
    }
}

