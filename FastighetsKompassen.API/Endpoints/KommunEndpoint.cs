﻿using FastighetsKompassen.API.Features.Muniplicity.Command.DeleteMuniplicityById;
using FastighetsKompassen.API.HATEOAS;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Services;
using FastighetsKompassen.Shared.Models.ErrorHandling;
using MediatR;

namespace FastighetsKompassen.API.Endpoints
{
    public static class KommunEndpoint
    {

        public static void MapKommunEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/kommun/{kommunId}", async ([AsParameters] DeleteMuniplicityByIdQuery query, ISender sender,IHateoasService hateoas) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade" });
                }

                var data = result;
                var links = new List<Link>
                {
                    hateoas.CreateLink("DeleteKommun",query,"self","DELETE")
                };

                var resource = hateoas.Wrap(data, links);
                return Results.Ok(result);
            })
            .WithTags("Kommun")
            .WithName("DeleteKommun")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
