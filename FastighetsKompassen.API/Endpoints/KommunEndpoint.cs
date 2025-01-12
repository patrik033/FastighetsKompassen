using FastighetsKompassen.API.Features.Muniplicity.Command.DeleteMuniplicityById;
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
            app.MapDelete("/api/kommun/{kommunId}", async ([AsParameters] DeleteMuniplicityByIdQuery query, ISender sender) =>
            {
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .WithTags("Kommun")
            .WithName("DeleteKommun");
        }
    }
}
