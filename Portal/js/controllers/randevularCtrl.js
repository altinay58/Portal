var ff;
angModule.controller("randevularCtrl", function ($scope, $log, randevularService) {
    var self = $scope;
    $scope.currentPage = 1
  , $scope.maxSize = 5;
    self.randevular = [];
    angular.element(document).ready(() => {
        self.getirData();
    });
    self.getirData = function () {
        randevularService.getirRandevulari(self.basTarih, self.bitisTarih, $scope.currentPage)
        .then((res) => {
            self.randevular = res.Data;
            ff = res;
            $scope.totalItems = res.ToplamSayi;
        });
    }
    self.tarihFormatUznStr = function (tarih) {
        if (tarih) {
            if (angular.isDate(tarih)) {
                let formattedDate = moment(tarih).format('DD.MM.YYYY HH:mm');
                return formattedDate;
            } else {
                if (tarih.indexOf('/Date(') > -1) {
                    let date = new Date(parseInt(tarih.replace("/Date(", "").replace(")/", ""), 10));
                    let formattedDate = moment(date).format('DD.MM.YYYY HH:mm');
                    return formattedDate;
                } else {
                    return moment(tarih).format("DD.YYYY HH:mm")
                }

            }

        } else {
            return "";
        }
    }
    self.pageChanged = function () {
        randevularService.getirRandevulari(self.basTarih, self.bitisTarih, $scope.currentPage)
       .then((res) => {
           self.randevular = res.Data;
           $scope.totalItems = res.ToplamSayi;
       });
    }
    self.randevuSil = function (id) {
        var result = confirm("Silmek istediğinizden eminmisiniz?");
        if (result) {
            var url= "/SatisBolumu/RandevuSil";
            url=url+"/"+id;
            let form=document.getElementById("form1");// $("#form1");
            form.action=url;
            form.submit();

        }
    }
    $scope.$watch('currentPage + numPerPage', function () {
        //var begin = (($scope.currentPage - 1) * $scope.numPerPage)
        //, end = begin + $scope.numPerPage;

        //$scope.filteredTodos = $scope.todos.slice(begin, end);
    });
})