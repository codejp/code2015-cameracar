/// <reference path="../typings/jquery/jquery.d.ts"/>
var CameraCarApp;
(function (CameraCarApp) {
    $('#btn-take-photo').click(function (e) {
        e.preventDefault();
        $.post('/Photo/Take').fail(function () { alert('Fail taking photo.'); });
    });
    //var $debugOut = $('#debug-out');
    var wireUpMotorActon = function (selector, action) {
        $(selector).on('mousedown touchstart', function (e) {
            e.preventDefault();
            $.post('/Car/' + action).fail(function () { alert('Fail ' + action); });
            //$debugOut.text(action);
        }).on('mouseup touchend', function (e) {
            e.preventDefault();
            $.post('/Car/StopAll').fail(function () { alert('Fail StopAll'); });
            //$debugOut.text('StopAll');
        }).on('click contextmenu', function (e) {
            e.preventDefault();
        });
    };
    wireUpMotorActon('#btn-move-forward', 'StartMoveForward');
    wireUpMotorActon('#btn-move-backward', 'StartMoveBackward');
    wireUpMotorActon('#btn-turn-left', 'StartTurnLeft');
    wireUpMotorActon('#btn-turn-right', 'StartTurnRight');
    wireUpMotorActon('#btn-lift-up', 'StartLiftUp');
    wireUpMotorActon('#btn-lift-down', 'StartLiftDown');
    var photoPreviewElem = document.getElementById('photo-preview');
    setInterval(function () {
        photoPreviewElem.style.backgroundImage = "url(/Photo/Preview?" + (new Date()).getTime() + ")";
    }, 1000);
})(CameraCarApp || (CameraCarApp = {}));
//# sourceMappingURL=app.js.map