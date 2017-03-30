var ipc;
Date.prototype.toJSON = function () { return moment(this).format(); }
class IsPlani {
    constructor(obj) {
        this.Id;
        this.Aciklama;
        this.RefSorumluKisiId;
        this.EtiketIsPlaniTipi;
        this.RefIsId;
        this.RefFirmaId;
        this.RefSatisFirsatiId;
        this.EtiketIsPlaniDurum;
        this.Tarih;
        this.TempEtiketIsPlaniDurum;
        for (var prop in obj)
        {
            this[prop] = obj[prop];
        }
    } 
}
angModule.controller("isPlaniCtrl", function ($scope, $timeout) {

    let self = $scope;
    self.guncelIsPlani;
    self.isPlanlari = [];
    self.etiketIsPlaniTipleri = {
        Is: 1, SatisFirsati: 2, Borclu: 3,Firma:4
    };
    self.etiketIsPlaniDurumlari = {
        Atlandi: 1, Eklendi: 2, Tamamlandi: 3
    };
    angular.element(document).ready(function () {
        ipc = self;
      
    });
    self.getirBuguneAitIsplanlari = function () {
        commonAjaxService.getDataFromRemote(url = "/IsPlani/BuguneAitIsPlanlari", data = {})
            .done(res=> {
            res.forEach(e=> {
                let isPlani = new IsPlani(e);
                isPlani.etiketIsPlaniTipDetay = self.etiketIsPlaniTipDetaylari.find(x=> { return x.Value == isPlani.EtiketIsPlaniTipi });
                isPlani.TempEtiketIsPlaniDurum = isPlani.EtiketIsPlaniDurum;
                self.isPlanlari.push(isPlani);
                self.$apply();
            });         
         
        });
    };
    self.init = function (guncelKullaniciId, jsnEtiketIsPlaniTipDetaylari) {
        self.guncelKullanici = guncelKullaniciId;
        self.etiketIsPlaniTipDetaylari = JSON.parse(jsnEtiketIsPlaniTipDetaylari);
        self.getirBuguneAitIsplanlari();
    };

    self.initData = function (etiketIsPlaniTipi, refId, aciklama, domainId) {
        self.aciklama = "";
        self.guncelIsPlani = new IsPlani();
        self.guncelIsPlani.RefSorumluKisiId = self.guncelKullanici;
        self.guncelIsPlani.EtiketIsPlaniTipi = etiketIsPlaniTipi;
        if (aciklama) {
            self.aciklama = `<a href="/Isler/DomainIsler/${domainId}">#${refId} ${aciklama}</a>`;
        }
        switch (etiketIsPlaniTipi) {
            case self.etiketIsPlaniTipleri.Is:
                self.guncelIsPlani.RefIsId = refId;
              
                break;
            case self.etiketIsPlaniTipleri.SatisFirsati:
                self.guncelIsPlani.RefSatisFirsatiId = refId;
                break;
            case self.etiketIsPlaniTipleri.Borclu:
                self.guncelIsPlani.RefFirmaId = refId;
                break;
            case self.etiketIsPlaniTipleri.Firma:
                self.guncelIsPlani.RefFirmaId = refId;
                break;
        }
    };
    self.kaydet = function () {
        let parts = self.tarih.split('.');
        let tarih = new Date(parts[2],parseInt(parts[1])-1, parts[0]);
        let saat_dakika = self.saatDakika.split(':');
        tarih.setHours(parseInt(saat_dakika[0]));
        tarih.setMinutes(parseInt(saat_dakika[1]));
        self.guncelIsPlani.Tarih = tarih;
        if (self.aciklama) {
            self.guncelIsPlani.Aciklama = self.aciklama;
        }
        
        self.guncelIsPlani.EtiketIsPlaniDurum = durumBelirle(self.guncelIsPlani.Tarih);
        commonAjaxService.getDataFromRemote(url = "/IsPlani/IsPlaniKaydet", data = {
            jsnIsPlani: JSON.stringify(self.guncelIsPlani)
        }).done(res=> {
            if (res.Basarilimi) {
                self.guncelIsPlani.Id = res.Data;
                self.guncelIsPlani.etiketIsPlaniTipDetay = self.etiketIsPlaniTipDetaylari.find(x=> { return x.Value == self.guncelIsPlani.EtiketIsPlaniTipi });
                self.isPlanlari.push(self.guncelIsPlani);
                self.$apply();
                $("#modalIsPlani").modal("hide");
                portalApp.mesajGoster("Kaydedildi");
                $("#todo-count").text(self.isPlanlari.length);
            } else {
                portalApp.mesajGoster("Seçili tarihde bu iş daha önceden kayıt edilmiş",'danger');
                $("#modalIsPlani").modal("hide");
            }
        });

    
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY HH:mm');
    };
    self.durumDegistir = function (isPlani,index) {
        let cevap = confirm("Tamamlandı olacak, Emin misiniz?");
        if (cevap) {
            let yeniDurum = yeniDurumBul(isPlani.TempEtiketIsPlaniDurum);
            commonAjaxService.getDataFromRemote(url = "/IsPlani/IsPlaniDurumDegistirme", data = {
                planId: isPlani.Id, yeniDurum: yeniDurum
            }).done(res=> {
                isPlani.EtiketIsPlaniDurum = self.etiketIsPlaniDurumlari.Tamamlandi;
                //self.isPlanlari.splice(index, 1);
                self.$apply();
                let count = self.isPlanlari.filter(x=> { return x.EtiketIsPlaniDurum != self.etiketIsPlaniDurumlari.Tamamlandi }).length;
                $("#todo-count").text(count);
                portalApp.mesajGoster("Kaydedildi");
            });
        } else {
            isPlani.EtiketIsPlaniDurum = isPlani.EtiketIsPlaniDurum.TempEtiketIsPlaniDurum;
        }
  
    };
    function yeniDurumBul(eskiDurum) {
        if (eskiDurum === self.etiketIsPlaniDurumlari.Tamamlandi) {
            return plan.TempEtiketIsPlaniDurum;
        } else {
            return self.etiketIsPlaniDurumlari.Tamamlandi;
        }
    }
    function durumBelirle(tarih) {
        let bugun = new Date();
        bugun.setHours(0,0,0,0);//saat dakika saniye salise alanlari sifirlandi
        let tempTarih = new Date(tarih.getTime());
        tempTarih.setHours(0,0,0,0);
        if (bugun.getTime() === tempTarih.getTime()) {
            return self.etiketIsPlaniDurumlari.Eklendi;
        } else {
            return self.etiketIsPlaniDurumlari.Atlandi;
        }
    }

})