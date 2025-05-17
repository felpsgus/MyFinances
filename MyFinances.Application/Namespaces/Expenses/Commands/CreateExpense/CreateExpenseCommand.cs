using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.ValueObjects;

namespace MyFinances.Application.Namespaces.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand : ICommand
{
    public CreateExpenseCommand(
        string name,
        string? description,
        decimal value,
        Guid namespaceId,
        bool paid = false,
        DateOnly? paymentDate = null,
        Guid? responsiblePersonId = null,
        Period? period = null,
        List<Guid>? tags = null,
        Guid? installmentId = null,
        Guid? debtId = null)
    {
        Name = name;
        Description = description;
        Value = value;
        NamespaceId = namespaceId;
        Paid = paid;
        PaymentDate = paymentDate;
        ResponsiblePersonId = responsiblePersonId;
        Period = period;
        Tags = tags;
        InstallmentId = installmentId;
        DebtId = debtId;
    }

    public string Name { get; init; }

    public string? Description { get; init; }

    public decimal Value { get; init; }

    [JsonIgnore]
    public Guid NamespaceId { get; init; }

    public bool Paid { get; init; } = false;

    public DateOnly? PaymentDate { get; init; }

    public Guid? ResponsiblePersonId { get; init; }

    public Period? Period { get; init; }

    public List<Guid>? Tags { get; init; }

    public Guid? InstallmentId { get; init; }

    public Guid? DebtId { get; init; }
}