using FastighetsKompassen.API.Features.RealEstate.Command.AddRealEstates;
using FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity;
using FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Endpoints
{
    public static class RealEstateEndpoint
    {
        public static void MapRealEstateEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/api/kommuner/realestate/{kommunId}", async ([AsParameters] GetLatestRealEstateByMuniplicityQuery query, ISender sender) =>
            {
                var results = await sender.Send(query);
                if (!results.IsSuccess)
                {
                    return Results.BadRequest(new { message = results.Error ?? "Ett okänt fel inträffade" });
                }
                return Results.Ok(results.Data);
            })
            .WithTags("RealEstate")
            .WithName("GetLatestRealEstateByMuniplicity")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);



            app.MapGet("/api/kommuner/realestateById/{realEstateId}", async ([AsParameters] GetRealEstateByIdQuery query, ISender sender) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade" });
                }
                return Results.Ok(result.Data);
            })
            .WithTags("RealEstate")
            .WithName("GetRealEstateById")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);



            app.MapPost("/api/real-estate", async ([FromBody] AddRealEstatesCommand query,ISender sender) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade" });
                }
                return Results.Ok(result);
            })
            .WithTags("RealEstate")
            .WithName("AddRealEstate")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
