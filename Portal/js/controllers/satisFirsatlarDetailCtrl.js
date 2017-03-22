var dd;
angModule.controller("satisFirsatlarDetailCtrl", function ($scope, $timeout) {
    let self = $scope;
    
    angular.element(document).ready(function () {
        dd = self;
        self.getirFirmaTeklifler();
    });
    self.init = function (guncelKullanici, satisAsamaEtiketleri, satisFirsati, firsatDurumEtiketleri) {
        self.guncelKullanici = JSON.parse(guncelKullanici);
        self.satisAsamaEtiketleri = JSON.parse(satisAsamaEtiketleri);
        self.satisFirsati = JSON.parse(satisFirsati);
        self.firsatDurumEtiketleri = JSON.parse(firsatDurumEtiketleri);
    };
    self.etiketClassBelirle = function (etiket) {
        if (etiket.Value === self.satisFirsati.EtiketSatisAsamaId) {
            return "bg-green-steel";
        } else {
            return "bg-grey-steel";
        }
    };
    self.degistirEtiketSatisDurumu = function (etiket) {
        commonAjaxService.getDataFromRemote(url = "/SatisFirsatis/DegistirEtiketSatisAsama", data = { satisId: self.satisFirsati.Id, yeniDurum: etiket.Value })
        .done(function (res) {
            self.satisFirsati.EtiketSatisAsamaId = etiket.Value;
            self.$apply();
            portalApp.mesajGoster("Durumu değişti");
        });
      
    };
    self.degistirFirsatDurum = function (etiket) {
        commonAjaxService.getDataFromRemote(url = "/SatisFirsatis/DegistirEtiketFirsatDuru", data = { satisId: self.satisFirsati.Id, yeniDurum: etiket.Value })
         .done(function (res) {
             self.satisFirsati.EtiketSatisFirsatDurumuId = etiket.Value;
             self.$apply();
             portalApp.mesajGoster("Durumu değişti");
             if (etiket.Text === "Satışa Dönüştü") {
                 location.href = "/CariHareket/CariHareketSatisEkle";
             }
         });       
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY');
    };
    self.getirFirmaTeklifler = function () {
        commonAjaxService.getDataFromRemote(url = "/SatisFirsatis/FirmaTeklifleri", data = { firmaId: self.satisFirsati.Firma.Id,guncelTeklifId:self.satisFirsati.Id })
          .done(function (res) {
              self.firmaTeklifler = res;
              self.$apply();
             
       });
    };
    self.teklifSatisFirsatiDurumuText = function (EtiketSatisFirsatDurumuId) {
        let durum = self.firsatDurumEtiketleri.find(x=> { return x.Value == EtiketSatisFirsatDurumuId });
        if (durum) {
            return durum.Text;
        }
    };
    self.teklifSatisFirsatiDurumuRenk = function (EtiketSatisFirsatDurumuId) {
        let durum = self.firsatDurumEtiketleri.find(x=> { return x.Value == EtiketSatisFirsatDurumuId });
        if (durum) {
            return durum.Renk;
        }
    };
});