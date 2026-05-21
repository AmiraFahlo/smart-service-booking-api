using MediatR;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.DeleteServiceProvider;

public record DeleteServiceProviderCommand(int Id) : IRequest<Result>;
