/// <reference path="../typings/jquery/jquery.d.ts"/>

module CameraCarApp {
    var photoPreviewElem = <HTMLImageElement>document.getElementById('photo-preview');

    document.getElementById('btn-take-photo')
        .addEventListener('click', e => {
            e.preventDefault();
            $.post('/Photo/Take').fail(() => { alert('Fail taking photo.'); });
        });

    setInterval(() => {
        photoPreviewElem.src = "/Photo/Preview?" + (new Date()).getTime();
    }, 1000);
}