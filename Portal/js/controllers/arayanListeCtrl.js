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
          console.log(arayan);
          if (arayan.KayitDurum) {
              return '<button type="button" class="btn btn-default red" onclick="angular.element($(this).scope().firmaKaydet({0}))">Kaydet</button>'.with(arayan.Id);
            
          } else {
              return "<span><a href='#'>Kayıtlı Firma</a></span>";
          }
      }
      //self.$watch('basTarih,bitisTarih', function () {
      //   // self.init();
      //});    
  });
