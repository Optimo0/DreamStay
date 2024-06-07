// Smooth scrolling for navbar links
$(document).ready(function () {
    // When a link with class "dropbtn" is clicked
    $(".dropbtn").click(function () {
        var target = $(this).attr('href');
        // Smoothly scroll to target section
        $('html, body').animate({
            scrollTop: $(target).offset().top - 60
        }, 1000);
    });
});

$(document).ready(function () {
    // When a link within dropdown content is clicked
    $(".dropdown-content a").click(function () {
        var target = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(target).offset().top - 60
        }, 1000);
    });
});


$(document).ready(function () {
    // When a link within side menu is clicked
    $("#sideMenu a").click(function () {
        var target = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(target).offset().top - 60
        }, 1000);
    });
});

// Show/hide side menu based on window width
$(document).ready(function () {
    if ($(window).width() < 650) {
        $("#sideMenu").show();
    } else {
        $("#sideMenu").hide();
    }
});

// Adjustments on window resize
$(window).resize(function () {
    if ($(window).width() < 650) {
        $("#sideMenu").show();
        $("header").hide();
    } else if (($(window).width() > 650) && ($(window).scrollTop() == 0)) {
        $("#sideMenu").hide();
        $("header").show();
    }
});

// Adjustments on page scroll
$(document).scroll(function () {
    if ($(window).scrollTop() == 0) {
        $("#sideMenu").hide();
        $("header").show();
    } else {
        $("header").hide();
        $("#sideMenu").show();
    }
});