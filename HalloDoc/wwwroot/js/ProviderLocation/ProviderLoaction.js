﻿var locationdata;
window.onload = function () {
    $('.admin-layout-nav').removeClass('admin-layout-nav-active');
    $('#nav-provider-location-tab').addClass('admin-layout-nav-active');
}
$.ajax({
    url: '/ProviderLocation/GetLocations',
    method: 'GET',
    async: false,
    success: function (response) {
        locationdata = JSON.parse(response)
    }
})

var map = L.map('map').setView([22.2, 77.1], 5);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);


for (var i = 0; i < locationdata.length; i++) {
    var popupContent;
    var iconHtml = '<div class="d-flex" style="width: 30px; height: 30px; border-radius: 50%; overflow: hidden; border: 4px solid #008000;">' +
        '<img src="' + locationdata[i].Photo + '" style="width: 100%; height: auto;" />' +
        '</div>' +
        '<div style="width: 0; height: 0; border-left: 10px solid transparent; border-right: 10px solid transparent; border-top: 15px solid #008000; margin-left : 5px ;margin-top: -4px;"></div>';

    var customIcon = L.divIcon({
        className: 'custom-icon',
        html: iconHtml,
        iconSize: [30, 45],
        iconAnchor: [15, 45],
    });
    if (locationdata[i].Photo != null) {
        popupContent = '<img class="openeditphy" data-id="' + locationdata[i].Physicianid + '" width = "60%" src="' + locationdata[i].Photo + '" />' +
            '<p>Physician: ' + locationdata[i].Name + '</p>';
    }
    else {
        popupContent = '<img class="openeditphy" data-id="' + locationdata[i].Physicianid + '" width = "60%" src="/images/profile-icon.png" />' +
            '<p>Physician: ' + locationdata[i].Name + '</p>';
    }
    var marker = L.marker([locationdata[i].Lat, locationdata[i].Long], { icon: customIcon }).addTo(map)
        .bindPopup(popupContent);

    marker.on('popupopen', function (e) {
        $('.openeditphy').on('click', function () {
            var physicianid = ($(this).data('id'));
            $.ajax({
                url: '/Provider/EditProviderAccount',
                type: 'POST',
                data: { physicianid: physicianid },
                success: function (result) {
                    $('#providerLocationMainDiv').html(result);
                }
            });
        });
    });
}