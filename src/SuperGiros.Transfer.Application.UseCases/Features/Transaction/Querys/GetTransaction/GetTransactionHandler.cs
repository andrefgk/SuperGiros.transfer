using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetTransaction
{
    public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, GetTransactionResponseDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetTransactionHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<GetTransactionResponseDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _applicationDbContext.transactions
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            var response = _mapper.Map<GetTransactionResponseDto>(transaction);
            return response;
        }
    }
}
