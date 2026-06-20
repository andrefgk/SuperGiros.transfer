using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomer
{
    public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, GetCustomerResponseDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetCustomerHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<GetCustomerResponseDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.customers
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            var response = _mapper.Map<GetCustomerResponseDto>(customer);
            return response;
        }
    }
}
