var dsfc;
angModule.controller("dashboardSatisFirsatiCtrl", function ($scope, $timeout) {
    let self = $scope;
    angular.element(document).ready(function () {
        dsfc = self;
        self.getirData();
    });
    self.init = function (bolgelerJsn) {
        self.bolgeler = JSON.parse(bolgelerJsn);
    };
    self.getirData = function (model, bolgemi) {
        self.yukleniyor = true;
        if ((model === undefined || model === "") || (model.length >= 3 || bolgemi)) {
            commonAjaxService.getDataFromRemote(url = "/SatisDashboard/SatisFirsatlariAra", data = {
                konumId: self.seciliBolge, adSoyad: self.adSoyad, firmaAdi: self.firmaAdi, telNo: self.telNo, cepTelNo: self.cepTelNo
            })
                .done(res => {
                    self.firsatlar = res;
                    self.$apply();
                    self.yukleniyor = false;
                    if (location.href.indexOf('SatisDashboard') > -1) {
                        $("a[href='#tab_satis_firsatlari'] p.sayi").text(res.length);
                    }
                });
        }
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY HH:mm');
    };
 
})