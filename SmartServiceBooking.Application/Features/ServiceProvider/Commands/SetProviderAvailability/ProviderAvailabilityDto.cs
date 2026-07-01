using System;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.SetProviderAvailability;

public record ProviderAvailabilityDto(
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
);
