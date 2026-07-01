namespace SmartServiceBookingPlatform_API.Infrastructure;

/// <summary>
/// Strongly-typed representation of the "ApiDocumentation" section in appsettings.json.
/// Bound via IOptions&lt;ApiDocumentationOptions&gt; — register with:
///   builder.Services.Configure&lt;ApiDocumentationOptions&gt;(
///       builder.Configuration.GetSection("ApiDocumentation"));
/// </summary>
public sealed class ApiDocumentationOptions
{
    public const string SectionName = "ApiDocumentation";

    /// <summary>
    /// When true, the GET /openapi/v1.json endpoint and Scalar UI are registered
    /// even outside the Development environment.
    /// Requires the "AdminOnly" authorization policy to be satisfied.
    /// Default: false.
    /// </summary>
    public bool ExposeInProduction { get; set; } = false;

    /// <summary>Title shown in the OpenAPI document info block.</summary>
    public string Title { get; set; } = "API";

    /// <summary>Description shown in the OpenAPI document info block.</summary>
    public string Description { get; set; } = string.Empty;
}
