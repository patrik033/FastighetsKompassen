using FastighetsKompassen.API.Services;

namespace FastighetsKompassen.API.Endpoints
{
    public static class KPIEnpoint
    {
        public static void MapKPIEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Dashboard/kpi", async (string kommun, KPIService kPIServices) =>
            {

                try
                {
                    // Anropa din service för att hämta data
                    var data = await kPIServices.GetKPIAsync(kommun);

                    return Results.Ok(data); // Returnera resultatet som en 200 OK
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message }); // 400 för ogiltig kommun
                }
                catch (Exception ex)
                {
                    return Results.Problem($"An error occurred: {ex.Message}"); // 500 för oväntade fel
                }

            });

        }
    }
}
