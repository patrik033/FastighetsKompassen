using FastighetsKompassen.API.Endpoints;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace FastighetsKompassen.API.Extensions
{
    public static class EndpointConfigurations
    {
        public static void ConfiguareEndpoints(this WebApplication app)
        {
            //app.MapUploadEndpoint();
            //app.MapBackupEndpoints();
            app.MapRealEstateEndpoints();
            app.MapSchoolEndpoints();
            app.MapRouteEndpoints();
            app.MapDashboardEndpoints();
            app.MapComparisonEndpoints();
            app.MapStatisticsEndpoints();
            //app.MapKommunEndpoints();
        }

        public static void ConfigureHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/api/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
