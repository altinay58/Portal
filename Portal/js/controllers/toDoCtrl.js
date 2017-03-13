var ddd;
angModule.controller('toDoCtrl', function ($scope, $timeout, toDoService) {

    let self = $scope;
    self.yukleniyor = false;
    self.lists = [];
    $scope.maxSize = 5;
    const DURUM_BEKLEMEDE=0;
    const DURUM_TAMAMLANMIS = 1;
    self.guncelDurum = DURUM_BEKLEMEDE;
    angular.element(document).ready(function () {
        self.getirData();
        ddd = self;

    });
    self.getirData = function () {
        self.yukleniyor = true;
        toDoService.getListData(self.page).then(res=> {
            self.yukleniyor = false;
            $scope.totalItems = res.ToplamSayi;
            self.lists = res.Data;
            extendArray();
        });      

    };
    self.pageChanged = function () {      
        self.getirData();
    };
    self.ekle = function () {
        if (self.aciklama!==null  && typeof self.aciklama!=='undefined' && self.aciklama!=="") {
            toDoService.ekle(self.aciklama).then(res=> {
                if (res.Basarilimi) {
                    res.Data.yeniDurum = res.Data.Durum;
                    self.lists.unshift(res.Data);
                    self.aciklama = "";
                    $("#todo-count").text(self.lists.length);
                    hesaplaBeklemedeSayisi();
                }

            });
        }
   
    };
    self.durumDegistir = function (id) {
        let todo = self.lists.find(e=> { return e.Id === id });

        toDoService.durumDegistir(todo.Id, todo.yeniDurum).then(res=> {
            if (res.Basarilimi) {
                todo.Durum = todo.yeniDurum;
                hesaplaBeklemedeSayisi();
            }

        });

    };
  
    self.degistirGuncelDurum = function (yeniDurum) {
        self.guncelDurum = yeniDurum;
    };
    self.filterByDurumu = function (todo) {
        if (todo.Durum === self.guncelDurum) {
            return true;
        } else {
            return false;
        }
    };
    self.tarihFormatStr = function (tarih) {
        let formattedDate = moment(tarih).format('DD/MM/YY');
        return formattedDate;
    };
    function hesaplaBeklemedeSayisi() {
        commonAjaxService.getDataFromRemote(url = "/Home/YapilacaklarSayisi", data = {}).done(res=> {
            console.log(res);
            $("#todo-count").text(res);
        });     
    }
    function extendArray() {
        self.lists.forEach(e=> {
            e.yeniDurum = e.Durum;
        });
    }
   



})