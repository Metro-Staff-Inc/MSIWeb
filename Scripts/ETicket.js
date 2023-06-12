
var ClientBaseURL = "http://localhost:52293/RestServiceWWW/Client/"; //
var RosterBaseURL = "http://localhost:52293/RestServiceWWW/Roster/"; //
ClientBaseURL = "../Client/";
RosterBaseURL = "../Roster/";

var eTicket = null;
var offices = null;
var officesOnTicket = null;
var locations = null;
var addr1 = null;
var addr2 = null;
var city = null;
var state = null;
var state = null;
var zip = null;
var phoneArea = null;
var phonePrefix = null;
var phoneLast4 = null;
var officeNums = null;

var userID = $("input[id*='userID']").val();// document.getElementById("ctlSubHeader_userID").value;
var clientID = $("input[id*='clientID']").val();// document.getElementById("ctlSubHeader_clientID").value;
var weekEnd = $("input[id*='weekEnd']").val();// document.getElementById("ctlSubHeader_weekEnd").value;

function ArrayIndexOf(a, fnc) {
    if (!fnc || typeof (fnc) != 'function') {
        return -1;
    }
    if (!a || !a.length || a.length < 1) return -1;
    for (var i = 0; i < a.length; i++) {
        if (fnc(a[i])) return i;
    }
    return -1;
}

function getOfficeNumbers() {
    var phoneNums = new Array();
    phoneNums.push('Dispatch Phone Numbers:<br/>');
    officesOnTicket = new Array();
    for (var i = 0; i < eTicket.Employees.length; i++) {
        var o = eTicket.Employees[i].BadgeNumber.substring(1, 2);
        if (officesOnTicket.indexOf(o) < 0 )
            officesOnTicket.push(o);
    }
    for (var j = 0; j < officesOnTicket.length; j++) {
        var i = ArrayIndexOf(offices, function (obj) {
            return obj.OfficeCode == officesOnTicket[j];
        });
        if (-1 != i) {
            phoneNums.push('T' + offices[i].OfficeCode + ': (' + offices[i].PhoneArea + ') ' +    
                            offices[i].PhonePrefix + '-' + offices[i].PhoneLast4 + '<br/>');
        }
    }
    return phoneNums;
}

function val(num) {
    if (isNaN(num))
        return num;
    return parseFloat(Math.round(num * 100) / 100).toFixed(2);
}

function getDateFromString(dt) {
    var firstSlash = dt.indexOf("/");
    var secSlash = dt.indexOf("/", firstSlash + 1);
    dt = new Date(dt.substring(secSlash + 1), dt.substring(0, firstSlash) - 1, dt.substring(firstSlash + 1, secSlash));
    return dt;
}

function getDate(dt) {
    return new Date(parseInt(dt.replace("/Date(", "").replace(")/", ""), 10));
}

function formatDate(dt) {
    return "" + (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function waitIndicator(on, titleBar) {
    $('#waiting').remove();
    if (on == true) {
        if (titleBar == null)
            titleBar = "loading";
        $("<div id='waiting'><h3 style='padding-left:88px'>please wait...</h3><img style='padding-left:88px' src='../Images/ajax-loader.gif'/></div>").dialog(
        { open: function (event, ui) {
            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
        },
            title: titleBar
        });
    }
    else {
        $('#waiting').remove();
    }
}

function printETicketMenu() {
    if ($('input[update="true"]').length > 0) {
        alert("Can't print - hours have been modified.  Please update first.");
        return;
    }
    $("<div>" +
        "<input id='printRoster' type='button' value='PRINT ROSTER' onclick='printRoster()'/><br/>" +
    "</div>").dialog({
        title: "PRINT CURRENT ROSTER",
        buttons: {
            "CLOSE": function () {
                //$(this).dialog("close");
                $(this).remove();
            }
        }
    });
}

function setHeader(w) {
    var offIdx = 0;
    officeNums = getOfficeNumbers();
    var client = $("select[id*='cboClient']").find("option[value='" + clientID + "']").text();
    var location = $("#ddlLocation option:selected").text();
    var weekEnd = $("#ddlWeekEnding option:selected").text();

    var d = w.document;
    d.write("<HTML>"); 
  d.write('<HEAD>');
    d.write('<META http-equiv="Content-Type" content="text/html; charset=utf-8">');
    d.write('<title>Work Ticket</title>');
    d.write('<style>table{font-family:"Trebuchet MS", Arial, Helvetica, sans-serif;width:100%;border-collapse:collapse;}' +
        '.outline tbody td,th{font-size:1em;border:1px solid #989898;padding:3px 7px 2px 7px;} ' +
        '.outline tfoot td,th{font-size:1em;border:1px solid #989898;padding:3px 7px 2px 7px;} ' +
        '.outline tbody th {font-size:1.1em;text-align:left;padding-top:5px;padding-bottom:4px;background-color:#A7A7A7;color:#ffffff;}' +
        '.outline tfoot th {font-size:1.1em;text-align:left;padding-top:5px;padding-bottom:4px;background-color:#A7A7A7;color:#ffffff;}' +
        '.legal {font-size:0.9em;text-align:left;padding-top:10px;padding-bottom:10px;padding-left:24px;padding-right:24px;}' +
    '</style>');

  d.write('</HEAD>');
  d.write('<body bgcolor="#ffffff">');
    d.write('<table width="100%" border="0" cellpadding="1" cellspacing="0" bordercolor="#cccccc">');
      d.write('<tr>');
        d.write('<td align="left" width="40%">');
          d.write('<table width="100%" border="0" cellpadding="1" cellspacing="0" bordercolor="#cccccc">');
            d.write('<tr STYLE="HEIGHT:4px">');
              d.write('<td align="left" bgcolor="#ffffff" width="60">');
                d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">Date:</font>');
              d.write('</td>');
              d.write('<td align="left" bgcolor="#ffffff">');
                    d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">' + weekEnd + '</font></td>');
            d.write('</tr>');
            d.write('<tr STYLE="HEIGHT:4px">');
              d.write('<td align="left" bgcolor="#ffffff" width="60">');
                d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">Customer:</font>');
              d.write('</td>');
              d.write('<td align="left" bgcolor="#ffffff">');
			    d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">' + client  + '</font></td>');
                d.write('</tr>');
                d.write('<tr STYLE="HEIGHT:4px">');
                d.write('<td align="left" bgcolor="#ffffff" width="60">');
                d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">Address:</font>');
              d.write('</td>');
              d.write('<td align="left" bgcolor="#ffffff">');
	            d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">' + addr1 + '</font></td>');
	            d.write('</tr>');

            d.write('<tr STYLE="HEIGHT:4px">');
              d.write('<td align="left" bgcolor="#ffffff" width="60"></td>');
              d.write('<td align="left" bgcolor="#ffffff">');
			    d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">' + city + ', ' + state + ' ' + zip + '</font></td>');
            d.write('</tr>');
          d.write('</table>');
        d.write('</td>');
        d.write('<td align="left" width="30%">');
          d.write('<span style="width:10px;"> </span>');
          d.write('<img src="http://www.msiwebtrax.com/Images/app_logo.jpg">');
          d.write('<br>');
          d.write('<span style="width:21px;"> </span>');
        d.write('</td>');
        d.write('<td align="left" width="30%">');
          d.write('<table width="100%" border="0" cellpadding="0" cellspacing="0" bordercolor="#cccccc">');
            d.write('<tr STYLE="height:4px">');
              d.write('<td align="left" bgcolor="#ffffff" valign="bottom">');
                d.write('<table width="100%" border="0" cellpadding="0" cellspacing="0" bordercolor="#cccccc">');

                d.write('<tr STYLE="HEIGHT:4px">');
                d.write('<td align="left" bgcolor="#ffffff">');
                d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">');
                for (var j = 0; j < officeNums.length; j++) {
                    d.write(officeNums[j]);
                }
                d.write('</font>');
                d.write('</td>');
                d.write('</tr>');

                d.write('</table>');
              d.write('</td>');
            d.write('</tr>');
            d.write('<tr STYLE="HEIGHT:4px">');
              d.write('<td colspan="2" align="left" bgcolor="#ffffff"></td>');
            d.write('</tr>');
          d.write('</table>');
        d.write('</td>');
      d.write('</tr>');
      d.write('<tr>');
        d.write('<td align="left" colspan="2"> </td>');
      d.write('</tr>');
    d.write('</table>');
    d.write('<table width="100%" border="1" cellpadding="1" cellspacing="0" bordercolor="#cccccc">');
      d.write('<tr STYLE="HEIGHT:4px">');
        d.write('<td align="left" bgcolor="#ffffff" width="45%" valign="top">');
        d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">' + $('#ddlShiftType option:selected').text() + ', ' + eTicket.DefaultStart + " to " + eTicket.DefaultEnd + '</font>');
        d.write('</td>');
        d.write('<td align="left" bgcolor="#ffffff" width="55%" valign="top">');
          d.write('<font style="FONT-WEIGHT: bold;FONT-SIZE: 14px;COLOR: #36488a;FONT-FAMILY: Arial">REPORT TO: ' + eTicket.Supervisor + '</font>');
        d.write('</td>');
      d.write('</tr>');
    d.write('</table>');
}

function setFooter(d) {
    d.write('<br/><table width="100%" border="0" cellpadding="3" cellspacing="0" bordercolor="#cccccc" ID="Table2">' + 
      '<tr STYLE="HEIGHT:4px">' + 
        '<td align="center" bgcolor="#ffffff" width="100%" colspan="2">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">' + 
								'Terms and Conditions<br></font>' + 
        '</td>' + 
      '</tr>' + 
      '<tr STYLE="HEIGHT:4px">' + 
        '<td align="left" bgcolor="#ffffff" width="30" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">1)</font>' + 
        '</td>' + 
        '<td align="left" bgcolor="#ffffff" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">CUSTOMER shall not entrust MSI Temporary Employees with valuables including but not limited to cash, securities, motor vehicles, tools or any CUSTOMER personal property without written consent from a MSI representative. CUSTOMER has assumed all personal liability for any of the above listed.</font>' + 
        '</td>' + 
      '</tr>' + 
      '<tr STYLE="HEIGHT:4px">' + 
        '<td align="left" bgcolor="#ffffff" width="30" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">2)</font>' + 
        '</td>' + 
        '<td align="left" bgcolor="#ffffff" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">CUSTOMER shall not use any MSI Temporary Employee to operate machinery, equipment or vehicles without written consent from a MSI Representative. CUSTOMER accepts all liability for any damage caused by any MSI Temporary Employee for using any of the above listed equipment.</font>' + 
        '</td>' + 
      '</tr>' + 
      '<tr STYLE="HEIGHT:4px">' + 
        '<td align="left" bgcolor="#ffffff" width="30" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">3)</font>' + 
        '</td>' + 
        '<td align="left" bgcolor="#ffffff" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">CUSTOMER will not ask or engage any MSI Temporary Employee to operate any dangerous machinery, work above ground level or on ladders. CUSTOMER will assume full Insurance &amp; Personal coverage if any of the above listed is allowed by CUSTOMER.</font>' + 
        '</td>' + 
      '</tr>' + 
      '<tr STYLE="HEIGHT:4px">' + 
        '<td align="left" bgcolor="#ffffff" width="30" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">4)</font>' + 
        '</td>' + 
        '<td align="left" bgcolor="#ffffff" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">CUSTOMER will indemnify and hold harmless MSI from any and all liabilities, demands or claims arising out of this contract.</font>' + 
        '</td>' + 
      '</tr>' + 
      '<tr STYLE="HEIGHT:4px">' + 
        '<td align="left" bgcolor="#ffffff" width="30" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">5)</font>' + 
        '</td>' + 
        '<td align="left" bgcolor="#ffffff" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">CUSTOMER shall pay to MSI the total amount due upon agreed payment terms. CUSTOMER will pay interest of 1 ½% per month (18% annual rate) on account balances more than 10 days past agreed upon payment terms. If MSI is forced to seek collection by using an Attorney for collection purposes the CUSTOMER agrees to pay all Legal Fees incurred by MSI.</font>' + 
        '</td>' + 
      '</tr>' + 
      '<tr STYLE="HEIGHT:4px">' + 
        '<td align="left" bgcolor="#ffffff" width="30" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">6)</font>' + 
        '</td>' + 
        '<td align="left" bgcolor="#ffffff" valign="top">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">CUSTOMER will not hire any MSI Temporary Employee prior to working 60 days (480 hours) at CUSTOMER. If CUSTOMER seeks to hire prior to 60 working days (480 hours) CUSTOMER will pay a 15% fee. This fee is based upon 15% of the Temporary Workers annual wages.</font>' + 
        '</td>' + 
      '</tr>' + 
    '</table>' + 
    '<table width="100%" border="0" cellpadding="0" cellspacing="0" bordercolor="#cccccc" ID="Table2">' + 
      '<tr>' + 
        '<td align="left" width="250"> </td>' + 
      '</tr>' + 
      '<tr>' + 
        '<td align="left" width="250"> </td>' + 
      '</tr>' + 
      '<tr>' + 
        '<td align="left" width="250">' + 
          '<font style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">_______________________________________</font>' + 
        '</td>' + 
      '</tr>' + 
      '<tr>' + 
        '<td align="left" width="250"> </td>' + 
      '</tr>' + 
      '<tr>' + 
        '<td align="left" width="250">' + 
          '<font align="left" style="FONT-WEIGHT: bold;FONT-SIZE: 10px;COLOR: #36488a;FONT-FAMILY: Arial">Authorized Signature</font>' + 
        '</td>' + 
      '</tr>' + 
    '</table>');
}

function printRoster() {
    $(".ui-dialog").remove();
    var curLoc = $("#ddlLocation option:selected").text();

    var notes = "";
    var mywindow = window.open('', 'ETicket Roster', 'height=600,width=800');
    setHeader(mywindow);
    var i = 1;
    mywindow.document.write("<table class='outline'>");
    mywindow.document.write("<thead><tr>");
    mywindow.document.write("<th><span>#</span></th><th><span>BADGE</span></th><th><span>NAME</span></th>");
    mywindow.document.write("<th><span>MON</span></th><th><span>TUE</span></th><th><span>WED</span></th>");
    mywindow.document.write("<th><span>THR</span></th><th><span>FRI</span></th><th><span>SAT</span></th><th><span>SUN</span></th>");
    mywindow.document.write("<th><span>TOT</span></th><th><span>RETURN</span></tot>");
    mywindow.document.write("</tr></thead>");
    mywindow.document.write("<tbody>");

    var $rows = $('#eTicket tbody tr:even');
    $rows.each(function () {
        var name = $(this).find(".name").text();
        if (name.indexOf("(note)") > -1)
            name = name.substring(0, name.length - 6);
        mywindow.document.write("<tr>");
        mywindow.document.write("<td><span>" + (i++) + ".</span></td><td><span>" + $(this).find(".aident").text() + "</td></span><td><span>" +
        name + "</td></span>");
        mywindow.document.write("<td><span>" + $(this).find(".day1").val() + "</span</td>");
        mywindow.document.write("<td><span>" + $(this).find(".day2").val() + "</span</td>");
        mywindow.document.write("<td><span>" + $(this).find(".day3").val() + "</span</td>");
        mywindow.document.write("<td><span>" + $(this).find(".day4").val() + "</span</td>");
        mywindow.document.write("<td><span>" + $(this).find(".day5").val() + "</span</td>");
        mywindow.document.write("<td><span>" + $(this).find(".day6").val() + "</span</td>");
        mywindow.document.write("<td><span>" + $(this).find(".day7").val() + "</span</td>");
        mywindow.document.write("<td><span>" + $(this).find(".totals").val() + "</span</td>");
        mywindow.document.write("<td><span>" + " " + "</span</td>");
        mywindow.document.write("</tr>");
        var note = $(this).next('tr').find('textarea').val();
        if (note.length > 0) {
            notes += "<p>" + $(this).find(".aident").text() + ", " + name + " - " + note + "</p>";
        }
    });
    //    mywindow.document.write("<span style='padding:12px 16px'>Totals:</span>");
    mywindow.document.write("</tbody>");

    $rows = $('#eTicket tfoot tr');
    $rows = $rows.find('.dayTotal').find('input');
    mywindow.document.write("<tfoot class='outline'><tr>");
    mywindow.document.write("<td colspan='3'><span>TOTALS:</span></td>");
    $rows.each(function () {
        mywindow.document.write("<td><span>" + $(this).val() + "</span></td>");
    });
    mywindow.document.write("<td><span>" + " " + "</span></td>");
    mywindow.document.write("</tr></tfoot>");
    mywindow.document.write("</table>");

    if (notes.length > 0) {
        mywindow.document.write("<br/><i>notes</i>" + notes);
    }
    setFooter(mywindow.document);

    mywindow.print();
    mywindow.close();

    return true;
}

function createETicket() {
    var locID = $("#ddlLocation").val();
    var shiftType = $("#ddlShiftType").val();
    var departmentID = $("#ddlDepartment").val();
    var weekEnding = $("#ddlWeekEnding").val();
    var Url = RosterBaseURL + "CreateETicket?clientID=" + clientID + "&locID=" + locID + "&shiftType=" + shiftType +
        "&deptID=" + departmentID + "&weekEnd=" + weekEnding;
    waitIndicator(true, "Creating ETicket..");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (retVal) {
            eTicket.EmployeeHoursHeaderId = retVal;
        },
        error: function (xhr) {
            alert("Error " + xhr);
        },
        complete: function (xhr, status) {
            waitIndicator(false);
        }
    });
}

function updateETicket($tr) {
    var employeeHoursHeaderId = eTicket.EmployeeHoursHeaderId;

    var $inp = $tr.find('*[update]');
    if ($inp.length == 0) return;
    if ($tr.find("input").length == 0)
        $tr = $tr.prev('tr');   // this is if only the NOTES section is changed 
    var h1 = $tr.find("input:eq(0)").val();
    var h2 = $tr.find("input:eq(1)").val();
    var h3 = $tr.find("input:eq(2)").val();
    var h4 = $tr.find("input:eq(3)").val();
    var h5 = $tr.find("input:eq(4)").val();
    var h6 = $tr.find("input:eq(5)").val();
    var h7 = $tr.find("input:eq(6)").val();
    var aident = $tr.find("td[class='aident'] span").text().substring(2);
    var notes = $tr.next('tr').find('textarea').val();

    //alert("ETicket ID = " + eTicket.EmployeeHoursHeaderId + ", Employee ID = " + aident + ", " + h1 + ", " + h2 + ", " + h3 + ", " + h4 + ", " + h5 + ", " + h6 + ", " + h7 + " Notes = " + notes + " created by = " + userID);
    $tr.find("input").removeAttr("update");
    var Url = RosterBaseURL + "UpdateETicket?employeeHoursHeaderId=" + eTicket.EmployeeHoursHeaderId + "&aidentNumber=" + aident +
    "&userName=" + userID + "&notes=" + notes + "&hours1=" + h1 + "&hours2=" + h2 + "&hours3=" + h3 + "&hours4=" + h4 + "&hours5=" + h5 +
    "&hours6=" + h6 + "&hours7=" + h7;
    waitIndicator(true, "Updating..");
    $.ajax({
        url: Url,
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (jsonArray) {
            $tr.find("input").removeClass("changed");
            $tr.next('tr').find('textarea').removeClass('changed');
        },
        error: function (xhr) {
            alert(xhr);
        },
        complete: function (xhr, status) {
            waitIndicator(false);
        }
    }); 
}

//alert("weekend = " + weekEnd);
function updateTable() {
    var eTicketID = eTicket.EmployeeHoursHeaderId;
    if (eTicketID == 0)
        createETicket();
    var $tr = $("#eTicket tbody tr");
    $('.dayTotal').find('input').removeClass("changed");
    $tr.each(function () {
        updateETicket($(this));
    });
}

function getETicket() {
    var $locationID = $('#ddlLocation').val();
    var $shiftType = $('#ddlShiftType').val();
    var $departmentID = $('#ddlDepartment').val();
    var $weekEnd = $('#ddlWeekEnding').val();
    var Url = RosterBaseURL + "ETicket?clientID=" + clientID + "&locID=" + $locationID + "&weekEnd=" + $weekEnd + "&shiftType=" + $shiftType +
                "&deptID=" + $departmentID;
    waitIndicator(true, "Retrieving..");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonArray) {
            //alert(jsonArray.Employees.length);
            var endDt = $("#ddlWeekEnding").children("option").filter(":selected").text();
            endDt = getDateFromString(endDt);
            var startDt = new Date(endDt.getFullYear(), endDt.getMonth(), endDt.getDate() - 6);
            //alert(endDt + ", " + startDt);
            eTicket = jsonArray;
            $('#eTicket tbody tr').remove();
            if (eTicket.Supervisor == null) eTicket.Supervisor = $("#ddlDepartment").children("option").filter(":selected").text();
            if (eTicket.Supervisor != null && eTicket.Supervisor.length > 8) eTicket.Supervisor = eTicket.Supervisor.substring(0, 8);
            $("#spanSupervisor").text(eTicket.Supervisor);
            var tempsOnly = $("input[name='temps']:checked").val() == "tempEmployees";
            var empNum = 0;
            var grandTotal = new Array();
            for (var i = 0; i < 8; i++)
                grandTotal[i] = 0;
            for (var i = 0; i < eTicket.Employees.length; i++) {
                for (var j = 0; j < eTicket.Employees[i].StartDate.length; j++) {
                    eTicket.Employees[i].StartDate[j] = getDate(jsonArray.Employees[i].StartDate[j]);
                    eTicket.Employees[i].EndDate[j] = getDate(jsonArray.Employees[i].EndDate[j]);
                }
                if (tempsOnly && !eTicket.Employees[i].Temp)
                    continue;

                var row = "<tr>";
                row += "<td><span>" + (empNum++ + 1) + ".</span></td>";
                var name = eTicket.Employees[i].LastName + ", " + eTicket.Employees[i].FirstName;
                if (eTicket.Employees[i].Notes.length > 0)
                    name += " (note)";
                row += "<td class='name'><span>" + name + "</span></td>";
                row += "<td class='aident'><span>" + eTicket.Employees[i].BadgeNumber + "</span><input type='hidden' id='payRate' value='" + eTicket.Employees[i].PayRate  + "'</td>";

                var shiftHour = eTicket.Employees[i].ShiftStart;
                var shiftMin = shiftHour.substring(shiftHour.indexOf(":") + 1);
                shiftHour = shiftHour.substring(0, shiftHour.indexOf(":"));
                var curDt = new Date(startDt.getFullYear(), startDt.getMonth(), startDt.getDate(), shiftHour, shiftMin, 0);
                /* fix end date - shift end */

                shiftHour = eTicket.Employees[i].ShiftEnd;
                shiftMin = shiftHour.substring(shiftHour.indexOf(":") + 1);
                shiftHour = shiftHour.substring(0, shiftHour.indexOf(":"));
                endDt = new Date(endDt.getFullYear(), endDt.getMonth(), endDt.getDate(), shiftHour, shiftMin, 0);
                var total = 0;
                for (var j = 0; j < 7; j++) {
                    var disabled = true;
                    for (var k = 0; k < eTicket.Employees[i].StartDate.length; k++) {
                        if (curDt >= eTicket.Employees[i].StartDate[k] && curDt <= eTicket.Employees[i].EndDate[k])
                            disabled = false;
                    }
                    if (disabled) {
                        row += "<td><input ";
                        row += " class='day" + (j + 1) + "' value='" + val(eTicket.Employees[i].DailyHours[j]) + "' style='background-color:#dddddd; border:0;' disabled='disabled' type='text' size='4'/>" + "</td>";
                    }
                    else {
                        row += "<td><input ";
                        row += " class='day" + (j + 1) + "' value='" + val(eTicket.Employees[i].DailyHours[j]) + "' type='text' size='4'/>" + "</td>";
                    }
                    curDt = new Date(curDt.getFullYear(), curDt.getMonth(), curDt.getDate() + 1, curDt.getHours(), curDt.getMinutes(), 0);
                    total += parseFloat(eTicket.Employees[i].DailyHours[j]);
                    grandTotal[j] += parseFloat(eTicket.Employees[i].DailyHours[j]);
                }
                row += "<td>" + "<input disabled='disabled' type='text' size='4' class='totals' value='" + val(total) + "'/>" + "</td>";
                row += "</tr>";
                row += "<tr style='display:none;'> + <td colspan='10'><textarea rows='6' cols='100'>" + eTicket.Employees[i].Notes + "</textarea></td></tr>";
                $("#eTicket tbody").append(row);
                grandTotal[7] += total;
            }

            $('.dayTotal').find('input').each(function (i) {
                $(this).val(val(grandTotal[i]));
            });

            $(".name").click(function () {
                $(this).closest('tr').next('tr').toggle();
            });

            $(".name").hover(function () {
                $(this).find('span').toggleClass('underline');
            });


            $("input[class*='day']").click(function () {
                this.select();
            });
            $("input[class*='day'], textarea").change(function () {
                $(this).addClass("changed");
                $(this).attr('update', 'true');
                updateTotals(this);
            });
            $("#eTicket").find('tbody').find('tr').hover(function () {
                $(this).hover(
                function () {
                    $(this).addClass("highLight");
                }, function () {
                    $(this).removeClass("highLight");
                });
            });
            $("#eTicket").show();
        },
        error: function (xhr) {
            alert("An error has occurred!");
        },
        complete: function (xhr, status) {
            waitIndicator(false);
        }
    });
}

function updateTotals(inp) {
    $(inp).val(val($(inp).val()));
    var tr = $(inp).closest('tr');
    var inps = tr.find('input[class*="day"]');
    var totCol = tr.find('input[class*="total"]');
    var total = parseFloat("0");
    for (var i = 0; i < inps.length - 1; i++) {
        total += parseFloat($(inps[i]).val());
    }
    totCol.val(val(total));
    totCol.addClass('changed');

    var idx = $(inp).closest('td').index() + 1;
    var cols = $(inp).closest('table').find('tr').find('td:nth-child(' + idx + ')');
    idx -= 2;
    var tf = $(inp).closest('table').find('tfoot').find('tr').find('td:nth-child(' + idx + ')');
    //alert( "Total of changed column = " + $(tf).find('input').val() );
    var grandTotal = 0;
    $(cols).each(function (i) {
        if( i < $(cols).length - 1 ) {
            grandTotal += parseFloat($(this).find('input').val());
        }
    });
    //alert($(tf).find('input').val() + ", " + tf.find('input').val());
    $(tf).find('input').val(val(grandTotal));
    $(tf).find('input').addClass('changed');
    /* set bottom right table grand total */
    total = 0;
    $(inp).closest('table').find('tfoot').find('tr').find('input').each(function (i) {
        if (i < 7)
            total += parseFloat($(this).val());
        else {
            $(this).val(val(total));
            $(this).addClass("changed");
        }
    });
}

function getDepartments() {
    var $locationID = $('#ddlLocation').val();
    var $shiftType = $('#ddlShiftType').val();

    var Url = ClientBaseURL + clientID + "/GetDepartments?locationID=" + $locationID + "&shiftType=" + $shiftType;
    waitIndicator(true, "Getting Departments..");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonArray) {
            var opt = "";
            //if (jsonArray.length > 1)
            //    opt += "<option value='0'>ALL DEPARTMENTS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                var dept = jsonArray[i].DepartmentName;
                if (dept.length > 22)
                    dept = dept.substring(0, 22);
                opt += "<option value=\"" + jsonArray[i].DepartmentID + "\">" + dept + "</option>";
            }
            $("#ddlDepartment").html(opt);
        },
        error: function (xhr) {
            var opt = "";
            opt += "<option value='0'>NO AVAILABLE DEPARTMENTS</option>";
            $ddlDepartment.html(opt);
        },
        complete: function (xhr, status) {
            waitIndicator(false);
        }
    });
}


function getShifts() {
    var Url = ClientBaseURL + clientID + "/ShiftTypes";
    waitIndicator(true, "Getting Shifts..");

    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonArray) {
            //jsonArray = jsonData;
            /* fill out shift drop down */
            var opt = "";
            //if (jsonArray.length > 1)
            //    opt += "<option value='0'>ALL SHIFTS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].ShiftTypeId + "\">" + jsonArray[i].ShiftTypeDesc + "</option>";
            }
            $("#ddlShiftType").html(opt);
            waitIndicator(false);
            getDepartments();
        },
        error: function (xhr) {
            waitIndicator(false);
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}
function getDispatchOffices() {
    Url = RosterBaseURL + "FullDispatch";
    waitIndicator(true, "Getting Locations..");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            offices = jsonData;
            getShifts();
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        },
        complete: function (xhr, status) {
            waitIndicator(false);
        }
    });
}

function getLocations() {
    /* populate client ddl when page is loaded */
    var jsonArray = new Array();

    Url = ClientBaseURL + userID + "/Locations?clientID=" + clientID;
    waitIndicator(true, "Getting Locations..");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            locations = jsonData;
            /* init client drop down */
            $("#ddlLocation").empty();
            var opt = "";
            for (var i = 0; i < locations.length; i++) {
                var loc = locations[i].LocationName;
                if (loc != null && loc.length > 8)
                    loc = loc.substr(0, 8);
                opt += "<option value=\"" + locations[i].LocationID + "\">" + loc + "</option>";
            }
            $("#ddlLocation").html(opt);
            addr1 = locations[0].Address1;
            addr2 = locations[0].Address2;
            city = locations[0].City;
            state = locations[0].State;
            zip = locations[0].Zip;
            phoneArea = locations[0].PhoneAreaCode;
            phonePrefix = locations[0].PhonePrefix;

            //            alert(addr1 + ", " + addr2 + ", " + city + ", " + state + ", " + zip);
            waitIndicator(false);
            getDispatchOffices();
        },
        error: function (xhr) {
            waitIndicator(false);
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function getDDLDates( dt, count) {
    waitIndicator(true, "Getting Dates..");
    var opt = "";
    for (var i = 0; i < count; i++) {
        var m = (dt.getMonth() + 1);
        if (m <= 9) m = "0" + m;
        var d = dt.getDate();
        if (d <= 9) d = "0" + d;
        opt += "<option value='" + dt.getFullYear() + "-" + m + "-" + d + "'>";
        opt += m + "/" + d + "/" + dt.getFullYear();
        opt += "</option>";
        dt.setDate(dt.getDate() - 7);
    }
    waitIndicator(false);
    return opt;
}

function setDropDownDates() {
    var dt = new Date();
    var weekEnding = $("input[id*='weekEnd']").val();

    while (dt.getDay() != (weekEnding % 7)) {
        dt.setDate(dt.getDate() + 1);
    }
    //alert(dt + ", " + dt.getDay());

    var opt = getDDLDates(dt, 53);
    $("#ddlWeekEnding").html(opt);
}


function supports_html5_storage() {
    try {
        return 'localStorage' in window && window['localStorage'] !== null;
    } catch (e) {
        return false;
    }
}

$(document).ready(function () {
    getLocations($("#ddlLocation"));
    $("#btnView").click(function () {
        getETicket();
    });

    //AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,
    //PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID
    //AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID
    //84740, "", "AGUIRRE", "RAMON", 11.50, 0.00, 8.25, 10.97, 0.00, 0.00, "1223"
    $("#btnCSV").click(function () {
        if (!supports_html5_storage()) {
            alert("HTML5 storage NOT supported!");
            return;
        }
        $("#eTicket").find(".aident").each(function () {
            var tr = $(this).closest("tr");
            var aident = tr.find(".aident").text().substring(2);
            var reg = tr.find(".totals").val();
            var ot = 0;
            if (reg > 40) {
                ot = reg - 40;
                reg = 40;
            }
            //var firstName = $(".name").
            alert("Id = " + aident + ", reg hours = " + reg + ", OT hours = " + ot);
        });
    });

    $("#btnUpdate").click(function () {
        updateTable();
    });

    $("#ddlWeekEnding, #ddlLocation, #ddlDepartment, #ddlShiftType").change(function () {
        $("#eTicket tbody tr").remove();
        $(".dayTotal").find('input').val(val('0')).removeClass("changed");
        $("#eTicket").hide();
        $("#spanSupervisor").text("");
    });
    $("#ddlLocation").change(function () {
        var id = $(this).val();
        var idx = 0;
        while (idx < locations.length && id != locations[idx].LocationID) {
            idx++;
        }
        if (idx < locations.length) {
            addr1 = locations[idx].Address1;
            addr2 = locations[idx].Address2;
            city = locations[idx].City;
            state = locations[idx].State;
            zip = locations[idx].Zip;
            phoneArea = locations[idx].PhoneAreaCode;
            phonePrefix = locations[idx].PhonePrefix;
            phoneLast4 = locations[idx].PhoneLast4;

            //alert(addr1 + ", " + addr2 + ", " + city + ", " + state + ", " + zip + ", (" + phoneArea + ") " + phonePrefix + " - " + phoneLast4);
        }
        else
            alert("Error setting location!!!");
    });

    setDropDownDates();
});
