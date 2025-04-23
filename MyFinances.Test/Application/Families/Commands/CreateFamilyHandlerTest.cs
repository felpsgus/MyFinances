using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Families.Commands.CreateFamily;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Families.Commands;

public class CreateFamilyHandlerTest
{
    private readonly Mock<IFamilyRepository> _familyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly CreateFamilyHandler _handler;

    public CreateFamilyHandlerTest()
    {
        _familyRepositoryMock = new Mock<IFamilyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new CreateFamilyHandler(_familyRepositoryMock.Object, _unitOfWorkMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task ShouldCreateFamily()
    {
        // Arrange
        var fakeFamily = FamilyFakerHelper.GetFakeFamily();
        var command = new CreateFamilyCommand(fakeFamily.Name);

        _familyRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Family>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeFamily);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _familyRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Family>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _userContextMock.Verify(uc => uc.UserId, Times.Once);
        fakeFamily.FamilyMembers.Should().HaveCount(1);
        result.Should().Be(fakeFamily.Id);
    }
}