using MyFinances.Application.Users.Commands.CreateUser;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Users.Commands;

public class CreateUserHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateUserHandler(_userRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task ShouldCreateUser()
    {
        // Arrange
        var fakeUser = UserFakerHelper.GetFakeUser();
        var command = new CreateUserCommand(
            fakeUser.Name,
            fakeUser.Email,
            fakeUser.BirthDate,
            fakeUser.Password
        );

        _userRepository
            .Setup(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepository.Verify(ur => ur.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().Be(fakeUser.Id);
    }
}