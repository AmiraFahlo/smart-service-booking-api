using System;
using SmartServiceBooking.Domain.Entities.Base;

namespace SmartServiceBooking.Domain.Entities.Service
{
    public class ProviderAvailability : BaseEntity<long>
    {
        public int ProviderId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public ServiceProvider Provider { get; set; }
    }
}
