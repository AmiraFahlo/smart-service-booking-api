// Leaflet Map Interop for SmartServiceBooking Aleppo Platform
window.aleppoMap = {
    mapInstance: null,
    markersGroup: null,

    init: function (elementId, lat, lng, zoom) {
        if (!lat) lat = 36.2021; // Aleppo center
        if (!lng) lng = 37.1343;
        if (!zoom) zoom = 13;

        var container = document.getElementById(elementId);
        if (!container) return;

        // If map already exists, remove it cleanly
        if (this.mapInstance) {
            this.mapInstance.remove();
            this.mapInstance = null;
        }

        this.mapInstance = L.map(elementId, {
            center: [lat, lng],
            zoom: zoom,
            zoomControl: true
        });

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors',
            maxZoom: 19
        }).addTo(this.mapInstance);

        this.markersGroup = L.layerGroup().addTo(this.mapInstance);

        // Force resize recalculation in case container was hidden
        setTimeout(() => {
            if (this.mapInstance) {
                this.mapInstance.invalidateSize();
            }
        }, 200);
    },

    setMarkers: function (providers) {
        if (!this.mapInstance || !this.markersGroup) return;

        this.markersGroup.clearLayers();

        if (!providers || providers.length === 0) return;

        var bounds = [];

        providers.forEach(p => {
            if (p.latitude && p.longitude) {
                var popupContent = `
                    <div style="direction: rtl; text-align: right; font-family: 'Cairo', sans-serif; min-width: 200px;">
                        <h6 style="font-weight: 700; color: #1e293b; margin-bottom: 4px;">${p.name || p.businessName}</h6>
                        <div style="display: flex; gap: 4px; margin-bottom: 6px; flex-wrap: wrap;">
                            <span style="background: #e0f2fe; color: #0369a1; font-size: 11px; padding: 2px 8px; border-radius: 9999px; font-weight: 600;">${p.specialty || ''}</span>
                            <span style="background: #f1f5f9; color: #475569; font-size: 11px; padding: 2px 8px; border-radius: 9999px;">حي ${p.district || 'حلب'}</span>
                        </div>
                        <div style="font-size: 12px; color: #059669; font-weight: 700; margin-bottom: 4px;">
                            ${p.priceFormattedSYP || (p.price ? p.price.toLocaleString() + ' ل.س' : '')} 
                            <span style="color: #64748b; font-weight: normal; font-size: 11px;">(${p.priceFormattedUSD || '$' + (p.priceInUSD || 0)})</span>
                        </div>
                        <div style="font-size: 12px; color: #f59e0b; margin-bottom: 8px;">
                            ★ ${p.rating} <span style="color: #94a3b8; font-size: 11px;">(${p.reviewCount || 0} تقييم)</span>
                        </div>
                        <a href="services" style="display: block; text-align: center; background: linear-gradient(135deg, #2563eb, #1d4ed8); color: #fff; padding: 5px 10px; border-radius: 8px; text-decoration: none; font-size: 12px; font-weight: 600;">
                            حجز موعد صيانة
                        </a>
                    </div>
                `;

                var marker = L.marker([p.latitude, p.longitude]).bindPopup(popupContent);
                this.markersGroup.addLayer(marker);
                bounds.push([p.latitude, p.longitude]);
            }
        });

        if (bounds.length > 0) {
            this.mapInstance.fitBounds(bounds, { padding: [40, 40], maxZoom: 15 });
        }
    },

    focusLocation: function (lat, lng, zoomLevel) {
        if (!this.mapInstance || !lat || !lng) return;
        this.mapInstance.flyTo([lat, lng], zoomLevel || 16, { duration: 1.2 });
    }
};
