using System;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Entities.Identity;
using SmartServiceBooking.Infrastructure.Identity.Options;
using SmartServiceBooking.Infrastructure.Identity.Services;
using SmartServiceBooking.Infrastructure.Persistence;
using SmartServiceBooking.Infrastructure.Persistence.Interceptors;
using SmartServiceBooking.Infrastructure.Persistence.Options;

namespace SmartServiceBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind DatabaseOptions
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

        // Register Interceptors
        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();

        // Add DbContext
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var auditInterceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();
            var domainEventsInterceptor = serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>();

            options.UseSqlServer(databaseOptions.ConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: databaseOptions.MaxRetryCount,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(databaseOptions.CommandTimeout);
            })
            .AddInterceptors(auditInterceptor, domainEventsInterceptor);

            if (databaseOptions.EnableDetailedErrors) options.EnableDetailedErrors();
            if (databaseOptions.EnableSensitiveDataLogging) options.EnableSensitiveDataLogging();
        });

        // Register Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // ─── JWT AUTHENTICATION ─────────────────────────────────────────────
        var jwtOptions = new JwtOptions();
        configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);
        services.AddSingleton(Options.Create(jwtOptions));

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddMemoryCache();
        services.AddScoped<ICacheService, Cache.MemoryCacheService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                NameClaimType = ClaimTypes.NameIdentifier,
                RoleClaimType = ClaimTypes.Role
            };
        });
        // ─────────────────────────────────────────────────────────────────────

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IServiceProviderRepository, SmartServiceBooking.Infrastructure.Persistence.Repositories.ServiceProviderRepository>();
        services.AddScoped<IServiceRepository, SmartServiceBooking.Infrastructure.Persistence.Repositories.ServiceRepository>();
        services.AddScoped<ICategoryRepository, SmartServiceBooking.Infrastructure.Persistence.Repositories.CategoryRepository>();
        services.AddScoped<IBookingRepository, SmartServiceBooking.Infrastructure.Persistence.Repositories.BookingRepository>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
