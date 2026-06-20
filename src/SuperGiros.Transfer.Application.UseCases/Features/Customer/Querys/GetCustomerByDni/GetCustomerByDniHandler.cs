using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomer;
using SuperGiros.Transfer.Domain.Enums; // ✅ Necesario para mapear el Enum del Estado

namespace SuperGiros.Transfer.Application.UseCases.Features.Customer.Querys.GetCustomerByDni
{
    public class GetCustomerByDniHandler : IRequestHandler<GetCustomerByDniQuery, GetCustomerResponseDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetCustomerByDniHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<GetCustomerResponseDto> Handle(GetCustomerByDniQuery request, CancellationToken cancellationToken)
        {
            // 🔄 CORRECCIÓN AQUÍ: Usamos x.State (que viene de BaseAuditableEntity) 
            // y lo comparamos usando el Enum nativo State.Activo para que Entity Framework compile limpio.
            var customer = await _applicationDbContext.customers
                .FirstOrDefaultAsync(x => x.NroDocumento == request.NroDocumento && x.State == State.Activo, cancellationToken);

            var response = _mapper.Map<GetCustomerResponseDto>(customer);
            return response;
        }
    }
}