angModule.service("mesaiCizelgesiService",
    function ($http, $q) {
        return ({
            getListData: function (kullaniciId, ay, yil) {
                var request = $http({
                    method: "get",
                    url: "/Yonetimsel/GetMesaiCizelgesi",
                    params: {
                        kullaniciId:kullaniciId, ay: ay, yil: yil
                    },
                    dataType: "json"
                });
                return (request.then(handleSuccess, handleError));
            },
            durumDegistir: function (id, value, column, jsnObj) {
                var request = $http({
                    method: "get",
                    url: "/Yonetimsel/MesaiCizelgeDegistir",
                    params: {
                        pk: id, value: value, ccolumn: column,jsnObj:jsnObj
                    },
                    dataType: "json"
                });
                return (request.then(handleSuccess, handleError));
            },
            hesaplaMesai: function (kullaniciId, yil, ay) {
                var request = $http({
                    method: "get",
                    url: "/Yonetimsel/HesaplaMesai",
                    params: {
                        kullanici: kullaniciId, yil: yil, ay: ay
                    },
                    dataType: "json"
                });
                return (request.then(handleSuccess, handleError));
            },
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
    });