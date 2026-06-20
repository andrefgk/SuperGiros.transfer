

using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetAllOffice
{
    public sealed record GetAllOfficeQuery : IRequest<IEnumerable<GetAllOfficeResponseDto>>
    {
        public int Id { get; init; }
    }
}
