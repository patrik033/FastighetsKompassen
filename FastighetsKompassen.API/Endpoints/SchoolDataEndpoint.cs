using FastighetsKompassen.API.Features.School.Command;
using FastighetsKompassen.API.HATEOAS;
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

            app.MapGet("/api/skolresultat", (IHateoasService hateoasService) =>
            {
                var links = new List<Link>
                {
                    hateoasService.CreateLink("UploadSchoolResultsGradeSix", null, "skolresultat", "POST"),
                    hateoasService.CreateLink("UploadSchoolResultsGradeNine", null, "compare", "POST")
                };

                return Results.Ok(new { message = "Tillgängliga trend-relaterade endpoints", links });
            })
           .WithName("GetSchools")
           .WithTags("SchoolResults")
           .WithOpenApi()
           .RequireRateLimiting("GlobalLimiter")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status429TooManyRequests)
           .Produces(StatusCodes.Status500InternalServerError);


            app.MapPost("/api/skolresultat/arskurs6", async (AddSchoolResultCommand<SchoolResultGradeSix> command, IMediator mediator,IHateoasService hateoas) =>
            {
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    var data = result;
                    var links = new List<Link>
                    {
                        hateoas.CreateLink("UploadSchoolResultsGradeSix",command,"self","POST")
                    };
                    var resource = hateoas.Wrap(data, links);
                    return Results.Ok(new { message = "Data för Årskurs 6 har laddats upp.",data = resource});
                }


                else
                    return Results.BadRequest(new { message = result.Error });

            })
            .WithName("UploadSchoolResultsGradeSix")
            .DisableAntiforgery()
            .WithTags("SchoolResults")
            .WithOpenApi()
            .Accepts<AddSchoolResultCommand<SchoolResultGradeSix>>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError); 


            app.MapPost("/api/skolresultat/arskurs9", async (AddSchoolResultCommand<SchoolResultGradeNine> command, IMediator mediator, IHateoasService hateoas) =>
            {
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                {
                    var data = result;
                    var links = new List<Link>
                    {
                        hateoas.CreateLink("UploadSchoolResultsGradeNine",command,"self","POST")
                    };
                    var resource = hateoas.Wrap(data, links);
                    return Results.Ok(new { message = "Data för Årskurs 6 har laddats upp." });
                }

                else
                    return Results.BadRequest(new { message = result.Error });

            })
            .WithName("UploadSchoolResultsGradeNine")
            .DisableAntiforgery()
            .WithTags("SchoolResults")
            .WithOpenApi()
            .Accepts<AddSchoolResultCommand<SchoolResultGradeNine>>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError); 
        }
    }
}
