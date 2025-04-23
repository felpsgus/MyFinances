using MyFinances.Domain.Enum;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

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