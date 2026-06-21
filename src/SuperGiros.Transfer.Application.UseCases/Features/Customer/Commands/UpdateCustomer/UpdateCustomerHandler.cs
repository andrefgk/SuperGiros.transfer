using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.UpdateCustomer
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateCustomerHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.customers
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            if (customer is null)
                throw new Exception($"Customer con Id {request.Id} no existe");

            customer.Nombre = request.Nombre;
            customer.ApellidoPaterno = request.ApellidoPaterno;
            customer.ApellidoMaterno = request.ApellidoMaterno;
            customer.Celular = request.Celular;
            customer.email = request.email;
            customer.TipoDocumento = request.TipoDocumento;
            customer.NroDocumento = request.NroDocumento;

            _applicationDbContext.customers.Update(customer);

            if (await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0)
                return true;
            else
                return false;
        }
    }
}
