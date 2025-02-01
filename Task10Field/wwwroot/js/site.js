// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    function showMessage() {
        var formMessage = document.getElementById("FormMsgId");
    if (formMessage) {
        formMessage.style.display = "block"; // Show the message
    setTimeout(function () {
        formMessage.style.display = "none"; // Hide after 5 seconds
            }, 5000);
        }
    }

    // Call showMessage() directly on page load if the message is present
    document.addEventListener("DOMContentLoaded", function () {
        showMessage();
    });
