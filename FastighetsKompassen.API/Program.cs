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
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;



ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
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



//builder.Services.AddScoped<BackupService>();
builder.Services.AddSingleton<ReadExcelDataToClass>();


builder.Services.AddScoped<BackupService>();
builder.Services.AddScoped<KommunService>();
builder.Services.AddScoped<PoliceService>();
builder.Services.AddScoped<RealEstateService>();
builder.Services.AddScoped<ChartService>();
builder.Services.AddScoped<KPIService>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<MultipleFileUploadOperationFilter>();
});

var app = builder.Build();


app.UseCors();
app.UseMiddleware<ValidationExceptionHandlingMiddleware>();
app.MapKommunEndpoints();
app.MapBackupEndpoints();
app.MapPoliceEndpoints();
app.MapRealEstateEndpoints();
app.MapSchoolEndpoints();
app.MapChartsEndpoints();
app.MapKPIEndpoints();
app.MapComparisonEndpoints();
app.MapStatisticsEndpoints();
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