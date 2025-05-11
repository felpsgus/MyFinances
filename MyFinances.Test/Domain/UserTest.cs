using Bogus;
using MyFinances.Domain.Entities;

namespace MyFinances.Test.Domain;

public class UserTest
{
    private static readonly Faker Faker = new();

    public static IEnumerable<object[]> GetTestData()
    {
        yield return [null, Faker.Internet.Email(), DateOnly.FromDateTime(DateTime.Now), Faker.Internet.Password()];
        yield return [Faker.Name.FirstName(), null, DateOnly.FromDateTime(DateTime.Now), Faker.Internet.Password()];
        yield return [Faker.Name.FirstName(), Faker.Internet.Email(), null, Faker.Internet.Password()];
        yield return [Faker.Name.FirstName(), Faker.Internet.Email(), DateOnly.FromDateTime(DateTime.Now), null];
    }

    [Fact]
    public void ShouldCreateUser()
    {
        // Arrange
        var name = Faker.Name.FirstName();
        var email = Faker.Internet.Email();
        var birthDate = DateOnly.FromDateTime(DateTime.Now);
        var password = Faker.Internet.Password();

        // Act
        var user = User.Create(name, email, birthDate, password);

        // Assert
        user.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(GetTestData), MemberType = typeof(UserTest))]
    public void ShouldThrowArgumentExceptionWhenDataIsInvalid(string name, string email, DateOnly birthDate,
        string password)
    {
        // Act & Assert
        FluentActions.Invoking(() => User.Create(name, email, birthDate, password))
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldUpdateToken()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var refreshToken = Guid.NewGuid().ToString();
        var expiration = DateTime.UtcNow.AddDays(30);

        // Act
        user.UpdateToken(refreshToken, expiration);

        // Assert
        user.RefreshToken.Should().Be(refreshToken);
        user.RefreshTokenExpiration.Should().Be(expiration);
    }

    [Fact]
    public void ShouldUpdateTokenWithoutExpiration()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var refreshToken = Guid.NewGuid().ToString();

        // Act
        user.UpdateToken(refreshToken);

        // Assert
        user.RefreshToken.Should().Be(refreshToken);
        user.RefreshTokenExpiration.Should().Be(null);
    }

    [Fact]
    public void ShouldThrowExceptionWhenRefreshTokenIsNullOrEmpty()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var refreshToken = string.Empty;

        // Act & Assert
        FluentActions.Invoking(() => user.UpdateToken(refreshToken))
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldUpdatePassword()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var newPassword = "newpassword";

        // Act
        user.UpdatePassword(newPassword);

        // Assert
        user.ValidatePassword(newPassword).Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowExceptionWhenPasswordIsNullOrEmptyOnUpdate()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var newPassword = string.Empty;

        // Act & Assert
        FluentActions.Invoking(() => user.UpdatePassword(newPassword))
            .Should()
            .Throw<ArgumentException>();
    }
}