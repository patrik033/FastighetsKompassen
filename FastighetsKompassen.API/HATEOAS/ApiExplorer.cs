namespace FastighetsKompassen.API.HATEOAS
{
    public static class ApiExplorer
    {
        public static IEnumerable<object> GetApiEndpoints(IEndpointRouteBuilder app)
        {
            return app.DataSources
           .SelectMany(ds => ds.Endpoints)
           .OfType<RouteEndpoint>()
           .Where(e => e.RoutePattern.RawText.StartsWith("/api/") &&
                       e.RoutePattern.RawText.Count(c => c == '/') == 2) // Exakt ett steg ned
           .Select(e => new
           {
               Path = e.RoutePattern.RawText,
               Method = e.Metadata
                   .OfType<HttpMethodMetadata>()
                   .FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "GET"
           })
           .DistinctBy(endpoint => endpoint.Path); // Undvik duplicerade endpoints
        }
    }
}
