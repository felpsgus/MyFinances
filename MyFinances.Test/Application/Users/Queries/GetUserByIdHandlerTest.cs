using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Users.Queries.GetUserById;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Users.Queries;

public class GetUserByIdHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUserContext> _userContext;
    private readonly GetUserByIdHandler _handler;

    public GetUserByIdHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _userContext = new Mock<IUserContext>();
        _handler = new GetUserByIdHandler(_userRepository.Object, _userContext.Object);
    }

    [Fact]
    public async Task ShouldGetUserById()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = UserFakerHelper.GetFakeUser();
        var query = new GetUserByIdQuery();

        _userContext
            .Setup(uc => uc.UserId)
            .Returns(userId);
        _userRepository
            .Setup(ur => ur.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _userRepository.Verify(ur => ur.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _userContext.Verify(uc => uc.UserId, Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery();

        _userContext
            .Setup(uc => uc.UserId)
            .Returns(userId);
        _userRepository
            .Setup(ur => ur.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _userRepository.Verify(ur => ur.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _userContext.Verify(uc => uc.UserId, Times.Once);
    }
}