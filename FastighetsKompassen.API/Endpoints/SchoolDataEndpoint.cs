using FastighetsKompassen.API.ReadToFile;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.SkolData;

namespace FastighetsKompassen.API.Endpoints
{
    public static class SchoolDataEndpoint
    {
        public static void MapSchoolEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/skolresultat/arskurs6", async (IFormFile excelFile, int yearRange, SchoolService schoolService, ReadExcelDataToClass readExcelData) =>
            {
                if (excelFile == null || excelFile.Length == 0)
                    return Results.BadRequest(new { message = "Ingen fil bifogades." });

                try
                {
                    using var stream = excelFile.OpenReadStream();
                    var schoolResults = readExcelData.ReadSheetData<SchoolResultGradeSix>(stream, yearRange, "Årskurs 6");
                    await schoolService.AddSchoolResultsAsync(schoolResults);
                    return Results.Ok(new { message = "Data för Årskurs 6 har laddats upp." });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade: {ex.Message}");
                }
            })
                .WithName("UploadSchoolResultsGradeSix")
                .DisableAntiforgery()
                .WithTags("SchoolResults");



            app.MapPost("/api/skolresultat/arskurs9", async (IFormFile excelFile, int yearRange, SchoolService schoolService, ReadExcelDataToClass readExcelData) =>
            {
                if (excelFile == null || excelFile.Length == 0)
                    return Results.BadRequest(new { message = "Ingen fil bifogades." });

                try
                {
                    using var stream = excelFile.OpenReadStream();
                    var schoolResults = readExcelData.ReadSheetData<SchoolResultGradeNine>(stream, yearRange, "Årskurs 9");
                    await schoolService.AddSchoolResultsAsync(schoolResults);
                    return Results.Ok(new { message = "Data för Årskurs 9 har laddats upp." });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade: {ex.Message}");
                }
            })
                .WithName("UploadSchoolResultsGradeNine")
                .DisableAntiforgery()
                .WithTags("SchoolResults");
        }

    }
}
