using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;

namespace SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.UpdateTransaction
{
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateTransactionHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _applicationDbContext.transactions
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            if (transaction is null)
                throw new Exception($"Transaction con Id {request.Id} no existe");

            transaction.AccountId = request.AccountId;
            transaction.TipoMovimiento = request.TipoMovimiento;
            transaction.Monto = request.Monto;
            transaction.Moneda = request.Moneda;
            transaction.Descripcion = request.Descripcion;
            transaction.Sede = request.Sede;
            transaction.FechaRealizacion = request.FechaRealizacion;
            transaction.State = (Domain.Enums.State)request.State;

            _applicationDbContext.transactions.Update(transaction);

            if (await _applicationDbContext.SaveChangeAsync(cancellationToken) > 0)
                return true;
            else
                return false;
        }
    }
}
