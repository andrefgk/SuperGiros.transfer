using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetOffice
{
    public class GetOfficeHandler : IRequestHandler<GetOfficeQuery, GetOfficeResponseDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetOfficeHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<GetOfficeResponseDto> Handle(GetOfficeQuery request, CancellationToken cancellationToken)
        {
            var office = await _applicationDbContext.offices.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            var response = _mapper.Map<GetOfficeResponseDto>(office);
            return response;
        }
    }
}
