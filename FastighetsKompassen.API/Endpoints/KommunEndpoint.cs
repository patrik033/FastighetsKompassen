using FastighetsKompassen.Backup.Interfaces;
using FastighetsKompassen.Kommuner.Interfaces;

namespace FastighetsKompassen.API.Endpoints
{
    public class KommunEndpoint
    {
        public static void MapEndpoints(WebApplication app)
        {
            // Endpoint för att ladda upp data
            app.MapPost("/api/kommuner/upload", async (IFormFile jsonFile, IKommunService kommunService) =>
            {
                if (jsonFile == null || jsonFile.Length == 0)
                    return Results.BadRequest("Ingen fil bifogades.");

                try
                {
                    using var stream = jsonFile.OpenReadStream();
                    var success = await kommunService.AddKommunFromJsonAsync(stream);

                    return success
                        ? Results.Ok("Kommundata uppladdad och sparad.")
                        : Results.BadRequest("Kommunen finns redan i databasen.");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade: {ex.Message}");
                }
            });

            // Endpoint för att skapa backup
            app.MapGet("/api/backup", async (IBackupService backupService) =>
            {
                try
                {
                    var backupPath = await backupService.CreateBackupAsync();
                    return Results.Ok($"Backup skapad: {backupPath}");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade vid backup: {ex.Message}");
                }
            });
        }
    }
}
