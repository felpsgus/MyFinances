using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MyFinances.Api.Interfaces;
using MyFinances.Application.Namespaces.Expenses.Commands.CreateExpense;

namespace MyFinances.Api.Endpoints;

public class NamespaceExpensesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("namespaces/{namespaceId:guid}/expenses").WithTags("Namespace Expenses");

        group.MapPost("", async (Guid namespaceId, CreateExpenseCommand command, ISender sender) =>
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
                Summary = "Create Expense",
                Description = "Create a new expense in the specified namespace.",
            });
    }
}