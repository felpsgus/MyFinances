using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Users.Commands.InsertUser;
using MyFinances.Application.Users.Queries.GetUserById;
using MyFinances.Application.Users.Views;

namespace MyFinances.Api.Endpoints;

public class UsersEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithGroupName("Users");

        group.MapPost("", async (InsertUserCommand request, ISender sender) =>
            {
                var result = await sender.Send(request);
                return Results.Created($"/users", result);
            })
            .Produces<Guid>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapGet("", async (ISender sender) =>
            {
                var result = await sender.Send(new GetUserByIdQuery());
                return Results.Ok(result);
            })
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<UserViewModel>();
    }
}