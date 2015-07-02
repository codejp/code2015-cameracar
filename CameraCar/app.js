/// <reference path="../typings/jquery/jquery.d.ts"/>
var CameraCarApp;
(function (CameraCarApp) {
    $('#btn-take-photo').click(function (e) {
        e.preventDefault();
        $.post('/Photo/Take').fail(function () { alert('Fail taking photo.'); });
    });
    var $debugOut = $('#debug-out');
    var wireUpMortorActon = function (selector, action) {
        $(selector).on('mousedown touchstart', function (e) {
            e.preventDefault();
            // $.post('/Car/' + action).fail(() => { alert('Fail ' + action); });
            $debugOut.text(action);
        }).on('mouseup touchend', function (e) {
            e.preventDefault();
            // $.post('/Car/StopAll').fail(() => { alert('Fail StopAll'); });
            $debugOut.text('StopAll');
        }).on('click contextmenu', function (e) {
            e.preventDefault();
        });
    };
    wireUpMortorActon('#btn-move-forward', 'StartMoveForward');
    wireUpMortorActon('#btn-move-backward', 'StartMoveBackward');
    wireUpMortorActon('#btn-turn-left', 'StartTurnLeft');
    wireUpMortorActon('#btn-turn-right', 'StartTurnRight');
    wireUpMortorActon('#btn-lift-up', 'StartLiftUp');
    wireUpMortorActon('#btn-lift-down', 'StartLiftDown');
    var photoPreviewElem = document.getElementById('photo-preview');
})(CameraCarApp || (CameraCarApp = {}));
