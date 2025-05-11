using System.ComponentModel;
using MyFinances.Domain.Enum;
using MyFinances.Domain.Extensions;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

[DisplayName("Family Member")]
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
        userId.ThrowIfDefault(nameof(userId));
        familyId.ThrowIfDefault(nameof(familyId));
        isHead.ThrowIfDefault(nameof(isHead));

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