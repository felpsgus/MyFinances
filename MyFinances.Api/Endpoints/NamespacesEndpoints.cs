using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Namespaces.Commands.CreateNamespace;
using MyFinances.Application.Namespaces.Commands.DeleteNamespace;
using MyFinances.Application.Namespaces.Commands.UpdateNamespace;
using MyFinances.Application.Namespaces.Queries.GetNamespaceById;
using MyFinances.Application.Namespaces.Queries.GetNamespaces;
using MyFinances.Application.Namespaces.Views;

namespace MyFinances.Api.Endpoints;

public class NamespacesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("namespaces").WithGroupName("Namespaces");

        group.MapPost("", async (CreateNamespaceCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Created($"/namespaces/{result}", result);
            })
            .Produces<Guid>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPut("{namespaceId:guid}", async (Guid namespaceId, UpdateNamespaceCommand command, ISender sender) =>
            {
                command = command with { NamespaceId = namespaceId };
                await sender.Send(command);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapDelete("{namespaceId:guid}", async (Guid namespaceId, ISender sender) =>
            {
                await sender.Send(new DeleteNamespaceCommand(namespaceId));
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapGet("", async (ISender sender) =>
            {
                var result = await sender.Send(new GetNamespacesQuery());
                return Results.Ok(result);
            })
            .Produces<List<NamespaceItemViewModel>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapGet("{namespaceId:guid}", async (Guid namespaceId, ISender sender) =>
            {
                var result = await sender.Send(new GetNamespaceByIdQuery(namespaceId));
                return Results.Ok(result);
            })
            .Produces<NamespaceItemViewModel>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}