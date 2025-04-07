using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Families.Commands;
using MyFinances.Application.Families.Queries.GetById;
using MyFinances.Application.Families.Views;

namespace MyFinances.Api.Endpoints;

public class FamiliesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("families").WithGroupName("Families");

        group.MapPost("", async (CreateFamilyCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Created($"/families/{result}", result);
            })
            .Produces<Guid>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapGet("{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetByIdQuery(id));
                return Results.Ok(result);
            })
            .Produces<FamilyViewModel>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}