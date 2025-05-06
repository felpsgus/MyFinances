using FluentValidation;

namespace MyFinances.Application.Namespaces.Commands.UpdateNamespace;

public class UpdateNamespaceValidator : AbstractValidator<UpdateNamespaceCommand>
{
    public UpdateNamespaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Namespace name is required.")
            .MaximumLength(100)
            .WithMessage("Namespace name must not exceed 100 characters.");
    }
}