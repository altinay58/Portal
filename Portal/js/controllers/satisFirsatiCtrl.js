var sfc;
angModule.controller("satisFirsatiCtrl", function ($scope, $timeout) {

    let self = $scope;
    angular.element(document).ready(function () {
        sfc = self;
        self.getirData();
    });
    self.init = function (bolgelerJsn) {
        self.bolgeler = JSON.parse(bolgelerJsn);
    };
    self.getirData = function (model,bolgemi) {
        self.yukleniyor = true;
        if ((model === undefined || model === "") || (model.length >= 3 || bolgemi)) {
            commonAjaxService.getDataFromRemote(url = "/IsPlani/SatisFirsatiAra", data = {
                konumId: self.seciliBolge, adSoyad: self.adSoyad,firmaAdi:self.firmaAdi,telNo:self.telNo,cepTelNo:self.cepTelNo
            })
            .done(res=> {
                self.firsatlar = res;
                self.$apply();
                self.yukleniyor = false;
            });
        }
    };
    self.gosterIsPlaniModal = function (satisFirsati) {
        console.log(satisFirsati);
        $("#modalIsPlani").modal("show");
        let isPlaniScope = angular.element($("[ng-controller='isPlaniCtrl']")).scope();
        isPlaniScope.initData(isPlaniScope.etiketIsPlaniTipleri.SatisFirsati);
        isPlaniScope.initData(etiketIsPlaniTipi = isPlaniScope.etiketIsPlaniTipleri.SatisFirsati, refId = satisFirsati.Id);
    };
})