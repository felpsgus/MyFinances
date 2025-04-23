using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Families.Commands.AcceptFamilyMembership;
using MyFinances.Domain.Enum;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Families.Commands;

public class AcceptedFamilyMembershipHandlerTest
{
    private readonly Mock<IFamilyRepository> _familyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly AcceptFamilyMembershipHandler _handler;

    public AcceptedFamilyMembershipHandlerTest()
    {
        _familyRepositoryMock = new Mock<IFamilyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new AcceptFamilyMembershipHandler(
            _familyRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _userContextMock.Object);
    }

    [Fact]
    public async Task ShouldAcceptFamilyMembership()
    {
        // Arrange
        var fakeFamily = FamilyFakerHelper.GetFakeFamilyWithMembers(3);
        var pendingFamilyMember = fakeFamily.FamilyMembers.First(fm => fm.Status == FamilyMembershipStatus.Pending);
        var command = new AcceptFamilyMembershipCommand(fakeFamily.Id);

        _familyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeFamily);
        _userContextMock
            .Setup(uc => uc.UserId)
            .Returns(pendingFamilyMember.UserId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _familyRepositoryMock.Verify(repo => repo.GetByIdAsync(fakeFamily.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _userContextMock.Verify(uc => uc.UserId, Times.Once);
        fakeFamily.FamilyMembers.Should().HaveCount(3);
        fakeFamily.FamilyMembers.Where(fm => fm.Status == FamilyMembershipStatus.Accepted).Should().HaveCount(2);
    }
}