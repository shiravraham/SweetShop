$(document).ready(function () {
    var canvas = document.getElementById('canvas');
    if (canvas.getContext) {
        var ctx = canvas.getContext('2d');

        ctx.beginPath();
        ctx.moveTo(1, 1);
        ctx.lineTo(1, 50);
        ctx.lineTo(50, 25);
        ctx.fillStyle = "white";
        ctx.fill();
    }

    $('#myModal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus');
    });
});
