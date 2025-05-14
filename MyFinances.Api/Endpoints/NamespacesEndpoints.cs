using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
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
        var group = app.MapGroup("namespaces").WithTags("Namespaces");

        group.MapPost("", async (CreateNamespaceCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Created($"/namespaces/{result}", result);
            })
            .Produces<Guid>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Create Namespace",
                Description = "Create a new namespace.",
            });

        group.MapPut("{namespaceId:guid}", async (Guid namespaceId, UpdateNamespaceCommand command, ISender sender) =>
            {
                command = command with { NamespaceId = namespaceId };
                await sender.Send(command);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Update Namespace",
                Description = "Update an existing namespace.",
            });

        group.MapDelete("{namespaceId:guid}", async (Guid namespaceId, ISender sender) =>
            {
                await sender.Send(new DeleteNamespaceCommand(namespaceId));
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Delete Namespace",
                Description = "Delete an existing namespace.",
            });

        group.MapGet("", async (ISender sender) =>
            {
                var result = await sender.Send(new GetNamespacesQuery());
                return Results.Ok(result);
            })
            .Produces<List<NamespaceItemViewModel>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Get Namespaces",
                Description = "Get a list of all namespaces.",
            });

        group.MapGet("{namespaceId:guid}", async (Guid namespaceId, ISender sender) =>
            {
                var result = await sender.Send(new GetNamespaceByIdQuery(namespaceId));
                return Results.Ok(result);
            })
            .Produces<NamespaceItemViewModel>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Get Namespace by ID",
                Description = "Get a namespace by its ID.",
            });
    }
}