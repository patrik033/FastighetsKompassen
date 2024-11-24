using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.RealEstate;

namespace FastighetsKompassen.API.Endpoints
{
    public static class RealEstateEndpoint
    {
        public static void MapRealEstateEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/real-estate", async (List<RealEstateData> realEstateData, RealEstateService realEstateService) =>
            {
                try
                {
                    await realEstateService.AddRealEstateDataAsync(realEstateData);
                    return Results.Ok(new { message = "Fastighetsdata har lagts till." });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade: {ex.Message}");
                }
            });
        }

    }
}
