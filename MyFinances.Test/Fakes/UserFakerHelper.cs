using Bogus;
using MyFinances.Domain.Entities;

namespace MyFinances.Test.Fakes;

public static class UserFakerHelper
{
    private static readonly Faker<User> UserFaker = new Faker<User>()
        .CustomInstantiator(f => User.Create(
                f.Name.FirstName(),
                f.Internet.Email(),
                f.Date.BetweenDateOnly(DateOnly.FromDateTime(DateTime.Now.AddYears(-100)),
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-10))),
                f.Random.String(8)
            )
        );

    public static User GetFakeUser() => UserFaker.Generate();
}