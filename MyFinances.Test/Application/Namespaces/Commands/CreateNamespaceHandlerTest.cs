using MyFinances.Application.Namespaces.Commands.CreateNamespace;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Test.Application.Namespaces.Commands;

public class CreateNamespaceHandlerTest
{
    private readonly Mock<INamespaceRepository> _namespaceRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IFamilyRepository> _familyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateNamespaceHandler _handler;

    public CreateNamespaceHandlerTest()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _familyRepositoryMock = new Mock<IFamilyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateNamespaceHandler(
            _namespaceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _familyRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task ShouldCreatePersonalNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var command = new CreateNamespaceCommand(
            @namespace.Name,
            @namespace.Type,
            @namespace.UserId,
            null
        );

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _namespaceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Namespace>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(@namespace);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(repo => repo.ExistsAsync(user.Id, It.IsAny<CancellationToken>()), Times.Once);
        _familyRepositoryMock.Verify(repo => repo.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _namespaceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Namespace>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().Be(@namespace.Id);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFound()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var command = new CreateNamespaceCommand(
            @namespace.Name,
            @namespace.Type,
            @namespace.UserId,
            null
        );

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var exception = new NotFoundException(typeof(User), user.Id);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exception.Message);
        _userRepositoryMock.Verify(repo => repo.ExistsAsync(user.Id, It.IsAny<CancellationToken>()), Times.Once);
        _familyRepositoryMock.Verify(repo => repo.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _namespaceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Namespace>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldCreateFamilyNamespace()
    {
        // Arrange
        var family = FamilyFakerHelper.GetFakeFamily();
        var @namespace = NamespaceFakerHelper.GetFakeFamilyNamespace(family.Id);
        var command = new CreateNamespaceCommand(
            @namespace.Name,
            @namespace.Type,
            null,
            @namespace.FamilyId
        );

        _familyRepositoryMock
            .Setup(repo => repo.ExistsAsync(family.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _namespaceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Namespace>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(@namespace);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(repo => repo.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _familyRepositoryMock.Verify(repo => repo.ExistsAsync(family.Id, It.IsAny<CancellationToken>()), Times.Once);
        _namespaceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Namespace>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().Be(@namespace.Id);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenFamilyNotFound()
    {
        // Arrange
        var family = FamilyFakerHelper.GetFakeFamily();
        var @namespace = NamespaceFakerHelper.GetFakeFamilyNamespace(family.Id);
        var command = new CreateNamespaceCommand(
            @namespace.Name,
            @namespace.Type,
            null,
            @namespace.FamilyId
        );

        _familyRepositoryMock
            .Setup(repo => repo.ExistsAsync(family.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var exception = new NotFoundException(typeof(Family), family.Id);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(exception.Message);
        _userRepositoryMock.Verify(repo => repo.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _familyRepositoryMock.Verify(repo => repo.ExistsAsync(family.Id, It.IsAny<CancellationToken>()), Times.Once);
        _namespaceRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Namespace>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}