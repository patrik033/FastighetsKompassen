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
                return Results.Ok(results);
            }).WithTags("Comparison");
        }
    }
}
