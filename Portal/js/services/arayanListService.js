var aryanListService = {
    getListData: function (basTar,bitTar,firma,telNo,note,adSoyad) {
        return $.ajax({
            url: "/Arayanlar/ArayanListesiParametre",
            method: "get",
            data: {
                basTarih: basTar, bitisTarih: bitTar, firma: firma, telNo: telNo,
                note: note, adSoyad:adSoyad
            },
            dataType: "json",
            cache: false
        });
        //return $.get("/Domain/DomainSorgula?domain="+domainName);
    }

}