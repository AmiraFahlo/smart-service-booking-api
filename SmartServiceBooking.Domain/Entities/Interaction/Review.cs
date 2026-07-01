using System;
using SmartServiceBooking.Domain.Entities.Base;
using SmartServiceBooking.Domain.Entities.Identity;
using SmartServiceBooking.Domain.Entities.Service;
using SmartServiceBooking.Domain.Events.Reviews;
using SmartServiceBooking.Domain.Errors;
using SmartServiceBooking.Domain.Shared;

namespace SmartServiceBooking.Domain.Entities.Interaction
{
    public class Review : AuditableEntity<long>, ISoftDeletable
    {
        private Review() { }

        public long BookingId { get; private set; }
        public int CustomerId { get; private set; }
        public int ServiceProviderId { get; private set; }
        public int Rating { get; private set; }
        public string? Comment { get; private set; }

        public Booking.Booking? Booking { get; private set; }
        public ApplicationUser? Customer { get; private set; }
        public ServiceProvider? ServiceProvider { get; private set; }

        public static Result<Review> Create(
            long bookingId,
            int customerId,
            int providerId,
            int rating,
            string? comment)
        {
            if (rating < 1 || rating > 5)
            {
                return Result.Failure<Review>(ReviewErrors.InvalidRating);
            }

            var review = new Review
            {
                BookingId = bookingId,
                CustomerId = customerId,
                ServiceProviderId = providerId,
                Rating = rating,
                Comment = comment
            };

            review.AddDomainEvent(new ReviewCreatedEvent(
                ReviewId: 0, 
                ServiceProviderId: providerId,
                Rating: rating));

            return review;
        }
    }
}
