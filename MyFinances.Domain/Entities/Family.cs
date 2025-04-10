using MyFinances.Domain.Enum;
using MyFinances.Domain.Shared;

namespace MyFinances.Domain.Entities;

public class Family : AuditEntity
{
    public string Name { get; private set; } = string.Empty;
    private readonly List<FamilyMember> _familyMembers = [];
    public IReadOnlyCollection<FamilyMember> FamilyMembers => _familyMembers.AsReadOnly();

    private Family()
    {
    }

    public Family(string name)
    {
        Name = name;
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

public class FamilyMember : Entity
{
    public Guid UserId { get; set; }
    public User? User { get; set; } = null!;

    public Guid FamilyId { get; set; }
    public Family? Family { get; set; } = null!;

    public bool IsHead { get; set; }
    public FamilyMembershipStatus Status { get; set; } = FamilyMembershipStatus.Pending;

    private FamilyMember()
    {
    }

    public FamilyMember(Guid userId, Guid familyId, bool isHead)
    {
        UserId = userId;
        FamilyId = familyId;
        IsHead = isHead;
        Status = isHead ? FamilyMembershipStatus.Accepted : FamilyMembershipStatus.Pending;
    }

    public void Accept()
    {
        Status = FamilyMembershipStatus.Accepted;
    }

    public void Refused()
    {
        Status = FamilyMembershipStatus.Refused;
    }
}