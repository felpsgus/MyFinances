using MyFinances.Domain.Entities;

namespace MyFinances.Domain.Primitives;

public abstract class AuditEntity : Entity
{
    public Guid? CreatedById { get; private set; }
    public User? CreatedBy { get; private set; }

    public Guid? UpdatedById { get; private set; }
    public User? UpdatedBy { get; private set; }

    public void UpdateEntity(Guid userId)
    {
        base.UpdateEntity();
        UpdatedById = userId;
    }

    public void CreateEntity(Guid userId)
    {
        CreatedById = userId;
    }
}