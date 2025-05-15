using MyFinances.Domain.Entities;

namespace MyFinances.Application.Namespaces.Views;

public class TagViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public static implicit operator TagViewModel(Tag tag)
    {
        return new TagViewModel()
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }
}