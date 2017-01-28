angModule.service("firmaKisiListService",function($http,$q){
  return({
    getirFirmaKisiList:function(ad,soyad,firmaAdi,departman){
      var req=$http({
             method:'get',
             url: '/Firmalar/FirmaListAra',
             params:{
               ad:ad,soyad:soyad,firmaAdi:firmaAdi,
               departman:departman
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
