using MyFinances.Domain.Extensions;
using MyFinances.Domain.Primitives;
using MyFinances.Domain.ValueObjects;

namespace MyFinances.Domain.Entities;

public class Expense : AuditEntity
{
    private readonly List<ExpenseTag> _expenseTags = [];

    private Expense()
    {
    }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public decimal Value { get; private set; }

    public Guid NamespaceId { get; private set; }

    public bool Paid { get; private set; } = false;

    public DateOnly? PaymentDate { get; private set; }

    public Guid? ResponsiblePersonId { get; private set; }
    public User? ResponsiblePerson { get; private set; }

    public Period Period { get; private set; }

    public Guid? DebtId { get; private set; }
    public Debt? Debt { get; private set; }

    public int? InstallmentNumber { get; private set; }

    public IReadOnlyCollection<Tag> Tags => _expenseTags.Select(et => et.Tag).ToList().AsReadOnly();

    public static Expense Create(
        string name,
        string? description,
        decimal value,
        Guid namespaceId,
        bool paid = false,
        DateOnly? paymentDate = null,
        Guid? responsiblePersonId = null,
        Period? period = null,
        List<Tag>? tags = null,
        Installment? installment = null)
    {
        name.ThrowIfNullOrEmpty(nameof(name));
        value.ThrowIfLessThanOrEqualTo(0, nameof(value));
        namespaceId.ThrowIfNull(nameof(namespaceId));

        if (paid)
            paymentDate.ThrowIfNull(nameof(paymentDate));

        if (installment != null && installment.Paid != paid)
            throw new InvalidOperationException("Installment paid status must be the same as the expense paid status.");

        var expense = new Expense
        {
            Name = name,
            Description = description,
            Value = value,
            NamespaceId = namespaceId,
            Paid = paid,
            PaymentDate = paymentDate,
            ResponsiblePersonId = responsiblePersonId,
            Period = period ?? Period.Create(DateTime.UtcNow),
            DebtId = installment?.DebtId,
            InstallmentNumber = installment?.Number
        };

        if (tags == null)
            return expense;

        foreach (var tag in tags)
        {
            tag.ThrowIfNull(nameof(tag));
            expense._expenseTags.Add(ExpenseTag.Create(tag.Id, expense.Id));
        }

        return expense;
    }

    public void Update(
        string name,
        decimal value,
        string? description,
        bool paid,
        DateOnly? paymentDate,
        Period? period = null,
        List<Tag>? tags = null,
        Guid? responsiblePersonId = null,
        Installment? installment = null)
    {
        name.ThrowIfNullOrEmpty(nameof(name));
        value.ThrowIfLessThanOrEqualTo(0, nameof(value));

        if (installment != null && installment.Paid != paid)
            throw new InvalidOperationException("Installment paid status must be the same as the expense paid status.");

        Name = name;
        Value = value;
        Description = description;
        Paid = paid;
        PaymentDate = paymentDate;
        ResponsiblePersonId = responsiblePersonId;
        InstallmentNumber = installment?.Number;

        if (period != null)
            Period = period;

        UpdateTags(tags);
    }

    private void UpdateTags(List<Tag>? tags)
    {
        if (tags == null)
            return;

        var tagsToRemove = _expenseTags
            .Where(t => tags.All(tag => tag.Id != t.TagId))
            .ToList();

        foreach (var tag in tagsToRemove) _expenseTags.Remove(tag);

        var tagsToAdd = tags
            .Where(tag => _expenseTags.All(t => t.TagId != tag.Id))
            .Select(tag => ExpenseTag.Create(tag.Id, Id))
            .ToList();

        _expenseTags.AddRange(tagsToAdd);
    }

    public void Pay(DateOnly paymentDate = new())
    {
        Paid = true;
        PaymentDate = paymentDate;

        Debt?.PayInstallment((int)InstallmentNumber, paymentDate);
    }

    public void Cancel()
    {
        Paid = false;
        PaymentDate = null;

        Debt?.CancelPayment((int)InstallmentNumber);
    }
}