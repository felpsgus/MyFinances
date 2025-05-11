using MyFinances.Application.Namespaces.Queries.GetNamespaceById;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Queries;

public class GetNamespaceByIdHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepositoryMock;
    private readonly GetNamespaceByIdHandler _handler;

    public GetNamespaceByIdHandlerTest()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _handler = new GetNamespaceByIdHandler(_namespaceRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldReturnNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var expectedNamespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var query = new GetNamespaceByIdQuery(expectedNamespace.Id);

        _namespaceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(expectedNamespace.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedNamespace);


        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        expectedNamespace.Id.Should().Be(result.Id);
        expectedNamespace.Name.Should().Be(result.Name);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var expectedNamespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var query = new GetNamespaceByIdQuery(expectedNamespace.Id);

        _namespaceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(expectedNamespace.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Namespace?)null);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}