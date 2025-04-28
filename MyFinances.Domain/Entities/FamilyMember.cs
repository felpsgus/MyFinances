using MyFinances.Domain.Enum;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class FamilyMember : Entity
{
    private FamilyMember()
    {
    }

    private FamilyMember(Guid userId, Guid familyId, bool isHead)
    {
        UserId = userId;
        FamilyId = familyId;
        IsHead = isHead;
        Status = isHead ? FamilyMembershipStatus.Accepted : FamilyMembershipStatus.Pending;
    }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid FamilyId { get; set; }
    public Family? Family { get; set; }

    public bool IsHead { get; set; }
    public FamilyMembershipStatus Status { get; set; } = FamilyMembershipStatus.Pending;

    internal static FamilyMember Create(Guid userId, Guid familyId, bool isHead)
    {
        return new FamilyMember(userId, familyId, isHead);
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