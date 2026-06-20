using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetAllCustomer
{
    public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, IEnumerable<GetAllCustomerResponseDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetAllCustomerHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCustomerResponseDto>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            var customers = await _applicationDbContext.customers.ToListAsync(cancellationToken);
            var response = _mapper.Map<List<GetAllCustomerResponseDto>>(customers);
            return response;
        }
    }
}
