using MyFinances.Domain.Extensions;

namespace MyFinances.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(Type type, Guid id) : this(type.GetDisplayName(), id.ToString())
    {
    }

    public NotFoundException(Type type) : this(type.GetDisplayName())
    {
    }

    private NotFoundException(string entity, string id)
        : base($"{entity} with ID {id} not found.")
    {
    }

    private NotFoundException(string entity)
        : base($"{entity} not found.")
    {
    }

    public override int StatusCode => 404;

    public override string Title => "Not found";
}