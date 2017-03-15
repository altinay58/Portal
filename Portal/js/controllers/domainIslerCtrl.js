var ff,gg,db;
String.prototype.toHHMMSS = function () {
    var sec_num = parseInt(this, 10);
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    return hours + ':' + minutes + ':' + seconds;
}
String.prototype.toplamZamanFormat = function () {
    var sec_num = parseInt(this, 10);
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    return hours + 'sa ' + minutes + 'dk ' + seconds+'s';
}
angModule.controller("domainIslerCtrl", function ($scope, $timeout, $window, domainIslerService) {
    var self = $scope;
    const HEPSI=0;
    self.guncelDomainId = 0;//287 karayeltasarim.com
    self.guncelFirmaId=0;
    self.domainIsler = [], self.guncelKullanici={}, self.toplamZaman, self.toplamZamanStr;
    self.filterdResults = [], self.domainNotlari=[];
    self.filterIsDurum = 0;
    self.filterUserId= "Hepsi";
    self.firmaKisiler = [];
    self.sonDomainNote = "", self.modelDomainNote = "";
    self.guncelDomainAksiyon = null;
    self.tempDomainAksiyon = null;//domain aksiyon değiştiginde,değer bu degiskende tutuluyor
    self.domainBilgi = {
        Id : 0,
        SatisOncelikli : false,
        GuncellemeSozlesmesiVarmi:false,
        OdemesiAlindi: false,
        DomainAksiyon:2
    };
    var signalDomain = null;
    let IsinDurumuEnum={
        Yapilacak : 1, YapilacakDeadline:2, Yapiliyor:3,
        OnayBekleyen:4, Biten:5
    }
    self.isDurum = {
        1: { ad: "Yapilacak", timelineIconClass: "font-blue", arrowClass: "arrow-yapilacak", bodyClass: "bg-blue" },
        2: { ad: "YapilacakDeadline", timelineIconClass: "font-blue", arrowClass: "arrow-yapilacak", bodyClass: "bg-blue" },
        3: { ad: "Yapiliyor", timelineIconClass: "font-green-jungle", arrowClass: "arrow-yapiliyor", bodyClass: "bg-green-jungle" },
        4: { ad: "OnayBekleyen", timelineIconClass: "font-yellow", arrowClass: "arrow-onay", bodyClass: "bg-yellow" },
        5: { ad: "Biten", timelineIconClass: "", arrowClass: "", bodyClass: "body-biten" }
    }
    self.domainAksiyon = {
        BeklemeyeAl:1,YayinaAl:2,YayiniDurdur:3
    }
   
    angular.element(document).ready(function () {
        console.log(self.guncelDomainId);
        self.getirDomainIsler();
        getirDomainSonNote();
        gg = self;
        signalDomain = $.connection.domainIsHub;
        $.connection.hub.logging = true;
        $.connection.hub.disconnected(function () {
            setTimeout(function () {
                $.connection.hub.start();
            }, 5000); // Restart connection after 5 seconds.
        });
        signalDomain.client.yorumEklendi = function (isId, jsnYorum, fromId, toId) {
            if (self.guncelKullanici.Id !== fromId) {
                let index = self.domainIsler.findIndex(f=> { return f.IsId === isId });
                if (index > -1) {
                    let yeniYorum = JSON.parse(jsnYorum);
                    self.domainIsler[index].Yorumlar.push(yeniYorum);
                    if (toId.indexOf(self.guncelKullanici.Id) > -1) {
                        portalApp.mesajGoster("#" + self.domainIsler[index].IsId + " ticket yeni yorum eklendi");
                        self.domainIsler[index].yorumEklendi = true;
                    }

                    self.$apply();
                    $timeout(function () {
                        self.domainIsler[index].yorumEklendi = false;
                    }, 2100);
                    //self.$apply(() => {


                    //});
                }
               
            }

        };
        signalDomain.client.domaisDurumDegisti = function (jsnDomainIs, fromId) {

            if (self.guncelKullanici.Id !== fromId) {
                let di = JSON.parse(jsnDomainIs);
                let index = self.domainIsler.findIndex(f=> { return f.IsId === di.IsId });

                self.domainIsler[index].IsDurum = di.IsDurum;
                self.domainIsler[index].iBtnClass = di.iBtnClass;
                self.domainIsler[index].IsGecenZaman = di.IsGecenZaman;
                portalApp.mesajGoster("#"+di.IsId+ " ticket durumu değişti");
                zamanAyarla(self.domainIsler[index]);
                self.$apply();
            }

        }
        signalDomain.client.domainDurumDegisti = function () {
            portalApp.mesajGoster("Domain durumu değişti", "success");
            $timeout(function () {
                $window.location.reload();
            }, 2000);

        }
    });
    self.init = function (jsnDomain,guncelKullanici,toplamZaman,firmaId) {
        console.log(jsnDomain);
        db=self.domainBilgi = JSON.parse(jsnDomain);
        self.guncelDomainId = self.domainBilgi.Id;
        self.guncelKullanici = JSON.parse(guncelKullanici);
        self.toplamZaman = toplamZaman;
        self.guncelFirmaId=firmaId;
        self.guncelDomainAksiyon = self.domainBilgi.DomainAksiyon;

    } 
    self.getirDomainIsler = function () {    
       
        domainIslerService.getirDomaineAitIsleri(self.guncelDomainId)
        .then(function (res) {
            ff = res;
            extendArray(res);
            self.domainIsler = res;
            self.toplamZamanStr = String(self.toplamZaman).toplamZamanFormat();
            arrayZamanHesapla();
            //$timeout(function () {
            //    console.log("post Digest with $timeout");
            //}, 0, false);
        })
    }
    function getirDomainSonNote() {
        domainIslerService.getirDomainSonNot(self.guncelDomainId)
        .then(function (res) {
            self.sonDomainNote = res;
        })
    }
    self.filterByIsinDurumu = function (domainIs) {
        if (self.filterIsDurum === HEPSI || self.filterIsDurum === domainIs.IsDurum) {
            if (self.filterUserId === 'Hepsi') {
                return true;
            }
            else
            {
                if (filterKullanici(domainIs)) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        else {
            return false;
        }
    }
    function filterKullanici(domainIs) {
        let ary = domainIs.IsiYapacakKullanicilar.filter((e)=> { return self.filterUserId === e.Id })
        if (ary.length > 0) {
            return true;
        } else { 
            return false;
        }
    }
    self.degistirFilterIsDurum = function (isDurum) {
        self.filterIsDurum = isDurum;
    }
    self.degistirFilterUser = function (userId) {
        self.filterUserId = userId;
    }
    self.gosterCariBilgi = function () {
      domainIslerService.getirFirmaKisileri(self.guncelFirmaId)
      .then((res)=>{
        self.firmaKisiler=res;
        $("#modalCari").modal("show");
      })

    }
    self.gosterDomainNotlari = function (domainAksiyon) {
        self.tempDomainAksiyon = domainAksiyon;
        domainIslerService.getirDomainNotlari(self.guncelDomainId)
        .then((res)=>{
          self.domainNotlari=res;
          $("#modalDomainNotlari").modal("show");
        })
    }
    self.domainNoteKaydet = function () {
        domainIslerService.domainNotKaydet(self.guncelDomainId, self.modelDomainNote)
        .then(res=> {
            self.gosterDomainNotlari(self.tempDomainAksiyon);
            self.sonDomainNote = self.modelDomainNote;
            if (self.tempDomainAksiyon != null) {
                self.domainAksiyonDegistir(self.tempDomainAksiyon);
            }
        });
    }
    self.tarihFormatStr = function (tarih) {
        if (tarih) {
            let date = new Date(parseInt(tarih.replace("/Date(", "").replace(")/", ""), 10));
            let formattedDate = moment(date).format('DD.MM.YYYY');
            return formattedDate;
        }else
        {
            return "";
        }
    }
    self.tarihFormatUznStr = function (tarih) {
        if (tarih) {
            if(angular.isDate(tarih)){
              let formattedDate = moment(tarih).format('DD.MM.YYYY HH:mm');
              return formattedDate;
            } else
            {
                if (tarih.indexOf('/Date(') > -1) {
                    let date = new Date(parseInt(tarih.replace("/Date(", "").replace(")/", ""), 10));
                    let formattedDate = moment(date).format('DD.MM.YYYY HH:mm');
                    return formattedDate;
                } else {
                    return moment(tarih).format("DD.YYYY HH:mm")
                }

            }

        }else
        {
            return "";
        }
    }
    self.timelineIconClassBelirleDurumaGore = function (durumId) {
        return self.isDurum[durumId].timelineIconClass;
    }
    self.timelineArrowClassBelirleDurumaGore = function (durumId) {
        return self.isDurum[durumId].arrowClass;
    }
    self.timelineBodyClassBelirleDurumaGore = function (durumId) {
        return self.isDurum[durumId].bodyClass;
    }
    self.isDurumDegistir = function (domainIs) {
        if (self.domainBilgi.DomainAksiyon === self.domainAksiyon.YayinaAl) {
            if (guncelKullaniciIsiYapacakmi(domainIs))
            {
                let yeniDurum = 0, iBtnClass = "fa fa-play";
                if (domainIs.IsDurum === IsinDurumuEnum.Yapilacak || domainIs.IsDurum === IsinDurumuEnum.YapilacakDeadline) {
                    yeniDurum = IsinDurumuEnum.Yapiliyor;
                    if (guncelKullaniciIsiYapliyorSayisi() > 0) {
                        portalApp.mesajGoster("Başka bir iş yaplıyor durumda.", "danger");
                        return;
                    }
                    iBtnClass = "fa fa-pause";
                    domainIs.GosterTamamlaBtn = true;
                }
                else {
                    domainIs.GosterTamamlaBtn = false;
                    if (domainIs.BitisTarihiVarmi) {
                        yeniDurum = IsinDurumuEnum.YapilacakDeadline;
                    } else {
                        yeniDurum = IsinDurumuEnum.Yapilacak;
                    }
                }
                durumDegistir(domainIs, yeniDurum, iBtnClass);
            }
            else {
                portalApp.mesajGoster("Yetkiniz yok. ", "danger");
            }
           
        }
        else {
            portalApp.mesajGoster("Domain yayın durduruldu  veya beklemeye alındı ","danger");
        }
    }
    self.isiYapanKullanicilar = function (kullanicilar) {
        let str = kullanicilar.map(e=> { return e.AdSoyad }).join();
        return str;        
    }
    function guncelKullaniciIsiYapliyorSayisi() {
        let sayi = 0;
        self.domainIsler.forEach(e=> {
            if (guncelKullaniciIsiYapacakmi(e)) {
                if (e.IsDurum === IsinDurumuEnum.Yapiliyor) {
                    sayi = sayi + 1;
                }
            }
        });
        return sayi;
    }
    function guncelKullaniciIsiYapacakmi(domainIs){
        let index= domainIs.IsiYapacakKullanicilar.findIndex(e=>{return e.Id===self.guncelKullanici.Id});
        return index > -1;
    }
    self.clickTamamlaBtn = function (domainIs) {
        if (guncelKullaniciIsiYapacakmi(domainIs)) {
            domainIs.GosterTamamlaBtn = false;
            domainIs.GosterIseBaslaBtn = false;
            domainIs.GosterOnaylaBtn = true;
            let yeniDurum = IsinDurumuEnum.OnayBekleyen;
            durumDegistir(domainIs, yeniDurum, "");
        } else {
            portalApp.mesajGoster("Yetkiniz yok. ", "danger");
        }
       
    }
    self.clickOnayla = function (domainIs) {
        if (self.guncelKullanici.Id === domainIs.IsiVerenKullanici.Id) {
            domainIs.GosterTamamlaBtn = false;
            domainIs.GosterIseBaslaBtn = false;
            domainIs.GosterOnaylaBtn = false;
            let yeniDurum = IsinDurumuEnum.Biten;
            durumDegistir(domainIs, yeniDurum, "");
        } else {
            portalApp.mesajGoster("Yetkiniz yok. ", "danger");
        }
        
    }
    self.revizeIste = function (domainIs) {
        if (self.guncelKullanici.Id === domainIs.IsiVerenKullanici.Id) {
            let iBtnClass = "fa fa-play";
            domainIs.GosterTamamlaBtn = false;
            domainIs.GosterIseBaslaBtn = true;
            domainIs.GosterOnaylaBtn = false;
            let yeniDurum = IsinDurumuEnum.Yapilacak;
            if (domainIs.BitisTarihiVarmi) {
                yeniDurum = IsinDurumuEnum.YapilacakDeadline;
            } else {
                yeniDurum = IsinDurumuEnum.Yapilacak;
            }
            durumDegistir(domainIs, yeniDurum, iBtnClass);
        } else {
            portalApp.mesajGoster("Yetkiniz yok. ", "danger");
        }

    }
    self.yorumEkle=function(domainIs,index){
        domainIslerService.kaydetYorum(self.guncelKullanici.Id, domainIs.yorumAciklamaModel, domainIs.IsId)
            .then((res) => {
                let yeniYorum = {
                    Aciklama: domainIs.yorumAciklamaModel, AdSoyad: self.guncelKullanici.AdSoyad,
                    KullaniciId: self.guncelKullanici.KullaniciId, Tarih: new Date()
                };
                domainIs.Yorumlar.push(yeniYorum);
                let toIds =String( domainIs.IsiVerenKullanici.Id);

                domainIs.IsiYapacakKullanicilar.forEach(x=> {
                    toIds = toIds+","+String(x.Id);
                });
                signalDomain.server.gonderYorumEklendi(domainIs.IsId,JSON.stringify(yeniYorum), self.guncelKullanici.Id,
                    toIds);
            });

    }
    self.degistirSatisOncelik = function () {
        self.domainBilgi.SatisOncelikli = !self.domainBilgi.SatisOncelikli;
        domainIslerService.degistirSatisOncelik(self.domainBilgi).
        then(res=> {
            portalApp.mesajGoster("Satış öncelik değişti", "success");
        });
    }
    self.degistirGuncellemeSozlesmesi = function () {
        self.domainBilgi.GuncellemeSozlesmesiVarmi = !self.domainBilgi.GuncellemeSozlesmesiVarmi;
        domainIslerService.degistirGuncellemeSozlesmesi(self.domainBilgi).
        then(res=> {
            portalApp.mesajGoster("Guncelleme Sozlesmesi  değişti", "success");
        });
    }
    self.degistirOdemesiAlindi = function () {
        self.domainBilgi.OdemesiAlindi = !self.domainBilgi.OdemesiAlindi;
        domainIslerService.degistirOdemesiAlindi(self.domainBilgi).
        then(res=> {
            portalApp.mesajGoster("Ödemesi Alındı  değişti", "success");
        });
    }
    self.domainAksiyonDegistir = function (aksiyon) {
        console.log(aksiyon);
        self.tempDomainAksiyon = aksiyon;
        self.gosterDomainNotlari();
        domainIslerService.domainAksiyonDegistir(self.guncelDomainId, aksiyon)
        .then((res) => {
            signalDomain.server.gonderSayfayiYenidenYukle();

        });
    }

    function durumDegistir(domainIs, yeniDurum, iBtnClass) {
        let strObj = JSON.stringify(domainIs);
        let objNew = JSON.parse(strObj);
        delete objNew.Yorumlar;
        domainIslerService.IsDurumuDegistir(objNew, yeniDurum)
       .then(function (res) {
           if (res.Basarilimi) {
               objNew.IsDurum = domainIs.IsDurum = yeniDurum;
               objNew.iBtnClass = domainIs.iBtnClass = iBtnClass;
               objNew.IsGecenZaman = domainIs.IsGecenZaman = res.Data.IsGecenZaman;
               self.timelineBodyClassBelirleDurumaGore(domainIs.IsDurum);
               self.timelineArrowClassBelirleDurumaGore(domainIs.IsDurum)
               zamanAyarla(domainIs);
               signalDomain.server.gonderDurumDegisti(JSON.stringify(objNew),self.guncelKullanici.Id);
               //angular.copy(res.Data, domainIs);
               //self.$apply();
           }
       })
    }

    $.connection.hub.start().done(function () {


    });
    function getDate(jsnDate) {
        //new Date(parseInt(jsnDate.replace("/Date(", "").replace(")/", ""), 10));
        if (jsnDate.indexOf('+02') > -1) {
            jsnDate = jsnDate.replace('+02', '+03');
        }
        return moment(jsnDate).toDate();       
    }
    function arrayZamanHesapla() {
        self.domainIsler.forEach(function (e) {
            zamanAyarla(e);
        })
    }
    function zamanAyarla (domainIs) {
        if (domainIs.IsDurum === IsinDurumuEnum.Yapiliyor) {
            if (domainIs.IsGecenZaman.ZamanBasTarih) {
                var now = moment(new Date());

                var end = moment(getDate(domainIs.IsGecenZaman.ZamanBasTarih));
                var duration = moment.duration(now.diff(end));
                domainIs.sec = now.diff(end,"seconds") + domainIs.IsGecenZaman.GecenZamanSaniye;
                domainIs.timer = setInterval(function () {
                    domainIs.sec++;
                    domainIs.gecenZaman = String(domainIs.sec).toHHMMSS();
                    self.$apply();
                }, 1000);
            }

        } else {
            clearInterval(domainIs.timer);
            if (domainIs.IsGecenZaman.GecenZamanSaniye) {
                domainIs.gecenZaman = String(domainIs.IsGecenZaman.GecenZamanSaniye).toHHMMSS();
            } else {
                domainIs.gecenZaman = "00:00:00";
            }

        }
    }
    function extendArray (ary) {
        ary.forEach(function (e) {
            e.yorumEklendi = false;
            e.yorumAciklamaModel="";
            e.iBtnClass = "";
            e.GosterTamamlaBtn = false;
            e.GosterOnaylaBtn = false;
            e.GosterIseBaslaBtn = false;
            if (e.IsDurum === IsinDurumuEnum.Yapilacak || e.IsDurum === IsinDurumuEnum.YapilacakDeadline
                || e.IsDurum===IsinDurumuEnum.Yapiliyor)
            {
                e.GosterIseBaslaBtn = true;

                e.iBtnClass = "fa fa-play";
                if (e.IsDurum === IsinDurumuEnum.Yapiliyor) {
                    e.iBtnClass = "fa fa-pause";
                    e.GosterTamamlaBtn = true;
                }
            }
            else if(e.IsDurum===IsinDurumuEnum.OnayBekleyen)
            {
                e.GosterOnaylaBtn = true;
            }

        })
    }
});
