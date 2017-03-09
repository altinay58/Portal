/// <reference path="_references.js" />
//genel olarak kullanilan ajax cagirmalari icin yapildi
var commonAjaxService = {
    getDomainInfo: function (domainName) {
        return $.ajax({
            url: "/Domain/DomainSorgula",
            method: "get",
            data: { domain: domainName },
            dataType: "html",
            cache:false
        });
        //return $.get("/Domain/DomainSorgula?domain="+domainName);
    },
    domainUzatmaMailiGonder: function (domainId) {
        return $.ajax({
            url: "/Domain/DomainUzatmaMailiGonder",
            method: "get",
            data: { domainID: domainId },
            dataType: "json",
            cache: false
        });
    },
    getDataFromRemote: function (url, data) {
        return $.ajax({
            url: url,
            method: "get",
            data: data,
            dataType: "json",
            cache: false
        });
    }

}