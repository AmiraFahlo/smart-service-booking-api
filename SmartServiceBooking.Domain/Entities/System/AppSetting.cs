using System;
using SmartServiceBooking.Domain.Entities.Base;

namespace SmartServiceBooking.Domain.Entities.System
{
    public class AppSetting : BaseEntity
    {
        private AppSetting() { }

        public string Key { get; private set; } = string.Empty;
        public string Value { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public static AppSetting Create(string key, string value, string? description = null, int? id = null, DateTime? createdAt = null)
        {
            var setting = new AppSetting
            {
                Key = key,
                Value = value,
                Description = description,
                CreatedAt = createdAt ?? DateTime.MinValue
            };

            if (id.HasValue)
                setting.Id = id.Value;

            return setting;
        }
    }
}
