namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetProviderAvailability;

public record GetProviderAvailabilityQuery(int ProviderId) : IRequest<Result<List<ProviderAvailabilityResponseDto>>>;
