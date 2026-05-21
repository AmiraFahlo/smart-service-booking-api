using System;

namespace SmartServiceBooking.Domain.Entities.Base
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public string? UpdatedBy { get; protected set; }
        public bool IsDeleted { get; protected set; }

        /// <summary>
        /// For Seed Data only — sets CreatedAt to a fixed value for stable migrations.
        /// </summary>
        public void SetAudit(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }

        /// <summary>
        /// Called by AuditableEntityInterceptor only — not for direct use in handlers.
        /// Sets CreatedAt if it hasn't been explicitly assigned (i.e., not a seed record).
        /// </summary>
        internal void SetCreatedAt(DateTime createdAt)
        {
            if (CreatedAt == default)
                CreatedAt = createdAt;
        }

        /// <summary>
        /// Called by AuditableEntityInterceptor only — sets the last modified timestamp.
        /// </summary>
        internal void SetUpdatedAt(DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
        }
    }
}
