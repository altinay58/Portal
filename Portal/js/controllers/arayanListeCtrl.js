﻿/// <reference path="C:\Projects\Portal\Portal\Scripts/angular.min.js" />
/// <reference path="../services/arayanListService.js" />

angModule.controller('arayanListeCtrl', function ($scope, arayanListService, $timeout) {
      var self = $scope; 
      self.arayanlar = [], self.yukleniyor = true;
      $scope.maxSize = 5;
      angular.element(document).ready(function () {        
          self.getirData();
      });
      self.init = function () { // url de aşağıdaki değişkenleri bulup değerlerini alır
          //aryanListService.getListData(self.basTarih, self.bitisTarih);
          self.basTarih = qs("basTarih");
          self.bitisTarih = qs("bitisTarih");
          self.currentPage = qs("currentPage") === null ? 1 : parseInt(qs("currentPage"));
          self.adSoyad = qs("adSoyad");
          self.firma = qs("firma");
          self.firmaid = qs("KayitliFirmaId");
          self.kayitlimi = qs("KayitliMi");
          self.telNo = qs("telNo");
          self.note = qs("note");
      }
      self.getirData = function (model) {
          self.yukleniyor = true;
          if ((model===undefined || model==="") || model.length >= 3) {
              arayanListService.getListData(qs("basTarih"), qs("bitisTarih"), qs("firma"), qs("telNo"), qs("note"), qs("adSoyad"), qs("currentPage"), qs("KayitliFirmaId"), qs("KayitliMi"))
            .then(function (res) {
                self.arayanlar = res.Data;
                self.yukleniyor = false;
                $scope.totalItems = res.ToplamSayi;
                //self.$apply();
            });
          } else {
              self.yukleniyor = false;
          }
        
      }
      self.kontrolTicket = function (ticket) {
          if (ticket === null) {
              return '-';
          } else {
              return `<a href='/Isler/IsEkleDuzenle/${ticket}'>${ticket}</a>`;
          }
      }
      self.firmaKaydet = function (index) {
          alert(index);
      }
      self.kontrolKayitDurumu = function (arayan) {         
          if (arayan.KayitDurum) {
              return `<a href="/SatisFirsatis/Kaydet" class="btn btn-default blue" style="color:#fff !important">Teklif</a>`;  //"<span><a href='#'>Kayıtlı Firma</a></span>";
              //return `<a href="/SatisFirsatis/Kaydet/${arayan.KayitliFirmaId}" class="btn btn-default blue" style="color:#fff !important">Teklif</a>`;  //"<span><a href='#'>Kayıtlı Firma</a></span>";
          } else {
              //return `<a href="/SatisFirsatis/Kaydet/${arayan.KayitliFirmaId}" class="btn btn-default blue" style="color:#fff !important">Teklif</a>`;
              return `<a href="/SatisFirsatis/Kaydet" class="btn btn-default blue" style="color:#fff !important">Teklif</a>`;
              //return `<button type="button" class="btn btn-default red" onclick="angular.element($(this).scope().firmaKaydet(${arayan.Id}))">Kaydet</button>`;
              //.with(arayan.Id);
          }
      }
      self.FirmaKayitliMi = function (arayan) {
              return `<a target="_blank" href="/CariHareket/Detay/${arayan.KayitliFirmaId}"> ${arayan.Firma} </a>`;
      }
      self.pageChanged = function () {
          console.log(self.currentPage);
      }
      function setIfEmpty(obj) {
          if (obj===null || typeof obj === 'undefined') {
              return "";
          } else {
              return obj;
          }
      }
      function qs(name) {
          return portalApp.getParameterByName(name);
}
      self.$watchCollection('[currentPage,basTarih,bitisTarih,firma,telNo,note,adSoyad]', function (nw, ov) {
          console.log(nw[0]+"-"+ov[0]);
          let q = `#?currentPage=${nw[0]}&adSoyad=${setIfEmpty(nw[6])}&firma=${setIfEmpty(nw[3])}`;
          q = q + `&telNo=${setIfEmpty(nw[4])}&note=${setIfEmpty(nw[5])}`;
          q = q + `&basTarih=${setIfEmpty(nw[1])}&bitisTarih=${setIfEmpty(nw[2])}`;
          document.location.href = q;
          let yeniSayfa = nw[0];
          let eskiSayfa = ov[0];
          if (yeniSayfa !== eskiSayfa) {
              self.getirData();
          }         
      });

  });
