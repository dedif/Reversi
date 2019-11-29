spa.model = (function ($) {
    let configMap;
    let widget;
    let user;

    function init() {
        widget = new Widget();
        return true;
    }

    function User(avatar) {
        this.avatar = avatar;
    }

    return {
        init: init,
        user: function () {
            return user;
        },
        User: User
    }
})(jQuery);