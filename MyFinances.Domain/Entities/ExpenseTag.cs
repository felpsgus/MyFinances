namespace MyFinances.Domain.Entities;

public class ExpenseTag
{
    private ExpenseTag()
    {
    }

    public Guid TagId { get; set; }
    public Tag Tag { get; set; }

    public Guid ExpenseId { get; set; }
    public Expense Expense { get; set; }

    internal static ExpenseTag Create(Tag tag, Guid expenseId)
    {
        return new ExpenseTag
        {
            TagId = tag.Id,
            Tag = tag,
            ExpenseId = expenseId
        };
    }
}