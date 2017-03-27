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
    self.getirData = function () {
        commonAjaxService.getDataFromRemote(url = "/IsPlani/SatisFirsatiAra", data = { konumId: self.seciliBolge })
        .done(res=> {
            self.firsatlar = res;
            self.$apply();
        });
    };
})