/// <reference path="../typings/jquery/jquery.d.ts"/>
var CameraCarApp;
(function (CameraCarApp) {
    var photoPreviewElem = document.getElementById('photo-preview');
    document.getElementById('btn-take-photo')
        .addEventListener('click', function (e) {
        e.preventDefault();
        $.post('/Photo/Take').fail(function () { alert('Fail taking photo.'); });
    });
    setInterval(function () {
        photoPreviewElem.src = "/Photo/Preview?" + (new Date()).getTime();
    }, 1000);
})(CameraCarApp || (CameraCarApp = {}));
