using FastighetsKompassen.API.Endpoints;
using FastighetsKompassen.API.Extensions;
using FastighetsKompassen.API.ReadToFile;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;



ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FastighetsKompassen.API")));


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
builder.Services.AddScoped<SchoolService>();






// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<MultipleFileUploadOperationFilter>();
});

var app = builder.Build();



app.MapKommunEndpoints();
app.MapBackupEndpoints();
app.MapPoliceEndpoints();
app.MapRealEstateEndpoints();
app.MapSchoolEndpoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();



app.Run();