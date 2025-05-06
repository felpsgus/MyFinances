using FluentValidation;
using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Services;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;
using MyFinances.Domain.Shared;

namespace MyFinances.Application.Families.Commands.RemoveFamilyMember;

public class RemoveFamilyMemberHandler : IRequestHandler<RemoveFamilyMemberCommand>
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IUserContext _userContext;

    public RemoveFamilyMemberHandler(IFamilyRepository familyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailService emailService, IUserContext userContext)
    {
        _familyRepository = familyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _userContext = userContext;
    }

    public async Task Handle(RemoveFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var family = await _familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);

        if (family == null)
            throw new NotFoundException(nameof(Family), request.FamilyId.ToString());

        if (!family.FamilyMembers.Any(x => x.UserId == _userContext.UserId && x.IsHead))
            throw new ValidationException(ValidationMessages.User.NotHeadOfFamily);

        var user = await _userRepository.GetByIdAsync(request.MemberId, cancellationToken);
        family.RemoveFamilyMember(user.Id);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _emailService.SendEmailAsync(user.Email, "Family Member Removed", $"You have been removed from the family with name {family.Name}.");
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new Exception("An error occurred while removing the family member.", e);
        }
    }
}