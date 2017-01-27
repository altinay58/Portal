/// <reference path="C:\Projects\Portal\Portal\Scripts/angular.min.js" />
/// <reference path="../angModule.js" />
let ff;
angModule.controller('domainNotlariCtrl', function ($scope,domainNotlariService) {

    let self = $scope;
    self.domainNotlari=[];
    angular.element(document).ready(function(){
      self.getirData();
    })
    self.getirData=function(){
      domainNotlariService.getListData(self.domainAdi,self.basTarih,self.bitisTarih)
      .then((data)=>{
        ff=data;
        self.domainNotlari=data;
      });
    }
    self.tarihFormatStr = function (tarih) {
       if (tarih) {
           let date = new Date(parseInt(tarih.replace("/Date(", "").replace(")/", ""), 10));
           let formattedDate = moment(date).format('DD.MM.YYYY');
           return formattedDate;
       }else
       {
           return "";
       }
   }
})
