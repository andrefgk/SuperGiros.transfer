using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomer
{
    public sealed record GetCustomerQuery : IRequest<GetCustomerResponseDto>
    {
        public int Id { get; set; }
    }
}
