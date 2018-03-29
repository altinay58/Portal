function MessageJsBosVeYaSifirMi(m) {
    if (m != null && m != '' && m != 'undefined' && m != 0) {
        return false;
    } else {
        return true;
    }
}
function MessageJsBosMu(m) {
    if (m != null && m != '' && m != 'undefined') {
        return false;
    } else {
        return true;
    }
}

$(document).ready(function () {
    $.SweetMessage = {
        Show: {
            Confirm: function (title, message, callFunction, closeOnConfirm) {
                if (MessageJsBosMu(title)) {
                    title = 'Onaylıyor musunuz?';
                }
                if (closeOnConfirm != true) {
                    closeOnConfirm = false;
                }

                swal({
                    title: title,
                    html: message,
                    type: "info",
                    showCancelButton: true,
                    confirmButtonClass: 'btn-danger',
                    confirmButtonText: 'Evet',
                    cancelButtonText: 'Hayır',
                    cancelButtonClass: 'btn-default',
                    closeOnConfirm: closeOnConfirm,
                    showLoaderOnConfirm: true
                }, callFunction);

                //$.SweetMessage.Show.Confirm('Onaylıyor musunuz?', 'Mesajınız?',
                //function (isConfirm) {
                //    if (isConfirm) {
                //        //Butonun pasif olmasını sağlıyor.
                //        swal.disableButtons();
                //        .
                //        .
                //        .
                //    }
                //});
            },
            Error: function (title, message, timer,textAlign) {

                if (MessageJsBosMu(title)) {
                    title = 'Yanlış Giden Bir Şeyler Var!';
                }
                if (MessageJsBosVeYaSifirMi(timer)) {
                    timer = null;
                }

                swal({
                    title: title,
                    html: message,
                    type: "error",
                    timer: timer,
                    closeOnConfirm: false
                });

                //HATA İLE GELEN STYLE ETİKETİNİ SİLİYORUZ
                $('.sweet-alert p').find('style').remove();
                $.SweetMessage.MessageBox.TextAlign(textAlign);
            },
            Success: function (title, message, timer,textAlign) {

                if (MessageJsBosMu(title)) {
                    title = 'Başarılı!';
                }
                if (MessageJsBosVeYaSifirMi(timer)) {
                    timer = null;
                }

                swal({
                    title: title,
                    html: message,
                    type: "success",
                    timer: timer,
                    closeOnConfirm: false
                });

                //HATA İLE GELEN STYLE ETİKETİNİ SİLİYORUZ
                $('.sweet-alert p').find('style').remove();
                $.SweetMessage.MessageBox.TextAlign(textAlign);
            }
        },
        Close: function () {
            try {
                window.sweetAlert.closeModal();
            } catch (e) { }
        },
        MessageBox: {
            TextAlign: function (alignClass) {
                if (alignClass != null && alignClass != '' && alignClass != 'undefined') {
                    $('.sweet-alert p').addClass(alignClass);
                }
            }
        }
    },
    $.NotifyMessage = {
        Show: {
            Error: function (title, message, closeTime, notifyPosition) {
                /*
                 * title            : Başlık
                 * message          : Mesaj içeriği
                 * closeTime        : Otomatik kapanma süresi [0 == Otomatik kapatma işlemi uygulanmaz]
                 * notifyPosition   : Notify açılır penceresinin ekranda nerede görüneceği bilgisi
                 *               ↓→ :   bottom-left, bottom-right, top-left, top-right, ''
                 */
                if (MessageJsBosMu(title)) {
                    title = 'Yanlış Giden Bir Şeyler Var!';
                }
                if (MessageJsBosMu(closeTime)) {
                    closeTime = 1000;
                }
                if (MessageJsBosMu(notifyPosition)) {
                    notifyPosition = 'bottom-left';
                }

                $.extend($.gritter.options, {
                    position: notifyPosition
                });
                $.gritter.add({
                    title: title + ' ',
                    text: message + ' ',
                    time: closeTime,
                    close_icon: 'entypo-icon-cancel s12',
                    icon: 'wpzoom-cog',
                    class_name: 'error-notice'
                });
            },
            Success: function (title, message, closeTime, notifyPosition) {
                /*
                 * title            : Başlık
                 * message          : Mesaj içeriği
                 * closeTime        : Otomatik kapanma süresi [0 ya da null == Otomatik kapatma işlemi uygulanmaz]
                 * notifyPosition   : Notify açılır penceresinin ekranda nerede görüneceği bilgisi
                 *               ↓→ :   bottom-left, bottom-right, top-left, top-right, '', varsayılan: [bottom-left]sol-alt
                 */
                if (MessageJsBosMu(title)) {
                    title = 'Başarılı!';
                }
                if (MessageJsBosMu(closeTime)) {
                    closeTime = 1000;
                }
                if (MessageJsBosMu(notifyPosition)) {
                    notifyPosition = 'bottom-left';
                }

                $.extend($.gritter.options, {
                    position: notifyPosition
                });
                $.gritter.add({
                    title: title + ' ',
                    text: message + ' ',
                    time: closeTime,
                    close_icon: 'entypo-icon-cancel s12',
                    icon: 'wpzoom-cog',
                    class_name: 'success-notice'
                });
            },
            Warning: function (title, message, closeTime, notifyPosition) {
                /*
                 * title            : Başlık
                 * message          : Mesaj içeriği
                 * closeTime        : Otomatik kapanma süresi [0 == Otomatik kapatma işlemi uygulanmaz]
                 * notifyPosition   : Notify açılır penceresinin ekranda nerede görüneceği bilgisi
                 *               ↓→ :   bottom-left, bottom-right, top-left, top-right, ''
                 */
                if (MessageJsBosMu(title)) {
                    title = 'Uyarı!';
                }
                if (MessageJsBosMu(closeTime)) {
                    closeTime = 1000;
                }
                if (MessageJsBosMu(notifyPosition)) {
                    notifyPosition = 'bottom-left';
                }

                $.extend($.gritter.options, {
                    position: notifyPosition
                });
                $.gritter.add({
                    title: title + ' ',
                    text: message + ' ',
                    time: closeTime,
                    close_icon: 'entypo-icon-cancel s12',
                    icon: 'wpzoom-cog',
                    class_name: 'warning-notice'
                });
            }
        }
    },
    $.MsgBoxMessage = {
        Show: {
            PromPt: function (title, inputs, buttons, successFunction, notifyPosition, afterShow, beforeShow, beforeClose, afterClose) {
                /*
                 * title            : Başlık
                 * inputs           : Örnek
                                        [
                                            { header: "User Name", type: "text", name: "userName" },
                                            { header: "Password", type: "password", name: "password" },
                                            { header: "Remember me", type: "checkbox", name: "rememberMe", value: "theValue" }
                                        ],
                 * buttons          : Örnek
                                        [
                                            { value: "Login" },
                                            { value: "Cancel" }
                                        ],
                 * successFunction  : Örnek
                                        function (result, values) {
                                            var v = result + " has been clicked\n";
                                            $(values).each(function (index, input) {
                                                v += input.name + " : " + input.value +
                                                (input.checked != null ? (" - checked: " + input.checked) : "") + "\n";
                                            });

                                            //MesajBox kutusunun kapatılmaması isteniyorsa bu kod yazılmalı
                                            getFocus();

                                            alert(v);
                                        }
                 * closeTime        : Otomatik kapanma süresi [0 == Otomatik kapatma işlemi uygulanmaz]
                 * notifyPosition   : Notify açılır penceresinin ekranda nerede görüneceği bilgisi
                 *               ↓→ :   bottom-left, bottom-right, top-left, top-right, ''
                 */

                $.msgBox({
                    type: "prompt",
                    title: title,
                    inputs: inputs,
                    buttons: buttons,
                    success: successFunction,
                    afterShow: afterShow,
                    beforeShow:beforeShow,
                    beforeClose: beforeClose,
                    afterClose: afterClose,

                    autoClose: false
                });

            }
        }
    }
});