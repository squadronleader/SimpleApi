(function (context) {

    context.SimpleApi = {
        alert: function () {
            window.alert.apply(window, arguments);
        }
    };

}(this));