namespace SmartServiceBooking.Application.Features.Services.Commands.DeleteService;
public record DeleteServiceCommand(long Id) : IRequest<Result>;
