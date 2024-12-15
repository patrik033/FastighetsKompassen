using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Endpoints
{
    public static class ComparisonEndpoint
    {



   
        public static void MapComparisonEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/comparison", async (ComparisonRequestDTO request, ComparisonService comparisonService) =>
            {
                try
                {
                var results = await comparisonService.CompareMunicipalities(request);
                return Results.Ok(results);

                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = "Invalid JSON format", details = ex.Message });

                }
            });
        }
    }
}
