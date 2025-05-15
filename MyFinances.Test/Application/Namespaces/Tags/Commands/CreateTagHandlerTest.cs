using MyFinances.Application.Namespaces.Tags.Commands.CreateTag;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Tags.Commands;

public class CreateTagHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateTagHandler _handler;

    public CreateTagHandlerTest()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateTagHandler(_namespaceRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ShouldCreateTag()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(userGuid);
        var command = new CreateTagCommand(@namespace.Id, "TestTag");

        _namespaceRepositoryMock.Setup(x => x.GetByIdAsync(@namespace.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(@namespace);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _namespaceRepositoryMock.Verify(x => x.GetByIdAsync(@namespace.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenNamespaceNotFound()
    {
        // Arrange
        var command = new CreateTagCommand(Guid.NewGuid(), "TestTag");

        _namespaceRepositoryMock.Setup(x => x.GetByIdAsync(command.NamespaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Namespace?)null);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
        _namespaceRepositoryMock.Verify(x => x.GetByIdAsync(command.NamespaceId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}