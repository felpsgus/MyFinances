namespace MyFinances.Domain.Entities;

public class ExpenseTag
{
    private ExpenseTag()
    {
    }

    public Guid TagId { get; set; }
    public Tag Tag { get; set; }
    public Guid ExpenseId { get; set; }

    internal static ExpenseTag Create(Guid tagId, Guid expenseId)
    {
        return new ExpenseTag
        {
            TagId = tagId,
            ExpenseId = expenseId
        };
    }
}