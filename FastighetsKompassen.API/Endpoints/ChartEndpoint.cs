using FastighetsKompassen.API.Features.Chart.Query.GetChart;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Endpoints
{
    public static class ChartEndpoint
    {

        
        public static void MapChartsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Dashboard/charts", async ([AsParameters] GetChartQuery query, ISender sender) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade." });
                }
                return Results.Ok(result.Data);

            })
            .WithTags("Chart")
            .WithName("GetChartData")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
