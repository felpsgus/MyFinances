using FluentValidation;
using MyFinances.Application.Abstractions.Repositories;

namespace MyFinances.Application.Families.Commands.AddFamilyMember;

public class AddFamilyMemberValidator : AbstractValidator<AddFamilyMemberCommand>
{
    public AddFamilyMemberValidator(IUserRepository userRepository, IFamilyRepository familyRepository)
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .MustAsync(async (email, cancellation) =>
                await userRepository.GetByEmailAsync(email, cancellation) != null)
            .WithMessage("User does not exist.");
    }
}