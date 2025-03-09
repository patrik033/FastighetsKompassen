using FastighetsKompassen.API.Features.Chart.Query.GetChart;
using FastighetsKompassen.API.Features.Kpi.Query.GetKPI;
using FastighetsKompassen.API.HATEOAS;
using FastighetsKompassen.API.Services;
using MediatR;

namespace FastighetsKompassen.API.Endpoints
{
    public static class DashboardEnpoint
    {
        public static void MapDashboardEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/api/dashboard", (IHateoasService hateoasService) =>
            {
                var links = new List<Link>
                {
                    hateoasService.CreateLink("GetKPIData", null, "kpi", "GET"),
                    hateoasService.CreateLink("GetChartData", null, "charts", "GET")
                };

                return Results.Ok(new { message = "Tillgängliga dashboard endpoints", links });

            })
            .WithTags("Dashboard")
            .WithName("GetDashboardRoot")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError); 



            app.MapGet("/api/dashboard/kpi", async ([AsParameters] GetKPIQuery query , ISender sender,IHateoasService hateoas) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade" });
                }
                var data = result.Data;
                var links = new List<Link>
                {
                    hateoas.CreateLink("GetKPIData",query,"self","GET")
                };
                var resource = hateoas.Wrap(data, links);

                return Results.Ok(resource);
            })
            .WithTags("Dashboard")
            .WithName("GetKPIData")
            .WithOpenApi()
            .RequireRateLimiting("GlobalLimiter")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .Produces(StatusCodes.Status500InternalServerError);


            app.MapGet("/api/dashboard/charts", async ([AsParameters] GetChartQuery query, ISender sender,IHateoasService hateoas) =>
            {
                var result = await sender.Send(query);
                if (!result.IsSuccess)
                    return Results.BadRequest(new { message = result.Error ?? "Ett okänt fel inträffade." });

                var data = result.Data;
                var links = new List<Link>
                {
                    hateoas.CreateLink("GetChartData",query,"self","GET")
                };

                var resource = hateoas.Wrap(data, links);
                return Results.Ok(resource);
            })
           .WithTags("Dashboard")
           .WithName("GetChartData")
           .WithOpenApi()
           .RequireRateLimiting("GlobalLimiter")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status429TooManyRequests)
           .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
