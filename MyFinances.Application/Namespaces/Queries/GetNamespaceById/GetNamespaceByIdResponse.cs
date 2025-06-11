using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;
using MyFinances.Domain.ValueObjects;

namespace MyFinances.Application.Namespaces.Queries.GetNamespaceById;

public class GetNamespaceByIdResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public NamespaceType Type { get; set; }

    public Guid? UserId { get; set; }

    public Guid? FamilyId { get; set; }

    public List<NamespaceExpenseResponse> Expenses { get; set; } = [];

    public static implicit operator GetNamespaceByIdResponse(Namespace @namespace)
    {
        return new GetNamespaceByIdResponse
        {
            Id = @namespace.Id,
            Name = @namespace.Name,
            Type = @namespace.Type,
            UserId = @namespace.UserId,
            FamilyId = @namespace.FamilyId,
            Expenses = @namespace.Expenses.Select(e => (NamespaceExpenseResponse)e).ToList(),
        };
    }
}

public class NamespaceExpenseResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Value { get; set; }

    public DateOnly? PaymentDate { get; private set; }

    public bool Paid { get; set; }

    public string? ResponsiblePerson { get; set; }

    public Period Period { get; set; }

    public List<ExpenseTagResponse> Tags { get; set; } = [];

    public static implicit operator NamespaceExpenseResponse(Expense expense)
    {
        return new NamespaceExpenseResponse
        {
            Id = expense.Id,
            Name = expense.Name,
            Description = expense.Description ?? string.Empty,
            Value = expense.Value,
            PaymentDate = expense.PaymentDate,
            Paid = expense.Paid,
            ResponsiblePerson = expense.ResponsiblePerson?.Name,
            Period = expense.Period,
            Tags = expense.Tags.Select(t => (ExpenseTagResponse)t).ToList(),
        };
    }
}

public class ExpenseTagResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public static implicit operator ExpenseTagResponse(Tag tag)
    {
        return new ExpenseTagResponse
        {
            Id = tag.Id,
            Name = tag.Name,
        };
    }
}