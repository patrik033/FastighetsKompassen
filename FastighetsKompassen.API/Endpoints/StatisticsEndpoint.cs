using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunRanking;
using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunTrends;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.DTO.Statistics;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace FastighetsKompassen.API.Endpoints
{
    public static class StatisticsEndpoint
    {
        public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/trends/ranking/", async ([AsParameters] GetKommunRankingQuery query,  ISender sender) =>
            {
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .WithTags("Trends");

            app.MapPost("/api/trends/compare", async (GetKommunTrendsQuery query,ISender sender) =>
            {
                var result = await sender.Send(query);
                return result;
            })
           .WithTags("Trends");
        }
    }
}
