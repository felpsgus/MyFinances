using MyFinances.Application.Namespaces.Tags.Queries.GetTags;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Tags.Queries;

public class GetTagsHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepositoryMock;
    private readonly GetTagsHandler _handler;

    public GetTagsHandlerTest()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _handler = new GetTagsHandler(_namespaceRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldReturnTags()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespaceWithTags(userGuid);
        var query = new GetTagsQuery(@namespace.Id);

        _namespaceRepositoryMock.Setup(x => x.GetTagsAsync(@namespace.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(@namespace.Tags);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<GetTagsResponse>>();
        result.Should().HaveCount(@namespace.Tags.Count);
    }
}