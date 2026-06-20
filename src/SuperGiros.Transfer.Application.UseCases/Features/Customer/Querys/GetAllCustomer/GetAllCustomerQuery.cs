using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetAllCustomer
{
    public sealed record GetAllCustomerQuery : IRequest<IEnumerable<GetAllCustomerResponseDto>>
    {
    }
}
