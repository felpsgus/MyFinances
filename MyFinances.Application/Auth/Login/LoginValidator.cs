using FluentValidation;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Auth.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator(IUserRepository userRepository)
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("E-mail is required.")
            .EmailAddress()
            .WithMessage("Invalid e-mail.")
            .MustAsync(async (email, cancelationToken) =>
                await userRepository.GetByEmailAsync(email, cancelationToken) != null)
            .WithMessage("E-mail not found.");

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}