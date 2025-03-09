using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunRanking;
using FastighetsKompassen.API.Features.Statistics.Commands.GetKommunTrends;
using FastighetsKompassen.API.HATEOAS;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.DTO.Statistics;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Endpoints
{
    public static class TrendsEndpoint
    {
        public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/api/trends", (IHateoasService hateoasService) =>
            {
                var links = new List<Link>
                {
                    hateoasService.CreateLink("GetKommunRanking", null, "ranking", "GET"),
                    hateoasService.CreateLink("GetKommunTrends", null, "compare", "POST")
                };

                return Results.Ok(new { message = "Tillgängliga trend-relaterade endpoints", links });
            })
            .WithTags("Trends")
            .WithName("GetTrendsRoot")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
            
            app.MapGet("/api/trends/ranking/", async ([AsParameters] GetKommunRankingQuery query,  ISender sender,IHateoasService hateoas) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett ökänt fel inträffade" });
                }

                var data = result.Data;
                var links = new List<Link>
                {
                    hateoas.CreateLink("GetKommunRanking",query,"self","GET")
                };

                var resource = hateoas.Wrap(data, links);


                return Results.Ok(resource);
            })
            .WithTags("Trends")
            .WithName("GetKommunRanking")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);


           

            app.MapPost("/api/trends/compare", async (GetKommunTrendsQuery query,ISender sender,IHateoasService hateoas) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = "Ett okänt fel inträffade" });
                }

                var data = result.Data;
                var links = new List<Link>
                {
                   hateoas.CreateLink("GetKommunTrends",query,"self","POST")
                };
                var resource = hateoas.Wrap(data,links);
                return Results.Ok(resource);
            })
           .WithTags("Trends")
           .WithName("GetKommunTrends")
           .WithOpenApi()
           .RequireRateLimiting("GlobalLimiter")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status429TooManyRequests)
           .Produces(StatusCodes.Status500InternalServerError); 
        }
    }
}
