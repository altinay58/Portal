/// <reference path="C:\Projects\Portal\Portal\Scripts/_references.js" />
//
var portalApp = {
    mesajGoster:function(msj,type){
        $.bootstrapGrowl(msj, {
            ele: 'body', // which element to append to
            type:typeof type!=='undefined'?type: 'info', // (null, 'info', 'danger', 'success')
            offset: { from: 'top', amount: 50 }, // 'top', or 'bottom'
            align: 'right', // ('left', 'right', or 'center')
            width: 250, // (integer, or 'auto')
            delay: 1500, // Time while the message will be displayed. It's not equivalent to the *demo* timeOut!
            allow_dismiss: true, // If true then will display a cross to close the popup.
            stackup_spacing: 10 // spacing between consecutively stacked growls.
        });
    
    },
    getParameterByName: function (name, url) {
        if (!url) {
            url = window.location.href;
        }
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)");
        results = regex.exec(url);
        if (!results) {
            regex = new RegExp("[?&]" + name + "(:([^&#]*)|&|#|$)");
            results = regex.exec(url);
        }
        
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
   
}