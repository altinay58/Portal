
var gg;
angModule.filter('trim', function () {
    return function (value) {
        if (!angular.isString(value)) {
            return value;
        }
        return value.replace(/^\s+|\s+$/g, ''); // you could use .trim, but it's not going to work in IE<9
    };
});
angModule.controller("mesaiCizelgesiCtrl", function ($scope, mesaiCizelgesiService, $timeout) {
    let self = $scope;
    self.isler = [], self.yukleniyor = true;
    $scope.maxSize = 5;
    self.mesailer = [];
    self.gunSayisi = 0;
    let gunler = { 1: 'Pazertesi', 2: 'Salı', 3: 'Çarşamba', 4: 'Perşembe', 5: 'Cuma', 6: 'Cumartesi', 0: 'Pazar' };
    self.kullanicilar = [];
    self.guncelKullaniciId = '';
    self.aylar = [{ ad: 'Ocak', val: 1 }, { ad: 'Şubat', val: 2 }, { ad: 'Mart', val: 3 },
       { ad: 'Nisan', val: 4 }, { ad: 'Mayıs', val: 5 }, { ad: 'Haziran', val: 6 },
       { ad: 'Temmuz', val: 7 }, { ad: 'Ağustos', val: 8 }, { ad: 'Eylül', val: 9 },
       { ad: 'Ekim', val: 10 }, { ad: 'Kasım', val: 11 }, { ad: 'Aralık', val: 12 }];
    angular.element(document).ready(function () {
        self.getirData();
        gg = self;
      
    });
    self.init = function (kullanicilar, guncelKullaniciId) {
        console.log(guncelKullaniciId);
        self.kullanicilar = JSON.parse(kullanicilar);
        self.guncelKullaniciId = self.kullanicilar.find(x=> { return x.Id === guncelKullaniciId }).Id;
        let cd = new Date();
        let y= cd.getFullYear();
        let oy = y - 1;
        self.yillar = [oy, y];
        self.yil = self.yillar[1];
        //self.ay = cd.getMonth()+1;
       
        self.ay = cd.getMonth() + 1;
    };
    self.getirData = function (model) {
        self.yukleniyor = true;
        mesaiCizelgesiService.getListData(self.guncelKullaniciId, self.ay, self.yil).then(function (res) {
            self.mesailer =JSON.parse(res.Data);
            self.yukleniyor = false;
            $timeout(function () {
                console.log("dom fni")
                editable();
            }, 0,false);
        });
    };
    self.getTarih = function (gun) {
        let dt = new Date(self.yil, self.ay-1, gun);
        return moment(dt).format('DD.MM.YYYY');
    };
   
    self.getColumnValue= function (gun,columnName) {
        let index = getIndex(gun);
        if (index > -1) {
            return self.mesailer[index][columnName];
        } else {
            return "Empty";
        }
    };
    self.pageChanged = function () {
        console.log(self.page);
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY');
    };
    self.setYil = function (val) {
        self.yil = val;
        console.log(self.yil);
    };
    function getIndex(gun) {
        let tarih = self.getTarih(gun);
        let index = self.mesailer.findIndex(x=> { return moment(x.Tarih).format("DD.MM.YYYY") === tarih });
        return index;
    }
    //self.gunSayisi = function () {
    //    //angular.isUndefinedOrNull = function (val) {
    //    //    return angular.isUndefined(val) || val === null
    //    //}
    //    return 
    //};
    self.setGunSayisi = function () {
        //angular.isUndefinedOrNull = function (val) {
        //    return angular.isUndefined(val) || val === null
        //}
        self.gunSayisi= moment(`${self.yil}-${self.ay}`, "YYYY-MM").daysInMonth();
    };
    self.getGunler= function () {
        let gunler = [];
        for (let i = 1; i <= self.gunSayisi; i++) {
            gunler.push(i);
        }
        return gunler;
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
    function editable() {
        $('.mesai').unbind().removeData();
        $('.mesai').editable({
            url: '/Yonetimsel/MesaiCizelgeDegistir',
            //params: { domainId: $(this).data('pk'), note: $(this).data('name') },
            params:{},
            type: 'text',
            name: '',
            title: '',
            success: function (response, newValue) {
                console.log(newValue);
                //userModel.set('username', newValue); //update backbone model
            }
   
        }).on('shown', function (e, editable) {
            editable.options.params.ccolumn = $(e.currentTarget).attr("column");
            // editable.options.params is now {a: 1, b: 2, c: 3}
        });
    }
    self.$watchCollection('[ay,yil]', function (nw, ov) {
      
        self.setGunSayisi();
    });
});