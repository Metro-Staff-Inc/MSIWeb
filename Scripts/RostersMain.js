
function swap(a, b, array) {
    var temp = array[a];
    array[a] = array[b];
    array[b] = temp;
}
function partition(array, begin, end, pivot, rowClass) {
    var piv = array[pivot];
    swap(pivot, end - 1, array);
    var store = begin;
    var ix;
    for (ix = begin; ix < end - 1; ++ix) {
        if ($(array[ix]).find('.' + rowClass).text() <= $(piv).find('.'+rowClass).text()) {
            swap(store, ix, array);
            ++store;
        }
    }
    swap(end - 1, store, array);
    return store;
}

function qsort(array, begin, end, rowClass) {
    if (end - 1 > begin) {
        var pivot = begin + Math.floor(Math.random() * (end - begin));
        pivot = partition(array, begin, end, pivot, rowClass);
        qsort(array, begin, pivot, rowClass);
        qsort(array, pivot + 1, end, rowClass);
    }
}

function sortTable(rowClass) {
    if (rowClass == null)
        return;
    var tr = $("#callTable tbody tr");
    qsort(tr, 0, tr.length, rowClass);

    $("#callTable tbody tr").remove();
    $("#callTable tbody").append($(tr));
    $("#callTable tbody tr").removeAttr("class").attr("class", "employeeRoot");
    $("#callTable tbody tr:odd").toggleClass("odd");

    $("#callTable tbody tr").each(function () {
        setCallTable(this);
    });

    $(".phoneNum").each(function () {
        if (phoneNumberIsValid($(this).text())) {
            $(this).closest('tr').removeClass("employeeRootBadPhoneNumber");
            $(this).closest('tr').addClass("employeeRoot");
        }
        else {
            $(this).closest('tr').removeClass("employeeRoot");
            $(this).closest('tr').addClass("employeeRootBadPhoneNumber");
        }
    });
}

function emailRadioList() {

    var list = "<div><table id='emailList'>";
    list += "<thead><tr><td>Add Email:</td><td><input type='text' placeholder='Enter Email Address'/><input class='addButton' type='button' value='ADD'/></td></tr></thead>";
    list += "<tbody>";
    if (emailList != null) {
        for (var i = 0; i < emailList.length; i++) {
            list += "<tr><td><input type='checkbox' checked></td>" +
            "<td>" + emailList[i] + "</td></tr>";
        }
    }
    list += "</tbody";
    list += "</table></div>";

    return list;
};

var officeDropDownList = "Employees From: <select id='officeList'>" +
            "<option value='-1'>ALL OFFICES</option>" +
            "<option value='TR'>AURORA</option>" +
            "<option value='TB'>BARTLETT</option>" +
            "<option value='TE'>ELGIN</option>" +
            "<option value='TW'>WEST CHICAGO</option>" +
            "<option value='TO'>BOLINGBROOK</option>" +
            "<option value='TG'>ELK GROVE</option>" +
            "<option value='TV'>VILLA PARK</option>" +
            "<option value='TH'>WHEELING</option>" +
            "</select>";
            
function emailRosterMenu() {
    if ($('#ddlClientRoster').val() != 8) {
        $("<div>" +
        "<input type='button' value='EMAIL ROSTER' onclick='emailRoster()'/><br/>" +
        "<input type='button' value='TEMPS ONLY' onclick='emailRosterDaily()'/><br/>" +
        "<input type='button' value='FIRST DAY ONLY' onclick='emailRosterNew()'/><br/>" +
        "<hr/>" +
        officeDropDownList +
        "<hr/>" +
        emailRadioList() +
        "</div>").dialog({
            title: "EMAIL CURRENT ROSTER",
            buttons: {
                "CLOSE": function () {
                    //$(this).dialog("close");
                    $(this).remove();
                }
            },
            close: function () {
                $(this).remove();
            }
        });
        $('.addButton').click(function () {
            $("#emailList").find('tbody').append($("<tr><td><input type='checkbox' checked></td><td>" +
                $('#emailList').find('thead').find('input').val() + 
                "</td></tr>"));
            //alert("add!!");
        });
    }
    else {
        $("<div>" +
        "<input type='button' value='EMAIL ROSTER' onclick='emailSuncastRoster()'/><br/>" +
        "</div>").dialog({
            title: "EMAIL CURRENT SUNCAST ROSTER",
            buttons: {
                "CLOSE": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function emailSuncastRoster() {
    waitIndicator(true, "Emailing Roster to Suncast");
    var shiftType = $('#ddlShift').val();
    var temps = true;
    Url = RosterBaseURL + "EmailSuncast?clientID=" + $('#ddlClientRoster').val() + "&shiftType=" + $('#ddlShiftRoster').val() + "&deptID=0" + "&date=" + $('#dateRoster').val() + "&tempsOnly=" + temps;
    //alert(Url);
    var success = false;
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var divId = "divSuncastRoster";
            waitIndicator(false);
            $("<div id='" + divId + "'><h1>Roster emailed.</h1>" + msg + "</div>").dialog();
            setTimeout(function () {
                $('#' + divId).remove();
            }, 20000);
        },
        error: function (msg) {
            waitIndicator(false);
            alert("an unexpected error has occurred! " + msg.d);
        }
    });
    return success;
}

function printRosterMenu() {
    $("<div>" +
        "<input type='button' value='PRINT ROSTER' onclick='printRoster()'/><br/>" +
        "<input type='button' value='TEMPS ONLY' onclick='printRosterDaily()'/><br/>" +
        "<input type='button' value='FIRST DAY ONLY' onclick='printRosterNew()'/><br/>" +
        officeDropDownList + 
    "</div>").dialog({
        title: "PRINT CURRENT ROSTER",
        buttons: {
            "CLOSE": function () {
                $(this).dialog("close");
            }
        }
    });
}

function setHeader( mywindow ) {
    var office = $('#officeList option:selected').val();
    if (office == -1)
        office = "All Offices";
    else
        office = $('#officeList option:selected').text() + " Office";
    mywindow.document.write('<html><head><title>' + $("#ddlClientRoster option:selected").text() + ', ' + $('#dateRoster').val() + '</title>');
    /*optional stylesheet*/ //mywindow.document.write('<link rel="stylesheet" href="main.css" type="text/css" />');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h1>' + $("#ddlClientRoster option:selected").text() + ', ' + $('#ddlLocationRoster option:selected').text() + '</h1>');
    mywindow.document.write('<h3>' + $('#ddlShiftRoster option:selected').text() + ', ' + $('#ddlDepartmentRoster option:selected').text() + '</h3>');
    mywindow.document.write('<h3>' + $('#dateRoster').val() + ', ' + office + '</h3>');
}

function printRoster() {
    var $rows = $('#clientRoster tbody tr');
    //alert($rows.length);
    var mywindow = window.open('', 'ClientRoster', 'height=400,width=600');
    setHeader(mywindow);
    var i = 1;
    $rows.each(function () {
        var idAtClient = $(this).find(".idAtClient").val();
        var aident = $(this).find(".aident").text();
        if (idAtClient > 0)
            aident += " / " + idAtClient;
        if ($('#officeList option:selected').val() == -1 || $('#officeList option:selected').val() == $(this).attr("officeID"))
            mywindow.document.write(i++ + ".<span style='padding:12px 16px'>" + aident +
            "</span>" + "<span style='padding:12px 16px'>" + $(this).find(".name").text() + "</span><hr/>");
    });
    mywindow.document.write('</body></html>');

    mywindow.print();
    mywindow.close();

    return true;
}
function getEmailSubject()
{
    var s = "ROSTER FOR ";
    s = s + $("#ddlClientRoster option:selected").text() + " ";
    s = s + $("#ddlShiftRoster option:selected").text() + ", ";
    s = s + $("#ddlDepartmentRoster option:selected").text() + " ";
    s = s + $("#dateRoster").val();
    alert(s);
    return s;
}

function emailRoster() {
    var $rows = $('#clientRoster tbody tr');
    var body = "";
    var i = 1;
    $rows.each(function () {
        var idAtClient = $(this).find(".idAtClient").val();
        var aident = $(this).find(".aident").text();
        if (idAtClient > 0)
            aident += " / " + idAtClient;
        body = body + (i++) + ". " + aident + "%09" + $(this).find(".name").text() + "%0D";
    });

    email = "";
    var mailto_link = 'mailto:' + email + '?subject=' + getEmailSubject() + '&body=' + body;
    var win = window.open(mailto_link, 'emailWindow');
    if (win && win.open && !win.closed) win.close();
    return true;
}
function emailRosterDaily() {
    var $rows = $('#clientRoster tbody tr');
    var body = "";
    var i = 1;
    $rows.each(function () {
        var startDate = stringToDate($(this).find("input[class*='startDt']").val());
        var endDate = stringToDate($(this).find("input[class*='endDt']").val());
        var diffDays = dateDiffInDays(startDate, endDate);
        if (diffDays <= 7) {
            var idAtClient = $(this).find(".idAtClient").val();
            var aident = $(this).find(".aident").text();
            if (idAtClient > 0)
                aident += " / " + idAtClient;
            body = body + (i++) + ". " + aident + "%09" + $(this).find(".name").text() + "%0D";
        }
    });

    email = "";
    var mailto_link = 'mailto:' + email + '?subject=' + getEmailSubject() + '&body=' + body;
    var win = window.open(mailto_link, 'emailWindow');
    if (win && win.open && !win.closed) win.close();
    return true;
}

function printRosterDaily() {
    var $rows = $('#clientRoster tbody tr');
    var mywindow = window.open('', 'ClientRoster', 'height=400,width=600');
    setHeader(mywindow);
    var i = 1;
    $rows.each(function () {
        var startDate = stringToDate($(this).find("input[class*='startDt']").val());
        var endDate = stringToDate($(this).find("input[class*='endDt']").val());
        var diffDays = dateDiffInDays(startDate, endDate);
        if (diffDays <= 7) {
            var idAtClient = $(this).find(".idAtClient").val();
            var aident = $(this).find(".aident").text();
            if (idAtClient > 0)
                aident += " / " + idAtClient;
            mywindow.document.write(i++ + ".<span style='padding:12px 16px'>" + aident + "</span><span style='padding:12px 16px'>" + $(this).find(".name").text() + "</span><hr/>");
        }
    });
    mywindow.document.write('</body></html>');

    mywindow.print();
    mywindow.close();

    return true;
}

function printRosterNew() {
    var $rows = $('#clientRoster tbody tr');
    var mywindow = window.open('', 'ClientRoster', 'height=400,width=600');
    setHeader(mywindow);
    var curDt = new Date();
    curDt.setHours(0, 0, 0);
    var i = 1;
    $rows.each(function () {
        var startDate = stringToDate($(this).find("input[class*='startDt']").val());
        //alert( startDate + ", " + curDt );
        if (formatDate(startDate) == formatDate(curDt)) {
            var idAtClient = $(this).find(".idAtClient").val();
            var aident = $(this).find(".aident").text();
            if (idAtClient > 0)
                aident += " / " + idAtClient;
            mywindow.document.write(i++ + ".<span style='padding:12px 16px'>" + aident + "</span><span style='padding:12px 16px'>" + $(this).find(".name").text() + "</span><hr/>");
        }
    });
    mywindow.document.write('</body></html>');

    mywindow.print();
    mywindow.close();

    return true;
}

var _MS_PER_DAY = 1000 * 60 * 60 * 24;

// a and b are javascript Date objects
function dateDiffInDays(a, b) {
    // Discard the time and time-zone information.
    var utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
    var utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());

    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}

function stringToDate(dt) {
    var dtArray = dt.split("/");
    return new Date(dtArray[2], dtArray[0].valueOf() - 1, dtArray[1]);
}

function getDate(dt) {
    return new Date(parseInt(dt.replace("/Date(", "").replace(")/", ""), 10));
}

function formatDate(dt) {
    return "" + (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function setPayPeriod(stElem, endElem) {
    var today = new Date();
    //alert("Pay period " + today);
    while (today.getDay() != 0)
        today.setDate(today.getDate() - 1);
    document.getElementById(stElem).value = formatDate(today);
    today.setDate(today.getDate() + 6);
    document.getElementById(endElem).value = formatDate(today);
}

function calcHours(payStart, payEnd) {
    //alert("Initial arguments: " + payStart + ", " + payEnd);
    ps = getDateFromString(payStart);
    pe = getDateFromString(payEnd);
    pe.setDate(pe.getDate() + 1);
    //alert("Converted: " + ps + ", " + pe);

    var totalHours = 0.0;
    var clientHours = 0.0;
    //alert("clients = " + data.length);
    for (i = 0; i < data.length; i++) {
        var ss = data[i].ShiftStart;
        var se = data[i].ShiftEnd;
        ss = parseFloat(ss.substring(0, ss.indexOf(":")) + '.' + parseFloat(ss.substring(ss.indexOf(":") + 1)) * 1.666667);
        se = parseFloat(se.substring(0, se.indexOf(":")) + '.' + parseFloat(se.substring(se.indexOf(":") + 1)) * 1.666667);
        var hrs = Math.abs(se - ss);
        //alert("Shift Start: " + ss + ", Shift End: " + se);
        payStart = new Date(ps);
        payEnd = new Date(pe);
        //alert("start and end date = " + ps + ", " + pe);
        var sd = new Date(data[i].StartDate.getFullYear(), data[i].StartDate.getMonth(), data[i].StartDate.getDate(), 0, 0, 0);
        var ed = new Date(data[i].EndDate.getFullYear(), data[i].EndDate.getMonth(), data[i].EndDate.getDate(), 23, 59, 0);
        //alert("payStart = " + payStart + ", payEnd = " + payEnd);
        while (payStart < payEnd) {
            //alert("Paystart = " + payStart + ", PayEnd = " + payEnd);
            //alert("Roster Start/End: " + sd + ", " + ed);
            if (payStart >= sd && payStart <= ed) {
                clientHours += hrs;
                //alert(clientHours);    
            }
            payStart.setDate(payStart.getDate() + 1);
        }
        $('#clientHours' + i).text("" + clientHours);
        totalHours += clientHours;
        clientHours = 0;
    }
    $('#totalHours').text("" + totalHours);
}

function updateRosters() {
    //alert("Update rosters!");
    var $rosters = $('tr[update="true"]');
    $rosters.each(function () {
        updateLine($(this));
    });
}

function updateLine($row) {
    var crId = $row.attr('cr_id');

    /* get start and end times */
    var start = new Date($row.find('.startDt').val());
    var end = new Date($row.find('.endDt').val());
    var id = $row.find('.aident').text();
    //alert(id + ", " + start + ", " + end);

    var subs = 0;
    if ($('#sub_' + crId).length > 0)
        subs = $('#sub_' + crId).val();

    var sel = $row.find('select');
    //alert(sel.length);
    var h = sel.eq(0).val();
    var m = sel.eq(1).val();
    if (sel.eq(2).val() == '12') {
        if (parseInt(h, 10) < 12)
            h = parseInt(h, 10) + 12;
    }
    else {
        if (parseInt(h, 10) == 12)
            h = '00';
    }
    var startTime = h + ":" + m;
    h = sel.eq(3).val();
    m = sel.eq(4).val();
    if (sel.eq(5).val() == '12') {
        if (parseInt(h, 10) < 12)
            h = parseInt(h, 10) + 12;
    }
    else {
        if (parseInt(h, 10) == 12)
            h = '00';
    }
    var endTime = h + ":" + m;
    //alert(startTime + ", " + endTime);

    var tStart = $('#trackingStart').text();
    var tEnd = $('#trackingEnd').text();

    var tStartInt = tStart.substring(0, 2) + tStart.substring(3, 5);
    var tEndInt = tEnd.substring(0, 2) + tEnd.substring(3, 5);
    //alert("Start time = " + tStartInt + ", End time = " + tEndInt);

    //alert("Start date = " + start + ", End Date = " + end);



    /* if overnight shift AND YET the same day... */
    if (tStartInt > tEndInt && start.getTime() == end.getTime()) {
        $("<div>You can't have a shift that extends overnight starting and ending on the same day!!</div>").dialog();
        return;
    }
    if (start > end) {
        $("<div>Ending date can't be less than starting date!!</div>").dialog();
        return;
    }

    /* is employee dnr'd at client or universally? */
    var t = dnrCheck(id, $('#ddlClientRoster').val(), $('#ddlShiftRoster').val(), $('#ddlDepartmentRoster').val(),
                            $row.find('.startDt').val(), $row.find('.endDt').val(), $('#ddlLocationRoster').val());

    if (t.length > 0) {
        $("<div>Can't update! Employee has been DNR'd!</div>").dialog();
        return;
    }
    /*
    Url = RosterBaseURL + "PunchesOutside/" + crId + "?startDate=" + start.getFullYear() + "-" + (start.getMonth() + 1) +
    "-" + start.getDate() + "&endDate=" + end.getFullYear() + "-" + (end.getMonth() + 1) + "-" + end.getDate() +
    "&startTime=" + tStart + "&endTime=" + tEnd;
    //alert(Url);
    $.ajax({
    cache: false,
    type: "GET",
    contentType: "application/json; charset=utf-8",
    async: true,
    dataType: "json",
    url: Url,
    success: function (data) {
    if (data != 0) {
    setDatesAndTrim(crId);
    }
    else {
    */
    //ok to update...
    Url = RosterBaseURL + "Update/" + crId + "?startDate=" + start.getFullYear() + "-" + (start.getMonth() + 1) + "-" + start.getDate() +
                            "&endDate=" + end.getFullYear() + "-" + (end.getMonth() + 1) + "-" + end.getDate() +
                            "&startTime=" + startTime + "&endTime=" + endTime +
                            "&trackStart=" + tStart + "&trackEnd=" + tEnd +
                            "&subs=" + subs;
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        url: Url,
        success: function (d) {
            $row.find('input').removeAttr('style');
            $row.find('select').removeAttr('style');
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function placeInDB(id, client, dept, loc, office, shift, startDate, endDate) {
    var retVal = 0;
    Url = ClientBaseURL + client + '/CreateRoster?loc=' + loc + '&aident=' + id + '&dept=' + dept + '&shift=' + shift + '&office=' + office +
                '&startDate=' + startDate + '&endDate=' + endDate;
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            /* update start and end times of shift */
            retVal = msg;
        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
            //return 0;
        }
    });
    return retVal;
}

function addToRosterAction(id, startDate, endDate, $tr, $dialog, override) {

    //alert("Add to roster: " + startDate + ", " + endDate);
    /* put employee w/id pnt onto the roster. */
    //var el = "#" + id;
    var name = null;
    if ($tr == null)
        name = $dialog.find('h3[id="name"]').text();
    else
        name = $tr.find('.name').text();
    var dept = $('#ddlDepartmentRoster').val();
    var loc = $('#ddlLocationRoster').val();
    var shift = $('#ddlShiftRoster').val();
    var office = $('#office').val();
    var client = $('#ddlClientRoster').val();

    /* add id to roster - new record */
    //alert("Place in Database: " + id + ", " + client + ", " + dept + ", " + office + ", " + shift + ", " + startDate + ", " + endDate);
    var cr_ID = placeInDB(id, client, dept, loc, office, shift, startDate, endDate);
    var $newRow;
    /* add to roster */
    if (cr_ID > 0) {
        var idx = $('#clientRoster tbody tr').length + 1;
        var clss = 'class="rosterRow"';
        if (idx % 2 == 1)
            clss = 'class="rosterRow odd"';
        //alert("Interval = " + days + ", " + $('#trackStart').text() + ", " + $('#trackEnd').text()  + ", " + endDate);
        var rw = '<tr ' + clss + ' cr_id="' + cr_ID + '">' +
            '<td>' + idx + '.</span></td>' +
            '<td>' + "T" + $("#office").val() + " (" + offices[$('#office').val()] + ")" + '</td>' +
            '<td><span class="aident">' + id + '</span></td>' +
            '<td><span class="name">' + name + '</span></td>';
        if (client == 8) {
            rw += "<td><input type='text' id='sub_" + cr_ID + "' value='0' size='6' ></td>";
        }
        rw += '<td><input class="inpDate startDt" type="text" value="' + startDate + '" size="8"></td>' +
            '<td>thru</td>' +
            '<td><input class="inpDate endDt" type="text" value="' + endDate + '" size="8"></td>' +
            '<td>S: ' + dropDownDateTime($('#shiftStart').text()) + '<br/>E: ' + dropDownDateTime($('#shiftEnd').text()) + '</td>' +
            '<td><input class="smallButton" type="button" value="REM"></td>' +
            '</tr>';

        $('#clientRoster tbody').append(rw);
        $newRow = $("tr[cr_id='" + cr_ID + "']");
        //alert( $newRow.length );
        $newRow.find('.inpDate').datepicker();
        $newRow.hover(
            function () {
                //var myClass = $(this).attr("class");
                $(this).addClass("highLight");
            }, function () {
                $(this).removeClass("highLight");
        });
        $newRow.find('.inpDate').change(function () {
            $('#updRosters').removeAttr('disabled').click(function () {
                updateRosters();
            });
            $(this).css('background-color', '#FFCCBB');
            $(this).closest('tr').attr('update', 'true');
        });
        $newRow.find('.smallButton').click(function () {
                deleteLine($(this).closest('tr').attr("cr_id"));
        });

        $newRow.find('.rosterAdd').attr('disabled', 'disabled');
        var divId = "div" + id;
        $("<div id='" + divId + "'><h1>Added " + name + " to roster.</h1></div>").dialog();
        setTimeout(function () {
            $('#' + divId).remove();
        }, 1500);
    }
    else {
        $("<div id='" + divId + "'><h1>Couldn't add " + name + " to roster.</h1><p>Maybe multiple AIDENT numbers.</p><p>Give Jonathan a call.</p><p>(773) 354 - 2056</p></div>").dialog();
    }
    if (override != null) {
        var src = $dialog.find("tr[id='shiftStartEnd']").find('td').find('select[class="inpDate"] :selected');
        var dst = $newRow.find("select");
        //alert($(dst.get(0)).val() + ", " + $(src.get(0)).val());
        $(dst.get(0)).val($(src.get(0)).val());
        $(dst.get(1)).val($(src.get(1)).val());
        $(dst.get(2)).val($(src.get(2)).val());
        $(dst.get(3)).val($(src.get(3)).val());
        $(dst.get(4)).val($(src.get(4)).val());
        $(dst.get(5)).val($(src.get(5)).val());
        $newRow.attr("update", "true");
        updateLine($newRow);
    }
}

function trimRoster($tr, retPE) {
    $('<div id="dialog-trim" title="Punch data exists!"><p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Roster can\'t be deleted because of existing punches.Trim dates of roster to punches?</p></div>'
    ).dialog({
        resizable: false,
        height: 240,
        modal: false,
        buttons: {
            "Trim": function () {
                var lp = new Date(retPE);
                var dt = lp.getFullYear() + "-" + (lp.getMonth() + 1) + "-" + lp.getDate() + " " + lp.getHours() + ":" + lp.getMinutes();
                Url = RosterBaseURL + "Trim/" + $tr.attr("cr_id");
                $.ajax({
                    cache: false,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    url: Url,
                    success: function (data) {
                        $tr.find('.startDt').val(data.substring(5, 7) + '/' + data.substring(8, 10) + '/' + data.substring(0, 4));
                        $tr.find('.endDt').val(data.substring(15, 17) + '/' + data.substring(18, 20) + '/' + data.substring(10, 14));
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });

                //alert("Update to " + dt.toString() + ", Time: tracking end of ClientShiftLocation");
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
    }

    function deleteLine(id) {
        var $tr = $('tr[cr_id=' + id + ']');
        var name = $tr.find('.name').text();
        var retPE = "";
        $('<div id="dialog-confirm" title="Delete Roster Row?"><p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>' + 
        'Permanently delete roster record for ' + name + '?</p></div>'
         ).dialog({
             resizable: false,
             height: 240,
             modal: false,
             buttons: {
                 "Delete": function () {
                     $(this).dialog("close");
                     retPE = punchesExist(id);
                     if (retPE.length == 0) {
                         removeFromRoster($tr);
                     }
                     else {
                         trimRoster($tr, retPE);
                     }
                 },
                 Cancel: function () {
                     $(this).dialog("close");
                 }
             }
         });
     }
     function punchesExist(crId) {
         var success = false;
         Url = RosterBaseURL + "Punches/" + crId;
         $.ajax({
             cache: false,
             type: "GET",
             contentType: "application/json; charset=utf-8",
             async: false,
             dataType: "json",
             url: Url,
             success: function (data) {
                 success = data;
                 //alert("1 - " + success);
             },
             error: function (xhr) {
                 alert("ERROR!!!" + xhr.responseText);
             }
         });
         return success;
     }


function getRosters(id, start, end) {
    var rosters = null;
    Url = RosterBaseURL + id + '/EmployeeRosters?startDate=' + start + '&endDate=' + start + '&clientID=' + $('#ddlClientRoster').val();
    waitIndicator(true, "Getting Rosters");
    alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (dta) {
            if (dta != null && dta.length > 0) {
                data = dta; //global :(
                rosters = "<table id='rosterOverlapTable'><thead><tr><td>Client</td><td>Start Date</td><td>End Date</td><td>Possible Hrs</td></tr></thead><tbody>";
                for (var i = 0; i < data.length; i++) {
                    data[i].StartDate = getDate(data[i].StartDate);
                    data[i].EndDate = getDate(data[i].EndDate);
                    rosters += "<tr><td>" + data[i].ClientName + "</td><td>" + formatDate(data[i].StartDate) + "</td><td>" + formatDate(data[i].EndDate) + "</td><td id='clientHours" + i + "'>--.--</td></tr>";
                }
                rosters += "<tr><td colspan='3'>Approx. Hours</td><td id='totalHours'>--.--</td></tr>";
                rosters += "<tr><td>Pay Period</td><td><input id='startRoster' type='text' size='10'/></td><td><input id='endRoster' type='text' size='10'/></td><td id='totHours'></td></tr>";
                rosters += "</tbody></table>";
            }
            waitIndicator(false);
        },
        error: function (msg) {
        }
    });
    //alert("End of get Rosters: " + data);
    return rosters;
}

function dnrCheck(id, client, shift, dept, start, end, loc) {
    Url = ClientBaseURL + id + "/GetDnr?clientID=" + client + "&shiftID=" + shift + "&deptID=" + dept +
                "&start=" + start + "&end=" + end + "&locationID=" + loc;
    var success = "";
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //jsonArray = msg;
            if (msg[0].AidentNumber == null)
                success = "";
            else
                success = "Employee has been DNR'd: <br/><b>" + msg[0].DNRReason + "</b>";
        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
        }
    });
    return success;
}

function overrideDaysFunc(overrideOT, id, startDate, endDate, conflictDaysMsg, conflictOTMsg, $tr) {
    $("<div id='workConflict' title='Allowed Days exceeded! '>" + conflictDaysMsg + "</div>").dialog({
        close: function (event, ui) { $('#workConflict').remove(); },
        resizable: false,
        height: 320,
        width: 400,
        modal: false,
        buttons: {
            "Add": function () {
                $('#workConflict').remove();
                if (overrideOT)
                    overrideOTFunc(id, startDate, endDate, conflictOTMsg);
                else
                    addToRosterAction(id, startDate, endDate, $tr, $dialog, true);
            },
            "Don't Add": function () {
                $('#workConflict').remove();
            }
        }
    });
}

function getDaysWorked(clientID, id) {
    var retVal = "";
    var endDate = this.formatDate(new Date());
    var startDate = new Date();  //this.formatDate(new Date(2014, 09, 1));
    if (startDate.getMonth() < 8)
        startDate = this.formatDate(new Date(parseInt(startDate.getFullYear())-1, 08, 1));
    else {
        var yr = parseInt(startDate.getFullYear());
        startDate = this.formatDate(new Date(yr, 08, 1));
    }
    //alert(endDate);
    Url = RosterBaseURL + "DWGetDaysWorked?id=" + id + "&clientID=" + clientID + "&startDate=" + startDate +
                    "&endDate=" + endDate;
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var dwr = eval(msg);
            //alert("DWR = " + dwr.toString() + ", --- " + dwr.TotalDaysWorked);
            if (dwr == null || dwr.TotalDaysWorked < 76)
                retVal = "";
            else {
                //alert("Too many days!!!!");
                retVal = "<b>Warning - This employee has worked " + dwr.TotalDaysWorked + " days</b><br/><b>Add to roster anyway?</b>";
            }
        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
            //return 0;
        }
    });
    return retVal;
}


function conflicts(id, startDate, endDate, $tr, $dialog) {
    var name;
    if ($tr == null)
        name = $dialog.find('h3[id="name"]').text();
    else
        name = $tr.find('.name').text();
    //alert(startDate + ', ' + endDate);
    var overrideOT = false;   // possibility to override conflict
    var overrideDays = false;   // possibility to override conflict

    var conflict = false;
    var conflictMsg = "";

    /* is there a table? */
    if ($('#clientRoster tbody').length <= 0) {
        conflictMsg = "First Select the roster table!";
        conflict = true;
    }

    /* is ID already on the table? */
    if (!conflict) {
        $('#clientRoster tr td').find('.aident').each(function () {
            if ($(this).text() == id) {
                conflictMsg = "Employee already on roster!";
                conflict = true;
            }
        });
    }

    /* is Dispatch office selected? */
    if (!conflict && $('#office').val() == 0) {
        conflictMsg = "No Dispatch Office Selected!";
        conflict = true;
    }
    //return true;

    /* is employee dnr'd at client or universally? */
    if (!conflict) {
        conflictMsg = dnrCheck(id, $('#ddlClientRoster').val(), $('#ddlShiftRoster').val(), $('#ddlDepartmentRoster').val(), startDate, endDate, $('#ddlLocationRoster').val());
        //conflictMsg = "Employee has been DNR'd!";
        //alert(conflictMsg.length + ", " + conflict);
        if (conflictMsg.length > 0)
            conflict = true;
    }

    /* is Employee already scheduled at Client? */
    if (!conflict) {
        var startDateTime = startDate + " " + $('#shiftStart').text(); //getCurrentDate() + " " + $('#shiftStart').text();
        var endDateTime = endDate + " " + $('#shiftEnd').text();

        //alert("Adding! " + startDate + ", " + endDate + ", " + getCurrentDate() + " " + $('#shiftStart').text());
        Url = ClientBaseURL + $("#ddlClientRoster").val() +
                    "/RostersAt?aident=" + id + "&startDate=" +
                    startDateTime + "&endDate=" + endDateTime;
        var msg;
        $.ajax({
            cache: false,
            type: "GET",
            url: Url,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.length > 0) {
                    conflict = true;
                    conflictMsg = '<p>' + name + " is already on a roster at the same client!</p><p>" + msg + '</p>';
                }
            },
            error: function (msg) {
                alert("an unexpected error has occurred! " + msg.d);
            }
        });
    }

    var conflictOTMsg = null;
    var conflictDaysMsg = null;

    if (!conflict) {    /* is employee already working 40 hours? */
        //alert("Start Date = " + startDate + ", End Date = " + endDate);
        conflictOTMsg = getRosters(id, startDate, endDate);
        if (conflictOTMsg != null) {
            conflict = true;
            overrideOT = true;
        }
    }

    if (!conflict || overrideOT == true) { /* is employee at Weber and exceeds the number of days allowed? */
        //alert("Going to check Days at weber ");
        if ($('#ddlClientRoster').val() == 92 || $('#ddlClientRoster').val() == 181) {
            conflictDaysMsg = getDaysWorked($('#ddlClientRoster').val(), id);
            if (conflictDaysMsg.length > 0) {
                //alert("There were too many days...");
                conflict = true;
                overrideDays = true;
            }
        }
    }

    if (conflict) {
        if (overrideDays) {
            //alert("first the days...");
            overrideDaysFunc(overrideOT, id, startDate, endDate, conflictDaysMsg, conflictOTMsg, $tr, $dialog);
        }
        else if (overrideOT) {
            overrideOTFunc(id, startDate, endDate, conflictOTMsg, $tr, $dialog);
        }
        else {
            $("<div>" + conflictMsg + "</div>").dialog();
        }
    }
    //alert(conflict);
    return conflict;
}
function overrideOTFunc(id, startDate, endDate, conflictOTMsg, $tr, $dialog) {
    $("<div id='workConflict' title='Employee Already Working!'>" + conflictOTMsg + "</div>").dialog({
        close: function (event, ui) { $('#workConflict').remove(); },
        resizable: false,
        height: 320,
        width: 400,
        modal: false,
        buttons: {
            "Add": function () {
                addToRosterAction(id, startDate, endDate, $tr, $dialog, true);
                $('#workConflict').remove();
            },
            "Don't Add": function () {
                $('#workConflict').remove();
            }
        }
    });
    setPayPeriod('startRoster', 'endRoster');
    $('#startRoster').datepicker();
    $('#endRoster').datepicker();
    calcHours($('#startRoster').val(), $('#endRoster').val());
}
function overrideDaysFunc(overrideOT, id, startDate, endDate, conflictDaysMsg, conflictOTMsg, $tr, $dialog) {
    $("<div id='workConflict' title='Allowed Days exceeded! '>" + conflictDaysMsg + "</div>").dialog({
        close: function (event, ui) { $('#workConflict').remove(); },
        resizable: false,
        height: 320,
        width: 400,
        modal: false,
        buttons: {
            "Add": function () {
                $('#workConflict').remove();
                if (overrideOT)
                    overrideOTFunc(id, startDate, endDate, conflictOTMsg, $tr, $dialog);
                else
                    addToRosterAction(id, startDate, endDate, $tr, $dialog);
            },
            "Don't Add": function () {
                $('#workConflict').remove();
            }
        }
    });
}

function removeFromRoster($tr) {
    /* get start and end times */
    var id = $tr.attr('cr_id');
    var start = new Date($tr.find("td input[class*='start']").val());
    var end = new Date($tr.find("td input[class*='end']").val());

    //alert(start + ", " + end);
    var sel = $tr.find('td select[class="inpDate"]');
    //alert(sel.length);
    h = sel.eq(0).val();
    m = sel.eq(1).val();
    if (sel.eq(2).val() == '12') {
        if (parseInt(h, 10) < 12)
            h = parseInt(h, 10) + 12;
    }
    else {
        if (parseInt(h, 10) == 12)
            h = '00';
    }
    startTime = h + ":" + m;
    h = sel.eq(3).val();
    m = sel.eq(4).val();
    if (sel.eq(5).val() == '12') {
        if (parseInt(h, 10) < 12)
            h = parseInt(h, 10) + 12;
    }
    else {
        if (parseInt(h, 10) == 12)
            h = '00';
    }
    endTime = h + ":" + m;
    //alert(startTime + ", " + endTime);

    //alert(1800 + (end.getFullYear() % 100));
    //alert(1800 + (start.getFullYear() % 100));

    end.setFullYear(1800 + (end.getFullYear() % 100), end.getMonth(), end.getDate());
    start.setFullYear(1800 + (start.getFullYear() % 100), start.getMonth(), start.getDate());
    /* hide row */
    $tr.hide('fast', function () {
        /* first remove from database */
        Url = RosterBaseURL + "Remove/" + id; /* + "?startDate=" + start.getFullYear() + "-" + (start.getMonth() + 1) + "-" + start.getDate() +
                     "&endDate=" + end.getFullYear() + "-" + (end.getMonth() + 1) + "-" + end.getDate() +
                     "&startTime=" + startTime + "&endTime=" + endTime +
                     "&trackStart=" + startTime + "&trackEnd=" + endTime;*/
        //alert(Url);
        $.ajax({
            cache: false,
            type: "GET",
            url: Url,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //alert("Success!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                //resetCount();
                /* then remove when done */
                $tr.remove();
                //$('#clientRoster tr').removeClass("odd");
                $('#clientRoster tr').each(function (i) {
                    $(this).removeClass("odd");
                    if( i % 2 == 1 )
                        $(this).addClass("odd");
                });
            },
            error: function (msg) {
                alert("an unexpected error has occurred! " + msg.d);
                $tr.show();
            }
        });
    });
}
