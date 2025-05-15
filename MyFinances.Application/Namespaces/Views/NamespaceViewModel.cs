using MyFinances.Domain.Entities;

namespace MyFinances.Application.Namespaces.Views;

public class NamespaceViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public static implicit operator NamespaceViewModel(Namespace @namespace)
    {
        return new NamespaceViewModel
        {
            Id = @namespace.Id,
            Name = @namespace.Name,
        };
    }
}