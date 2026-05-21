using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SmartServiceBookingPlatform_API.Extensions.Transformers;

namespace SmartServiceBookingPlatform_API.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "Smart Service Booking API",
                    Version = "v1",
                    Description = "API for the Smart Service Booking Platform. Built with Clean Architecture and .NET 9."
                };
                return Task.CompletedTask;
            });
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
        });

        return services;
    }

    public static WebApplication UseOpenApiDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Smart Service Booking API");
                options.RoutePrefix = "swagger"; // Explicitly set route
            });

            app.MapScalarApiReference(options => 
            {
                options.WithTitle("Smart Booking API Reference")
                       .WithTheme(ScalarTheme.Mars)
                       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
            
            // Serve Landing Page at root
            app.MapGet("/", () => 
            {
                var content = GetLandingPageHtml(app.Environment);
                return Results.Content(content, "text/html");
            }).ExcludeFromDescription();
        }
        else 
        {
             // Production Mode: Only Scalar, No Swagger/OpenApi JSON exposure if minimal needed, 
             // but user asked for "Scalar in Production" usually implies exposing the docs.
             // However, AddOpenApi typically registers the endpoints only if MapOpenApi is called. 
             // MapOpenApi enables the JSON endpoint. Scalar needs the JSON.
             
             app.MapOpenApi(); // Required for Scalar to fetch the spec
             app.MapScalarApiReference(options => 
             {
                 options.WithTitle("Smart Booking API Reference")
                        .WithTheme(ScalarTheme.Mars);
             });

             // Serve Landing Page at root (Production Mode)
             app.MapGet("/", () => 
             {
                 var content = GetLandingPageHtml(app.Environment);
                 return Results.Content(content, "text/html");
             }).ExcludeFromDescription();
        }

        return app;
    }

    private static string GetLandingPageHtml(IWebHostEnvironment env)
    {
        var filePath = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), "api-landing.html");
        
        if (!File.Exists(filePath))
        {
             // Fallback if file missing
             return "<html><body><h1>API Documentation</h1><a href='/scalar/v1'>Scalar API</a></body></html>";
        }

        var html = File.ReadAllText(filePath);
        
        var swaggerOption = env.IsDevelopment() 
            ? @"<a href=""/swagger"" class=""card"">
                    <div class=""card-icon"">⚡</div>
                    <h2>Swagger UI</h2>
                    <p>Classic interface for testing and debugging.</p>
                </a>" 
            : "";

        return html.Replace("<!--SWAGGER_BUTTON_PLACEHOLDER-->", swaggerOption);
    }
}
