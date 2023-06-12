var ClientBaseURL = "../Client/";
var ClientBaseURL2 = "../Client/";// "http://localhost:52676/RestServiceWWW/Client/";
var RosterBaseURL2 = "../Roster/";// "http://localhost:52676/RestServiceWWW/Roster/";
var Url;

function getTextDate(d) {
    /* d is Date object */
    var month = d.getMonth() + 1;
    if (month < 10)
        month = '0' + month;
    var dt = d.getDate();
    if (dt < 10)
        dt = '0' + dt;
    return month + '/' + dt + '/' + d.getFullYear();
}

function setShifts() {
    if ($('#ddlClient').val() == 0) {
        /* init client drop down */
        $("#ddlShift").empty();
        var opt = "";
        opt += "<option value=\"0\">ALL SHIFTS</option>";
        $('#ddlShift').html(opt);
        return;
    }
    /* populate the DNR Reasons */
    Url = ClientBaseURL2 + "Shifts/" + $('#ddlClient').val();
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            jsonArray = jsonData;
            /* init client drop down */
            $("#ddlShift").empty();
            var opt = "";
            opt += "<option value=\"0\">ALL SHIFTS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].ID + "\">" + jsonArray[i].Desc + "</option>";
            }
            $('#ddlShift').html(opt);
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}
function setLocations() {
    if ($('#ddlClient').val() == 0) {
        /* init client drop down */
        $("#ddlLocation").empty();
        var opt = "";
        opt += "<option value=\"0\">ALL LOCATIONS</option>";
        $('#ddlLocation').html(opt);
        setShifts();
        return;
    }
    /* "{userID}/Locations?clientID={clientID}" */
    Url = ClientBaseURL2 + $('#userID').val() + "/Locations?clientID=" + $('#ddlClient').val();
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            jsonArray = jsonData;
            /* init client drop down */
            $("#ddlLocation").empty();
            var opt = "";
            if( jsonArray.length > 1 )
                opt += "<option value=\"0\">ALL LOCATIONS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].LocationID + "\">" + jsonArray[i].LocationName + "</option>";
            }
            $('#ddlLocation').html(opt);
            setShifts();
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}
function deleteDNR(idx) {
    Url = ClientBaseURL2 + idx + "/DeleteDnr?userID=" + $('#userID').val();
    //Url = ClientBaseURL2 + idx + "/DeleteDnr?userID=itdept";
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            $('#btnID').trigger('click');
        },
        error: function () {
        }
    });
}
function clearRosters(id, clientID, shiftID) {
    if (shiftID == '')
        shiftID = 0;
    Url = RosterBaseURL2 + id + "/DnrTrim?clientID=" + clientID + "&shiftID=" + shiftID;
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            //alert("Success!!");
        },
        error: function () {
            alert("ERROR!!!" + xhr.responseText);
        }
    });     
}

function setDNR() {
    var allClients = false;
    var allLocations = false;
    var client = $("#ddlClient").val();
    if (client == 0)
        allClients = true;
    var location = $("#ddlLocation").val();
    var supervisor = $("#super").val();
    var reason = $("#ddlReason").val();
    if (reason == "NEW REASON")
        reason = $("#ddlReason2").val();
    var start = $("#start").val();
    var userID = $('#userID').val();
    var shiftID = $('#ddlShift').val();
    if (shiftID == 0)
        shiftID = '';
    var id = $('#empID').val();
    Url = ClientBaseURL2 + "SetDnr?name=" + userID + "&id=" + id + "&client=" + client + "&shift=" + shiftID + 
        "&supervisor=" + supervisor + "&reason=" + reason + "&allClients=" + allClients + "&start=" + start + "&location=" + location;
    //alert(Url);

    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            $('#btnID').trigger('click');
            //alert("Clear any Roster!");
            clearRosters(id, client, shiftID);
        },
        error: function () {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

jQuery(document).ready(function () {
    //alert("Here we go!");

    /* init events */
    $('#start').datepicker();
    $('#start').val(getTextDate(new Date()));

    $('#btnDNR').click(function (e) {
        setDNR();
    });
    $('#ddlClient').change(function (e) {
        setLocations(); //setShifts();
    });
    var jsonArray = new Array();

    $('#empID').keypress(function (event) {
        if (event.which == 13) {
            $('#btnID').trigger('click');
            return false;
        }
    });

    $('#btnID').click(function (e) {
        e.preventDefault();
        var id = $('#empID');
        if (id.val().length == 0)
            alert("Please enter a value!");
        else {
            Url = ClientBaseURL2 + id.val() + "/GetDnr";
            $.ajax({
                cache: false,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                url: Url,
                success: function (jsonData) {
                    //alert("success!!!");
                    jsonArray = jsonData;
                    $("#empName").text(jsonArray[0].LastName + ", " + jsonArray[0].FirstName);
                    $("#divRecords").empty();

                    var opt = "";
                    $('#dnrRecords').html(opt);
                    if (jsonArray[0].AidentNumber != null) {
                        opt += "<table id=\"dnrTable\" border=\"1\" cellpadding=\"4\"><thead><tr><th>#</th><th>Client</th><th>Location</th><th>Shift/Dept</th><th>Supervisor</th><th>Reason</th>" +
                            "<th>Starting</th><th>Remove</th></tr></thead>";

                        for (var i = 0; i < jsonArray.length; i++) {
                            if (jsonArray[i].ClientName == null)
                                continue;
                            var clientName = jsonArray[i].ClientName;
                            if (jsonArray[i].AllClients == "true")
                                clientName = "ALL CLIENTS!";

                            opt += "<tr><td>" + (i + 1) + "</td><td>" + jsonArray[i].ClientName + "</td><td>" + jsonArray[i].LocationName + "</td><td>" + jsonArray[i].Shift + "</td><td>" + jsonArray[i].Supervisor + "</td><td>" + jsonArray[i].DNRReason +
                                "</td><td>" + jsonArray[i].StartDate + "</td><td>";
                            if( $("#hdnRemoveDnr").val() == 'true' )
                                opt += "<input type='button' id='btnDnr" + i + "'value='REM' onclick='deleteDNR(" + jsonArray[i].ClientDnrID + ")'/>";
                            opt += "</td></tr>";
                        }
                        opt += "</table>";
                        $('#dnrRecords').html(opt);
                        $('#dnrTable').fixedHeaderTable({
                            footer: true,
                            cloneHeadToFoot: true,
                            altClass: 'odd',
                            themeClass: 'fancyTable',
                            footer: false,
                            cloneHeadToFoot: false,
                            fixedColumn: false,
                            autoShow: true
                        });
                    }
                    else {
                        opt = "<span>" + jsonArray[0].FirstName + " " + jsonArray[0].LastName + " IS NOT DNR'd</span>";
                        $('#dnrRecords').html(opt);
                    }

                    $('#ddlClient').removeAttr("disabled");
                    $('#ddlLocation').removeAttr("disabled");
                    $('#ddlReason').removeAttr("disabled");
                    $('#start').removeAttr("disabled");
                    $('#super').removeAttr("disabled");
                    if ($("#hdnSetDnr").length == 0 || $("#hdnSetDnr").val() == 'true') {
                        $('#btnDNR').removeAttr("disabled");
                    }
                    $('#ddlShift').removeAttr("disabled");
                },
                error: function (xhr) {
                    alert("ERROR!!!" + xhr.responseText);
                }
            });
        }
        return false;
    });

    /* populate the DNR Reasons */
    Url = ClientBaseURL + "GetDnrReasons";
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            jsonArray = jsonData;
            /* init client drop down */
            $("#ddlReason").empty();
            var opt = "";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i] + "\">" + jsonArray[i] + "</option>";
            }
            var userID = $('#userID').val();
            if( userID.toUpperCase() == "MMENDEZ" || userID.toUpperCase() == "ITDEPT" || userID.toUpperCase() == "SANCHEZM" || userID.toUpperCase() == "CHAVEZZ" )
                opt += "<option value=\"NEW REASON\">NEW REASON</option>";
            $('#ddlReason').html(opt);

            $('#ddlReason').change(function () {
                var $this = $(this),
                id = $this.attr('id'),
                $txt = $('input:text[name=' + id + ']');
                if ($this.val() == 'NEW REASON') {
                    if ($txt.length == 0) {
                        $txt = $('<input type="text" id="' + id + '2" />');
                        $this.after($txt);
                    }
                    $txt.show().focus();
                }
                else {
                    $('#ddlReason2').hide();
                }
            });
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });

    /* populate client ddl when page is loaded */
    Url = ClientBaseURL + $('input[id$="userID"]').val() + "/Active";
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            jsonArray = jsonData;
            /* init client drop down */
            $("#ddlClient").empty();
            var opt = "";
            opt += "<option value=\"0\">ALL CLIENTS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].ClientID + "\">" + jsonArray[i].ClientName + "</option>";
            }
            $('#ddlClient').html(opt);
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
    setLocations(); //    setShifts();

});