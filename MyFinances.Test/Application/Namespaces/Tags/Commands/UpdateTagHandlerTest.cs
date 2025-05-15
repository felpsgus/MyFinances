using MyFinances.Application.Namespaces.Tags.Commands.UpdateTag;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Tags.Commands;

public class UpdateTagHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateTagHandler _handler;

    public UpdateTagHandlerTest()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateTagHandler(_namespaceRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ShouldUpdateTag()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespaceWithTags(userGuid);
        var tag = @namespace.Tags.First();
        var command = new UpdateTagCommand(@namespace.Id, tag.Id, "UpdatedTag");

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
        var command = new UpdateTagCommand(Guid.NewGuid(), Guid.NewGuid(), "UpdatedTag");

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