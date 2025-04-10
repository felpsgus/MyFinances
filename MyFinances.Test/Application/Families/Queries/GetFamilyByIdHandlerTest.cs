using MyFinances.Application.Families.Queries.GetFamilyById;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Test.Application.Families.Queries;

public class GetFamilyByIdHandlerTest
{
    private readonly Mock<IFamilyRepository> _familyRepositoryMock;
    private readonly GetFamilyByIdHandler _getFamilyByIdHandler;

    public GetFamilyByIdHandlerTest()
    {
        _familyRepositoryMock = new Mock<IFamilyRepository>();
        _getFamilyByIdHandler = new GetFamilyByIdHandler(_familyRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldReturnFamilyById()
    {
        // Arrange
        var fakeFamily = FamilyFakerHelper.GetFakeFamily();
        var query = new GetFamilyByIdQuery(fakeFamily.Id);

        _familyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeFamily);

        // Act
        var result = await _getFamilyByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        _familyRepositoryMock.Verify(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeFamily);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenFamilyNotFound()
    {
        // Arrange
        var familyId = Guid.NewGuid();
        var query = new GetFamilyByIdQuery(familyId);

        _familyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(familyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Family?)null);

        // Act
        Func<Task> act = () => _getFamilyByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Family with ID {familyId} not found.");
        _familyRepositoryMock.Verify(repo => repo.GetByIdAsync(familyId, It.IsAny<CancellationToken>()), Times.Once);
    }
}