using FluentValidation;
using MyFinances.Application.Abstractions.Repositories;

namespace MyFinances.Application.Users.Commands.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator(IUserRepository userRepository)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(p => p.Email)
            .EmailAddress()
            .WithMessage("Invalid e-mail.")
            .NotEmpty()
            .WithMessage("E-mail is required.")
            .MustAsync(async (email, cancellationToken) =>
                await userRepository.GetByEmailAsync(email, cancellationToken) == null)
            .WithMessage("E-mail already in use.");

        RuleFor(p => p.BirthDate)
            .NotEmpty()
            .WithMessage("Birth date is required.");

        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Minimum length is 6 characters.");
    }
}