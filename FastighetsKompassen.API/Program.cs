using FastighetsKompassen.API.Endpoints;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FastighetsKompassen.API")));

//builder.Services.AddScoped<BackupService>();
builder.Services.AddScoped<BackupService>();
builder.Services.AddScoped<KommunService>();




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.MapKommunEndpoints();
app.MapBackupEndpoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();



app.Run();