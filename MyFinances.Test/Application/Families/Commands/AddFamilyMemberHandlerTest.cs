using Castle.Core.Smtp;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Families.Commands.AddFamilyMember;

namespace MyFinances.Test.Application.Families.Commands;

public class AddFamilyMemberHandlerTest
{
    private readonly Mock<IFamilyRepository> _familyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly AddFamilyMemberHandler _addFamilyMemberHandler;

    public AddFamilyMemberHandlerTest()
    {
        _familyRepositoryMock = new Mock<IFamilyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _emailServiceMock = new Mock<IEmailService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _addFamilyMemberHandler = new AddFamilyMemberHandler(
            _familyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _emailServiceMock.Object,
            _userRepositoryMock.Object
        );
    }

    [Fact]
    public async Task ShouldAddFamilyMember()
    {
        // Arrange
        var fakeFamily = FamilyFakerHelper.GetFakeFamily();
        var fakeUser = UserFakerHelper.GetFakeUser();
        var command = new AddFamilyMemberCommand(fakeFamily.Id, fakeUser.Email);

        _familyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeFamily);
        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(fakeUser.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUser);

        // Act
        await _addFamilyMemberHandler.Handle(command, CancellationToken.None);

        // Assert
        _familyRepositoryMock.Verify(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(fakeUser.Email, It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _emailServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        fakeFamily.FamilyMembers.Should().HaveCount(2);
        fakeFamily.FamilyMembers.Should().ContainSingle(fm => fm.UserId == fakeUser.Id);
    }
}