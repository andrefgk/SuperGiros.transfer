using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CancelOffice
{
    public class CancelOfficeHandler : IRequestHandler<CancelOfficeCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public CancelOfficeHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(CancelOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _applicationDbContext.offices
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (office is null)
                throw new Exception($"Office con Id {request.Id} no existe");

            // SOFT DELETE: cambiar Estado a Inactiva (valor = 0 según el enum)
            office.Estado = OfficeStatus.Inactiva;

            // Marcar explícitamente como modificado para que EF lo persista
            _applicationDbContext.offices.Update(office);

            return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
