using System.Runtime.InteropServices.JavaScript;
using MyFinances.Application.Namespaces.Expenses.Commands.CreateExpense;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Expenses.Command;

public class CreateExpenseHandlerTest
{
    private readonly Mock<INamespaceRepository> _mockNamespaceRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateExpenseHandler _handler;

    public CreateExpenseHandlerTest()
    {
        _mockNamespaceRepository = new Mock<INamespaceRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockUserRepository = new Mock<IUserRepository>();
        _handler = new CreateExpenseHandler(_mockNamespaceRepository.Object, _mockUnitOfWork.Object,
            mockUserRepository.Object);
    }

    [Fact]
    public async Task ShouldCreateExpense()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(userId);
        var expense = ExpenseFakerHelper.GetFakeExpense(@namespace.Id);
        var command = new CreateExpenseCommand(
            expense.Name,
            expense.Description,
            expense.Value,
            @namespace.Id
        );

        _mockNamespaceRepository
            .Setup(x => x.GetByIdAsync(command.NamespaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(@namespace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockNamespaceRepository.Verify(x => x.GetByIdAsync(command.NamespaceId, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        @namespace.Expenses.Should().ContainSingle(e => e.Name == command.Name);
    }
}