var dd;
angModule.controller("satisFirsatlarDetailCtrl", function ($scope, $timeout) {
    let self = $scope;
    
    angular.element(document).ready(function () {
        dd = self;
        //self.getirFirmaTeklifler();
    });
    self.init = function (guncelKullanici, satisAsamaEtiketleri, satisFirsati, firsatDurumEtiketleri, gorusmeEtiketleri) {
        self.guncelKullanici = JSON.parse(guncelKullanici);
        self.satisAsamaEtiketleri = JSON.parse(satisAsamaEtiketleri);
        self.satisFirsati = JSON.parse(satisFirsati);
        self.firsatDurumEtiketleri = JSON.parse(firsatDurumEtiketleri);
        self.gorusmeEtiketleri = JSON.parse(gorusmeEtiketleri);
    };
    self.degistirEtiketSatisDurumu = function (etiketID,satisID) {
        commonAjaxService.getDataFromRemote(url = "/SatisFirsatis/DegistirEtiketSatisAsama", data = { satisId: satisID, yeniDurum: etiketID })
        .done(function (res) {
            console.log(res);
            self.EtiketSatisAsamaId = etiketID;
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
    //self.getirFirmaTeklifler = function () {
    //    commonAjaxService.getDataFromRemote(url = "/SatisFirsatis/FirmaTeklifleri", data = { firmaId: self.satisFirsati.Firma.Id,guncelTeklifId:self.satisFirsati.Id })
    //      .done(function (res) {
    //          self.firmaTeklifler = res;
    //          self.$apply();
             
    //   });
    //};
    self.teklifSatisFirsatiDurumuText = function (EtiketSatisFirsatDurumuId) {
        let durum = self.firsatDurumEtiketleri.find(x=> { return x.Value === EtiketSatisFirsatDurumuId });
        if (durum) {
            return durum.Text;
        }
    };
    self.teklifSatisFirsatiDurumuRenk = function (EtiketSatisFirsatDurumuId) {
        let durum = self.firsatDurumEtiketleri.find(x=> { return x.Value === EtiketSatisFirsatDurumuId });
        if (durum) {
            return durum.Renk;
        }
    };
 
});