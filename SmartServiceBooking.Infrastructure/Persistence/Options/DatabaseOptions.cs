namespace SmartServiceBooking.Infrastructure.Persistence.Options;

public class DatabaseOptions
{
    public const string SectionName = "DatabaseOptions";

    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; } = 3;
    public int CommandTimeout { get; set; } = 30;
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
}
