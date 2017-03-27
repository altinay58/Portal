var bfc;
angModule.controller("borcluFirmalarCtrl", function ($scope, $timeout) {
    let self = $scope;
    self.yukleniyor = false;
    $scope.maxSize = 5;
    angular.element(document).ready(function () {
        bfc = self;
        self.getirData();
    });
    self.init = function (bolgelerJsn) {
        self.bolgeler = JSON.parse(bolgelerJsn);
    };
    self.getirData = function () {
        commonAjaxService.getDataFromRemote(url = "/IsPlani/BorcluFirmalarAra", data = {
            konumId: self.seciliBolge,page:self.page,firmaAdi:self.firmaAdi
        }).done(res=> {
            self.firmalar = res.Data;
            $scope.totalItems = res.ToplamSayi;
            self.$apply();
        });
    };
    self.pageChanged = function () {
        self.getirData();

        console.log(self.page);
    };
})