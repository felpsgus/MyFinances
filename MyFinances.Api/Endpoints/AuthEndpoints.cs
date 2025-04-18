using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Auth.Login;
using MyFinances.Application.Auth.PasswordResetChange;
using MyFinances.Application.Auth.PasswordResetConfirm;
using MyFinances.Application.Auth.PasswordResetRequest;
using MyFinances.Application.Auth.RefreshToken;
using MyFinances.Application.Auth.Views;

namespace MyFinances.Api.Endpoints;

public class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth").WithGroupName("Auth");

        group.MapPost("login", async (LoginCommand request, ISender sender) =>
            {
                var result = await sender.Send(request);
                return result;
            })
            .Produces<LoginViewModel>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("refresh-token", async (RefreshTokenCommand request, ISender sender) =>
            {
                var result = await sender.Send(request);
                return result;
            })
            .Produces<LoginViewModel>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("password-reset/request", async (PasswordResetRequestCommand command, ISender sender) =>
            {
                await sender.Send(command);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("password-reset/verify", async (PasswordResetVerifyCommand command, ISender sender) =>
            {
                await sender.Send(command);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost("password-reset/change", async (PasswordResetChangeCommand command, ISender sender) =>
            {
                await sender.Send(command);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();
    }
}