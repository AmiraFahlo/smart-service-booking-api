using System;
using System.Collections.Generic;
using SmartServiceBooking.Domain.Entities.Base;

namespace SmartServiceBooking.Domain.Entities.Service
{
    public class Category : AuditableEntity, ISoftDeletable
    {
        private Category() { }

        public int? ParentId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? IconUrl { get; private set; }
        public string? Path { get; private set; }
        public int Level { get; private set; }
        public bool IsActive { get; private set; }

        public Category? Parent { get; private set; }
        public ICollection<Category> SubCategories { get; private set; } = new HashSet<Category>();
        public ICollection<ServiceEntity> Services { get; private set; } = new HashSet<ServiceEntity>();

        public static Category Create(
            string name,
            int level,
            int? parentId = null,
            bool isActive = true,
            int? id = null,
            DateTime? createdAt = null)
        {
            var category = new Category
            {
                Name = name,
                Level = level,
                ParentId = parentId,
                IsActive = isActive
            };

            if (id.HasValue)
                category.Id = id.Value;

            if (createdAt.HasValue)
                category.SetAudit(createdAt.Value);

            return category;
        }

        /// <summary>
        /// Updates mutable fields. UpdatedAt is set automatically by AuditableEntityInterceptor.
        /// </summary>
        public void Update(string name, bool isActive, string? iconUrl = null)
        {
            Name = name;
            IsActive = isActive;
            IconUrl = iconUrl;
        }

        /// <summary>
        /// Soft delete — marks as deleted and deactivates.
        /// Only allowed when no active services belong to this category.
        /// Guard enforced in DeleteCategoryCommandHandler.
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            IsActive = false;
        }

        /// <summary>
        /// Restores a soft-deleted category. Requires .IgnoreQueryFilters() in the handler.
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            IsActive = true;
        }
    }
}
