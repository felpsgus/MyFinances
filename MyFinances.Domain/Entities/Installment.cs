using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Installment : AuditEntity
{
    private Installment()
    {
    }

    public int Number { get; private set; }

    public decimal Value { get; private set; }

    public DateOnly DueDate { get; private set; }

    public bool Paid { get; private set; } = false;

    public DateOnly? PaymentDate { get; private set; }

    public Guid DebtId { get; private set; }

    internal static Installment Create(
        int number,
        decimal value,
        DateOnly dueDate,
        Guid debtId,
        bool paid = false,
        DateOnly? paymentDate = null)
    {
        return new Installment
        {
            Number = number,
            Value = value,
            DueDate = dueDate,
            Paid = paid,
            PaymentDate = paymentDate,
            DebtId = debtId
        };
    }

    internal void Pay(DateOnly paymentDate = new())
    {
        Paid = true;
        PaymentDate = paymentDate;
    }

    internal void Cancel()
    {
        Paid = false;
        PaymentDate = null;
    }
}