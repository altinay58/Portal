angModule.service("domainIslerService", function ($http, $q) {

    return ({
        getirDomaineAitIsleri: function (domainId) {
            var request = $http({
                method: "get",
                url: "/Isler/DomainAitIsler",
                params: {
                    domainId: domainId
                },
                dataType: "json"
            });
            return (request.then(handleSuccess, handleError));
        },
        IsDurumuDegistir: function (domainIs, yeniDurum) {
            var request = $http({
                method: "get",
                url: "/Isler/IsDurumuDegistir",
                params: {
                    domainIs: JSON.stringify(domainIs),
                    yeniDurum: yeniDurum
                },
                dataType: "json"
            });
            return (request.then(handleSuccess, handleError));
        },
        getirDomainNotlari:function(domainId){
          var request = $http({
              method: "get",
              url: "/Isler/GetirDomainNotlari",
              params: {
                  domainId:domainId
              },
              dataType: "json"
          });
          return (request.then(handleSuccess, handleError));
        },
        getirFirmaKisileri:function(firmaId){
          var request = $http({
              method: "get",
              url: "/Isler/FirmaKisiler",
              params: {
                  firmaId:firmaId
              },
              dataType: "json"
          });
          return (request.then(handleSuccess, handleError));
        },
        kaydetYorum:function(kullaniciId,aciklama,isId){
          var request=$http({
            method:"get",
            url:"/Isler/YorumKaydet",
            params:{
              kullaniciId:String(kullaniciId),aciklama:aciklama,isId:isId
            },
            dataType:"json"
          });
          return (request.then(handleSuccess, handleError));
        },
        degistirSatisOncelik: function (domainBilgi) {
            var request = $http({
                method: "get",
                url: "/Isler/DegistirSatisOncelik",
                params: {
                    domainId: domainBilgi.Id, value: domainBilgi.SatisOncelikli
                },
                dataType: "json"
            });
            return (request.then(handleSuccess, handleError));
        },
        degistirGuncellemeSozlesmesi: function (domainBilgi) {
            var request = $http({
                method: "get",
                url: "/Isler/DegistirGuncellemeSozlesmesi",
                params: {
                    domainId: domainBilgi.Id, value: domainBilgi.SatisOncelikli
                },
                dataType: "json"
            });
            return (request.then(handleSuccess, handleError));
        },
        degistirOdemesiAlindi: function (domainBilgi) {
            var request = $http({
                method: "get",
                url: "/Isler/DegistirOdemesiAlindi",
                params: {
                    domainId: domainBilgi.Id, value: domainBilgi.SatisOncelikli
                },
                dataType: "json"
            });
            return (request.then(handleSuccess, handleError));
        },
        domainAksiyonDegistir: function (domainId,domainAksiyon) {
            var request = $http({
                method: "get",
                url: "/Isler/DomainAksiyonDegistir",
                params: {
                    domainId: domainId, domainAksiyon: domainAksiyon
                },
                dataType: "json"
            });
            return (request.then(handleSuccess, handleError));
        }
    });
    /// private methods
    function handleError(response) {
        if (
            !angular.isObject(response.data) ||
            !response.data.message
            ) {
            return ($q.reject("An unknown error occurred."));
        }
        return ($q.reject(response.data.message));
    }

    function handleSuccess(response) {
        return (response.data);
    }
})
