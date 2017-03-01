

angModule.controller("mesaiCizelgesiCtrl", function ($scope, mesaiCizelgesiService) {
    let self = $scope;
    self.isler = [], self.yukleniyor = true;
    $scope.maxSize = 5;
    angular.element(document).ready(function () {
        self.getirData();
    });
    self.init = function () {
        //aryanListService.getListData(self.basTarih, self.bitisTarih);
        self.basTarih = qs("basTarih");
        self.bitisTarih = qs("bitisTarih");
        self.page = qs("page") === null ? 1 : parseInt(qs("page"));
        self.isAdi = qs("isAdi");
    };
    self.getirData = function (model) {
        self.yukleniyor = true;
        if ((model === undefined || model === "") || model.length >= 3) {
            mesaiCizelgesiService.getListData(qs("basTarih"), qs("bitisTarih"), qs("isAdi"), qs("page"))
          .then(function (res) {
              self.isler = res.Data;
              self.yukleniyor = false;
              $scope.totalItems = res.ToplamSayi;
              //self.$apply();
          });
        } else {
            self.yukleniyor = false;
        }

    };
    self.pageChanged = function () {
        console.log(self.page);
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY HH:mm');
    };
    function setIfEmpty(obj) {
        if (obj === undefined) {
            return "";
        } else {
            return obj;
        }
    }
    function qs(name) {
        return portalApp.getParameterByName(name);
    }
    self.$watchCollection('[page,basTarih,bitisTarih,isAdi]', function (nw, ov) {
        console.log(nw[0] + "-" + ov[0]);
        let q = `#?page=${nw[0]}`;
        if (nw[3] !== null && nw[3] !== "") {
            q = q + `&isAdi:${setIfEmpty(nw[3])}`;
        }
        if (nw[1] !== "" && nw[2] !== "") {
            q = q + `&basTarih:${setIfEmpty(nw[1])}&bitisTarih:${setIfEmpty(nw[2])}`;
        }
        document.location.href = q;
        let yeniSayfa = nw[0];
        let eskiSayfa = ov[0];
        if (yeniSayfa !== eskiSayfa) {
            self.getirData();
        }
    });
});