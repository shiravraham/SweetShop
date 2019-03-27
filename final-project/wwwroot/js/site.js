$(document).ready(function () {
    var tlv = { lat: 32.0660902, lng: 34.7776924};
    var map = new google.maps.Map(
        document.getElementById('map'), { zoom: 13.98, center: tlv });
    var marker = new google.maps.Marker({ position: tlv, map: map });
});