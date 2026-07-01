using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Errors;
using SmartServiceBooking.Domain.Shared;

using SmartServiceBooking.Application.Features.Bookings.Extensions;

namespace SmartServiceBooking.Application.Features.Bookings.Queries.GetBookingById;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetBookingByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.ParsedUserId;
        if (userId == null)
            return Result.Failure<BookingDto>(new Error("Auth.Unauthorized", "User is not authenticated.", ErrorType.Unauthorized));

        var dto = await _context.Bookings
            .AsNoTracking()
            .Where(b => b.Id == request.Id)
            .MapToDto()
            .FirstOrDefaultAsync(cancellationToken);

        if (dto == null)
            return Result.Failure<BookingDto>(BookingErrors.NotFound(request.Id));

        // Access control: only the booking owner, their assigned provider, or an administrator can view it
        var isCustomer = dto.CustomerId == userId.Value;
        var isAdmin = _currentUserService.IsInRole("Admin");
        var isAssignedProvider = false;

        if (!isCustomer && !isAdmin && _currentUserService.IsInRole("Provider"))
        {
            var provider = await _context.ServiceProviders
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId.Value, cancellationToken);

            isAssignedProvider = provider != null && dto.ProviderId == provider.Id;
        }

        if (!isCustomer && !isAssignedProvider && !isAdmin)
            return Result.Failure<BookingDto>(new Error("Auth.Forbidden", "You do not have access to this booking.", ErrorType.Forbidden));

        return Result.Success(dto);
    }
}
