using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Kommuner.Interfaces;
using FastighetsKompassen.Kommuner.Services;
using FastighetsKompassen.Backup.Interfaces;
using FastighetsKompassen.Backup.Services;
using Microsoft.EntityFrameworkCore;
using FastighetsKompassen.Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FastighetsKompassen.API")));

builder.Services.AddScoped<BackupService>();
builder.Services.AddScoped<JsonDataSeeder>();
builder.Services.AddScoped<KommunService>();




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.MapPost("/upload-json", async (IFormFile file, [FromServices] KommunService kommunerService) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("Ingen fil uppladdad eller filen är tom.");
    }

    try
    {
        // Processa filen med tjänsten
        using (var stream = file.OpenReadStream())
        {
            var result = await kommunerService.AddKommunFromJsonAsync(stream);

            if (!result)
            {
                return Results.Conflict("Kommunen finns redan i databasen.");
            }
        }

        return Results.Ok("Kommunen har lagts till framgångsrikt.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Ett fel uppstod: {ex.Message}");
    }
})
.WithName("UploadJsonFile")
.WithTags("FileUpload")
.DisableAntiforgery();

// API för att skapa en backup av databasen
//app.MapPost("/backup/create", async (BackupService backupService) =>
//{
//    var backupPath = await backupService.CreateBackupAsync();
//    return Results.Ok($"Backup skapades på: {backupPath}");
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();





app.Run();


