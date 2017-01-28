angModule.controller("firmaKisiListCtrl",function($scope,firmaKisiListService){
  var self=$scope;
  self.kisiler=[];
  angular.element(document).ready(()=>{
    self.getirData();
  });
  self.getirData=function(){
    firmaKisiListService.getirFirmaKisiList(self.ad,self.soyad,self.firmaAdi,self.departman)
    .then((res)=>{
      self.kisiler=res;
    });
  }
})
