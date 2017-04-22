var bfc; // Satış bölümünde bulunan iş planı için yazıldı
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
        //TODO : var konumlarJsn =JsonConvert.SerializeObject(Database.DbHelper.GetDb.Konums.Select(x=>new { KonumID=x.KonumID,Konum=x.Konum1}).ToList()); ile verileri veritabanından alıp javasctip nesnesine atıyoruz. bU KOD View kısmına yazılır.
        //TODO : <div ng-controller="borcluFirmalarCtrl" ng-init="init(bolgelerJsn='@konumlarJsn')" class="tab-pane fade table-scrollable" id="tab_borclu_firmalar"> BURADA konumlarJsn gelen verileri bolgelerJsn değişkenine atayıp bunuda borcluFirmalarCtrl controllerına gönderiyoruz.
        //TODO : Bölgeler diye bir tane değişken oluşturduk bunun içerisini  bolgelerJsn ile dolduruldu. Bu değer cshtml sayfasının içerisindeki ng-init="init(bolgelerJsn='@konumlarJsn')" kodu ile doldurulur. 
        self.bolgeler = JSON.parse(bolgelerJsn); 
        // TODO :  <option ng-repeat="k in bolgeler" value="{{k.KonumID}}">{{k.Konum}}</option> yukarıdaki self.bolgeler kodunu select in option kısmında bu şekilde kullanıyoruz. ng-repaet bizim bir döngüye girmemizi sağlıyor. bolgeler değişkeninden gelen verilerin içerisinde foreach gibi çalışır. k ile değişken tanımlayıp bu değişkene gelen konum ve konumID yi yazdırdık. Buradaki konum ve konumID ilk yazılan kodun içerisinden geldi. Yorumun ilk satırındaki javascript nesnesi.
    };

    //TODO: select değiştiğinde yada inputlardan biri değiştirğinde verileri nasıl geldiğini açıklıyoruz.
    //TODO: <select ng-model="seciliBolge" class="form-control" ng-change="getirData(seciliBolge,true)"> İLE SELECT DEĞİŞTİĞİNDE verilerin filtrelenerek tabloya gelmesini sağlıyoruz. 
    //TODO: ng-change olayındaki getirData fonksiyonu ng-change i yazdığımız yeri içerisine kapsayan ng-controller="borcluFirmalarCtrl" controller da çalışır.
    //TODO: ng-model="seciliBolge" bu selectin içeriisinde seçili olan option ın value değerini alır ve gönderir post eder aşağıdaki fonksiyonu çalıştırır.

    self.getirData = function (model,bolgemi) {
        self.yukleniyor = true;
        if ((model === undefined || model === "") || (model.length >= 3 || bolgemi)) {

            //commonAjaxService fonksiyonunu ajaxServics.js dosyasının içerisine biz yazıyoruz. Bu fonksiyonel ile her hangi bir url den veri gönderip almamızı sağlıyor. Yoksa çok kod yazmamız gerekiyor. Bunu yazdığımız içinde self.$apply(); kodunu kullanmamız gerekiyor.

            commonAjaxService.getDataFromRemote(url = "/IsPlani/BorcluFirmalarAra", data = {
                konumId: self.seciliBolge,
                page: self.page,
                firmaAdi: self.firmaAdi,
                telNo: self.telNo,
                cepTelNo: self.cepTelNo,
                yetkili: self.yetkiliAd,
                odeme: self.odeme
            }).done(res=> {
                self.firmalar = res.Data; //TODO: gelen datayı aldık firmalar değişkenine atadık. Bu değişken borcluFirmalarCtrl in içerisinde kullanıldığında bu datalar ekranda gözükür.
                $scope.totalItems = res.ToplamSayi; // TODO: toplam veri sayısı
                self.$apply(); // TODO: bu kodda done fonskyionun içerisinde doldurduğumuz firmalar değişkeninin güncellendiğini angularjs ile bildirmemize yazıyor. bu fonksiyon angularjs nin içerisinden gelir.
               
            });
        }
    };
    self.pageChanged = function () {
        self.getirData();

        console.log(self.page);
    };
    self.gosterIsPlaniModal = function (f) {
        console.log(f);
        $("#modalIsPlani").modal("show");
        let isPlaniScope = angular.element($("[ng-controller='isPlaniCtrl']")).scope();
        isPlaniScope.initData(etiketIsPlaniTipi=isPlaniScope.etiketIsPlaniTipleri.Borclu,refId=f.firma.FirmaID);
    };
})