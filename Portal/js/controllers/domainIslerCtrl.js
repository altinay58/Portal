var ff;
angModule.controller("domainIslerCtrl", function ($scope, domainIslerService) {
    var self = $scope;
    self.guncelDomainId = 0;//287 karayeltasarim.com
    self.domainIsler = [];
    self.isDurum = {
        1: { ad: "Yapilacak", timelineIconClass: "font-blue", arrowClass: "arrow-yapilacak", bodyClass: "bg-blue" },
        2: { ad: "YapilacakDeadline", timelineIconClass: "font-blue", arrowClass: "arrow-yapilacak", bodyClass: "bg-blue" },
        3: { ad: "Yapiliyor", timelineIconClass: "font-green-jungle", arrowClass: "arrow-yapiliyor", bodyClass: "bg-green-jungle" },
        4: { ad: "KontrolBekleyen", timelineIconClass: "font-yellow", arrowClass: "arrow-yapilacak", bodyClass: "bg-yellow" },
        5: { ad: "Biten", timelineIconClass: "", arrowClass: "", bodyClass:"" }
    }
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
    self.tarihFormatStr = function (tarih) {
        let date = new Date(parseInt(tarih.replace("/Date(", "").replace(")/", ""), 10));       
        let formattedDate = moment(date).format('DD.MM.YYYY');
        return formattedDate;
    }
    self.timelineIconClassBelirleDurumaGore = function (durumId) {
        let r = self.isDurum[durumId].timelineIconClass;
        console.log(self.isDurum[durumId].timelineIconClass);
        return self.isDurum[durumId].timelineIconClass;
    }
    self.timelineArrowClassBelirleDurumaGore = function (durumId) {      
        return self.isDurum[durumId].arrowClass;
    }
    self.timelineBodyClassBelirleDurumaGore = function (durumId) {     
        return self.isDurum[durumId].bodyClass;
    }
});