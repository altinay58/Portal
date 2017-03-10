angModule.service("toDoService", function ($http, $q) {
    return ({
        getListData: function (aciklama) {
            var req = $http({
                method: 'get',
                url: '/Home/TodoList',
                params: {                   
                },
                dataType: 'json'
            });
            return (req.then(handleSuccess, handleError));
        },
        ekle: function (aciklama) {
            var req = $http({
                method: 'get',
                url: '/Home/EkleTodo',
                params: {
                    aciklama: aciklama
                },
                dataType: 'json'
            });
            return (req.then(handleSuccess, handleError));
        },
        durumDegistir: function (id,yeniDurum) {
            var req = $http({
                method: 'get',
                url: '/Home/TodoDurumDegistir',
                params: {
                    id: id, yeniDurum: yeniDurum
                },
                dataType: 'json'
            });
            return (req.then(handleSuccess, handleError));
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
})