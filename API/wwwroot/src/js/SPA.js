let spa = (function ($) {
    let _configMap;

    function init() {
        spa.data.init("development");
        return true;
    }

    return {
        init : init,
    }
})(jQuery);