
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Services;
using FastighetsKompassen.Kommuner.Interfaces;

namespace FastighetsKompassen.API.Endpoints
{
    public static class KommunEndpoint
    {
        public static void MapKommunEndpoints(this IEndpointRouteBuilder app)
        {
            // Endpoint för att ladda upp data
            app.MapPost("/api/kommuner/upload", async (IFormFile jsonFile, KommunService kommunService) =>
            {
                if (jsonFile == null || jsonFile.Length == 0)
                    return Results.BadRequest(new {message = "Ingen fil bifogades." });

                try
                {
                    using var stream = jsonFile.OpenReadStream();
                    var success = await kommunService.AddKommunFromJsonAsync(stream);

                    return success
                        ? Results.Ok(new { message = "Kommundata uppladdad och sparad." })
                        : Results.Conflict(new { message = "Kommunen finns redan i databasen." });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade: {ex.Message}");
                }
            })
                
               .WithName("UploadKommunJson")
               .DisableAntiforgery()
               .WithTags("Kommun")
               .Accepts<IFormFile>("multipart/form-data")
               .Produces(200)
               .Produces(400)
               .Produces(409)
               .Produces(500);

           
        }
    }
}
