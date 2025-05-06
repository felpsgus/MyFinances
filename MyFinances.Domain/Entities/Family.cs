using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Family : AuditEntity
{
    private readonly List<FamilyMember> _familyMembers = [];

    private Family()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyList<FamilyMember> FamilyMembers => _familyMembers;

    public static Family Create(string name)
    {
        return new Family()
        {
            Name = name
        };
    }

    public void AddFamilyMember(Guid userId)
    {
        var familyMember = FamilyMember.Create(userId, Id, FamilyMembers.Count == 0);
        _familyMembers.Add(familyMember);
    }

    public void RemoveFamilyMember(Guid userId)
    {
        var familyMember = FamilyMembers.FirstOrDefault(fm => fm.UserId == userId);

        if (familyMember == null) throw new NotFoundException(nameof(FamilyMember), userId.ToString());

        if (familyMember?.IsHead == true)
        {
            var newHead = FamilyMembers.FirstOrDefault(fm => fm.UserId != userId);
            if (newHead != null) newHead.IsHead = true;
        }

        _familyMembers.Remove(familyMember);
    }
}