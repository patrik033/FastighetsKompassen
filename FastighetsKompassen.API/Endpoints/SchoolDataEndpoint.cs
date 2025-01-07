using FastighetsKompassen.API.Features.School.Command;
using FastighetsKompassen.API.ReadToFile;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Shared.Models.SkolData;
using MediatR;

namespace FastighetsKompassen.API.Endpoints
{
    public static class SchoolDataEndpoint
    {
        public static void MapSchoolEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/skolresultat/arskurs6", async (AddSchoolResultCommand<SchoolResultGradeSix> command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                    return Results.Ok(new { message = "Data för Årskurs 6 har laddats upp." });

                else
                    return Results.BadRequest(new { message = result.Error });

            })
            .WithName("UploadSchoolResultsGradeSix")
            .DisableAntiforgery()
            .WithTags("SchoolResults")
            .Accepts<AddSchoolResultCommand<SchoolResultGradeSix>>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError); 


            app.MapPost("/api/skolresultat/arskurs9", async (AddSchoolResultCommand<SchoolResultGradeNine> command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                    return Results.Ok(new { message = "Data för Årskurs 6 har laddats upp." });

                else
                    return Results.BadRequest(new { message = result.Error });

            })
            .WithName("UploadSchoolResultsGradeNine")
            .DisableAntiforgery()
            .WithTags("SchoolResults")
            .Accepts<AddSchoolResultCommand<SchoolResultGradeNine>>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError); 
        }
    }
}
