using MyFinances.Application.Namespaces.Commands.DeleteNamespace;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Commands;

public class DeleteNamespaceHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly DeleteNamespaceHandler _handler;

    public DeleteNamespaceHandlerTest()
    {
        _namespaceRepository = new Mock<INamespaceRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteNamespaceHandler(_namespaceRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task ShouldDeleteNamespace()
    {
        // Arrange
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(Guid.NewGuid());
        var command = new DeleteNamespaceCommand(@namespace.Id);

        _namespaceRepository
            .Setup(repo => repo.GetByIdAsync(command.NamespaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(@namespace);
        _namespaceRepository
            .Setup(repo => repo.Delete(@namespace))
            .Callback((Namespace n, CancellationToken ct) => n.DeleteEntity())
            .Returns(@namespace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _namespaceRepository.Verify(x => x.Delete(@namespace), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        @namespace.Deleted.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenNamespaceDoesNotExist()
    {
        // Arrange
        var command = new DeleteNamespaceCommand(Guid.NewGuid());

        _namespaceRepository
            .Setup(repo => repo.GetByIdAsync(command.NamespaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Namespace?)null);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}