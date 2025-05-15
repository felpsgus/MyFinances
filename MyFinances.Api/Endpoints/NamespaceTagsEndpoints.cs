using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Namespaces.Tags.Commands.CreateTag;
using MyFinances.Application.Namespaces.Tags.Commands.DeleteTag;
using MyFinances.Application.Namespaces.Tags.Commands.UpdateTag;
using MyFinances.Application.Namespaces.Tags.Queries.GetTags;

namespace MyFinances.Api.Endpoints;

public class NamespaceTagsEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("namespaces/{namespaceId:guid}/tags").WithTags("Namespace Tags");


        group.MapPost("", async (Guid namespaceId, CreateTagCommand command, ISender sender) =>
            {
                command = command with { NamespaceId = namespaceId };
                await sender.Send(command);
                return Results.Created($"/namespaces/{namespaceId}", null);
            })
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Create Tag",
                Description = "Create a new tag in the specified namespace.",
            });

        group.MapPut("{tagId:guid}",
                async (Guid namespaceId, Guid tagId, UpdateTagCommand command, ISender sender) =>
                {
                    command = command with { NamespaceId = namespaceId, TagId = tagId };
                    await sender.Send(command);
                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Update Tag",
                Description = "Update an existing tag in the specified namespace.",
            });

        group.MapGet("", async (Guid namespaceId, ISender sender) =>
            {
                var result = await sender.Send(new GetTagsQuery(namespaceId));
                return Results.Ok(result);
            })
            .Produces<List<GetTagsResponse>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Get Tags",
                Description = "Get a list of all tags in the specified namespace.",
            });

        group.MapDelete("{tagId:guid}", async (Guid namespaceId, Guid tagId, ISender sender) =>
            {
                await sender.Send(new DeleteTagCommand(namespaceId, tagId));
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Delete Tag",
                Description = "Delete an existing tag in the specified namespace.",
            });
    }
}