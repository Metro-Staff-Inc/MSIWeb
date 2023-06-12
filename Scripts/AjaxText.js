jQuery(document).ready(function () {
    webServiceLoc = $('input[id$="webServiceLoc"]').val();
    alert("Here we go - TEST!");


    $.get(
    "http://" + webServiceLoc + "Roster/Hello",
    "",
    function (data) { alert(data); },
    "json"
);
});