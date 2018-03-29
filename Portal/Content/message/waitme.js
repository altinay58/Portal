function WaitMeJsBosVeYaSifirMi(m) {
    if (m != null && m != '' && m != 'undefined' && m != 0) {
        return false;
    } else {
        return true;
    }
}
function WaitMeJsBosMu(m) {
    if (m != null && m != '' && m != 'undefined') {
        return false;
    } else {
        return true;
    }
}
$(document).ready(function () {
    $.WaitMe = {
        Show: function (idOrClass, text) {
            if (text == null || text == '' || text == 'undefined') {
                $(idOrClass).append('<div class="bekleKapla"></div><div class="bekleYazi">Lütfen bekleyiniz.</div>');
            } else {
                $(idOrClass).append('<div class="bekleKapla"></div><div class="bekleYazi">' + text + '</div>');
            }
        },
        Hide: function (idOrClass) {
            $(idOrClass + ' .bekleKapla').remove(0, null);
            $(idOrClass + ' .bekleYazi').remove(0, null);
        },

        BlockUi: {
            Show: function (closeStartTime, mesaj) {
                if (WaitMeJsBosVeYaSifirMi(mesaj)) {
                    mesaj = 'Lütfen bekleyiniz...';
                }

                $.blockUI({
                    message: mesaj,
                    css: {
                        border: 'none',
                        padding: '15px',
                        open: { height: 'toggle' },
                        close: { height: 'toggle' },
                        backgroundColor: '#000',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#fff'
                    }
                });

                if (!WaitMeJsBosVeYaSifirMi(closeStartTime)) {
                    if (closeStartTime > 0) {
                        $.WaitMe.BlockUi.Hide(closeStartTime);
                    }
                }
            },
            Hide: function (closeStartTime) {
                if (WaitMeJsBosMu(closeStartTime)) {
                    closeStartTime = 0;
                }

                setTimeout($.unblockUI, closeStartTime);
            }
        }
    }
});