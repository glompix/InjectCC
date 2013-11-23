var injectcc = {
    models: {},
    init: function () {
        // Navigation toggles.
        $('.phone-menu-toggle').click(function () {
            $('.main-menu').toggle();
            $('body').toggleClass('phone-menu-visible');
        });
        $('.phone-login-toggle').click(function () {
            $('body').toggleClass('phone-login-visible');
            // Hide the logo in phone view to make room for the login form.
            var $loginCompact = $(this).siblings('.login-compact');
            if ($loginCompact.is(':hidden')) {
                $(this).text('Sign in');
            }
            else {
                $(this).text('Cancel');
            }
        });
    }
};