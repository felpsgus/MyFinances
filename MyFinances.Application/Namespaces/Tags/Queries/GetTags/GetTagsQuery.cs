using MediatR;
using MyFinances.Application.Namespaces.Views;

namespace MyFinances.Application.Namespaces.Tags.Queries.GetTags;

public record GetTagsQuery(Guid NamespaceId) : IRequest<List<TagViewModel>>;