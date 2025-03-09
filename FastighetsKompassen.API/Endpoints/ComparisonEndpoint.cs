using FastighetsKompassen.API.Features.Comparison.Query.GetComparisonResult;
using FastighetsKompassen.API.HATEOAS;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Endpoints
{
    public static class ComparisonEndpoint
    {

        public static void MapComparisonEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/comparison", async ([FromBody] GetComparisonResultQuery query,  ISender sender, IHateoasService hateoas) =>
            {
                var results = await sender.Send(query);
                if (!results.IsSuccess)
                {
                    return Results.BadRequest(new {message = results.Error?? "Ett okänt fel inträffade"});
                }

                var data = results.Data;
                var links = new List<Link>
                {
                    hateoas.CreateLink("GetComparisonResult",query,"self","POST")
                };

                var resource = hateoas.Wrap(data, links);

                return Results.Ok(resource);
            })
            .WithTags("Comparison")
            .WithName("GetComparisonResult")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
