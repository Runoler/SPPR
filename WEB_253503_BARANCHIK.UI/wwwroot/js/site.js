﻿$(document).ready(function () {
    $(document).on("click", ".page-link", function (e) {
        e.preventDefault();
        const url = $(this).attr("href");

        $.get(url, function (data) {
            $("#room-list-container").html(data);
        });
    });
});