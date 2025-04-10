using MyFinances.Application.Families.Queries.GetAllFamilies;
using MyFinances.Application.Families.Views;

namespace MyFinances.Test.Application.Families.Queries;

public class GetAllFamiliesHandlerTest
{
    private readonly Mock<IFamilyRepository> _familyRepositoryMock;
    private readonly GetAllFamiliesHandler _getAllFamiliesHandler;

    public GetAllFamiliesHandlerTest()
    {
        _familyRepositoryMock = new Mock<IFamilyRepository>();
        _getAllFamiliesHandler = new GetAllFamiliesHandler(_familyRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldReturnAllFamilies()
    {
        // Arrange
        var fakeFamilies = FamilyFakerHelper.GetFakeFamilies(5);
        var fakeFamiliesViewModel = fakeFamilies.Select(f => (FamilyViewModel)f).ToList();
        var query = new GetAllFamiliesQuery();

        _familyRepositoryMock
            .Setup(repo => repo.GetAllFamiliesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeFamiliesViewModel);

        // Act
        var result = await _getAllFamiliesHandler.Handle(query, CancellationToken.None);

        // Assert
        _familyRepositoryMock.Verify(repo => repo.GetAllFamiliesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeFamiliesViewModel);
    }
}