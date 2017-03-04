
var gg;
//function gunO() {
//    this.gun = null; this.tarih = null; this.gunIsim = null; this.durum = null;
//}
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
    self.gunIsimleri = { 1: 'Pazertesi', 2: 'Salı', 3: 'Çarşamba', 4: 'Perşembe', 5: 'Cuma', 6: 'Cumartesi', 0: 'Pazar' };
    self.kullanicilar = [];
    self.guncelKullaniciId = '';
    self.gunler = [];
    self.aylar = [{ ad: 'Ocak', val: 1 }, { ad: 'Şubat', val: 2 }, { ad: 'Mart', val: 3 },
       { ad: 'Nisan', val: 4 }, { ad: 'Mayıs', val: 5 }, { ad: 'Haziran', val: 6 },
       { ad: 'Temmuz', val: 7 }, { ad: 'Ağustos', val: 8 }, { ad: 'Eylül', val: 9 },
       { ad: 'Ekim', val: 10 }, { ad: 'Kasım', val: 11 }, { ad: 'Aralık', val: 12 }];
    self.mesaiDurumlar = [{ id: 1, ad: 'Mesai' }, { id: 2, ad: 'Hafta Sonu' }, { id: 3, ad: 'Resmi Tatil' },
    { id: 4, ad: 'Rapor' }, { id: 5, ad: 'Üçretli İzin' }, { id: 6, ad: 'Üçretsiz İzin' },
    { id: 7, ad: 'Yıllık İzin' }];
  
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
       
        self.yillar = [y-1, y];
        self.yil = self.yillar[1];
        //self.ay = cd.getMonth()+1;
       
        self.ay = cd.getMonth() + 1;
    };
    self.getirData = function (model) {
        self.setGunSayisi();
        self.yukleniyor = true;
        mesaiCizelgesiService.getListData(self.guncelKullaniciId, self.ay, self.yil).then(function (res) {
            self.mesailer = JSON.parse(res.Data);
            extendMesailer();
            self.yukleniyor = false;
            $timeout(function () {
              
            editable();
            }, 0,false);
        });
        hesaplaMesai();
    };
    self.getTarih = function (gun) {
        let dt = new Date(self.yil, self.ay-1, gun);
        return dt;
    };
    self.tarihFormat = function (tarih) {
        return moment(tarih).format('DD.MM.YYYY');
    };

    function getIndex(gun) {
        let tarih = self.getTarih(gun);
        let index = self.mesailer.findIndex(x=> { return moment(x.Tarih).toDate().getTime() === tarih.getTime()});
        return index;
    }
    self.setGunSayisi = function () {       
        self.gunSayisi= moment(`${self.yil}-${self.ay}`, "YYYY-MM").daysInMonth();
    };
    self.getGunSayisi= function () {    
        let now = new Date();
        let dt = new Date(self.yil, self.ay - 1, self.gunSayisi);
     
        if (dt.getMonth() > now.getMonth() && dt.getFullYear()>=now.getFullYear()) {
            return 0;
        }
        else if(now.getMonth()===dt.getMonth() && now.getFullYear()===dt.getFullYear()){         
            return now.getDate();
        }
        else {         
            return self.gunSayisi;
        }
      
    }; 

    self.getGun = function (gun) {
        let dt = new Date(self.yil, self.ay - 1, gun);
        return self.gunIsimleri[dt.getDay()];
    };
    self.durumdegistir = function (tarih) {
        //self.durums[gun
        //console.log(self.mesailer[gun].Durum);
        let index = self.mesailer.findIndex(m=> { return m.TarihStr === tarih });
        let mesai = self.mesailer[index];
        mesaiCizelgesiService.durumDegistir(mesai.Id, mesai.Durum, "Durum", JSON.stringify(mesai))
            .then(res=> {
                portalApp.mesajGoster("Kaydedildi");
                mesai.Id = res.Data.Id;
                hesaplaMesai();
            });
        //console.log(self.mesailer[gun]);
      
    };
    self.getMesaiSuresi = function (mesaiSuresi) {
        if (mesaiSuresi) {
            return String(mesaiSuresi).replace(".", ",");
        }
    };
    self.getYillikToplam = function () {
        if (self.yillikToplam) {
            return String(self.yillikToplam).replace(".", ",");
        }
    };
    self.getAylikToplam = function () {
        if (self.aylikToplam) {
            return String(self.aylikToplam).replace(".", ",");
        }
    };
    function editable() {
        $('.mesai').unbind().removeData();
        $('.mesai').editable({
            url: '/Yonetimsel/MesaiCizelgeDegistir',
            //params: { domainId: $(this).data('pk'), note: $(this).data('name') },
            params:{},
            type: 'text',
            name: '',
            title: '',
            success: function (res, newValue) {
                console.log(newValue);
                console.log(res.Data);
                let mesai = self.mesailer.find(m=> { return m.TarihStr.trim() === moment(res.Data.Tarih).format("DD.MM.YYYY") });
                mesai.Id = res.Data.Id;
                hesaplaMesai();
                portalApp.mesajGoster("Kaydedildi");
                //userModel.set('username', newValue); //update backbone model
            }
   
        }).on('shown', function (e, editable) {
            editable.options.params.ccolumn = $(e.currentTarget).attr("column");
            let ptr = $(e.currentTarget).closest("tr");//.find("td:eq(0)").text()
            let ts = $(ptr).find("td:eq(0)").text().trim();
            let mesai = self.mesailer.find(e=> { return e.TarihStr === ts });           
         
            editable.options.params.jsnObj = JSON.stringify(mesai);
            // editable.options.params is now {a: 1, b: 2, c: 3}
        });
    }
    function hesaplaMesai() {
        mesaiCizelgesiService.hesaplaMesai(self.guncelKullaniciId, self.yil, self.ay).then(function (res) {
            self.yillikToplam = res.yillikToplam;
            self.aylikToplam = res.aylikToplam;
        });
    }
    self.$watchCollection('[ay,yil,guncelKullaniciId]', function (nw, ov) {
      
        //self.setGunSayisi();      

    });
    function extendMesailer() {
        
        for (let i = 0; i < self.getGunSayisi(); i++) {
           
            let index = getIndex(i+1);
            if (index < 0) {
                let obj = { CikisSaat: '', KullaniciId: self.guncelKullaniciId, MesaiSuresi: '', Tarih: '' };
                obj.Tarih = self.getTarih(i+1);
                obj.TarihStr = self.tarihFormat(obj.Tarih);
                obj.Gun = self.getGun(i+1);             
                obj.index = i;
                obj.Durum = null;
              
                if (obj.Gun === "Pazar") {
                    obj.Durum = 2;
                }
                self.mesailer.push(obj);
            } else {
                let mesai = self.mesailer[index];
                mesai.TarihStr = self.tarihFormat(mesai.Tarih);
                mesai.Gun = self.getGun(i+1);
                mesai.index = i;
              
            }
        }
        
    }
});