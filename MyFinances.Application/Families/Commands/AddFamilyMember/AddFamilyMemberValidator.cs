using FluentValidation;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Families.Commands.AddFamilyMember;

public class AddFamilyMemberValidator : AbstractValidator<AddFamilyMemberCommand>
{
    public AddFamilyMemberValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family Id is required.");

        RuleFor(x => x.UserEmail)
            .NotEmpty()
            .WithMessage("User email is required.")
            .MustAsync(async (email, cancellation) => await userRepository.GetByEmailAsync(email, cancellation) != null)
            .WithMessage("User does not exist.");
    }
}