using Bogus;
using MyFinances.Domain.Entities;

namespace MyFinances.Test.Fakes;

public class DebtFakerHelper
{
    public static Debt GetFakeDebt(Guid namespaceId, int installments = 1)
    {
        return new Faker<Debt>()
            .CustomInstantiator(f => Debt.CreateNotPaid(
                f.Commerce.ProductName(),
                f.Lorem.Sentence(),
                f.Random.Decimal(1, 1000),
                namespaceId,
                DateOnly.FromDateTime(f.Date.Past(1)),
                f.Random.Int(1, 31),
                installments
            )).Generate();
    }

    public static Debt GetPaidFakeDebt(Guid namespaceId, int installments = 1)
    {
        return new Faker<Debt>()
            .CustomInstantiator(f => Debt.CreatePaid(
                f.Commerce.ProductName(),
                f.Lorem.Sentence(),
                f.Random.Decimal(1, 1000),
                namespaceId,
                DateOnly.FromDateTime(f.Date.Past(1)),
                f.Random.Int(1, 31),
                DateOnly.FromDateTime(f.Date.Future(installments - 1)),
                installments
            )).Generate();
    }
}