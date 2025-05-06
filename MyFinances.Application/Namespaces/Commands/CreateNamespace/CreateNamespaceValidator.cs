using FluentValidation;
using MyFinances.Domain.Enum;

namespace MyFinances.Application.Namespaces.Commands.CreateNamespace;

public class CreateNamespaceValidator : AbstractValidator<CreateNamespaceCommand>
{
    public CreateNamespaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid namespace type.");

        When(x => x.Type == NamespaceType.Personal, () =>
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage("User ID is required for personal namespace.");
        });

        When(x => x.Type == NamespaceType.Family, () =>
        {
            RuleFor(x => x.FamilyId)
                .NotNull()
                .WithMessage("Family ID is required for family namespace.");
        });
    }
}