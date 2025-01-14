using FastighetsKompassen.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FastighetsKompassen.API.Endpoints
{
    public static class BackupEndpoint
    {
        public static void MapBackupEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/backup", async ([FromServices] BackupService backupService) =>
            {
                var backupPath = await backupService.CreateBackupAsync();
                return Results.Ok(new {message = "Backup Created Successfully",backupPath});
            })
            .WithName("CreateDatabaseBackup")
            .WithTags("Database")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
