using System.Text.Json.Serialization;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;

namespace MyFinances.Application.Families.Views;

public class FamilyViewModel
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public List<FamilyMemberViewModel> FamilyMembers { get; init; } = new();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HaveIAccepted { get; set; }

    public static implicit operator FamilyViewModel(Family family)
    {
        return new FamilyViewModel
        {
            Id = family.Id,
            Name = family.Name,
            FamilyMembers = family.FamilyMembers.Select(m => new FamilyMemberViewModel
            {
                UserId = m.UserId,
                UserName = m.User.Name,
                IsHead = m.IsHead,
                Status = m.Status
            }).ToList()
        };
    }

    public static FamilyViewModel FromEntity(Family family, IUserContext userContext)
    {
        FamilyViewModel view = family;
        var userFamilyMember = family.FamilyMembers.FirstOrDefault(m => m.UserId == userContext.UserId);
        if (userFamilyMember != null)
            view.HaveIAccepted = userFamilyMember.Status == FamilyMembershipStatus.Accepted;
        return view;
    }
}

public class FamilyMemberViewModel
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public bool IsHead { get; set; }
    public FamilyMembershipStatus Status { get; set; }
}