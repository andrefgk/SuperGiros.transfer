using AutoMapper;
using MediatR;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Commands.CreateOffice
{
    public class CreateOfficeHandler : IRequestHandler<CreateOfficeCommand, int>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public CreateOfficeHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = _mapper.Map<SuperGiros.Transfer.Domain.Entities.Offices>(request);
            await _applicationDbContext.offices.AddAsync(office, cancellationToken);
            if (await _applicationDbContext.SaveChangeAsync(cancellationToken) > 0)
                return office.Id;
            return 0;
        }
    }
}
