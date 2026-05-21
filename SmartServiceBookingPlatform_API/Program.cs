using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SmartServiceBookingPlatform_API.Extensions;
using SmartServiceBooking.Infrastructure;
using SmartServiceBooking.Application;
using Microsoft.OpenApi.Models; // For OpenAPI models

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Register Application Layer
builder.Services.AddApplicationServices();

// Register Infrastructure Layer
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add OpenAPI Documentation (Swagger/Scalar)
builder.Services.AddOpenApiDocumentation();

// Exception Handling & Problem Details
builder.Services.AddExceptionHandler<SmartServiceBookingPlatform_API.Common.Errors.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Output Cache — In-Memory (swap AddOutputCaching internals for Redis when needed)
builder.Services.AddOutputCaching();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SmartServiceBooking.Infrastructure.Persistence.ApplicationDbContext>();
        var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<SmartServiceBooking.Domain.Entities.Identity.ApplicationUser>>();
        var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<SmartServiceBooking.Domain.Entities.Identity.ApplicationRole>>();

        await context.Database.MigrateAsync();
        await SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed.IdentitySeeder.SeedAsync(userManager, roleManager, context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseOpenApiDocumentation();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Output Cache Middleware — must be AFTER auth and BEFORE MapControllers
app.UseOutputCaching();

app.MapControllers();

app.Run();
