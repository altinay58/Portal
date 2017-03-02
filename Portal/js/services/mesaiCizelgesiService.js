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
    });