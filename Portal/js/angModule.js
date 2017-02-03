var angModule = angular.module('angularApp', ['ui.bootstrap']);
angModule.filter('to_trusted', ['$sce', function ($sce) {
    return function (text) {
        return $sce.trustAsHtml(text);
    };
}]);