using MyFinances.Domain.Entities;

namespace MyFinances.Application.Namespaces.Tags.Queries.GetTags;

public class GetTagsResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public static implicit operator GetTagsResponse(Tag tag)
    {
        return new GetTagsResponse()
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }
}