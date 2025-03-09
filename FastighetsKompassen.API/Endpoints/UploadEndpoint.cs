
using FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Infrastructure.Services;

using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FastighetsKompassen.API.Endpoints
{
    public static class UploadEndpoint
    {

        
       
        public static void MapUploadEndpoint(this IEndpointRouteBuilder app)
        {

            // Endpoint för att ladda upp data
            app.MapPost("/api/kommuner/upload", async (IFormFile jsonFile, KommunService kommunService) =>
            {
                if (jsonFile == null || jsonFile.Length == 0)
                    return Results.BadRequest(new { message = "Ingen fil bifogades." });

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
               .WithTags("Upload")
               .WithOpenApi()
               .Accepts<IFormFile>("multipart/form-data")
               .Produces(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status400BadRequest)
               .Produces(StatusCodes.Status429TooManyRequests)
               .Produces(StatusCodes.Status500InternalServerError);

          


            app.MapPost("/api/kommuner/upload-folder", async (string letter, KommunService kommunService) =>
            {
                // Kontrollera att bokstaven är en giltig enbokstavig sträng
                if (string.IsNullOrWhiteSpace(letter) || letter.Length != 1 || !char.IsLetter(letter[0]))
                    return Results.BadRequest(new { message = "Ogiltig bokstav. Ange en bokstav mellan A-Ö." });

                // Hitta mappen baserat på bokstaven
                //var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads", letter.ToUpper());
                var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
                if (!Directory.Exists(folderPath))
                    return Results.NotFound(new { message = $"Mappen för bokstav '{letter}' kunde inte hittas." });

                // Hämta alla filer i mappen
                var files = Directory.GetFiles(folderPath, "*.json");
                if (files.Length == 0)
                    return Results.NotFound(new { message = $"Inga JSON-filer hittades i mappen för bokstav '{letter}'." });

                var results = new List<object>();
                foreach (var file in files)
                {
                    try
                    {
                        await using var stream = File.OpenRead(file);
                        var success = await kommunService.AddKommunFromJsonAsync(stream);
                        results.Add(new { file = Path.GetFileName(file), status = success ? "Lyckad" : "Redan existerande" });
                    }
                    catch (Exception ex)
                    {
                        results.Add(new { file = Path.GetFileName(file), status = "Misslyckad", error = ex.Message });
                    }
                }

                return Results.Ok(new { message = $"Bearbetning av filer i mappen '{letter}' klar.", results });
            })
            .WithName("UploadKommunFromFolder")
            .DisableAntiforgery()
            .WithOpenApi()
            .WithTags("Upload")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
