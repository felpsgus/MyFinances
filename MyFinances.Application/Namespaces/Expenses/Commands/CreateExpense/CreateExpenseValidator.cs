using FluentValidation;

namespace MyFinances.Application.Namespaces.Expenses.Commands.CreateExpense;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("Value must be greater than 0.");

        RuleFor(x => x.NamespaceId)
            .NotEmpty()
            .WithMessage("Namespace ID is required.");

        RuleFor(x => x.PaymentDate)
            .NotEmpty()
            .WithMessage("Payment date is required when the expense is paid.")
            .When(x => x.Paid);

        RuleFor(x => x.InstallmentId)
            .NotEmpty()
            .WithMessage("Installment ID is required when the expense is part of an installment.")
            .When(x => x.DebtId != null);

        RuleFor(x => x.DebtId)
            .NotEmpty()
            .WithMessage("Debt ID is required when the expense is part of a debt.")
            .When(x => x.InstallmentId != null);
    }
}