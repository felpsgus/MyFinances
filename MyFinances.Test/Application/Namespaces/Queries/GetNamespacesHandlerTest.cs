using MyFinances.Application.Namespaces.Queries.GetNamespaces;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Queries;

public class GetNamespacesHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepositoryMock;
    private readonly GetNamespacesHandler _handler;

    public GetNamespacesHandlerTest()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _handler = new GetNamespacesHandler(_namespaceRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldReturnNamespaces()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var family = FamilyFakerHelper.GetFakeFamily();
        var namespaces = new List<Namespace>
        {
            NamespaceFakerHelper.GetFakePersonalNamespace(user.Id),
            NamespaceFakerHelper.GetFakeFamilyNamespace(family.Id)
        };
        var query = new GetNamespacesQuery();

        _namespaceRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(namespaces);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<GetNamespacesResponse>>();
        result.Should().HaveCount(2);
    }
}