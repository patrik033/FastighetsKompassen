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
                return Results.Ok(result.Data);

            })
            .WithTags("Chart")
            .WithName("GetChartData");
        }
    }
}
