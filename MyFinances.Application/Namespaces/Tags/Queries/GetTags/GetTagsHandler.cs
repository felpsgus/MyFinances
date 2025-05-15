using MediatR;
using MyFinances.Application.Namespaces.Views;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Tags.Queries.GetTags;

public class GetTagsHandler : IRequestHandler<GetTagsQuery, List<TagViewModel>>
{
    private readonly INamespaceRepository _namespaceRepository;

    public GetTagsHandler(INamespaceRepository namespaceRepository)
    {
        _namespaceRepository = namespaceRepository;
    }

    public async Task<List<TagViewModel>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        return await _namespaceRepository.GetTagsAsync(request.NamespaceId, cancellationToken)
            .ContinueWith(t => t.Result.Select(tag => (TagViewModel)tag).ToList(), cancellationToken);
    }
}