using MediatR;
using MyFinances.Domain.Repositories;

namespace MyFinances.Application.Namespaces.Tags.Queries.GetTags;

public class GetTagsHandler : IRequestHandler<GetTagsQuery, List<GetTagsResponse>>
{
    private readonly INamespaceRepository _namespaceRepository;

    public GetTagsHandler(INamespaceRepository namespaceRepository)
    {
        _namespaceRepository = namespaceRepository;
    }

    public async Task<List<GetTagsResponse>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        return await _namespaceRepository.GetTagsAsync(request.NamespaceId, cancellationToken)
            .ContinueWith(t => t.Result.Select(tag => (GetTagsResponse)tag).ToList(), cancellationToken);
    }
}