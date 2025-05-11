using Bogus;
using MyFinances.Domain.Entities;

namespace MyFinances.Test.Fakes;

public static class ExpenseFakerHelper
{
    public static Expense GetUnpaidFakeExpense(Guid namespaceId)
    {
        return new Faker<Expense>()
            .CustomInstantiator(f => Expense.Create(
                f.Commerce.ProductName(),
                f.Lorem.Sentence(),
                f.Random.Decimal(1, 1000),
                namespaceId
            )).Generate();
    }
}