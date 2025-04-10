using Bogus;
using MyFinances.Domain.Entities;

namespace MyFinances.Test.Fakes;

public static class FamilyFakerHelper
{
    private static readonly Faker<Family> FamilyFaker = new Faker<Family>()
        .CustomInstantiator(f => new Family(f.Name.LastName()));

    public static Family GetFakeFamily()
    {
        var family = FamilyFaker.Generate();
        family.AddFamilyMember(Guid.NewGuid());
        return family;
    }

    public static Family GetFakeFamilyWithMembers(int numberOfMembers)
    {
        var family = FamilyFaker.Generate();

        for (int i = 0; i < numberOfMembers; i++)
        {
            family.AddFamilyMember(Guid.NewGuid());
        }

        return family;
    }

    public static List<Family> GetFakeFamilies(int numberOfFamilies)
    {
        var families = new List<Family>();
        for (int i = 0; i < numberOfFamilies; i++)
        {
            var family = GetFakeFamily();
            foreach (var familyMember in family.FamilyMembers)
            {
                familyMember.User = UserFakerHelper.GetFakeUser();
                familyMember.UserId = familyMember.User.Id;
            }
            families.Add(family);
        }

        return families;
    }
}