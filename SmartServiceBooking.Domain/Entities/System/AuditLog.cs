using System;
using SmartServiceBooking.Domain.Entities.Base;

namespace SmartServiceBooking.Domain.Entities.System
{
    public class AuditLog : BaseEntity
    {
        public int UserId { get; set; }
        public string EntityName { get; set; }
        public int EntityId { get; set; }
        public string Action { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
