namespace MyFinances.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entity, string id)
        : base($"{entity} with ID {id} not found.")
    {
    }

    public NotFoundException(string entity)
        : base($"{entity} not found.")
    {
    }
}