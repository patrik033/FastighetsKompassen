using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.PoliceData;

namespace FastighetsKompassen.API.Endpoints
{
    public static class PoliceEndpoint
    {
        public static void MapPoliceEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/police-events", async (List<PoliceEvent> policeEvents, PoliceService policeEventService) =>
            {
                try
                {
                    await policeEventService.AddPoliceEventsAsync(policeEvents);
                    return Results.Ok(new { message = "Polisdata har lagts till." });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade: {ex.Message}");
                }
            });
        }
    }
}
