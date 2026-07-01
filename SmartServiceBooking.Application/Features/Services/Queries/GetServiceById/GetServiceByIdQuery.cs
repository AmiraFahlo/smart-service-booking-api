namespace SmartServiceBooking.Application.Features.Services.Queries.GetServiceById;
public record GetServiceByIdQuery(long Id) : IRequest<Result<ServiceDetailDto>>;
