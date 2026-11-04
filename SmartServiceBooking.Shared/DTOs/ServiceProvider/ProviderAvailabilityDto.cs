using System;

namespace SmartServiceBooking.Shared.DTOs.ServiceProvider
;
public record ProviderAvailabilityDto(
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
);


