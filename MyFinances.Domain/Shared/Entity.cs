using MyFinances.Domain.Entities;

namespace MyFinances.Domain.Shared;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public bool Deleted { get; private set; } = false;

    public void Delete()
    {
        Deleted = true;
    }

    public void Update()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

public abstract class AuditEntity : Entity
{
    public Guid? CreatedById { get; private set; }
    public User? CreatedBy { get; private set; }

    public Guid? UpdatedById { get; private set; }
    public User? UpdatedBy { get; private set; }

    public void Update(Guid userId)
    {
        base.Update();
        UpdatedById = userId;
    }

    public void Create(Guid userId)
    {
        CreatedById = userId;
    }
}