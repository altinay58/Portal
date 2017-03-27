angModule.service("anaSayfaService",
    function ($http, $q) {
        return ({
            getListData: function (basTar, bitTar, isAdi, page, firma, domain, isiKontrolEden, isiYapacakKisi, isinDurumu,isId,bolgeId) {
                var request = $http({
                    method: "get",
                    url: "/Home/ListIsAra",
                    params: {
                        page: page,basTarih: basTar, bitisTarih: bitTar, isAdi: isAdi,
                        firma: firma, domain: domain, isiKontrolEden: isiKontrolEden,
                        isiYapacakKisi: isiYapacakKisi, isinDurumu: isinDurumu,isId:isId,bolgeId:bolgeId
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