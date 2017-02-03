angModule.service("randevularService",function($http,$q){
  return ({
    getirRandevulari:function(basTar,bitTar){
      var request = $http({
          method: "get",
          url: "/SatisBolumu/RandevuAra",
          params: {
              basTarih: basTar, bitisTarih: bitTar
          },
          dataType: "json"
      });
      return (request.then(handleSuccess, handleError));
    }
  });
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
}﻿)
