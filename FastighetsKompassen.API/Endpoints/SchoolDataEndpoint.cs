using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.SkolData;

namespace FastighetsKompassen.API.Endpoints
{
    public static class SchoolDataEndpoint
    {
        public static void MapSchoolEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/school-data", async (List<SchoolResultGradeSix> gradeSixData, List<SchoolResultGradeNine> gradeNineData, SchoolService schoolDataService) =>
            {
                try
                {
                    await schoolDataService.AddSchoolDataAsync(gradeSixData, gradeNineData);
                    return Results.Ok(new { message = "Skoldata har lagts till." });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Ett fel inträffade: {ex.Message}");
                }
            });
        }

    }
}
