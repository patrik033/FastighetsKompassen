using FastighetsKompassen.API.Features.RealEstate.Command.AddRealEstates;
using FastighetsKompassen.API.Features.RealEstate.Query.GetLatestRealEstateByMuniplicity;
using FastighetsKompassen.API.Features.RealEstate.Query.GetRealEstateById;
using FastighetsKompassen.API.HATEOAS;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Endpoints
{
    public static class RealEstateEndpoint
    {
        public static void MapRealEstateEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/api/realestate",  (IHateoasService hateoas) =>
            {
                var links = new List<Link>
                {
                    hateoas.CreateLink("GetLatestRealEstateByMuniplicity", new {kommunId= "1234"}, $"getAllForMuniplicity/1234", "GET"),
                    hateoas.CreateLink("GetRealEstateById", new {realEstateId= 12}, $"getRealestateById/12", "GET"),
                    hateoas.CreateLink("AddRealEstate", null, "add", "POST")
                };

                return Results.Ok(new { message = "Tillgängliga realestate endpoints", links });
            })
           .WithTags("RealEstate")
           .WithName("GetRealestateRoot")
           .WithOpenApi()
           .RequireRateLimiting("GlobalLimiter")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status429TooManyRequests)
           .Produces(StatusCodes.Status500InternalServerError);

            app.MapGet("/api/realestate/getAllForMuniplicity/{kommunId}", async ([AsParameters] GetLatestRealEstateByMuniplicityQuery query, ISender sender, IHateoasService hateoas) =>
            {
                var results = await sender.Send(query);
                if (!results.IsSuccess)
                {
                    return Results.BadRequest(new { message = results.Error ?? "Ett okänt fel inträffade" });
                }

                var data = results.Data;
                var links = new List<Link>
                {
                    hateoas.CreateLink("GetLatestRealEstateByMuniplicity",query,"self","GET")
                };

                var resource = hateoas.Wrap(data, links);
                return Results.Ok(resource);
            })
            .WithTags("RealEstate")
            .WithName("GetLatestRealEstateByMuniplicity")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);



            app.MapGet("/api/realestate/getRealestateById/{realEstateId}", async ([AsParameters] GetRealEstateByIdQuery query, ISender sender, IHateoasService hateoas) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade" });
                }
                var data = result.Data;
                var resource = hateoas.WrapLinksAndData(data, "GetRealEstateById", query, "self", "GET");

                return Results.Ok(resource);
            })
            .WithTags("RealEstate")
            .WithName("GetRealEstateById")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);



            app.MapPost("/api/realestate/add", async ([FromBody] AddRealEstatesCommand query,ISender sender, IHateoasService hateoas) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade" });
                }

                var data = result;
                var links = new List<Link>
                {
                    hateoas.CreateLink("AddRealEstate",query,"self","POST")
                };

                var resource = hateoas.Wrap(data, links);
                return Results.Ok(resource);
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
