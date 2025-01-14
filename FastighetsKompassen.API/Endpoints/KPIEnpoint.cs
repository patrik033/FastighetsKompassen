using FastighetsKompassen.API.Features.Kpi.Query.GetKPI;
using FastighetsKompassen.API.Services;
using MediatR;

namespace FastighetsKompassen.API.Endpoints
{
    public static class KPIEnpoint
    {
        public static void MapKPIEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Dashboard/kpi", async ([AsParameters] GetKPIQuery query , ISender sender) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade" });
                }
                return Results.Ok(result.Data);
            })
            .WithTags("KPI")
            .WithName("GetKPIData")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);

        }
    }
}
