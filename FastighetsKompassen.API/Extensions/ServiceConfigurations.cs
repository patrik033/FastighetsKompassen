using FastighetsKompassen.API.HATEOAS;
using FastighetsKompassen.API.ReadToFile;
using FastighetsKompassen.API.Services;
using FastighetsKompassen.API.Validation;
using FastighetsKompassen.Infrastructure.Data;
using FastighetsKompassen.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;

namespace FastighetsKompassen.API.Extensions
{
    public static class ServiceConfigurations
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
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
        }

        public static void ConfigureGlobaFixedlRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddFixedWindowLimiter("GlobalLimiter", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 30;
                    limiterOptions.Window = TimeSpan.FromSeconds(30);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 30;
                });
            });
        }

        public static void ConfigureRateLimiterByIPAddress(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("fixed", httpcontext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpcontext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromSeconds(10)
                    }));
            });
        }

        public static void ConfigureAddDbContext(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("FastighetsKompassen.API"));
            });
        }

        public static void ConfigureAddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
        }

        public static void ConfigureServicesForUploads(this IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 419430400;
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 400 * 1024 * 1024;
            });
        }

        public static void ConfigureInjections(this IServiceCollection services)
        {
            //singleton
            services.AddSingleton<ReadExcelDataToClass>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //scoped
            services.AddScoped<IHateoasService, HateoasService>();
            services.AddScoped<BackupService>();
            services.AddScoped<KommunService>();
        }
    }
}
