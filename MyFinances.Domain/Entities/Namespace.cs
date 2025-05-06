using MyFinances.Domain.Enum;
using MyFinances.Domain.Extensions;
using MyFinances.Domain.Primitives;

namespace MyFinances.Domain.Entities;

public class Namespace : AuditEntity
{
    private Namespace()
    {
    }

    public string Name { get; private set; }

    public NamespaceType Type { get; private set; }

    public Guid? UserId { get; private set; }
    public User? User { get; private set; }

    public Guid? FamilyId { get; private set; }
    public Family? Family { get; private set; }

    public static Namespace Create(string name, NamespaceType type, Guid? userId, Guid? familyId)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        if (type == NamespaceType.Personal && userId == null)
            throw new ArgumentNullException(nameof(userId), "User ID cannot be null for personal namespace.");

        if (type == NamespaceType.Family && familyId == null)
            throw new ArgumentNullException(nameof(familyId), "Family ID cannot be null for family namespace.");

        return new Namespace
        {
            Name = name,
            Type = type,
            UserId = userId,
            FamilyId = familyId
        };
    }

    public void Update(string name)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        Name = name;
    }
}