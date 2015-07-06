/// <reference path="../typings/jquery/jquery.d.ts"/>

module CameraCarApp {
    $('#btn-take-photo').click(e => {
        e.preventDefault();
        $.post('/Photo/Take').fail(() => { alert('Fail taking photo.'); });
    });

    //var $debugOut = $('#debug-out');

    var wireUpMotorActon = (selector: string, action: string) => {
        $(selector).on('mousedown touchstart', e => {
            e.preventDefault();
            $.post('/Car/' + action).fail(() => { alert('Fail ' + action); });
            //$debugOut.text(action);
        }).on('mouseup touchend', e => {
            e.preventDefault();
            $.post('/Car/StopAll').fail(() => { alert('Fail StopAll'); });
            //$debugOut.text('StopAll');
        }).on('click contextmenu', e => {
            e.preventDefault();
        });
    };

    wireUpMotorActon('#btn-move-forward', 'StartMoveForward');
    wireUpMotorActon('#btn-move-backward', 'StartMoveBackward');
    wireUpMotorActon('#btn-turn-left', 'StartTurnLeft');
    wireUpMotorActon('#btn-turn-right', 'StartTurnRight');
    wireUpMotorActon('#btn-lift-up', 'StartLiftUp');
    wireUpMotorActon('#btn-lift-down', 'StartLiftDown');

    var photoPreviewElem = <HTMLImageElement>document.getElementById('photo-preview');
    setInterval(() => {
        photoPreviewElem.style.backgroundImage = `url(/Photo/Preview?${(new Date()).getTime() })`;
    }, 1000);
}