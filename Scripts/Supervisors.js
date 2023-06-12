var ClientBaseURL = "http://localhost:52293/RestServiceWWW/Client/"; //
var RosterBaseURL = "http://localhost:52293/RestServiceWWW/Roster/"; //
ClientBaseURL = "../Client/";
RosterBaseURL = "../Roster/";
OpenBaseURL = "../Open/";

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

function setSupervisor(shiftType, deptId, userId) {
    var Url = OpenBaseURL + "SetSupervisor";
    waitIndicator(true, "Setting Supervisor");   
    var data = {};
    data.DepartmentId = deptId;
    data.UserId = userId;
    data.ShiftType = shiftType;
    data.ClientId = clientID; //global

    $.ajax({
        cache: false,
        type: "POST",
        contentType: "application/json",
        async: true,
        dataType: "json",
        data: JSON.stringify(data),
        url: Url,
        success: function (retVal) {
            if (retVal.indexOf("error") < 0) {
                $('.sup' + deptId + "_" + shiftType).empty();
                $('.sup' + deptId + "_" + shiftType).append(
                    getSupervisorButton(1, shiftType, deptId, userId)
                );
            }
            else {
                alert(retVal);
            }
        },
        error: function (retVal) {
            alert("error: " + retVal);
        },
        complete: function () {
            waitIndicator(false);
        }
    });
}

function departmentViewHide(view, deptId, clientMembershipId) {

    var Url = OpenBaseURL + "DepartmentViewHide";
    if (view) {
        waitIndicator(true, "Making department viewable");
    }
    else {
        waitIndicator(true, "Hiding department from view");
    }
    var data = {};
    data.DepartmentId = deptId;
    data.ClientMembershipId = clientMembershipId;
    data.Hide = !view;

    $.ajax({
        cache: false,
        type: "POST",
        contentType: "application/json",
        async: true,
        dataType: "json",
        data: JSON.stringify(data),
        url: Url,
        success: function (retVal) {

            if (retVal.indexOf("Error") >= 0) {
                alert(retVal);
            }
            else {
                $('.' + deptId).empty();
                $('.' + deptId).append(
                    makeDepartmentVisibilityButton(view, deptId, clientMembershipId)
                );
            }

            waitIndicator(false);
        },
        error: function (errorMsg) {
            waitIndicator(false);
            alert(errorMsg);
        },
        complete: function (xhr, status) {
            waitIndicator(false);
        }
    });
}

function getSupervisorButton(supervisorId, shiftType, deptId, userId) {
    /* use supervisorId as a flag, actual ID not necessary */
    var rows = "";
    if (supervisorId > 0) rows += "<span>Supervisor</span>";
    else rows += "<input type='button' value='Make Supervisor' " +
        "onClick='setSupervisor(" + shiftType + ", " + deptId + ", \"" + userId + "\")' />";
    return rows;
}
function makeDepartmentVisibilityButton(view, deptId, clientMembershipId) {
    var btn;

    if (view) {
        btn = "<span style='Color:Green; font-weight:bold; font-size:1.2em'>Viewable</span><br/><br/>" +
            "<input style='color:Red' type='button' value='Hide' " +
            "onClick='departmentViewHide(false, " + deptId +
            "," + clientMembershipId + ")' />";
    }
    else {
        btn = "<span style='color:Red; font-weight:bold; font-size:1.2em'>Hidden</span><br/><br/>" +
            "<input style='color:Green' type='button' value='UnHide' " +
            "onClick = 'departmentViewHide(true, " + deptId +
            "," + clientMembershipId + ")' />";
    }
    return btn;
}
function getDepartmentsAndSupervisorInfo() {
    var dsr = {
        "UserId": $('#ddlUserName').val(),
        "ClientId": clientID
    };
    var clientMembershipId = $('option:selected', $('#ddlUserName')).attr('client_membership_id');

    var Url = OpenBaseURL + "DepartmentSupervisors";
    waitIndicator(true, "Getting Departments and Supervisors");
    $.ajax({
        cache: false,
        type: "POST",
        contentType: "application/json",
        async: true,
        dataType: "json",
        data: JSON.stringify(dsr),
        url: Url,
        success: function (jsonArray) {
        /* remove old table if necessary */
            $('#tblDepartmentsAndSupervisors').remove();
            /* generate table */
            var table =
                "<table  id=\"tblDepartmentsAndSupervisors\" style='width:100%; border:2px solid black; border-collapse:collapse'>" +
                "<thead>" +
                "<tr>" +
                "<th>Department Name</th>" +
                "<th>Shift</th>" +
                "<th>Visible</th>" +
                "<th>Supervisor</th>" +
                "</tr>" +
                "</thead>";

            var i = 0;
            var deptIdx = 0;
            var rows = "<tbody>";
            while (i < jsonArray.length) {
                var j = i+1;
                while (j < jsonArray.length && jsonArray[i].DepartmentName == jsonArray[j].DepartmentName) j++;
                deptIdx++;
                for (k = i; k < j; k++) {
                    rows += "<tr style='border:2px solid black; border-collapse:collapse ";
                    if (deptIdx % 2 == 0) {
                        rows += ";background-color:LightYellow'>";
                    }
                    else {
                        rows += "'>";
                    }
                    rows += "<td>" + jsonArray[k].DepartmentName + "</td>" +
                        "<td>" + jsonArray[k].ShiftType + "</td>";
                    if (k == i) {
                        rows +=
                            "<td class='" + jsonArray[k].DepartmentId + "' style='text-align:center' rowspan='" + (j - i) + "'>";

                        rows += makeDepartmentVisibilityButton(jsonArray[k].DepartmentViewable,
                            jsonArray[k].DepartmentId, clientMembershipId);
                        rows += "</td>";
                    }
                    rows += "<td class='sup" + jsonArray[k].DepartmentId + "_" + jsonArray[k].ShiftType + "' style='text-align:center'>";

                    rows += getSupervisorButton(jsonArray[k].SupervisorId, jsonArray[k].ShiftType,
                        jsonArray[k].DepartmentId, dsr.UserId);
                    rows += "</td>";
                }
                i = k;
            }
            rows += "</tbody>";
            table += rows;
            $("#divDepartmentsAndSupervisors").append(table);

            waitIndicator(false);
        }, 
        error: function () {
            waitIndicator(false);
        },
        complete: function (xhr, status) {
            waitIndicator(false);
        }
    });
}


function getDepartments() {
    var $locationID = $('#ddlLocation').val();
    var $shiftType = $('#ddlShiftType').val();

    var Url = ClientBaseURL + clientID + "GetDepartments?locationID=" + $locationID + "&shiftType=" + $shiftType;
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

function getUserNames() {

    /* populate client ddl when page is loaded */
    var jsonArray = new Array();

    Url = OpenBaseURL + "/UsersByClient";
    waitIndicator(true, "Getting Authorized Users..");
    $.ajax({
        cache: false,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        data:clientID, 
        url: Url,
        success: function (jsonData) {
            userNames = jsonData;
            /* init client drop down */
            $("#ddlUserName").empty();
            var opt = "";
            for (var i = 0; i < userNames.length; i++) {
                var userName = userNames[i].UserName;
                opt += "<option client_membership_id = \"" + userNames[i].ClientMembershipId + "\" value=\"" + userNames[i].UserID + "\">" + userName + "</option>";
            }
            $("#ddlUserName").html(opt);
            waitIndicator(false);
            $("#ddlUserName").change(function () {
                getDepartmentsAndSupervisorInfo();
            });
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

$(document).ready(function () {
    getUserNames($("#ddlUserNames"));

});
