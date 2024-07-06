$(document).ready(function () {
    var path = window.location.pathname;

    $('.nav-link').each(function () {
        var href = $(this).attr('href');

        if (path === href) {
            $(this).closest('.nav-option').addClass('active');
        } else {
            $(this).closest('.nav-option').removeClass('active');
        }
    });
});
