using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Extensions;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Debt : AuditEntity
{
    private readonly List<Installment> _installments = [];

    private Debt()
    {
    }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public decimal Value { get; private set; }

    public Guid NamespaceId { get; private set; }

    public DateOnly StartDate { get; private set; }

    public int DueDay { get; private set; } = 1;

    public bool Paid { get; private set; } = false;

    public int Installments { get; private set; } = 1;

    public decimal ValuePaid { get; private set; } = 0;

    public DateOnly? PaymentDate { get; private set; }

    public IReadOnlyCollection<Installment> InstallmentsList => _installments.AsReadOnly();

    public static Debt CreateNotPaid(
        string name,
        string? description,
        decimal value,
        Guid namespaceId,
        DateOnly startDate,
        int dueDay,
        int installments = 1)
    {
        name.ThrowIfNullOrEmpty(nameof(name));
        value.ThrowIfLessThanOrEqualTo(0, nameof(value));
        namespaceId.ThrowIfNull(nameof(namespaceId));
        startDate.ThrowIfNull(nameof(startDate));
        dueDay.ThrowIfOutOfRange(1, 31, nameof(dueDay));
        installments.ThrowIfLessThanOrEqualTo(0, nameof(installments));

        var instance = new Debt
        {
            Name = name,
            Description = description,
            Value = value,
            StartDate = startDate,
            Installments = installments,
            DueDay = dueDay,
            NamespaceId = namespaceId
        };
        instance.GenerateInstallments();

        return instance;
    }

    public static Debt CreatePaid(
        string name,
        string? description,
        decimal value,
        Guid namespaceId,
        DateOnly startDate,
        int dueDay,
        DateOnly paymentDate,
        int installments = 1)
    {
        name.ThrowIfNullOrEmpty(nameof(name));
        value.ThrowIfLessThanOrEqualTo(0, nameof(value));
        namespaceId.ThrowIfNull(nameof(namespaceId));
        startDate.ThrowIfNull(nameof(startDate));
        dueDay.ThrowIfOutOfRange(1, 31, nameof(dueDay));
        paymentDate.ThrowIfNull(nameof(paymentDate));

        var instance = new Debt
        {
            Name = name,
            Description = description,
            Value = value,
            StartDate = startDate,
            Installments = installments,
            DueDay = dueDay,
            NamespaceId = namespaceId,
            Paid = true,
            ValuePaid = value,
            PaymentDate = paymentDate
        };

        instance.GenerateInstallments();

        return instance;
    }

    private void GenerateInstallments()
    {
        var baseValue = Math.Round(Value / Installments * 100) / 100;
        var diference = (int)Math.Round(Value - baseValue) * 100;
        var dueDate = new DateOnly(StartDate.Year, StartDate.Month + 1, DueDay);

        for (var i = 1; i <= Installments; i++)
        {
            var installmentValue = baseValue;
            if (i <= diference)
                installmentValue += 0.01m;

            var installment = Installment.Create(
                i,
                installmentValue,
                dueDate,
                Id,
                Paid,
                Paid ? dueDate : null);

            _installments.Add(installment);
            dueDate = dueDate.AddMonths(1);
        }
    }

    public void PayInstallment(int installmentNumber, DateOnly paymentDate = new())
    {
        installmentNumber.ThrowIfOutOfRange(1, Installments, nameof(installmentNumber));

        var installment = _installments.FirstOrDefault(i => i.Number == installmentNumber);
        if (installment is null)
            throw new NotFoundException(typeof(Installment));

        if (installment.Paid)
            throw new InvalidOperationException($"Installment number {installmentNumber} is already paid.");

        installment.Pay(paymentDate);
        ValuePaid += installment.Value;

        if (ValuePaid < Value) return;

        Paid = true;
        PaymentDate = _installments.OrderByDescending(i => i.PaymentDate).FirstOrDefault()?.PaymentDate;
    }

    public void CancelPayment(int installmentNumber)
    {
        installmentNumber.ThrowIfOutOfRange(1, Installments, nameof(installmentNumber));

        var installment = _installments.FirstOrDefault(i => i.Number == installmentNumber);
        if (installment is null)
            throw new NotFoundException(typeof(Installment));

        if (!installment.Paid)
            throw new InvalidOperationException($"Installment number {installmentNumber} is not paid.");

        installment.Cancel();
        ValuePaid -= installment.Value;

        Paid = false;
        PaymentDate = null;
    }
}