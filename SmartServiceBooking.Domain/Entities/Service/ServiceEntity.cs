using System;
using System.Collections.Generic;
using SmartServiceBooking.Domain.Entities.Base;
using SmartServiceBooking.Domain.Enums;

namespace SmartServiceBooking.Domain.Entities.Service
{
    public class ServiceEntity : AuditableEntity<long>, ISoftDeletable
    {
        private ServiceEntity() { }

        public int CategoryId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public decimal BasePrice { get; private set; }
        public PriceUnit PriceUnit { get; private set; }
        public bool IsActive { get; private set; }
        public Category Category { get; private set; } = default!;
        public ICollection<ProviderService> ProviderServices { get; private set; } = new HashSet<ProviderService>();

        public static ServiceEntity Create(
            int categoryId,
            string name,
            decimal basePrice,
            PriceUnit priceUnit,
            string? description = null,
            bool isActive = true,
            long? id = null,
            DateTime? createdAt = null)
        {
            var service = new ServiceEntity
            {
                CategoryId = categoryId,
                Name = name,
                BasePrice = basePrice,
                PriceUnit = priceUnit,
                Description = description,
                IsActive = isActive
            };

            if (id.HasValue)
                service.Id = id.Value;

            if (createdAt.HasValue)
                service.SetAudit(createdAt.Value);

            return service;
        }

        /// <summary>
        /// Updates the service's mutable fields.
        /// UpdatedAt will be set automatically by AuditableEntityInterceptor.
        /// </summary>
        public void Update(
            int categoryId,
            string name,
            decimal basePrice,
            PriceUnit priceUnit,
            string? description,
            bool isActive)
        {
            CategoryId = categoryId;
            Name = name;
            BasePrice = basePrice;
            PriceUnit = priceUnit;
            Description = description;
            IsActive = isActive;
        }

        /// <summary>
        /// Marks the service as deleted (Soft Delete).
        /// The record stays in the database but will be filtered out from queries.
        /// UpdatedAt will be set automatically by AuditableEntityInterceptor.
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            IsActive = false;
        }
    }
}
