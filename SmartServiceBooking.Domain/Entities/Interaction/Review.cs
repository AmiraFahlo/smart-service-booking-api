using System;
using SmartServiceBooking.Domain.Entities.Base;
using SmartServiceBooking.Domain.Entities.Identity;

namespace SmartServiceBooking.Domain.Entities.Interaction
{
    public class Review : AuditableEntity<long>, ISoftDeletable
    {
        private Review() { }

        public long BookingId { get; private set; }
        public int? ReviewerId { get; private set; }
        public int RateeId { get; private set; }
        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public Booking.Booking? Booking { get; private set; }
        public ApplicationUser? Reviewer { get; private set; }
        public ApplicationUser? Ratee { get; private set; }

        public static Review Create(
            long bookingId,
            int? reviewerId,
            int rateeId,
            int rating,
            string? comment = null,
            long? id = null,
            DateTime? createdAt = null)
        {
            var review = new Review
            {
                BookingId = bookingId,
                ReviewerId = reviewerId,
                RateeId = rateeId,
                Rating = rating,
                Comment = comment
            };

            if (id.HasValue)
                review.Id = id.Value;

            if (createdAt.HasValue)
                review.SetAudit(createdAt.Value);

            return review;
        }
    }
}
