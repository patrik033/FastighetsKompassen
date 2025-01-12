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
                var result = await sender.Send(query);
                return Results.Ok(result.Data);
            })
            .WithTags("RealEstate")
            .WithName("GetLatestRealEstateByMuniplicity");



            app.MapGet("/api/kommuner/realestateById/{realEstateId}", async ([AsParameters] GetRealEstateByIdQuery query, ISender sender) =>
            {
                var result = await sender.Send(query);
                return Results.Ok(result.Data);
            })
            .WithTags("RealEstate")
            .WithName("GetRealEstateById");



            app.MapPost("/api/real-estate", async ([FromBody] AddRealEstatesCommand query,ISender sender) =>
            {
                var result = await sender.Send(query);
                return Results.Ok(result.IsSuccess);
            })
            .WithTags("RealEstate")
            .WithName("AddRealEstate");
        }
    }
}
