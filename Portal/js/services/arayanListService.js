//var aryanListService = {
//    getListData: function (basTar,bitTar,firma,telNo,note,adSoyad) {
//        return $.ajax({
//            url: "/Arayanlar/ArayanListesiParametre",
//            method: "get",
//            data: {
//                basTarih: basTar, bitisTarih: bitTar, firma: firma, telNo: telNo,
//                note: note, adSoyad:adSoyad
//            },
//            dataType: "json",
//            cache: false
//        });
//        //return $.get("/Domain/DomainSorgula?domain="+domainName);
//    }

//}
angModule.service("arayanListService",
                   function ($http, $q) {
                       return ({
                           getListData: function (basTar, bitTar, firma, telNo, note, adSoyad) {                            
                               var request = $http({
                                   method: "get",
                                   url: "/Arayanlar/ArayanListesiParametre",
                                   params: {
                                       basTarih: basTar, bitisTarih: bitTar, firma: firma, telNo: telNo,
                                       note: note, adSoyad: adSoyad
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