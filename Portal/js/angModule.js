var angModule = angular.module('angularApp', ['ui.bootstrap']); // angularjs nin çalışması için layout a html etiketine eklenmeli
angModule.filter('to_trusted', ['$sce', function ($sce) { // to_trusted text i html olarak yazdırmamızı sağlıyor burada tanımladıktan sorna viewlerin içerisinde fonksiyonlardan sorna  | ile ayırıp kullanılır
    return function (text) {
        return $sce.trustAsHtml(text);
    };
}]);