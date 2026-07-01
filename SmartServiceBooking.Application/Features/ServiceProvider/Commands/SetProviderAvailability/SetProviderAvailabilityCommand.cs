using System.Collections.Generic;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.SetProviderAvailability;

public record SetProviderAvailabilityCommand(
    int ProviderId,
    List<ProviderAvailabilityDto> Availabilities
) : IRequest<Result>;
