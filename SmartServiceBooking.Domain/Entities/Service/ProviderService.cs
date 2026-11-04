namespace SmartServiceBooking.Domain.Entities.Service
{
    public class ProviderService : AuditableEntity<long>
    {
        private ProviderService() { }

        public int ProviderId { get; private set; }
        public long ServiceId { get; private set; }
        public decimal? CustomPrice { get; private set; } // in SYP
        public decimal? CustomPriceInUSD { get; private set; } // in USD
        public bool IsActive { get; private set; }
        public ServiceProvider? Provider { get; private set; }
        public ServiceEntity? Service { get; private set; }

        public static ProviderService Create(
            int providerId,
            long serviceId,
            decimal? customPrice = null,
            decimal? customPriceInUSD = null,
            bool isActive = true,
            long? id = null)
        {
            var ps = new ProviderService
            {
                ProviderId = providerId,
                ServiceId = serviceId,
                CustomPrice = customPrice,
                CustomPriceInUSD = customPriceInUSD ?? (customPrice.HasValue ? Math.Round(customPrice.Value / 15000m, 2) : null),
                IsActive = isActive
            };

            if (id.HasValue)
                ps.Id = id.Value;

            return ps;
        }
    }
}
