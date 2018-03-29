var guncellenecekEvent = null;

$(function () {
    $('#calendar').fullCalendar({
        lang: 'tr', //Türkçe(tr) geçerli olabilmesi için ilgili js dosyası eklenmesi lazım. Js Dosyası: /Content/assets/global/plugins/fullcalendar/lang/tr.js
        header:
        {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultDate: $('#bugun').val(),
        selectable: true,
        selectHelper: true,
        select: function (start, end, a, event) {
            var allDay = !start.hasTime() && !end.hasTime(); //Tüm gün alanına tıklandığında allDay, true olur.
            $('#etkinlikId').val('');
            $('#takvimBasTar').val(moment(start).format('DD.MM.YYYY'));
            $('#takvimBasSaat').val(moment(start).format('HH:mm'));
            $('#takvimBitTar').val(moment(end).format('DD.MM.YYYY'));
            $('#takvimBitSaat').val(moment(end).format('HH:mm'));
            $('#tumGunMu').prop('checked', allDay);

            $("#modal-takvim-bilgi-giris").modal();

            $('.btn-takvim-etkinliksil').hide();
            $("input[is-date='1']").datepicker('update');
        },
        eventClick: function (event, element) {
            guncellenecekEvent = event;
            $('.btn-takvim-etkinliksil').show();
            var userId = $('#araUserId').val();

            $.Takvim.EtkinlikGetir({
                UserId: userId,
                EtkinlikId: event._id
            }, function (c) {
                if (c.Durum) {
                    var event1 = JSON.parse(c.Veri);
                    $('#etkinlikId').val(event1._id);
                    $('#modalUserId').val($('#araUserId').val());
                    $('#takvimBasTar').val(moment(event1.start).format('DD.MM.YYYY'));
                    $('#takvimBasSaat').val(moment(event1.start).format('HH:mm'));
                    $('#takvimBitTar').val(moment(event1.end).format('DD.MM.YYYY'));
                    $('#takvimBitSaat').val(moment(event1.end).format('HH:mm'));
                    $('#tumGunMu').prop('checked', event1.allDay);

                    $('#takvimBaslik').val(event1.title);
                    $('#takvimIcerik').val(event1.description);
                    
                    $("#modal-takvim-bilgi-giris").modal();

                    $("input[is-date='1']").datepicker('update');
                } else {
                    $.SweetMessage.Show.Error('', c.HtmlMesaj);
                }
            });
        },
        editable: false,
        eventLimit: false, // allow "more" link when too many events
        events: []
    });
});

$(function () {
    $(document).on('click', '.btn-takvim-etkinlikkaydet', function () {
        $.Takvim.EtkinlikKaydet();
    });
    $(document).on('click', '.btn-takvim-etkinliksil', function () {
        $.Takvim.EtkinlikSil();
    });
    $(document).on('click', '.btn-takvim-etkinlikgetir', function () {
        $.Takvim.TakvimYukle();
    });
});
$(function () {
    $.Takvim = {
        EtkinlikSil: function () {
            $.SweetMessage.Show.Confirm('', 'Seçilen etkinlik silinecektir!', function (evetMi) {
                if (evetMi) {
                    $.SweetMessage.Close();

                    var etkinlikId = $('#etkinlikId').val();

                    $.ajax({
                        type: "POST",
                        url: '/Takvim/EtkinlikSil',
                        dataType: "json",
                        data: $('#frm1').serialize(),
                        cache: false,
                        beforeSend: function () {
                            $.WaitMe.BlockUi.Show();
                        },
                        success: function (c) {
                            $('#modal-takvim-bilgi-giris').modal('hide');

                            if (c.Durum) {
                                $.Takvim.Temizle();
                                $('#calendar').fullCalendar('removeEvents', etkinlikId);
                                $.NotifyMessage.Show.Success('', c.HtmlMesaj, 3000);
                            } else {
                                $.SweetMessage.Show.Error('', c.HtmlMesaj);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            portalApp.mesajGoster('Beklenmeyen bir hata oluştu.');
                        },
                        complete: function () {
                            $.WaitMe.BlockUi.Hide();
                        }
                    });
                }
            });
        },
        EtkinlikKaydet: function () {
            $.ajax({
                type: "POST",
                url: '/Takvim/Index',
                dataType: "json",
                data: $('#frm1').serialize(),
                cache: false,
                beforeSend: function () {
                    $.WaitMe.BlockUi.Show();
                },
                success: function (c) {
                    if (c.Durum) {
                        $('#modal-takvim-bilgi-giris').modal('hide');

                        $.Takvim.Temizle();
                        $.NotifyMessage.Show.Success('', c.HtmlMesaj, 1500);

                        if (c.Durum2) {
                            $('#calendar').fullCalendar('renderEvent', JSON.parse(c.Veri), true);
                        } else {
                            var event = JSON.parse(c.Veri);
                            guncellenecekEvent._id = event._id;
                            guncellenecekEvent.title = event.title;
                            guncellenecekEvent.start = event.start;
                            guncellenecekEvent.end = event.end;
                            guncellenecekEvent.allDay = event.allDay;

                            $('#calendar').fullCalendar('updateEvent', guncellenecekEvent);
                        }
                    } else {
                        $.SweetMessage.Show.Error('', c.HtmlMesaj, 1500);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    portalApp.mesajGoster('Beklenmeyen bir hata oluştu.');
                },
                complete: function () {
                    $.WaitMe.BlockUi.Hide();
                }
            });
        },
        TakvimYukle: function () {
            $('#calendar').fullCalendar('removeEvents');

            $.ajax({
                type: "POST",
                url: '/Takvim/EtkinlikGetir',
                dataType: "json",
                data: $('#frm2').serialize(),
                cache: false,
                beforeSend: function () {
                    $.WaitMe.BlockUi.Show();
                },
                success: function (c) {
                    if (c.Basarilimi) {
                        var eventData = JSON.parse(c.Data);
                        $.map(eventData, function (data) {
                            $('#calendar').fullCalendar('renderEvent', data, true);
                        });

                        portalApp.mesajGoster('Takvim ekrana yansıtıldı.');
                    } else {
                        portalApp.mesajGoster(c.HtmlMesaj);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    portalApp.mesajGoster('Beklenmeyen bir hata oluştu.');
                },
                complete: function () {
                    $.WaitMe.BlockUi.Hide();
                }
            });
        },
        EtkinlikGetir: function (veri, successFunction) {
            var veriler = { userId: veri.UserId, etkinlikId: veri.EtkinlikId };
            $.ajax({
                type: "POST",
                url: '/Takvim/EtkinlikGetirTek',
                dataType: "json",
                data: veriler,
                cache: false,
                beforeSend: function () {
                    $.WaitMe.BlockUi.Show();
                },
                success: successFunction,
                error: function (xhr, ajaxOptions, thrownError) {
                    portalApp.mesajGoster('Beklenmeyen bir hata oluştu.');
                },
                complete: function () {
                    $.WaitMe.BlockUi.Hide();
                }
            });
        },
        Temizle: function () {
            $('#takvimBaslik').val('');
            $('#takvimIcerik').val('');
            $('#lokasyon').val('');
            $('#tumGunMu').prop('checked', false);
        }
    };
});

var MapsGoogle = function () {
    var mapGeocoding = function () {

        var map = new GMaps({
            div: '#gmap_geocoding',
            lat: -12.043333,
            lng: -77.028333
        });

        GMaps.geolocate({ // KULLANICININ KONUMUNU ALIYORUZ.
            success: function (position) {
                map.setCenter(position.coords.latitude, position.coords.longitude);
            }
        });

        var handleAction = function () {
            var text = $.trim($('#araLokasyon').val());
            GMaps.geocode({
                address: text,
                callback: function (results, status) {
                    if (status == 'OK') {
                        var latlng = results[0].geometry.location;
                        map.setCenter(latlng.lat(), latlng.lng());

                        var myMarker = map.addMarker({
                            lat: latlng.lat(),
                            lng: latlng.lng(),
                            draggable: true
                        });

                        google.maps.event.addListener(myMarker, 'click', function (evt) {
                            document.getElementById('current').innerHTML = '<p>Seçilen Konumu: ' + evt.latLng.lat().toFixed(6) + ', ' + evt.latLng.lng().toFixed(6) + '</p>';
                            document.getElementById('lokasyon').value = evt.latLng.lat().toFixed(6) + ', ' + evt.latLng.lng().toFixed(6);
                        });

                        google.maps.event.addListener(myMarker, 'dragend', function (evt) {
                            document.getElementById('current').innerHTML = '<p>Seçilen Konumu: ' + evt.latLng.lat().toFixed(6) + ', ' + evt.latLng.lng().toFixed(6) + '</p>';
                            document.getElementById('lokasyon').value = evt.latLng.lat().toFixed(6) + ', ' + evt.latLng.lng().toFixed(6);
                        });

                        google.maps.event.addListener(myMarker, 'dragstart', function (evt) {
                            document.getElementById('current').innerHTML = '<p>Lokasyon belirleniyor...</p>';
                        });

                        App.scrollTo($('#gmap_geocoding'));

                        $('#gmap_geocoding').css({
                            width: '100%',
                            height: '400px'
                        });
                    }
                }
            });
        }

        $('#gmap_geocoding_btn').click(function (e) {
            e.preventDefault();
            handleAction();
        });

        $("#araLokasyon").keypress(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == '13') {
                e.preventDefault();
                handleAction();
            }
        });

    }

    return {
        //main function to initiate map samples
        init: function () {
            mapGeocoding();
        }

    };

}();
jQuery(document).ready(function () {
    MapsGoogle.init();
    $('#gmap_geocoding').css({
        width: '100%',
        height: '400px'
    });

    $('#modal-takvim-google-harita').on('shown.bs.modal', function (e) {
        $('#araLokasyon').val('');
        $('#araLokasyon').focus();
    });
    $('#modal-takvim-bilgi-giris').on('shown.bs.modal', function (e) {
        $('#takvimBaslik').focus();
    });
});