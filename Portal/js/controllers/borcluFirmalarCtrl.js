var bfc;
angModule.controller("borcluFirmalarCtrl", function ($scope, $timeout) {
    let self = $scope;
    self.yukleniyor = false;
    $scope.maxSize = 5;
  
    angular.element(document).ready(function () {
        bfc = self;
        self.getirData();
        console.log(self.page);
    });
    self.init = function (bolgelerJsn) {
        self.bolgeler = JSON.parse(bolgelerJsn);
    };
    self.getirData = function (model,bolgemi) {
        self.yukleniyor = true;
        if ((model === undefined || model === "") || (model.length >= 3 || bolgemi)) {
            commonAjaxService.getDataFromRemote(url = "/IsPlani/BorcluFirmalarAra", data = {
                konumId: self.seciliBolge, page: self.page, firmaAdi: self.firmaAdi, telNo: self.telNo, cepTelNo: self.cepTelNo,
                yetkili: self.yetkiliAdi
                
            }).done(res=> {
                self.firmalar = res.Data;
                $scope.totalItems = res.ToplamSayi;
                self.$apply();
            });
        }
    };
    self.pageChanged = function () {
        self.getirData();

        console.log(self.page);
    };
    self.gosterIsPlaniModal = function (firma) {
        console.log(firma);
        $("#modalIsPlani").modal("show");
        let isPlaniScope = angular.element($("[ng-controller='isPlaniCtrl']")).scope();
        isPlaniScope.initData(isPlaniScope.etiketIsPlaniTipleri.Borclu);
    };
})