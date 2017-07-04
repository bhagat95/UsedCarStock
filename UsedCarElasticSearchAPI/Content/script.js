var MINBUDGET = 10000;
var MAXBUDGET = 50000000;
$(document).ready(function () {

    $('.numbersOnly').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });
    var res = getUrlVars();
    var page = getUrlVars()["page"];
    if (page == 0 || typeof page === "undefined") {
        $('#btnprev').each(function () {
            $(this).css("display", "none");
        });
    }
    $('#btnprev').click(function () {
        var res = getUrlVars();
        var page = getUrlVars()["page"];
        if (typeof page === "undefined" || page == 0) {
            page = 0;
        }
        else {
            page--;
        }
        var cityName = getUrlVars()["city"];
        if (!cityName) {
            cityName = "all";
        }
        var minBudget = getUrlVars()["minbudget"];
        if (!minBudget || minBudget <= MINBUDGET) {
            minBudget = MINBUDGET;
        }
        var maxBudget = getUrlVars()["maxbudget"];
        if (!maxBudget || maxBudget >= MAXBUDGET) {
            maxBudget = MAXBUDGET;
        }
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?page=' + page + '&city=' + cityName + '&minbudget=' + minBudget + '&maxbudget=' + maxBudget;
        window.history.pushState({ path: newurl }, '', newurl);
        $("#listing-section").load("/UsedCars/Filter/?page=" + page + "&city=" + cityName + "&minbudget=" + minBudget + "&maxbudget=" + maxBudget, function (response, status) {
            if (status == "error") {
            }
        });
        if (page === 0) {
            $(this).css("display", "none");
        }
        else {
            $(this).css("display", "block");
        }
    });
    $('#btnnext').click(function () {
        $('#btnprev').each(function () {
            $(this).css("display", "block");
        });
        var res = getUrlVars();
        var page = getUrlVars()["page"];
        if (!page) {
            page = 0;
            page++;
        }
        else {
            page++;
        }
        var cityName = getUrlVars()["city"];
        if (!cityName) {
            cityName = "all";
        }
        var minBudget = getUrlVars()["minbudget"];
        if (!minBudget || minBudget <= MINBUDGET) {
            minBudget = MINBUDGET;
        }
        var maxBudget = getUrlVars()["maxbudget"];
        if (!maxBudget || maxBudget >= MAXBUDGET) {
            maxBudget = MAXBUDGET;
        }
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?page=' + page + '&city=' + cityName + '&minbudget=' + minBudget + '&maxbudget=' + maxBudget;
        window.history.pushState({ path: newurl }, '', newurl);
        $("#listing-section").load("/UsedCars/Filter/?page=" + page + "&city=" + cityName + "&minbudget=" + minBudget + "&maxbudget=" + maxBudget, function (response, status) {
            if (status == "error") {
            }
        });
    });
    $('#gobtn').click(function () {
        var minBudget = $('#minBudget').val();
        var maxBudget = $('#maxBudget').val();
        if (minBudget < MINBUDGET || !minBudget) {
            alert("min budget should be more than " + MINBUDGET);
            $('#minBudget').val(MINBUDGET);
            minBudget = MINBUDGET;
        }
        if (maxBudget > MAXBUDGET || !maxBudget || maxBudget < MINBUDGET) {
            alert("max budget should be less than " + MAXBUDGET);
            $('#maxBudget').val(MAXBUDGET);
            maxBudget = MAXBUDGET;
        }
        var cityName = $('#location').val();
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?city=' + cityName + '&minbudget=' + minBudget + '&maxbudget=' + maxBudget;
        window.history.pushState({ path: newurl }, '', newurl);
        $("#listing-section").load("/UsedCars/Filter/?city=" + cityName + "&minbudget=" + minBudget + "&maxbudget=" + maxBudget, function (response, status) {
            if (status == "error") {
            }
        });
    });
    $('#location').change(function () {
        var minBudget = $('#minBudget').val();
        var maxBudget = $('#maxBudget').val();
        if (minBudget < MINBUDGET || !minBudget) {

            $('#minBudget').val(MINBUDGET);
            minBudget = MINBUDGET;
        }
        if (maxBudget > MAXBUDGET || !maxBudget || maxBudget < MINBUDGET) {

            $('#maxBudget').val(MAXBUDGET);
            maxBudget = MAXBUDGET;
        }
        var cityName = $(this).val();
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?city=' + cityName;
        window.history.pushState({ path: newurl }, '', newurl);
        $("#listing-section").load("/UsedCars/Filter/?city=" + cityName + "&minbudget=" + minBudget + "&maxbudget=" + maxBudget, function (response, status) {
            if (status == "error") {
            }
        });
    });
});