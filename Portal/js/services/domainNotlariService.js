angModule.service("domainNotlariService",function($http,$q){
  return({
    getListData:function(domainAdi,basTar,bitTar){
      var req=$http({
        method:'get',
        url:'/Domain/DomainNoteAra',
        params:{
          domainAdi:domainAdi,basTarih:basTar,
          bitisTarih:bitTar
        },
        dataType:'json'
      });
      return (req.then(handleSuccess,handleError));
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
