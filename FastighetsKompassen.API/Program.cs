using FastighetsKompassen.API.Endpoints;
using FastighetsKompassen.API.Extensions;
using FastighetsKompassen.API.Health;
using FastighetsKompassen.API.ReadToFile;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.API.Validation;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Infrastructure.Services;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Threading.RateLimiting;





ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);
bool isProduction = builder.Environment.IsProduction();

builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
       // policy.WithOrigins("http://localhost:3000") // Replace with your domains

        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("RestrictToNextJs", policy =>
    {
        policy.WithOrigins("https://your-nextjs-domain.com", "http://localhost:3000") // Replace with your domains
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("GlobalLimiter", limiterOptions =>
    {
        
        limiterOptions.PermitLimit = 10; // Max 10 requests per minute
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });
});





builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FastighetsKompassen.API")));

builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});


builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 419430400;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 400 * 1024 * 1024;
});



builder.Services.AddSingleton<ReadExcelDataToClass>();

builder.Services.AddScoped<BackupService>();
builder.Services.AddScoped<KommunService>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<MultipleFileUploadOperationFilter>();
});

var app = builder.Build();


var corsPolicy = app.Environment.IsDevelopment() ? "AllowAll" : "RestrictToNextJs";
app.UseCors(corsPolicy);

app.UseRateLimiter();
app.UseMiddleware<ValidationExceptionHandlingMiddleware>();

//app.MapUploadEndpoint();
//app.MapBackupEndpoints();
app.MapRealEstateEndpoints();
app.MapSchoolEndpoints();
app.MapChartsEndpoints();
app.MapKPIEndpoints();
app.MapComparisonEndpoints();
app.MapStatisticsEndpoints();
//app.MapKommunEndpoints();
app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();


