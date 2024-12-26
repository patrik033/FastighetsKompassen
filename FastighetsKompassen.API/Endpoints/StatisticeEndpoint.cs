using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.DTO.Statistics;

namespace FastighetsKompassen.API.Endpoints
{
    public static class StatisticeEndpoint
    {
        public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/trends/ranking/{year}", async (int year, StatisticsService statisticsService) =>
            {
                var result = await statisticsService.GetKommunRankingAsync(year);
                return result;
            })
            .WithTags("Trends");

            app.MapPost("/api/trends/compare", async (KommunTrendsRequestDto request, StatisticsService statisticsService) =>
            {
                var result = await statisticsService.GetKommunTrendsAsync(request.kommunId,request.Year);
                return result;
            })
           .WithTags("Trends");


        }
    }
}
