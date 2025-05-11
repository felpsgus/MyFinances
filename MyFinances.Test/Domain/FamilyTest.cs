using Bogus;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Test.Domain;

public class FamilyTest
{
    private static readonly Faker Faker = new();

    [Fact]
    public void ShouldCreateNewFamily()
    {
        // Arrange
        var name = Faker.Name.LastName();

        // Act
        var family = Family.Create(name);

        // Assert
        family.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldThrowArgumentExceptionWhenNameIsInvalid(string name)
    {
        // Act & Assert
        FluentActions.Invoking(() => Family.Create(name))
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldAddFamilyMember()
    {
        // Arrange
        var family = FamilyFakerHelper.GetFakeFamily();
        var userId = Guid.NewGuid();

        // Act
        family.AddFamilyMember(userId);

        // Assert
        family.FamilyMembers.Should().HaveCount(2);
        family.FamilyMembers[1].UserId.Should().Be(userId);
    }

    [Fact]
    public void ShouldThrowAlreadyExistsExceptionWhenFamilyMemberAlreadyExists()
    {
        // Arrange
        var family = FamilyFakerHelper.GetFakeFamilyWithMembers(2);
        var userId = family.FamilyMembers.First().UserId;

        // Act & Assert
        FluentActions.Invoking(() => family.AddFamilyMember(userId))
            .Should()
            .Throw<AlreadyExistsException>();
    }

    [Fact]
    public void ShouldRemoveFamilyMember()
    {
        // Arrange
        var family = FamilyFakerHelper.GetFakeFamilyWithMembers(2);
        var userId = family.FamilyMembers.First().UserId;

        // Act
        family.RemoveFamilyMember(userId);

        // Assert
        family.FamilyMembers.Should().HaveCount(1);
        family.FamilyMembers.First().UserId.Should().NotBe(userId);
    }

    [Fact]
    public void ShouldThrowNotFoundExceptionWhenFamilyMemberDoesNotExist()
    {
        // Arrange
        var family = FamilyFakerHelper.GetFakeFamilyWithMembers(2);
        var userId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => family.RemoveFamilyMember(userId))
            .Should()
            .Throw<NotFoundException>();
    }
}