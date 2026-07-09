using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SmartServiceBookingPlatform_API.Infrastructure;
using SmartServiceBookingPlatform_API.Extensions.Transformers;

namespace SmartServiceBookingPlatform_API.Extensions;

public static class OpenApiExtensions
{
    // Production-only cache for the landing page HTML.
    // Populated on first request and reused thereafter to avoid repeated disk I/O.
    // Intentionally NOT used in Development so that edits to api-landing.html
    // are reflected immediately without restarting the server.
    private static string? _cachedLandingPage;

    /// <summary>
    /// Registers Microsoft.AspNetCore.OpenApi ("v1" document) with JWT Bearer
    /// security scheme transformers for both document-level and operation-level decoration.
    /// Title and Description are read from ApiDocumentation config via IOptions.
    /// </summary>
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
        // Register IOptions<ApiDocumentationOptions> bound to "ApiDocumentation" section
        services.AddOptions<ApiDocumentationOptions>()
                .BindConfiguration(ApiDocumentationOptions.SectionName);

        services.AddOpenApi("v1", options =>
        {
            // Use a document transformer that resolves IOptions at runtime,
            // so Title/Description always come from appsettings (never hardcoded).
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                var opts = context.ApplicationServices
                                  .GetRequiredService<IOptions<ApiDocumentationOptions>>()
                                  .Value;

                document.Info = new OpenApiInfo
                {
                    Title       = opts.Title,
                    Version     = "v1",
                    Description = opts.Description
                };
                return Task.CompletedTask;
            });

            // Registers the Bearer scheme in document.Components.SecuritySchemes
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

            // Adds the security requirement on each [Authorize] operation
            options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
        });

        return services;
    }

    /// <summary>
    /// Configures the OpenAPI middleware pipeline.
    ///
    /// Path note — Microsoft.AspNetCore.OpenApi registers:
    ///   GET /openapi/{documentName}.json
    /// For document name "v1" this resolves to:
    ///   GET /openapi/v1.json   ✔
    /// The SwaggerEndpoint below uses the same path, so they are guaranteed to align.
    ///
    /// Production exposure is controlled by ApiDocumentation:ExposeInProduction in
    /// appsettings.json (defaults to false). When enabled outside Development,
    /// the /openapi/v1.json endpoint is protected by the "AdminOnly" policy.
    /// </summary>
    public static WebApplication UseOpenApiDocumentation(this WebApplication app)
    {
        var isDev            = app.Environment.IsDevelopment();
        var opts             = app.Services.GetRequiredService<IOptions<ApiDocumentationOptions>>().Value;
        var shouldExposeSpec = isDev || opts.ExposeInProduction;

        // ── OpenAPI JSON endpoint: GET /openapi/v1.json ──────────────────────
        // Always on in Development (no auth required — easy local testing).
        // In Production: opt-in via config AND protected by "AdminOnly" policy.
        if (shouldExposeSpec)
        {
            var openApiEndpoint = app.MapOpenApi(); // → GET /openapi/v1.json

            if (!isDev)
            {
                // Protect the spec endpoint in Production so it cannot be accessed
                // without a valid JWT that includes the "Admin" role.
                // Returns 401 for unauthenticated requests, 403 for wrong role.
                openApiEndpoint.RequireAuthorization(AuthorizationExtensions.PolicyNames.AdminOnly);
            }

            // ── Scalar API Reference UI ───────────────────────────────────────
            app.MapScalarApiReference(options =>
            {
                options.WithTitle(opts.Title)
                       .WithTheme(ScalarTheme.Mars);

                // Pre-select a default HTTP client only in Development.
                // In Production the Scalar UI opens with no pre-selection.
                if (isDev)
                    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        // ── Swagger UI (Development only) ────────────────────────────────────
        // Swashbuckle.AspNetCore.SwaggerUI is used purely as a UI shell;
        // it fetches the spec from /openapi/v1.json produced by MapOpenApi() above.
        if (isDev)
        {
            app.UseSwaggerUI(options =>
            {
                // Path MUST match what MapOpenApi() registers: /openapi/{name}.json
                options.SwaggerEndpoint("/openapi/v1.json", $"{opts.Title} v1");
                options.RoutePrefix = "swagger";
            });
        }

        // ── Landing page (always registered) ─────────────────────────────────
        app.MapGet("/", () =>
        {
            var html = GetLandingPageHtml(app.Environment);
            return Results.Content(html, "text/html");
        }).ExcludeFromDescription();

        return app;
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    /// <summary>
    /// Returns the landing page HTML, applying cache logic based on environment:
    /// • Development → always reads from disk (instant feedback during development).
    /// • Production  → reads once and caches; subsequent calls return the cached string.
    /// </summary>
    private static string GetLandingPageHtml(IWebHostEnvironment env)
    {
        // In Development: bypass cache so file changes are visible without restart
        if (env.IsDevelopment())
            return BuildLandingPageHtml(env);

        // In Production: lazy-initialise the cache on first request (??= is
        // effectively thread-safe for string references: worst case two threads
        // both read the file and one result is discarded — no corrupted state).
        return _cachedLandingPage ??= BuildLandingPageHtml(env);
    }

    /// <summary>
    /// Reads api-landing.html from wwwroot, injects the Swagger card placeholder
    /// in Development, and returns the final HTML string.
    /// </summary>
    private static string BuildLandingPageHtml(IWebHostEnvironment env)
    {
        var filePath = Path.Combine(
            env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"),
            "api-landing.html");

        if (!File.Exists(filePath))
        {
            // Graceful fallback when the file is missing (e.g. fresh clone, CI)
            return "<html><body><h1>API Documentation</h1><a href='/scalar/v1'>Scalar API</a></body></html>";
        }

        var html = File.ReadAllText(filePath);

        // The Swagger card is injected only in Development where the UI is available
        var swaggerCard = env.IsDevelopment()
            ? """
              <a href="/swagger" class="card">
                  <div class="card-icon">⚡</div>
                  <h2>Swagger UI</h2>
                  <p>Classic interface for testing and debugging.</p>
              </a>
              """
            : string.Empty;

        return html.Replace("<!--SWAGGER_BUTTON_PLACEHOLDER-->", swaggerCard);
    }
}
