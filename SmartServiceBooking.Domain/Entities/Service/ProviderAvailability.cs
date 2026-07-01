namespace SmartServiceBooking.Domain.Entities.Service;

public class ProviderAvailability : BaseEntity<long>
{
    private ProviderAvailability() { } // EF Core required

    public int ProviderId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public ServiceProvider? Provider { get; private set; }

    public static ProviderAvailability Create(
        int providerId, 
        DayOfWeek dayOfWeek, 
        TimeOnly startTime, 
        TimeOnly endTime)
    {
        if (endTime <= startTime)
        {
            throw new ArgumentException("End time must be greater than start time.", nameof(endTime));
        }

        return new ProviderAvailability
        {
            ProviderId = providerId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime
        };
    }
}
