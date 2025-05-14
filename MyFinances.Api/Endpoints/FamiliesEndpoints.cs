using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Families.Commands.AcceptFamilyMembership;
using MyFinances.Application.Families.Commands.AddFamilyMember;
using MyFinances.Application.Families.Commands.CreateFamily;
using MyFinances.Application.Families.Commands.RefuseFamilyMembership;
using MyFinances.Application.Families.Commands.RemoveFamilyMember;
using MyFinances.Application.Families.Queries.GetAllFamilies;
using MyFinances.Application.Families.Queries.GetFamilyById;
using MyFinances.Application.Families.Views;

namespace MyFinances.Api.Endpoints;

public class FamiliesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("families").WithTags("Families");

        group.MapPost("", async (CreateFamilyCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Created($"/families/{result}", result);
            })
            .Produces<Guid>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Create Family",
                Description = "Create a new family.",
            });

        group.MapGet("{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetFamilyByIdQuery(id));
                return Results.Ok(result);
            })
            .Produces<FamilyViewModel>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Get Family",
                Description = "Get family details by ID.",
            });

        group.MapPost("{id:guid}/add-member", async (Guid id, AddFamilyMemberCommand command, ISender sender) =>
            {
                var fixedCommand = command with { FamilyId = id };
                await sender.Send(fixedCommand);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Add Family Member",
                Description = "Add a member to the family.",
            });

        group.MapPost("{id:guid}/remove-member", async (Guid id, RemoveFamilyMemberCommand command, ISender sender) =>
            {
                var fixedCommand = command with { FamilyId = id };
                await sender.Send(fixedCommand);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Remove Family Member",
                Description = "Remove a member from the family.",
            });

        group.MapGet("", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllFamiliesQuery());
                return Results.Ok(result);
            })
            .Produces<List<FamilyViewModel>>()
            .Produces(StatusCodes.Status204NoContent)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Get All Families",
                Description = "Get a list of all families.",
            });

        group.MapPost("{id:guid}/accept-membership", async (Guid id, ISender sender) =>
            {
                await sender.Send(new AcceptFamilyMembershipCommand(id));
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Accept Family Membership",
                Description = "Accept a family membership invitation.",
            });

        group.MapPost("{id:guid}/refuse-membership", async (Guid id, ISender sender) =>
            {
                await sender.Send(new RefuseFamilyMembershipCommand(id));
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Refuse Family Membership",
                Description = "Refuse a family membership invitation.",
            });
    }
}