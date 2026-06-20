// src/SuperGiros.Transfer.Application.UseCases/Features/Customer/Querys/GetCustomerByDni/GetCustomerByDniQuery.cs
using MediatR;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomer;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomerByDni
{
    // Reutilizamos el mismo GetCustomerResponseDto porque la salida será idéntica
    public sealed record GetCustomerByDniQuery : IRequest<GetCustomerResponseDto>
    {
        public int NroDocumento { get; set; } = 0;
    }
}