Array.prototype.clear = function () {
    for (var i = this.length - 1; i >= 0; i--) {
        this.splice(i, 1);
    }
}
if (!String.prototype.with) {
    String.prototype.with = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
        ? args[number]
        : match
            ;
        });
    };
}