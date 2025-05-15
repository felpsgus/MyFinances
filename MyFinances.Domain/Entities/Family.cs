using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Extensions;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Family : AuditEntity
{
    private readonly List<FamilyMember> _familyMembers = [];

    private Family()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<FamilyMember> FamilyMembers => _familyMembers.AsReadOnly();

    public static Family Create(string name)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        return new Family
        {
            Name = name
        };
    }

    public void AddFamilyMember(Guid userId)
    {
        userId.ThrowIfDefault(nameof(userId));

        if (FamilyMembers.Any(fm => fm.UserId == userId))
            throw new AlreadyExistsException(typeof(FamilyMember), userId);

        var familyMember = FamilyMember.Create(userId, Id, FamilyMembers.Count == 0);
        _familyMembers.Add(familyMember);
    }

    public void RemoveFamilyMember(Guid userId)
    {
        userId.ThrowIfDefault(nameof(userId));

        var familyMember = FamilyMembers.FirstOrDefault(fm => fm.UserId == userId);

        if (familyMember == null) throw new NotFoundException(typeof(FamilyMember), userId);

        if (familyMember?.IsHead == true)
        {
            var newHead = FamilyMembers.FirstOrDefault(fm => fm.UserId != userId);
            if (newHead != null) newHead.IsHead = true;
        }

        _familyMembers.Remove(familyMember);
    }
}