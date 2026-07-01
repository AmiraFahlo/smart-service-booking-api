using System;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProviderAvailability;

public record ProviderAvailabilityResponseDto(
    string DayOfWeek, // We can return it as string for better frontend consumption
    int DayOfWeekValue,
    TimeOnly StartTime,
    TimeOnly EndTime
);
