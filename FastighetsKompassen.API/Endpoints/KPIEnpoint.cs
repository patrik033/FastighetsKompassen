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
                return Results.Ok(result.Data);
            })
                .WithTags("KPI")
                .WithName("GetKPIData");

        }
    }
}
