using MediatR;
using MyFinances.Domain.Entities;
using MyFinances.Domain.Exceptions;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Expenses.Commands.CreateExpense;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand>
{
    private readonly INamespaceRepository _namespaceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateExpenseHandler(INamespaceRepository namespaceRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _namespaceRepository = namespaceRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var @namespace = await _namespaceRepository.GetByIdAsync(request.NamespaceId, cancellationToken);
        if (@namespace == null)
            throw new NotFoundException(typeof(Namespace), request.NamespaceId);

        var tagsToAdd = new List<Tag>();
        if (request.Tags != null)
        {
            var tags = await _namespaceRepository.GetTagsAsync(request.NamespaceId, cancellationToken);
            foreach (var tagId in request.Tags)
            {
                var tag = tags.FirstOrDefault(t => t.Id == tagId);
                if (tag == null)
                    throw new NotFoundException(typeof(Tag), tagId);

                tagsToAdd.Add(tag);
            }
        }

        Installment? installment = null;
        if (request is { InstallmentId: not null, DebtId: not null })
        {
            var debt = @namespace.Debts.FirstOrDefault(d => d.Id == request.DebtId);
            if (debt == null)
                throw new NotFoundException(typeof(Debt), (Guid)request.DebtId);

            installment = debt.InstallmentsList.FirstOrDefault(i => i.Id == request.InstallmentId);
            if (installment == null)
                throw new NotFoundException(typeof(Installment), (Guid)request.InstallmentId);
        }

        User? user = null;
        if (request.ResponsiblePersonId != null)
            user = await _userRepository.GetByIdAsync((Guid)request.ResponsiblePersonId, cancellationToken);

        var expense = Expense.Create(
            request.Name,
            request.Description,
            request.Value,
            request.NamespaceId,
            request.Paid,
            request.PaymentDate,
            user?.Id,
            request.Period,
            tagsToAdd,
            installment);
        @namespace.AddExpense(expense);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

    }
}