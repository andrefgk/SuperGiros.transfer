using MediatR;

namespace SuperGiros.Transfer.Application.UseCases.Features.Offices.Querys.GetOffice
{
    public sealed record GetOfficeQuery : IRequest<GetOfficeResponseDto>
    {
        public int Id { get; set; }
    }
}
