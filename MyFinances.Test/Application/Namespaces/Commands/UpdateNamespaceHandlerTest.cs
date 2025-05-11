using MyFinances.Application.Namespaces.Commands.UpdateNamespace;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Commands;

public class UpdateNamespaceHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateNamespaceHandler _handler;

    public UpdateNamespaceHandlerTest()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateNamespaceHandler(_namespaceRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ShouldUpdateNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var newName = "Updated Namespace";
        var command = new UpdateNamespaceCommand(@namespace.Id, newName);

        _namespaceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(@namespace.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(@namespace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _namespaceRepositoryMock.Verify(x => x.GetByIdAsync(@namespace.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        @namespace.Name.Should().Be(newName);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenNamespaceDoesNotExist()
    {
        // Arrange
        var command = new UpdateNamespaceCommand(Guid.NewGuid(), "Updated Namespace");

        _namespaceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.NamespaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Namespace?)null);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}