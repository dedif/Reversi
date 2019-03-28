spa.data = (function ($) {
    let _configMap;

    function init(environment = "development") {
        _configMap = {
            environment: environment,
            endpoints: []
        };
        if (environment === "production") {
            _configMap.endpoints.push("api/Spel")
        }
        return true
    }

    let _getGames = new Promise(function (resolve, reject) {
        $.ajax("https://localhost:44358/api/chuck").done(function (data) {
            // alert("Hoi");
            // data.forEach(function (item) {
            //     alert(item.rating)
            // });
            resolve(data)
        });
    });
    function getSpellen() {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: 'GET',
                dataType: 'json',
                url: "https://localhost:44358/api/chuck",
                timeout: 5000,
                success: function (data) {
                    resolve(data)
                },
                error: function (xhr, textStatus, errorThrown) {
                    widget.showMessage("Error retrieving games",textStatus);
                    reject(textStatus)
                }
            });
        })
        // $.get("https://localhost:44358/api/chuck").done(function (data) {
        //
        // });
    }

    return {
        init : init,
        configMap : function () {
            return _configMap
        },
        getSpellen: getSpellen
    }
})(jQuery);