using AutoMapper;
using MediatR;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetAllOffice
{
    public class GetAllOfficeHandler : IRequestHandler<GetAllOfficeQuery, IEnumerable<GetAllOfficeResponseDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public GetAllOfficeHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllOfficeResponseDto>> Handle(GetAllOfficeQuery request, CancellationToken cancellationToken)
        {
            var offices = await _applicationDbContext.offices.ToListAsync(cancellationToken);
            var response = _mapper.Map<List<GetAllOfficeResponseDto>>(offices);

            return response;
        }
    }

}
