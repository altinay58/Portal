var qq;
angModule.controller('sifreListCtrl', function ($scope, $timeout, sifreListService) {

    let self = $scope;
    self.sifreler = [];
    self.yukleniyor = false;
    self.getirData = function (model) {
        self.yukleniyor = true;
        if (model.length >= 3) {
            sifreListService.getListData(model)
          .then(function (res) {
              self.sifreler = res;
              self.yukleniyor = false;
              qq = res;
              //self.$apply();
          });
        } else {
            self.yukleniyor = false;
            self.sifreler = [];
        }

    };
    self.demoMu = function(sifre) {
        if (sifre.SifreWebSitesi.indexOf('demo') > -1) {
            return true;
        } else {
            return false;
        }
    };


})