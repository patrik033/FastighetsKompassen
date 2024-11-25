﻿
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Services;
using FastighetsKompassen.Kommuner.Interfaces;

namespace FastighetsKompassen.API.Endpoints
{
    public static class KommunEndpoint
    {
        public static void MapKommunEndpoints(this IEndpointRouteBuilder app)
        {


            app.MapDelete("/api/kommun/{kommunId}", async (int kommunId, KommunService kommunService) =>
            {
                var result = await kommunService.DeleteKommunAsync(kommunId);
                return result.IsSuccess ? Results.Ok("Kommunen och dess relaterade data har tagits bort.") : Results.BadRequest(result.Error);
            })
            .WithTags("Kommun")
            .WithName("DeleteKommun");


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
               .WithTags("Kommun")
               .Accepts<IFormFile>("multipart/form-data")
               .Produces(200)
               .Produces(400)
               .Produces(409)
               .Produces(500);

            //upload multiple files
            app.MapPost("/api/kommuner/upload-multiple", async (IFormFileCollection files, KommunService kommunService) =>
            {
                if (files == null || !files.Any())
                {
                    return Results.BadRequest(new { message = "Inga filer bifogades." });
                }

                List<string> successFiles = new();
                List<string> failedFiles = new();

                foreach (var file in files)
                {
                    try
                    {
                        using var stream = file.OpenReadStream();
                        var success = await kommunService.AddKommunFromJsonAsync(stream);

                        if (success)
                        {
                            successFiles.Add(file.FileName);
                        }
                        else
                        {
                            failedFiles.Add(file.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        failedFiles.Add($"{file.FileName} - Fel: {ex.Message}");
                    }
                }

                return Results.Ok(new
                {
                    message = "Batch-upload klar.",
                    succeeded = successFiles,
                    failed = failedFiles
                });
            })
                .WithName("UploadMultipleKommunJson")
                .DisableAntiforgery()
                .WithTags("Kommun")
                .Accepts<IFormFileCollection>("multipart/form-data")
                .Produces(200)
                .Produces(400)
                .Produces(500);

            app.MapPost("/api/kommuner/upload-folder/{letter}", async (string letter, KommunService kommunService) =>
            {
                // Kontrollera att bokstaven är en giltig enbokstavig sträng
                if (string.IsNullOrWhiteSpace(letter) || letter.Length != 1 || !char.IsLetter(letter[0]))
                    return Results.BadRequest(new { message = "Ogiltig bokstav. Ange en bokstav mellan A-Ö." });

                // Hitta mappen baserat på bokstaven
                //var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads", letter.ToUpper());
                var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"uploads");
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
                        results.Add(new {file = Path.GetFileName(file), status = "Misslyckad", error = ex.Message});
                    }
                }

                return Results.Ok(new { message = $"Bearbetning av filer i mappen '{letter}' klar.", results });
            })
    .WithName("UploadKommunFromFolder")
    .DisableAntiforgery()
    .WithTags("Kommun")
    .Produces(200)
    .Produces(400)
    .Produces(404)
    .Produces(500);

        }
    }
}
