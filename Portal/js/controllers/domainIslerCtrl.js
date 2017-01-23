var ff;
angModule.controller("domainIslerCtrl", function ($scope, domainIslerService) {
    var self = $scope;
    self.guncelDomainId = 0;//287 karayeltasarim.com
    self.domainIsler = [];
    angular.element(document).ready(function () {
        console.log(self.guncelDomainId);
        self.getirDomainIsler();
    });
    self.init = function (domainId) {
        console.log(domainId);
        self.guncelDomainId = domainId;
    }
    self.getirDomainIsler = function () {
        domainIslerService.getirDomaineAitIsleri(self.guncelDomainId)
        .then(function (res) {
            console.log(res);
            ff = res;
            self.domainIsler = res;
        })
    }
    self.gosterCariBilgi = function () {
        $("#modalCari").modal("show");
    }
});