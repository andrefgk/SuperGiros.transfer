using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.CancelCustomer
{
    public class CancelCustomerCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
