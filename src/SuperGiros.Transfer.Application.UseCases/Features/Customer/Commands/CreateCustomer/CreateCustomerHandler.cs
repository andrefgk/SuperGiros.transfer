using AutoMapper;
using MediatR;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Messaging.Events;
using SuperGiros.Transfer.Messaging.Publishers;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Commands.CreateCustomer
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDBContext;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        public CreateCustomerHandler(IApplicationDbContext applicationDBContext, IMapper mapper, IEventPublisher eventPublisher)
        {
            _applicationDBContext = applicationDBContext;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<SuperGiros.Transfer.Domain.Entities.Customer>(request);
            await _applicationDBContext.customers.AddAsync(customer, cancellationToken);

            if (await _applicationDBContext.SaveChangeAsync(cancellationToken) > 0)
            {
                await _eventPublisher.PublishCustomerCreatedAsync(new CustomerCreatedMessage
                {
                    Id              = customer.Id,
                    Nombre          = customer.Nombre,
                    ApellidoPaterno = customer.ApellidoPaterno,
                    ApellidoMaterno = customer.ApellidoMaterno,
                    Celular         = customer.Celular,
                    Email           = customer.email,
                    OcurridoEn      = DateTime.UtcNow
                }, cancellationToken);
                return true;
            }
            return false;
        }
    }
}
