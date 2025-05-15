using Bogus;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Enum;

namespace MyFinances.Test.Fakes;

public class NamespaceFakerHelper
{
    private static readonly Faker Faker = new();

    public static Namespace GetFakePersonalNamespace(Guid userGuid)
    {
        var faker = new Faker<Namespace>()
            .CustomInstantiator(f => Namespace.Create(
                f.Lorem.Sentence(10),
                NamespaceType.Personal,
                userGuid,
                null
            ));

        return faker.Generate();
    }

    public static Namespace GetFakePersonalNamespaceWithTags(Guid userGuid)
    {
        var faker = new Faker<Namespace>()
            .CustomInstantiator(f => Namespace.Create(
                f.Lorem.Sentence(10),
                NamespaceType.Personal,
                userGuid,
                null
            )).Generate();

        var tags = Faker.Random
            .WordsArray(5)
            .ToList();
        foreach (var tag in tags)
        {
            faker.AddTag(tag);
        }

        return faker;
    }

    public static Namespace GetFakeFamilyNamespace(Guid familyGuid)
    {
        var faker = new Faker<Namespace>()
            .CustomInstantiator(f => Namespace.Create(
                f.Lorem.Sentence(10),
                NamespaceType.Family,
                null,
                familyGuid
            ));

        return faker.Generate();
    }
}