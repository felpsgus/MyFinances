using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Users.Commands.CreateUser;
using MyFinances.Application.Users.Queries.GetUserById;
using MyFinances.Application.Users.Views;

namespace MyFinances.Api.Endpoints;

public class UsersEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithTags("Users");

        group.MapPost("", async (CreateUserCommand request, ISender sender) =>
            {
                var result = await sender.Send(request);
                return Results.Created($"/users", result);
            })
            .Produces<Guid>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous()
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Create User",
                Description = "Create a new user.",
            });

        group.MapGet("", async (ISender sender) =>
            {
                var result = await sender.Send(new GetUserByIdQuery());
                return Results.Ok(result);
            })
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<UserViewModel>()
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Get User",
                Description = "Get user details based on the token.",
            });
    }
}