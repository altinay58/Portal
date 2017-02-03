var ff;
angModule.controller("randevularCtrl", function ($scope, $log, randevularService) {
    var self = $scope;
    $scope.filteredTodos = []
  , $scope.currentPage = 1
  , $scope.numPerPage = 5
  , $scope.maxSize = 5;
    self.randevular = [];
    angular.element(document).ready(() => {
        self.getirData();
    });
    self.getirData = function () {
        randevularService.getirRandevulari(self.basTarih, self.bitisTarih)
        .then((res) => {
            self.randevular = res;
            ff=res;
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

    $scope.$watch('currentPage + numPerPage', function () {
        //var begin = (($scope.currentPage - 1) * $scope.numPerPage)
        //, end = begin + $scope.numPerPage;

        //$scope.filteredTodos = $scope.todos.slice(begin, end);
    });
})