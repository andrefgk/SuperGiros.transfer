using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Querys.GetAllTransaction
{
    public class GetAllTransactionHandler : IRequestHandler<GetAllTransactionQuery, IEnumerable<GetAllTransactionResponseDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetAllTransactionHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllTransactionResponseDto>> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _applicationDbContext.transactions.ToListAsync(cancellationToken);
            var response = _mapper.Map<List<GetAllTransactionResponseDto>>(transactions);
            return response;
        }
    }
}
