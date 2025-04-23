namespace MyFinances.Domain.Primitives;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public bool Deleted { get; private set; } = false;

    public void DeleteEntity()
    {
        Deleted = true;
    }

    public void UpdateEntity()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}