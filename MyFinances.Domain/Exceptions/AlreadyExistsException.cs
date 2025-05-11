using MyFinances.Domain.Extensions;

namespace MyFinances.Domain.Exceptions;

public class AlreadyExistsException : DomainException
{
    public AlreadyExistsException(Type entity, Guid id) : this(entity.GetDisplayName(), id.ToString())
    {
    }

    public AlreadyExistsException(Type entity, string parameterName, string parameterValue)
        : base($"Entity {entity.GetDisplayName()} with {parameterName} = {parameterValue} already exists.")
    {
    }

    private AlreadyExistsException(string entityName, string id)
        : base($"Entity {entityName} with id {id} already exists.")
    {
    }

    public override int StatusCode => 400;

    public override string Title => "Already exists";
}