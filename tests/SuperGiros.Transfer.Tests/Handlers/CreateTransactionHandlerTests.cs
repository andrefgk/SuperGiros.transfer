using AutoMapper;
using Moq;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Application.UseCases.Commons.Mapping;
using SuperGiros.Transfer.Application.UseCases.Features.Transaction.Commands.CreateTransaction;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;
using SuperGiros.Transfer.Messaging.Events;
using SuperGiros.Transfer.Messaging.Publishers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SuperGiros.Transfer.Tests.Handlers
{
    public class CreateTransactionHandlerTests
    {
        private readonly IMapper _mapper;

        public CreateTransactionHandlerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        // ✅ TEST 1: Crear transacción válida → retorna true y publica evento
        [Fact]
        public async Task Handle_TransaccionValida_ReturnsTrueYPublicaEvento()
        {
            // Arrange
            var mockContext   = new Mock<IApplicationDbContext>();
            var mockPublisher = new Mock<IEventPublisher>();
            var data          = new List<Transaction>();
            var fakeDbSet     = CrearDbSetFake(data);

            mockContext.Setup(c => c.transactions).Returns(fakeDbSet);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            mockPublisher
                .Setup(p => p.PublishTransactionCreatedAsync(It.IsAny<TransactionCreatedMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CreateTransactionCommand
            {
                AccountId        = 10,
                TipoMovimiento   = TransactionType.Giro,
                Monto            = 500,
                Moneda           = "PEN",
                Descripcion      = "Giro de prueba",
                Sede             = "Lima Centro",
                FechaRealizacion = DateTime.UtcNow
            };

            var handler = new CreateTransactionHandler(mockContext.Object, _mapper, mockPublisher.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockPublisher.Verify(
                p => p.PublishTransactionCreatedAsync(It.IsAny<TransactionCreatedMessage>(), It.IsAny<CancellationToken>()),
                Times.Once,
                "Debe publicarse exactamente 1 evento al crear exitosamente");
        }

        // ✅ TEST 2: Si SaveChanges falla → retorna false y NO publica evento
        [Fact]
        public async Task Handle_SaveFalla_ReturnsFalseYNoPublicaEvento()
        {
            // Arrange
            var mockContext   = new Mock<IApplicationDbContext>();
            var mockPublisher = new Mock<IEventPublisher>();
            var fakeDbSet     = CrearDbSetFake(new List<Transaction>());

            mockContext.Setup(c => c.transactions).Returns(fakeDbSet);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            var command = new CreateTransactionCommand
            {
                AccountId = 1, TipoMovimiento = TransactionType.Transferencia,
                Monto = 100, Moneda = "USD", Descripcion = "Test",
                Sede = "Sede1", FechaRealizacion = DateTime.UtcNow
            };

            var handler = new CreateTransactionHandler(mockContext.Object, _mapper, mockPublisher.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            mockPublisher.Verify(
                p => p.PublishTransactionCreatedAsync(It.IsAny<TransactionCreatedMessage>(), It.IsAny<CancellationToken>()),
                Times.Never,
                "NO debe publicarse evento si el guardado falló");
        }

        // ✅ TEST 3: El handler llama a SaveChangeAsync exactamente 1 vez
        [Fact]
        public async Task Handle_LlamaSaveChangeAsyncExactamenteUnaVez()
        {
            // Arrange
            var mockContext   = new Mock<IApplicationDbContext>();
            var mockPublisher = new Mock<IEventPublisher>();
            var fakeDbSet     = CrearDbSetFake(new List<Transaction>());

            mockContext.Setup(c => c.transactions).Returns(fakeDbSet);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            mockPublisher.Setup(p => p.PublishTransactionCreatedAsync(It.IsAny<TransactionCreatedMessage>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            var command = new CreateTransactionCommand
            {
                AccountId = 5, TipoMovimiento = TransactionType.Giro,
                Monto = 200, Moneda = "PEN", Descripcion = "Test save",
                Sede = "Cusco", FechaRealizacion = DateTime.UtcNow
            };

            var handler = new CreateTransactionHandler(mockContext.Object, _mapper, mockPublisher.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // Helper: DbSet fake en memoria
        private static DbSet<Transaction> CrearDbSetFake(List<Transaction> data)
        {
            var queryable = data.AsQueryable();
            var mockSet   = new Mock<DbSet<Transaction>>();
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(m => m.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
                   .Callback<Transaction, CancellationToken>((t, _) => data.Add(t))
                   .ReturnsAsync((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Transaction>)null!);
            return mockSet.Object;
        }
    }
}
