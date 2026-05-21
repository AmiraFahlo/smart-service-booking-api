using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Queries.GetServiceProviderById;

public record GetServiceProviderByIdQuery(int Id) : IRequest<Result<ServiceProviderProfileDto>>;
