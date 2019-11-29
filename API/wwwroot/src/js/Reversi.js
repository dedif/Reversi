spa.reversi = (function ($) {
    let configMap;
    let widget;

    function init() {
        widget = new Widget();
        return true;
    }

    return {
        init : init
    }
})(jQuery);