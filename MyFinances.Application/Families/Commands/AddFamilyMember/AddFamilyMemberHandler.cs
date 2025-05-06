using FluentValidation;
using MediatR;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Services;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;
using MyFinances.Domain.Shared;

namespace MyFinances.Application.Families.Commands.AddFamilyMember;

public class AddFamilyMemberHandler : IRequestHandler<AddFamilyMemberCommand>
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public AddFamilyMemberHandler(
        IFamilyRepository familyRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _familyRepository = familyRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task Handle(AddFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var family = await _familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);

        if (family == null)
            throw new NotFoundException(nameof(Family), request.FamilyId.ToString());

        if (!family.FamilyMembers.Any(x => x.UserId == _userContext.UserId && x.IsHead))
            throw new ValidationException(ValidationMessages.User.NotHeadOfFamily);

        var user = await _userRepository.GetByEmailAsync(request.UserEmail, cancellationToken);
        family.AddFamilyMember(user.Id);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _emailService.SendEmailAsync(user.Email, "Family Member Added", $"You have been added to the family with name {family.Name}.");
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new Exception("An error occurred while adding the family member.", e);
        }
    }
}