using FluentValidation;
using MyFinances.Application.Abstractions.Repositories;

namespace MyFinances.Application.Families.Commands.RemoveFamilyMember;

public class RemoveFamilyMemberValidator : AbstractValidator<RemoveFamilyMemberCommand>
{
    public RemoveFamilyMemberValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID cannot be empty.");

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID cannot be empty.")
            .MustAsync(async (id, cancellation) => await userRepository.ExistsAsync(id, cancellation))
            .WithMessage("Member does not exist.");
    }
}