using MediatR;

namespace MyFinances.Application.Namespaces.Tags.Queries.GetTags;

public record GetTagsQuery(Guid NamespaceId) : IRequest<List<GetTagsResponse>>;