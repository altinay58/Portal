var ddd;
angModule.controller('toDoCtrl', function ($scope, $timeout, toDoService) {

    let self = $scope;
    self.yukleniyor = false;
    self.lists = [];
    const DURUM_BEKLEMEDE=0;
    const DURUM_TAMAMLANMIS = 1;
    self.guncelDurum = DURUM_BEKLEMEDE;
    angular.element(document).ready(function () {
        self.getirData();
        ddd = self;

    });
    self.getirData = function () {
        self.yukleniyor = true;
        toDoService.getListData().then(res=> {
            self.yukleniyor = false;
            self.lists = res;
            extendArray();
        });      

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
        let formattedDate = moment(tarih).format('DD/MM/YY H');
        return formattedDate;
    };
    function hesaplaBeklemedeSayisi() {
        let count = self.lists.filter(e=> { return e.Durum === DURUM_BEKLEMEDE }).length;
        $("#todo-count").text(count);
    }
    function extendArray() {
        self.lists.forEach(e=> {
            e.yeniDurum = e.Durum;
        });
    }
   



})