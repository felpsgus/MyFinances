using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Family : AuditEntity
{
    public string Name { get; private set; } = string.Empty;

    private readonly List<FamilyMember> _familyMembers = [];
    public IReadOnlyCollection<FamilyMember> FamilyMembers => _familyMembers.AsReadOnly();

    private Family()
    {
    }

    public static Family Create(string name)
    {
        return new Family()
        {
            Name = name
        };
    }

    public void AddFamilyMember(Guid userId)
    {
        var familyMember = new FamilyMember(userId, Id, FamilyMembers.Count == 0);
        _familyMembers.Add(familyMember);
    }

    public void RemoveFamilyMember(Guid userId)
    {
        var familyMember = FamilyMembers.FirstOrDefault(fm => fm.UserId == userId);
        if (familyMember?.IsHead == true)
        {
            var newHead = FamilyMembers.FirstOrDefault(fm => fm.UserId != userId);
            if (newHead != null) newHead.IsHead = true;
        }

        if (familyMember != null)
        {
            _familyMembers.Remove(familyMember);
        }
    }
}