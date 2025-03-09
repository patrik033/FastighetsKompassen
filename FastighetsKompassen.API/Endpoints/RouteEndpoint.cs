using FastighetsKompassen.API.HATEOAS;

namespace FastighetsKompassen.API.Endpoints
{


     
public static class RouteEndpoint
    {
    public static void MapRouteEndpoints(this IEndpointRouteBuilder app)
    {
            app.MapGet("/api", () =>
            {
                var endpoints = ApiExplorer.GetApiEndpoints(app);
                return Results.Ok(new
                {
                    Message = "Tillgängliga API-endpoints",
                    Endpoints = endpoints
                });
            });

        }
    }
}
