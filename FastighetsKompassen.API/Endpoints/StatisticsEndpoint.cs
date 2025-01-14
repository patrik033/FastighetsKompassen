using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunRanking;
using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunTrends;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.DTO.Statistics;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace FastighetsKompassen.API.Endpoints
{
    public static class StatisticsEndpoint
    {
        public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/trends/ranking/", async ([AsParameters] GetKommunRankingQuery query,  ISender sender) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett ökänt fel inträffade" });
                }
                return Results.Ok(result.Data);
            })
            .WithTags("Trends")
            .WithName("GetKommunRanking")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapPost("/api/trends/compare", async (GetKommunTrendsQuery query,ISender sender) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = "Ett okänt fel inträffade" });
                }
                return Results.Ok(result.Data);
            })
           .WithTags("Trends")
           .WithName("GetKommunTrends")
           .WithOpenApi()
           .RequireRateLimiting("GlobalLimiter")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status429TooManyRequests)
           .Produces(StatusCodes.Status500InternalServerError); 
        }
    }
}
