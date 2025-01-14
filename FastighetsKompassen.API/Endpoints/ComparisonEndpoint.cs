using FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Endpoints
{
    public static class ComparisonEndpoint
    {

        public static void MapComparisonEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/comparison", async ([FromBody] GetComparisonResultQuery query,  ISender sender) =>
            {
                var results = await sender.Send(query);
                if (!results.IsSuccess)
                {
                    return Results.BadRequest(new {message = results.Error?? "Ett okänt fel inträffade"});
                }
                return Results.Ok(results.Data);
            })
            .WithTags("Comparison")
            .WithName("GetComparisonResult")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
