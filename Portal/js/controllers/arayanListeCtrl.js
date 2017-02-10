/// <reference path="C:\Projects\Portal\Portal\Scripts/angular.min.js" />
/// <reference path="../services/arayanListService.js" />




angModule.controller('arayanListeCtrl', function ($scope,arayanListService) {
      var self = $scope; 
      self.arayanlar = [];
      angular.element(document).ready(function () {        
          self.getirData();
      });
      self.init = function () {
          aryanListService.getListData(self.basTarih, self.bitisTarih);        
      }
      self.getirData = function () {
        
          arayanListService.getListData(self.basTarih, self.bitisTarih, self.firma, self.telNo, self.note, self.adSoyad)
              .then(function (data) {               
                  self.arayanlar = data;
                
                  //self.$apply();
              });
      }
      self.kontrolTicket = function (ticket) {
          if (ticket == null) {
              return '-';
          } else {
              return "<a href='#'>" + ticket + "</a>";
          }
      }
      self.firmaKaydet = function (index) {

          alert(index);
      }
      self.kontrolKayitDurumu = function (arayan) {         
          if (arayan.KayitDurum) {
              return "<span><a href='#'>Kayıtlı Firma</a></span>";            
          } else {
              return `<a href="/Firmalar/ArayaniFirmayaKaydet/${arayan.Id}">Firmayı Kaydet</a>`;
              //return `<button type="button" class="btn btn-default red" onclick="angular.element($(this).scope().firmaKaydet(${arayan.Id}))">Kaydet</button>`;
              //.with(arayan.Id);
          }
      }
      //self.$watch('basTarih,bitisTarih', function () {
      //   // self.init();
      //});    
  });
