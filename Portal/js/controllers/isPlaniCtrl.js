var ipc;
class IsPlani {
    constructor() {
        this.Id;
        this.Aciklama;
        this.RefSorumluKisiId;
        this.EtiketIsPlaniTipi;
        this.RefIsId;
        this.RefFirmaId;
        this.RefSatisFirsatiId;
        this.EtiketIsPlaniDurum;
        this.Tarih;
    } 
}
angModule.controller("isPlaniCtrl", function ($scope, $timeout) {

    let self = $scope;
    self.guncelIsPlani;
    self.isPlanlari = [];
    self.etiketIsPlaniTipleri = {
        Is: 1, SatisFirsati: 2, Borclu: 3
    };
    angular.element(document).ready(function () {
        ipc = self;
    });
    self.init = function (guncelKullaniciId) {
        self.guncelKullaniciId = guncelKullaniciId;
 
    };
    self.initData = function (etiketIsPlaniTipi) {
        self.aciklama = "";
        self.guncelIsPlani = new IsPlani();
        self.guncelIsPlani.RefSorumluKisiId = self.guncelKullaniciId;
        self.guncelIsPlani.EtiketIsPlaniTipi = etiketIsPlaniTipi;
    };
    self.kaydet = function () {
        let tarih = moment(self.tarih, "tr").toDate();
        let saat_dakika = self.saatDakika.split(':');
        tarih.setHours(parseInt(saat_dakika[0]));
        tarih.setMinutes(parseInt(saat_dakika[1]));
        self.guncelIsPlani.Tarih = tarih
        self.guncelIsPlani.Aciklama = self.aciklama;
        self.isPlanlari.push(self.guncelIsPlani);
    
        $("#modalIsPlani").modal("hide");
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY HH:mm');
    };

})