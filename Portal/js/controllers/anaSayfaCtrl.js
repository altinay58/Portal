
var ss;
angModule.controller("anaSayfaCtrl", function ($scope, anaSayfaService) {
    let self = $scope;
    self.linkDeGoster = true;
    self.isler = [], self.yukleniyor = false;
    $scope.maxSize = 5;
    self.seciliKontrolEden, self.seciliYapacakKisi;
    self.isinDurumlari = ["Yapilacak", "YapilacakDeadline", "Yapiliyor", "KontrolBekleyen", "Biten"];
    self.isRenkler = {
        "Yapilacak": { color: "#3598dc" }, "YapilacakDeadline": { color: "#3598dc" },
        "Yapiliyor": { color: "#26c281" }, "KontrolBekleyen": { color: "#c49f47" },
        "Biten": { color: "darkgrey" }
    };
    let models = ["page", "basTarih", "bitisTarih", "isAdi", "domain", "firma", "seciliKontrolEden", "seciliYapacakKisi", "seciliIsDurum", "isId", "seciliBolge"];
    let loaded = false;
    angular.element(document).ready(function () {
        self.getirData();
        ss = self;
    });
    self.init = function (kullanicilar, guncelKullaniciId, bolgelerJsn, isDurum) {
        //aryanListService.getListData(self.basTarih, self.bitisTarih);
        self.kullanicilar = JSON.parse(kullanicilar);
        if (bolgelerJsn) {
            self.bolgeler = JSON.parse(bolgelerJsn);
        }    
        if (guncelKullaniciId) {
            self.guncelKullanici = self.kullanicilar.find(x=> { return x.Id === guncelKullaniciId }).AdSoyad;
        }
        if (isDurum) {
            self.seciliIsDurum = isDurum;
        }
    
        self.basTarih = qs("basTarih");
        self.bitisTarih = qs("bitisTarih");
        self.page = qs("page"); //=== null ? 1 : parseInt(qs("page"));
        self.isAdi = qs("isAdi");
        self.firma = qs("firma");
        self.domain = qs("domain");
        self.seciliKontrolEden = qs("seciliKontrolEden");
        self.seciliYapacakKisi = qs("seciliYapacakKisi") === null ? self.guncelKullanici : qs("seciliYapacakKisi");
        self.seciliIsDurum = qs("seciliIsDurum") == null ? self.seciliIsDurum : qs("seciliIsDurum");
        self.isId = qs("isId");
        self.seciliBolge = qs("seciliBolge");
    };
    self.getirData = function (model) {
        self.yukleniyor = true;
        if ((model === undefined || model === "") || model.length >= 3) {
            anaSayfaService.getListData(qs("basTarih"), qs("bitisTarih"), qs("isAdi"), qs("page"), qs("firma"), qs("domain"),
                qs("seciliKontrolEden"), qs("seciliYapacakKisi"), qs("seciliIsDurum"), qs("isId"), qs("seciliBolge"))
             .then(function (res) {
              self.isler = res.Data;
              self.yukleniyor = false;
              $scope.totalItems = res.ToplamSayi;
              if (location.href.indexOf('SatisDashboard') > -1 && self.seciliIsDurum === "Yapiliyor") {
                  $("a[href='#tab_yapilan_isler'] p.sayi").text(res.ToplamSayi);
              }
             
          });
            //self.$apply();
        } else {
            self.yukleniyor = false;
        }

    };
    self.pageChanged = function () {
        if (!loaded) {
            self.page = qs("page") === null ? 1 : parseInt(qs("page"));
            loaded = true;
        }
     
        console.log(self.page);
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY HH:mm');
    };
    //İş planı sayfasında çalışır sadece
    self.gosterIsPlaniModal = function (job) {
        
        $("#modalIsPlani").modal("show");
        let isPlaniScope = angular.element($("[ng-controller='isPlaniCtrl']")).scope();
        isPlaniScope.initData(etiketIsPlaniTipi = isPlaniScope.etiketIsPlaniTipleri.Is, refId = job.Id, aciklama = job.IsAdi, domainId = job.DomainId);
    };
    self.guncelKullaniciBuIsiYapiyormu = function (isiYapacakKisiler) {
        return isiYapacakKisiler.indexOf(self.guncelKullanici) > -1;
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
    self.$watchCollection('[page,basTarih,bitisTarih,isAdi,domain,firma,seciliKontrolEden,seciliYapacakKisi,seciliIsDurum,isId,seciliBolge]', function (nw, ov) {
   
        console.log(nw[0] + "-" + ov[0]);
        let q = "";//`#?page=${nw[0]}`;
        let first = true;
        for (let i = 0; i < models.length; i++) {
            if (nw[i] && nw[i] !== null && nw[i] !== "") {
                if (first) {
                    q = `#?${models[i]}=${setIfEmpty(nw[i])}`;
                    first = false;
                } else {
                    q = q + `&${models[i]}=${setIfEmpty(nw[i])}`;
                }

            }
        }


        if (q) {
            document.location.href = q;
        } else {
            document.location.href = "#";
        }

        let yeniSayfa = nw[0];
        let eskiSayfa = ov[0];
        const BOLGE = 10;
        if (yeniSayfa !== eskiSayfa || nw[6] !== ov[6] || nw[7] !== ov[7] || nw[8] !== ov[8] || nw[BOLGE] !== ov[BOLGE]) {
            self.getirData();
        }    
      
      
    });
});