using FastighetsKompassen.API.Azure;
using FastighetsKompassen.API.Extensions;
using FastighetsKompassen.API.Health;
using FastighetsKompassen.API.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Azure.Identity;


ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);










var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");







bool isProduction = builder.Environment.IsProduction();

//key vault
//await azureKeyVault.Initialize();
// Ersätt {password} med det faktiska lösenordet från Key Vault





//caching
builder.Services.AddMemoryCache();
//hateoas
builder.Services.AddHttpContextAccessor();

//inject singleton & scoped services
builder.Services.ConfigureInjections();
//Cors
builder.Services.ConfigureCors();
//RateLimiter
builder.Services.ConfigureGlobaFixedlRateLimiter();
builder.Services.ConfigureRateLimiterByIPAddress();
//MediatR
builder.Services.ConfigureAddMediatR();
//DbContext
builder.Services.ConfigureAddDbContext(connectionStringTemplate);
//health checks
builder.Services.ConfigureHealthChecks(builder.Configuration);
//validators
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
//maxbody size & multipartbody length extended
builder.Services.ConfigureServicesForUploads();


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

app.ConfiguareEndpoints();
app.ConfigureHealthChecks();







// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();