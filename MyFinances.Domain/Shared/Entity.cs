using MyFinances.Domain.Entities;

namespace MyFinances.Domain.Shared;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public bool Deleted { get; private set; } = false;

    public void Delete() => Deleted = true;

    public void Update() => UpdatedAt = DateTimeOffset.UtcNow;
}

public abstract class AuditEntity : Entity
{
    public Guid CreatedBy { get; private set; }
    public User? CreatedByUser { get; private set; }

    public Guid UpdatedBy { get; private set; }
    public User? UpdatedByUser { get; private set; }

    public void Update(Guid userId)
    {
        base.Update();
        UpdatedBy = userId;
    }

    public void Create(Guid userId)
    {
        CreatedBy = userId;
    }
}