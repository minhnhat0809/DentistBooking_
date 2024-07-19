$(document).ready(function () {
    var path = window.location.pathname;

    function setActiveLink() {
        $('.nav-option').removeClass('active');
        $('.nav-link').each(function () {
            var href = $(this).attr('href');
            if (path === href) {
                $(this).closest('.nav-option').addClass('active');
            }
        });
    }
    
    setActiveLink();
    
    $('.nav-option').on('click', function (e) {
        if (e.target.tagName.toLowerCase() !== 'a') {
            var link = $(this).find('.nav-link');
            var href = link.attr('href');
            if (href) {
                window.location.href = href;
            }
        }
    });

    $('.nav-link').on('click', function () {
        var href = $(this).attr('href');
        window.location.href = href;
    });
});
