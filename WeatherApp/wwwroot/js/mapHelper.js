let map = null;
let marker = null;

export function initializeMap(elementId, viewLat, viewLon, zoom, markerLat, markerLon) {
    if (map) {
        map.remove();
        map = null;
    }

    map = L.map(elementId).setView([viewLat, viewLon], zoom);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    marker = L.marker([markerLat, markerLon]).addTo(map);
}

export function updateMap(lat, lon) {
    if (map) {
        map.flyTo([lat, lon], 13);

        if (marker) {
            marker.setLatLng([lat, lon]);
        } else {
            marker = L.marker([lat, lon]).addTo(map);
        }
    }
}