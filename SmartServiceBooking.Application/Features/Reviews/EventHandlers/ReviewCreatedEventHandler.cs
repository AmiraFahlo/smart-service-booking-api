using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartServiceBooking.Application.Common.Interfaces;
using SmartServiceBooking.Domain.Events.Reviews;

namespace SmartServiceBooking.Application.Features.Reviews.EventHandlers;

public sealed class ReviewCreatedEventHandler : INotificationHandler<ReviewCreatedEvent>
{
    private readonly IApplicationDbContext _context;

    public ReviewCreatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ReviewCreatedEvent notification, CancellationToken cancellationToken)
    {
        var provider = await _context.ServiceProviders
            .FirstOrDefaultAsync(sp => sp.Id == notification.ServiceProviderId, cancellationToken);

        if (provider is null)
            return;

        // Recalculate from source of truth (all reviews for this provider)
        var stats = await _context.Reviews
            .Where(r => r.ServiceProviderId == notification.ServiceProviderId)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                AverageRating = g.Average(r => (decimal)r.Rating),
                TotalCount = g.Count()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (stats is not null)
        {
            provider.UpdateRating(stats.AverageRating, stats.TotalCount);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
