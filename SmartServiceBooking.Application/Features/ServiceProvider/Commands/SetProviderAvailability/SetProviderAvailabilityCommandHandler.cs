namespace SmartServiceBooking.Application.Features.ServiceProvider.Commands.SetProviderAvailability;

public class SetProviderAvailabilityCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    ICacheInvalidator cacheInvalidator)
    : IRequestHandler<SetProviderAvailabilityCommand, Result>
{
    public async Task<Result> Handle(SetProviderAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var provider = await context.ServiceProviders
            .Include(p => p.Availabilities)
            .FirstOrDefaultAsync(p => p.Id == request.ProviderId, cancellationToken);

        if (provider == null)
            return Result.Failure(ServiceProviderErrors.NotFound);

        // 🛡️ Authorization Check (Ownership or Admin)
        var isAdmin = currentUserService.IsInRole(Roles.Admin);
        var isOwner = provider.UserId == currentUserService.ParsedUserId;

        if (!isAdmin && !isOwner)
        {
            return Result.Failure(new Error(
                "Auth.Forbidden",
                "You are not authorized to update this profile's schedule.",
                ErrorType.Forbidden));
        }

        // 1. Bulk Delete Old Schedule
        context.ProviderAvailabilities.RemoveRange(provider.Availabilities);

        // 2. Add New Schedule
        var newAvailabilities = request.Availabilities.Select(dto => 
            ProviderAvailability.Create(
                providerId: provider.Id,
                dayOfWeek: dto.DayOfWeek,
                startTime: dto.StartTime,
                endTime: dto.EndTime
            )).ToList();

        context.ProviderAvailabilities.AddRange(newAvailabilities);

        // 3. Save and Invalidate Cache
        var cacheTag = $"provider-availability-{provider.Id}";
        
        return await context.SaveAndInvalidateAsync(cacheInvalidator, cacheTag, cancellationToken);
    }
}
