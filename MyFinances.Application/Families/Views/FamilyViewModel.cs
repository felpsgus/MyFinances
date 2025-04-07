using AutoMapper;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;

namespace MyFinances.Application.Families.Views;

public class FamilyViewModel
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public List<FamilyMemberViewModel> FamilyMembers { get; init; } = new();
}

public class FamilyMemberViewModel
{
    public string UserName { get; set; }
    public bool IsHead { get; set; }
    public FamilyMembershipStatus Status { get; set; }
}

public class FamilyMapper : Profile
{
    public FamilyMapper()
    {
        CreateMap<Family, FamilyViewModel>()
            .ForMember(f => f.FamilyMembers,
                opt => opt.MapFrom(f => f.FamilyMembers.Select(m => new FamilyMemberViewModel
                {
                    UserName = m.User.Name,
                    IsHead = m.IsHead,
                    Status = m.Status
                })));
    }
}