using Moq;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CancelTransaction;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;
using SuperGiros.Transfer.Messaging.Events;
using SuperGiros.Transfer.Messaging.Publishers;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;

namespace SuperGiros.Transfer.Tests.Handlers
{
    public class CancelTransactionHandlerTests
    {
        // ✅ TEST 4: Cancelar transacción existente → soft delete (State=Inactivo) + evento publicado
        [Fact]
        public async Task Handle_TransaccionExistente_CambiaEstadoYPublicaEvento()
        {
            // Arrange — usamos Transaction.Create() porque el constructor es protected
            var transaction = Transaction.Create(
                accountId: 10,
                tipoMovimiento: TransactionType.Giro,
                monto: 500,
                moneda: "PEN",
                descripcion: "Test",
                sede: "Lima",
                fechaRealizacion: DateTime.UtcNow
            );

            var data = new List<Transaction> { transaction };

            var mockContext   = new Mock<IApplicationDbContext>();
            var mockPublisher = new Mock<IEventPublisher>();

            var mockSet = BuildAsyncDbSet(data);
            mockContext.Setup(c => c.transactions).Returns(mockSet);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            mockPublisher
                .Setup(p => p.PublishTransactionCanceledAsync(It.IsAny<TransactionCanceledMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CancelTransactionHandler(mockContext.Object, mockPublisher.Object);

            // Act
            var result = await handler.Handle(new CancelTransactionCommand { Id = 0 }, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(State.Inactivo, transaction.State);
            mockPublisher.Verify(
                p => p.PublishTransactionCanceledAsync(It.IsAny<TransactionCanceledMessage>(), It.IsAny<CancellationToken>()),
                Times.Once,
                "Debe publicarse 1 evento al cancelar exitosamente");
        }

        // ✅ TEST 5: Cancelar transacción inexistente → lanza Exception
        [Fact]
        public async Task Handle_TransaccionNoExiste_LanzaException()
        {
            // Arrange — lista vacía, ningún Id coincide
            var data = new List<Transaction>();

            var mockContext   = new Mock<IApplicationDbContext>();
            var mockPublisher = new Mock<IEventPublisher>();

            var mockSet = BuildAsyncDbSet(data);
            mockContext.Setup(c => c.transactions).Returns(mockSet);

            var handler = new CancelTransactionHandler(mockContext.Object, mockPublisher.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => handler.Handle(new CancelTransactionCommand { Id = 999 }, CancellationToken.None));
        }

        // Helper: construye un DbSet<T> que soporta FirstOrDefaultAsync de EF Core
        private static DbSet<T> BuildAsyncDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet   = new Mock<DbSet<T>>();

            mockSet.As<IAsyncEnumerable<T>>()
                   .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                   .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet.Object;
        }
    }

    // Helpers async para Moq + EF Core
    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
        public T Current => _inner.Current;
        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_inner.MoveNext());
        public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; }
    }

    internal class TestAsyncQueryProvider<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;
        public IQueryable CreateQuery(System.Linq.Expressions.Expression e) => _inner.CreateQuery(e);
        public IQueryable<TEl> CreateQuery<TEl>(System.Linq.Expressions.Expression e) => _inner.CreateQuery<TEl>(e);
        public object? Execute(System.Linq.Expressions.Expression e) => _inner.Execute(e);
        public TResult Execute<TResult>(System.Linq.Expressions.Expression e) => _inner.Execute<TResult>(e);
        public TResult ExecuteAsync<TResult>(System.Linq.Expressions.Expression e, CancellationToken ct = default)
        {
            var result = _inner.Execute(e);
            return (TResult)typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(typeof(TResult).GetGenericArguments()[0])
                .Invoke(null, new[] { result })!;
        }
    }
}
