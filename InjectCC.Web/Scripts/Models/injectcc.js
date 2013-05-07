var injectcc = {
    models: {},
    init: function () {
        $('.phone-menu-toggle').click(function () {
            $('.main-menu').toggle();
            $('body').toggleClass('phone-menu-visible');
        });
    }
};