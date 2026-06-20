using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Domain.Enums;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.CancelCustomer
{
    public class CancelCustomerHandler : IRequestHandler<CancelCustomerCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public CancelCustomerHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(CancelCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.customers
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (customer is null)
                throw new Exception($"Customer con Id {request.Id} no existe");

            // ✅ SOFT DELETE: cambia estado a Inactivo(0), NO elimina el registro
            customer.State = State.Inactivo;
            _applicationDbContext.customers.Update(customer);

            return await _applicationDbContext.SaveChangeAsync(cancellationToken) > 0;
        }
    }
}
