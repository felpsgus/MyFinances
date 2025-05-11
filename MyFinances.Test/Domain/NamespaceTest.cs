using Bogus;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Test.Domain;

public class NamespaceTest
{
    private readonly Faker _faker = new();

    [Fact]
    public void ShouldCreatePersonalNamespace()
    {
        // Arrange
        var name = _faker.Person.FullName;
        var type = NamespaceType.Personal;
        var userId = Guid.NewGuid();
        Guid? familyId = null;

        // Act
        var namespaceEntity = Namespace.Create(name, type, userId, familyId);

        // Assert
        namespaceEntity.Should().NotBeNull();
        namespaceEntity.Name.Should().Be(name);
        namespaceEntity.Type.Should().Be(type);
        namespaceEntity.UserId.Should().Be(userId);
        namespaceEntity.FamilyId.Should().BeNull();
    }

    [Fact]
    public void ShouldCreateFamilyNamespace()
    {
        // Arrange
        var name = _faker.Person.FullName;
        var type = NamespaceType.Family;
        Guid? userId = null;
        var familyId = Guid.NewGuid();

        // Act
        var namespaceEntity = Namespace.Create(name, type, userId, familyId);

        // Assert
        namespaceEntity.Should().NotBeNull();
        namespaceEntity.Name.Should().Be(name);
        namespaceEntity.Type.Should().Be(type);
        namespaceEntity.UserId.Should().BeNull();
        namespaceEntity.FamilyId.Should().Be(familyId);
    }

    [Theory]
    [InlineData(null, NamespaceType.Personal, null, null)]
    [InlineData("", NamespaceType.Personal, null, null)]
    [InlineData("test", null, null, null)]
    public void ShouldThrowExceptionWhenDataInvalid(string name, NamespaceType type, Guid? userId, Guid? familyId)
    {
        // Act & Assert
        FluentActions.Invoking(() => Namespace.Create(name, type, userId, familyId))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenCreatingPersonalNamespaceWithoutUserId()
    {
        // Arrange
        var name = _faker.Person.FullName;
        var type = NamespaceType.Personal;
        Guid? userId = null;
        Guid? familyId = null;

        // Act & Assert
        FluentActions.Invoking(() => Namespace.Create(name, type, userId, familyId))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenCreatingFamilyNamespaceWithoutFamilyId()
    {
        // Arrange
        var name = _faker.Person.FullName;
        var type = NamespaceType.Family;
        Guid? userId = null;
        Guid? familyId = null;

        // Act & Assert
        FluentActions.Invoking(() => Namespace.Create(name, type, userId, familyId))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ShouldUpdateNamespaceName()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var newName = _faker.Person.FullName;

        // Act
        @namespace.Update(newName);

        // Assert
        @namespace.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldThrowExceptionWhenUpdatingNamespaceWithNullOrEmptyName(string name)
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.Update(name))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldAddTagToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var tagName = _faker.Commerce.ProductAdjective();

        // Act
        @namespace.AddTag(tagName);

        // Assert
        @namespace.Tags.Should().HaveCount(1);
        @namespace.Tags.First().Name.Should().Be(tagName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldThrowExceptionWhenAddingTagWithNullOrEmptyName(string tagName)
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddTag(tagName))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingExistingTagToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var tagName = _faker.Commerce.ProductAdjective();
        @namespace.AddTag(tagName);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddTag(tagName))
            .Should().Throw<AlreadyExistsException>();
    }

    [Fact]
    public void ShouldRemoveTagFromNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var tagName = _faker.Commerce.ProductAdjective();
        @namespace.AddTag(tagName);
        var tag = @namespace.Tags.First();

        // Act
        @namespace.RemoveTag(tag.Id);

        // Assert
        @namespace.Tags.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void ShouldThrowExceptionWhenRemovingTagWithInvalidId(string tagId)
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.RemoveTag(Guid.Parse(tagId)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenRemovingTagNotInNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var tagId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => @namespace.RemoveTag(tagId))
            .Should().Throw<NotFoundException>();
    }

    [Fact]
    public void ShouldAddExpenseToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var expense = ExpenseFakerHelper.GetUnpaidFakeExpense(@namespace.Id);

        // Act
        @namespace.AddExpense(expense);

        // Assert
        @namespace.Expenses.Should().Contain(expense);
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingNullExpenseToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddExpense(null))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingExpenseWithDifferentNamespaceId()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var expense = ExpenseFakerHelper.GetUnpaidFakeExpense(Guid.NewGuid());

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddExpense(expense))
            .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingExistingExpenseToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var expense = ExpenseFakerHelper.GetUnpaidFakeExpense(@namespace.Id);
        @namespace.AddExpense(expense);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddExpense(expense))
            .Should().Throw<AlreadyExistsException>();
    }

    [Fact]
    public void ShouldRemoveExpenseFromNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var expense = ExpenseFakerHelper.GetUnpaidFakeExpense(@namespace.Id);
        @namespace.AddExpense(expense);

        // Act
        @namespace.RemoveExpense(expense.Id);

        // Assert
        @namespace.Expenses.Should().NotContain(expense);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void ShouldThrowExceptionWhenRemovingExpenseWithInvalidId(string expenseId)
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var expense = ExpenseFakerHelper.GetUnpaidFakeExpense(@namespace.Id);
        @namespace.AddExpense(expense);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.RemoveExpense(Guid.Parse(expenseId)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenRemovingExpenseNotInNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var expenseId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => @namespace.RemoveExpense(expenseId))
            .Should().Throw<NotFoundException>();
    }

    [Fact]
    public void ShouldAddDebtToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var debt = DebtFakerHelper.GetFakeDebt(@namespace.Id);

        // Act
        @namespace.AddDebt(debt);

        // Assert
        @namespace.Debts.Should().Contain(debt);
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingNullDebtToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddDebt(null))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingDebtWithDifferentNamespaceId()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var debt = DebtFakerHelper.GetFakeDebt(Guid.NewGuid());

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddDebt(debt))
            .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingExistingDebtToNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var debt = DebtFakerHelper.GetFakeDebt(@namespace.Id);
        @namespace.AddDebt(debt);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.AddDebt(debt))
            .Should().Throw<AlreadyExistsException>();
    }

    [Fact]
    public void ShouldRemoveDebtFromNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var debt = DebtFakerHelper.GetFakeDebt(@namespace.Id);
        @namespace.AddDebt(debt);

        // Act
        @namespace.RemoveDebt(debt.Id);

        // Assert
        @namespace.Debts.Should().NotContain(debt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void ShouldThrowExceptionWhenRemovingDebtWithInvalidId(string debtId)
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var debt = DebtFakerHelper.GetFakeDebt(@namespace.Id);
        @namespace.AddDebt(debt);

        // Act & Assert
        FluentActions.Invoking(() => @namespace.RemoveDebt(Guid.Parse(debtId)))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenRemovingDebtNotInNamespace()
    {
        // Arrange
        var user = UserFakerHelper.GetFakeUser();
        var @namespace = NamespaceFakerHelper.GetFakePersonalNamespace(user.Id);
        var debtId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => @namespace.RemoveDebt(debtId))
            .Should().Throw<NotFoundException>();
    }
}