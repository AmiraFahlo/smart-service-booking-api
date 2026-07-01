using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Events.Reviews;

public sealed record ReviewCreatedEvent(
    long ReviewId,
    int ServiceProviderId,
    int Rating) : IDomainEvent;
