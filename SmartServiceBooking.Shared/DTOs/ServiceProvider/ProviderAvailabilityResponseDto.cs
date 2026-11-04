using System;

namespace SmartServiceBooking.Shared.DTOs.ServiceProvider
;
public record ProviderAvailabilityResponseDto(
    string DayOfWeek, // We can return it as string for better frontend consumption
    int DayOfWeekValue,
    TimeOnly StartTime,
    TimeOnly EndTime
);


