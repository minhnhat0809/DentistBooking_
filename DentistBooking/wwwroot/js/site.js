// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let menuicn = document.querySelector(".menuicn");
let nav = document.querySelector(".navcontainer");

menuicn.addEventListener("click", () => {
    nav.classList.toggle("navclose");
})

document.addEventListener("DOMContentLoaded", function () {
    var path = window.location.pathname;
    var navOptions = document.querySelectorAll(".nav-option");

    navOptions.forEach(function (option) {
        var link = option.getAttribute("href");
        if (path === link) {
            option.classList.add("active");
        }
    });
});