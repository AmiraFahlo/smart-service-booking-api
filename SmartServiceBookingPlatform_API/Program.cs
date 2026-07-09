using Microsoft.EntityFrameworkCore;
using SmartServiceBookingPlatform_API.Extensions;
using SmartServiceBooking.Infrastructure;
using SmartServiceBooking.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Register Application Layer
builder.Services.AddApplicationServices();

// Register Infrastructure Layer
builder.Services.AddInfrastructureServices(builder.Configuration);

// Authorization Policies — centralized definitions consumed by controllers & middleware
builder.Services.AddAuthorizationPolicies();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// OpenAPI/Swagger/Scalar — registered AFTER auth middleware so that
// RequireAuthorization("AdminOnly") on the spec endpoint is enforced correctly.
app.UseOpenApiDocumentation();

// Output Cache Middleware — must be AFTER auth and BEFORE MapControllers
app.UseOutputCaching();

app.MapControllers();

app.Run();
