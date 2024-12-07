using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastighetsKompassen.API.Endpoints
{
    public static class ChartEndpoint
    {

        
        public static void MapChartsEndpoints(this IEndpointRouteBuilder app)
        {
            //app.MapDelete("/api/statistics/", async (string kommun, ChartService statisticsService) =>
            //{

            //    try
            //    {
            //        // Anropa din service för att hämta data
            //        var data = await statisticsService.GetDashboardData(kommun);

            //        return Results.Ok(data); // Returnera resultatet som en 200 OK
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        return Results.BadRequest(new { error = ex.Message }); // 400 för ogiltig kommun
            //    }
            //    catch (Exception ex)
            //    {
            //        return Results.Problem($"An error occurred: {ex.Message}"); // 500 för oväntade fel
            //    }

            //});
           
        }
    }
}
