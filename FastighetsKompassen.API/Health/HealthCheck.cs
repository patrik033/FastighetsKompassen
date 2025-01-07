using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FastighetsKompassen.API.Health
{
    public static class HealthCheck
    {
        public static void ConfigureHealthChecks(this IServiceCollection services,IConfiguration config)
        {
            services.AddHealthChecks()
                .AddSqlServer(config["ConnectionStrings:DefaultConnection"]
                , healthQuery: "select 1"
                , name: "SQL Server"
                , failureStatus: HealthStatus.Unhealthy
                , tags: new[] { "Feedback", "Database" });

            
        }
    }
}
