using MyFinances.Domain.Enum;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Extensions;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Namespace : AuditEntity
{
    private readonly List<Tag> _tags = [];
    private readonly List<Expense> _expenses = [];
    private readonly List<Debt> _debts = [];

    private Namespace()
    {
    }

    public string Name { get; private set; }

    public NamespaceType Type { get; private set; }

    public Guid? UserId { get; private set; }
    public User? User { get; private set; }

    public Guid? FamilyId { get; private set; }
    public Family? Family { get; private set; }

    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

    public IReadOnlyCollection<Debt> Debts => _debts.AsReadOnly();

    public static Namespace Create(string name, NamespaceType type, Guid? userId, Guid? familyId)
    {
        name.ThrowIfNullOrEmpty(nameof(name));
        type.ThrowIfDefault(nameof(type));

        if (type == NamespaceType.Personal && userId == null)
            throw new ArgumentNullException(nameof(userId), "User ID cannot be null for personal namespace.");

        if (type == NamespaceType.Family && familyId == null)
            throw new ArgumentNullException(nameof(familyId), "Family ID cannot be null for family namespace.");

        return new Namespace
        {
            Name = name,
            Type = type,
            UserId = userId,
            FamilyId = familyId
        };
    }

    public void Update(string name)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        Name = name;
    }

    public void AddTag(string tagName)
    {
        tagName.ThrowIfNullOrEmpty(nameof(tagName));

        if (_tags.Any(t => t.Name == tagName))
            throw new AlreadyExistsException(typeof(Tag), nameof(Tag.Name), tagName);

        var tag = Tag.Create(tagName, Id);
        _tags.Add(tag);
    }

    public void RemoveTag(Guid tagId)
    {
        tagId.ThrowIfDefault(nameof(tagId));

        var tag = _tags.FirstOrDefault(t => t.Id == tagId);
        if (tag == null)
            throw new NotFoundException(typeof(Tag), tagId);

        _tags.Remove(tag);
    }

    public void AddExpense(Expense expense)
    {
        expense.ThrowIfNull(nameof(expense));

        if (expense.NamespaceId != Id)
            throw new InvalidOperationException("Expense does not belong to this namespace.");

        if (_expenses.Any(e => e.Id == expense.Id))
            throw new AlreadyExistsException(typeof(Expense), expense.Id);

        _expenses.Add(expense);
    }

    public void RemoveExpense(Guid expenseId)
    {
        expenseId.ThrowIfDefault(nameof(expenseId));

        var expense = _expenses.FirstOrDefault(e => e.Id == expenseId);
        if (expense == null)
            throw new NotFoundException(typeof(Expense), expenseId);

        _expenses.Remove(expense);
    }

    public void AddDebt(Debt debt)
    {
        debt.ThrowIfNull(nameof(debt));

        if (debt.NamespaceId != Id)
            throw new InvalidOperationException("Debt does not belong to this namespace.");

        if (_debts.Any(d => d.Id == debt.Id))
            throw new AlreadyExistsException(typeof(Debt), debt.Id);

        _debts.Add(debt);
    }

    public void RemoveDebt(Guid debtId)
    {
        debtId.ThrowIfDefault(nameof(debtId));

        var debt = _debts.FirstOrDefault(d => d.Id == debtId);
        if (debt == null)
            throw new NotFoundException(typeof(Debt), debtId);

        _debts.Remove(debt);
    }
}