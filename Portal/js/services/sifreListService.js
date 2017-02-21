angModule.service("sifreListService", function ($http, $q) {
    return ({
        getListData: function (q) {
            var req = $http({
                method: 'get',
                url: '/Sifres/SifreAra',
                params: {
                    q: q                  
                },
                dataType: 'json'
            });
            return (req.then(handleSuccess, handleError));
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