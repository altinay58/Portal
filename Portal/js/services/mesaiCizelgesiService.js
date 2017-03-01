angModule.service("mesaiCizelgesiService",
    function ($http, $q) {
        return ({
            getListData: function (basTar, bitTar, isAdi, page) {
                var request = $http({
                    method: "get",
                    url: "/Isler/ListIsAra",
                    params: {
                        basTarih: basTar, bitisTarih: bitTar, isAdi: isAdi, page: page
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