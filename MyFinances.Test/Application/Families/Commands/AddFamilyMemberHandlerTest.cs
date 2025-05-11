using FluentValidation;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Services;
using MyFinances.Application.Families.Commands.AddFamilyMember;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;
using MyFinances.Domain.Shared;

namespace MyFinances.Test.Application.Families.Commands;

public class AddFamilyMemberHandlerTest
{
    private readonly Mock<IFamilyRepository> _familyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly AddFamilyMemberHandler _handler;

    public AddFamilyMemberHandlerTest()
    {
        _familyRepositoryMock = new Mock<IFamilyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _emailServiceMock = new Mock<IEmailService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new AddFamilyMemberHandler(
            _familyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _emailServiceMock.Object,
            _userRepositoryMock.Object,
            _userContextMock.Object
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
        _userContextMock
            .Setup(uc => uc.UserId)
            .Returns(fakeFamily.FamilyMembers.First().UserId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

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

    [Fact]
    public async Task ShouldThrowNotFoundException()
    {
        // Arrange
        var fakeFamily = FamilyFakerHelper.GetFakeFamily();
        var fakeUser = UserFakerHelper.GetFakeUser();
        var command = new AddFamilyMemberCommand(fakeFamily.Id, fakeUser.Email);

        _familyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Family?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _familyRepositoryMock.Verify(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(fakeUser.Id, It.IsAny<CancellationToken>()),
            Times.Never);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _emailServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        _userContextMock.Verify(uc => uc.UserId, Times.Never);
        fakeFamily.FamilyMembers.Should().HaveCount(1);
    }

    [Fact]
    public async Task ShouldThrowNotHeadOfFamilyException()
    {
        // Arrange
        var fakeFamily = FamilyFakerHelper.GetFakeFamily();
        var fakeUser = UserFakerHelper.GetFakeUser();
        var command = new AddFamilyMemberCommand(fakeFamily.Id, fakeUser.Email);

        _familyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeFamily);
        _userContextMock
            .Setup(uc => uc.UserId)
            .Returns(Guid.NewGuid());

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>().WithMessage(ValidationMessages.User.NotHeadOfFamily);
        _familyRepositoryMock.Verify(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(fakeUser.Email, It.IsAny<CancellationToken>()),
            Times.Never);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _emailServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        _userContextMock.Verify(uc => uc.UserId, Times.Once);
        fakeFamily.FamilyMembers.Should().HaveCount(1);
    }
}