/*
Status 	Description

queued 	The call is ready and waiting in line before going out.
ringing 	The call is currently ringing.
in-progress 	The call was answered and is currently in progress.
canceled 	The call was hung up while it was queued or ringing.
completed 	The call was answered and has ended normally.
busy 	The caller received a busy signal.
failed 	The call could not be completed as dialed, most likely because the phone number was non-existent.
no-answer 	The call ended without being answered.
*/

var statusArray = new Array('waiting', 'queued', 'ringing', 'in-progress', 'cancelled', 'completed', 'busy', 'failed', 'no-answer');
var employees = null;
var callListArray = null;
var callListIdx = 0;
function Success3(data) {

    // this fuction is called but i am not sure what to write here to retrieve the image
    alert("Hello!");
}
function Error3(request, status, error) {

    alert("error3");
    $("#label1").html(request.statusText);
}

$("#callListDiv").dialog({
    autoOpen: false,
    height: 580,
    width: 480,
    modal: true,
    open: function () {
        $(this).find("input").off("click");
        $(this).find("input").off("change");

        $(this).find("#name").text($(callListArray[0]).find('.name').text());
        $(this).find("#id").text($(callListArray[0]).find('.aident').text());
        $(this).find("#aident").text($(callListArray[0]).find('.aident').text());
        $(this).find("#picDiv").find("img").attr("src", "http://msiwebtrax.com/Dropbox/ids/" + $(this).find(".aident").text() + ".jpg");

        $(this).find('select').change(function () {
            $(this).find('option[selected]').removeAttr('selected');
            $(this).find('option:selected').attr('selected', 'selected');
        })

        $(this).find('textarea').change(function () {
            //alert("Change!!!");
            $(this).css("background-color", "#FAA");
            $(this).closest('div').find("#updEmployeeNotes").removeAttr("disabled");
        })

        $(this).find("#next").click(function () {
            callListAdv($(this).closest("div"), 1);
        });

        $(this).find("#prev").click(function () {
            callListAdv($(this).closest("div"), -1);
        });

        $(this).find("#call").click(function (event) {
            event.stopPropagation();
            directCall(null, $(this));
        });
        
        $(this).find("#text").click(function (event) {
            //alert("Here we go!");
            event.stopPropagation();
            directTextConfirm(null, $(this).closest('div'));
        });

        $(this).find("#updEmployeeNotes").click(function () {
            updateEmployeeNotes($(this));
        });

        $(this).find("#addToRoster").click(function (event) {
            event.stopPropagation();
            addToRosterCallList($(this).closest('div'));
        });

        $(this).find("#shiftStartEnd").html(
            "<td colspan='2'>S: " + dropDownDateTime($('span[id="shiftStart"]').text()) + "<br/>E: " +
            dropDownDateTime($('span[id="shiftEnd"]').text()) + "</td>"
        );


        $(this).find("#clientName").text($('#ddlClientRoster').children('option').filter(':selected').text());
        $(this).find("#workDate").text($('#dateRoster').val());
        $(this).find("#shiftInfo").text($('#ddlShiftRoster').children('option').filter(':selected').text());
        $(this).find("#deptInfo").text($('#ddlDepartmentRoster').children('option').filter(':selected').text());
        callListAdv($(this).closest("div"), 1);

    },
    buttons: {
        "CLOSE": function () {
            $(this).dialog("close");
        }
    },
    close: function () {
        Twilio.Device.disconnectAll();
        $(this).find("#call").val("CALL").removeAttr('disabled').css("color", "black");
        $(this).find("#next").removeAttr("disabled");
        $(this).find("#prev").removeAttr("disabled");
    }
});

$("#phoneBlastDiv").dialog({
    autoOpen: false,
    height: 300,
    width: 350,
    modal: true,
    buttons: {
        "Phone Blast!": function () {
            initStatuses();
            makeCalls();
            $(this).dialog("close");
        },
        Cancel: function () {
            $(this).dialog("close");
        }
    },
    close: function () {
    }
});

function callList(index) {
    callListArray = new Array();
    $("#callTable").find("tbody").find("tr[class*='employeeRoot']").each(function () {
        callListArray.push($(this));
    });
    if (callListArray.length > 0) {
        callListIdx = index - 1;
        $("#callListDiv").dialog("open");
    }
}

function callListAdv(dialog, stp) {
    callListIdx = (callListIdx + stp);
    if (callListIdx < 0) callListIdx += callListArray.length;
    if (callListIdx >= callListArray.length) callListIdx = 0;
    var tr = callListArray[callListIdx];
    dialog.find("#name").text(tr.find(".name").text());
    dialog.find("#id").text(tr.find(".aident").text());
    dialog.find("#picDiv").find("img").attr("src", "http://msiwebtrax.com/Dropbox/ids/" + tr.find(".aident").text() + ".jpg");
    dialog.find("textarea").val(jsonArray.RecruitPoolCollection[callListIdx].Notes);
    dialog.find("textarea").css("background-color", "#FFF");
    dialog.find("#updEmployeeNotes").attr("disabled", "disabled");

    dialog.find("#text").css("background-color", "#FFA");
    dialog.find("#call").css("background-color", "#AFA");
    var pt = tr.find(".phoneNum").text();
    var phone = "";
    for (var i = 0; i < pt.length; i++) {
        if (pt.charAt(i) >= '0' && pt.charAt(i) <= '9')
            phone = phone + pt.charAt(i);
    }
    if (phoneNumberIsValid(phone)) {
        dialog.find("#area").val(phone.substring(0, 3));
        dialog.find("#prefix").val(phone.substring(3, 6));
        dialog.find("#last4").val(phone.substring(6));
        dialog.find("#call").removeAttr("disabled");
        dialog.find("#text").removeAttr("disabled");
    }
    else {
        dialog.find("#area").val('');
        dialog.find("#prefix").val('');
        dialog.find("#last4").val('');
        dialog.find("#call").attr("disabled", "disabled");
        dialog.find("#text").attr("disabled", "disabled");
    }
}

//$("#mainCall").attr("disabled", "disabled");
updateStatuses();

function phoneNumberIsValid(num) {
    var count = 0;
    for (var i = 0; i < num.length; i++) {
        if (num[i] >= '0' && num[i] <= '9')
            count++;
    }
    return count == 10;
}

function phoneBlast() {
    $("#phoneBlastDiv").dialog("open");
}

var phoneBlastCallCount = 0;
var maxPhoneBlastCalls = 0;

function makeCalls() {
    //get phone numbers in the call list.
    var tbl = $("#callTable");
    var rows = $(tbl).find('.employeeRoot');
    $(rows).each(function (idx) {
        var num = $(this).find('.phoneNum').text();
        if (!phoneNumberIsValid(num)) {
            $(this).removeClass("employeeRoot").addClass("employeeRootBadPhoneNumber");
            $(this).find('.callStatus').text('Invalid #');
        }
        
        else if ($(this).attr('phoneBlast') == 'callInProgress') {
            /* initiate call 
            $(this).attr('phoneBlast', 'callInProgress');
            $(this).find('.workStatus').text('Phone Blast');
            $(this).find('.callStatus').text('initializing');
            makeCall($(this), num);
            */
            var btn = $(this).find('input.directText');
            directText("", btn, num);
        } 
    });
}

function makeSingleCall($tr) {
        var num = $tr.find('.phoneNum').text();
        if (!phoneNumberIsValid(num)) {
            $tr.removeClass("employeeRoot").addClass("employeeRootBadPhoneNumber");
            $tr.find('.callStatus').text('Invalid #');
        }
        else {//if ($tr.attr('phoneBlast') == 'callInProgress') {
            /* initiate call */
            $tr.attr('phoneBlast', 'callInProgress');
            $tr.find('.workStatus').text('Phone Blast');
            $tr.find('.callStatus').text('initializing');
            makeCall($tr, num);
        }
}

function makeCall($row, phoneNum) {
    var url = "http://localhost:52293/RestServiceWWW/Roster/PBMakeCall?to=" + phoneNum +
                                    "&msgUrl=https://metrostaff.fwd.wf/RestServiceWWW/Roster/PBJobGreeting";
    /* server */
    url = "../Roster/" + "PBMakeCall?to=" + phoneNum + "&msgUrl=http://msiwebtrax.com/Roster/PBJobGreeting"; 
                                            
    //alert(url);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        url: url,
        success: function (data) {
            if (data != "-1") {
                $row.find('.callStatus').attr("callID", data);
                $row.find('.callStatus').text('initiating...');
            }
            else {
                $row.removeClass("employeeRoot").addClass("employeeRootBadPhoneNumber");
                $row.find('.callStatus').text('Invalid #');
                $row.find('.directCall').attr('disabled', 'disabled');
                $row.attr('phoneBlast', 'called');
            }
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function initStatuses() {
    var maxPhoneBlastCalls = $('#maxPhoneBlastSize').val();
    var tbl = $("#callTable");
    var rows = $(tbl).find('.employeeRoot');
    $(rows).each(function (idx) {
        var num = $(this).find('.phoneNum').text();
        if (!phoneNumberIsValid(num)) {
            $(this).removeClass("employeeRoot").addClass("employeeRootBadPhoneNumber");
            $(this).find('.callStatus').text('Invalid #');
        }
        else if ($(this).attr('phoneBlast') != 'callFinished' && phoneBlastCallCount < maxPhoneBlastCalls) {
            /* looks good so far... */
            var res = updateWorkStatus($(this), false);
            if( res == true ) {
                $(this).attr('phoneBlast', 'callInProgress');
                $(this).find('.workStatus').text('Phone Blast');
                phoneBlastCallCount += 1;
            }
        }
    });
    phoneBlastCallCount = 0;
}

function updateStatuses() {
    var $rows = $('#callTable tbody tr[phoneBlast!="notCalled"][phoneBlast!="callFinished"]');
    var updateFinished = true;

    $rows.each(function (idx) {
        var callTxt = $(this).find('.callStatus').text();
        var workTxt = $(this).find('.workStatus').text();
        if ((callTxt != "" && callTxt != "waiting" && callTxt != "busy" && callTxt != "completed" && callTxt != "no-answer" &&
            callTxt != "failed" && callTxt != "Invalid #")) {
            updateFinished = false;
            updateCallStatus($(this));
        }
    });

    $rows.each(function (idx) {
        var workTxt = $(this).find('.workStatus').text();
        var callTxt = $(this).find('.callStatus').text();
        if ((callTxt != "" && callTxt != "waiting" && callTxt != "busy" && callTxt != "completed" && callTxt != "Invalid #" &&
                    callTxt != "failed" && callTxt != "no-answer" ) || workTxt == "Initiating") {
            updateFinished = false;
            updateWorkStatus($(this));
        }
    });

    if (updateFinished && $rows.length > 0)
        $rows.attr('phoneBlast', 'callFinished');
    setTimeout(updateStatuses, 8000); //repeat after 8 seconds...
}

function updateCallStatus($row) {
    var id = $row.find('.callStatus').attr("callID");
    if (id == null || id.length == 0)
        return;

    var url = "../Roster/PBGetCall?id=" + id;
    activityIndicator = false;

    $.ajax({
        cache: false,  
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        url: url,
        success: function (data) {
            $row.find('.callStatus').text(data);
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function updateWorkStatus($row, asyncIn) {
    var retRes = true;
    if ($row.length == 0)
        return;
    if (asyncIn == null)
        asyncIn = true;
    var id = $row.find('.callStatus').attr("callID");
    id = $row.find('.phoneNum').text();

    var url = "../Roster/PBGetWorkStatus?id=" + id;
    activityIndicator = false;
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: asyncIn,
        dataType: "json",
        url: url,
        success: function (data) {
            $row.find('.workStatus').text(data);
            if (data == "Available!") {
                $row.removeClass("employeeRoot").addClass("employeeRootAvailable");
                $row.find(".callStatus").text("completed");
                $row.attr('phoneBlast', 'callFinished');
                retRes = false;
            }
            if (data == "Not Available") {
                $row.removeClass("employeeRoot").addClass("employeeRootUnavailable");
                $row.find(".callStatus").text("completed");
                $row.attr('phoneBlast', 'callFinished');
                retRes = false;
            }
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
    return retRes;
}

