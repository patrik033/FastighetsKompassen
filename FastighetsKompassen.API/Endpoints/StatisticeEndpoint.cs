using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.DTO.Statistics;
using Microsoft.AspNetCore.Http;

namespace FastighetsKompassen.API.Endpoints
{
    public static class StatisticeEndpoint
    {
        public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/trends/ranking/", async (int year,int? currentPage,int? pageSize,  StatisticsService statisticsService,HttpContext context) =>
            {
                var page = currentPage ?? 1;
                var size = pageSize ?? 10;

                if (page < 1 || size < 1 || size > 100)
                {
                    return Results.BadRequest("Ogiltiga pagineringsparametrar.");
                }

                var result = await statisticsService.GetKommunRankingAsync(year, page, size);

             
                return Results.Ok(result);
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
