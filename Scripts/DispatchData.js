var clientList = [];
var clientOpt;

$(document).ready(function () {
    //alert("ready to go!");
    waitIndicator(true);
    $(".weekly").hide().removeAttr("checked");
    var submitted = false;
    var submitAvailable = false;
    var GetDispatchOffices = "../Roster/Dispatch";
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        url: GetDispatchOffices,
        success: function (data) {
            data += "<option value='T1'>ALL DISPATCH LOCATIONS</option>";
            $("#selDispatch").html(data);
            getClients();
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });

    $("#selShift").append("<option value='-1'>SELECT SHIFT</option>");
    $("#selShift").append("<option value='1'>1st SHIFT</option>");
    $("#selShift").append("<option value='2'>2nd SHIFT</option>");
    $("#selShift").append("<option value='3'>3rd SHIFT</option>");
    $("#selShift").append("<option id='allShifts' value='0'>ALL SHIFTS</option");

    $('#selDispatch').change(function () {
        $("#selDate").val("");
        $("#selDate").prop("disabled", false);
        $("#selShift").prop("disabled", true);
        $("#btnSubmit").prop("disabled", true);
        var officeCd = $("#selDispatch").val();
        if (officeCd.length > 1) {
            officeCd = officeCd.substring(1);
        }
        if (officeCd == '1') {
            $(".weekly").show();
        }
        else {
            $(".weekly").find("input").removeAttr("checked");
            $(".weekly").hide().find("input").removeAttr("checked");
        }
        clearTableBodyRows("clientTable");
    });
    $(".weekly").find("input").click(function () {
        if ($(".weekly").find("input").attr("checked").length > 0) {
            setWeekEnd();
        }
        $("#selShift option[value='-1']").prop('selected', true);
        clearTableBodyRows("clientTable");
    });
    $('#selDate').change(function () {
        $("#selShift option[value='-1']").prop('selected', true);
        $("#selShift").prop("disabled", false);
        $("#allShifts").prop("disabled", false);
        if ($("td[class='weekly']").find("input").prop("checked")) {
            //alert("Checked!");
            setWeekEnd();
        }
        clearTableBodyRows("clientTable");
    });
    $("#selShift").change(function () {
        getDispatchData();
    });
    $("#btnSubmit").click(function () {
        updateDailyDispatchInfo();
    });
    $("#addClient").click(function () {
        addRow();
    });
    $("#btnExcel").click(function () {
        exportToXlsx("clientTable");
    });
});

function setWeekEnd() {
    var d = new Date($('#selDate').val());
    d.setHours(d.getHours() + 6);
    if (Object.prototype.toString.call(d) === "[object Date]") {
        // it is a date
        if (isNaN(d.getTime())) {  // d.valueOf() could also work
            // date is not valid
            return;
        } else {
            // date is valid
            while (d.getDay() != 0) {
                var dt = (d.getDate() + 1) % 7;
                d.setDate(d.getDate() + 1);
            }
            var day = ("0" + d.getDate()).slice(-2);
            var month = ("0" + (d.getMonth() + 1)).slice(-2);

            var today = d.getFullYear() + "-" + (month) + "-" + (day);
            $("#selDate").val(today);
        }
    } else {
        // not a date
        return;
    }
}

function addRow(data) {
    var row;
    if (typeof data == "undefined") {
        row = "<tr>" +
            "<td><select class=\"clientNames\"></select></td>" +
            "<td><input readonly='readonly' class='totSent' maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"0\" /></td>" +
            "<td><input class='totReg' maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"0\" /></td>" +
            "<td><input class='totSwing' maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"0\" /></td>" +
            "<td><input class='totDriven' maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"0\" /></td>" +
            /*"<td><input maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"0\" /></td>" +*/
            "<td><input readonly='readonly' class='overUnder' maxlength=\"2\" size=\"4\" type=\"number\" min=\"-999\" max=\"999\" value=\"0\" /></td>" +
            "<td><input maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"0\" /></td>" + 
            "<td><textarea cols=\"40\" rows=\"1\" placeholder=\"Enter any notes here\"></textarea></td>" +
        "</tr>";
        $('#clientTable tbody tr:last').before(row);
        /* get list of clients already selected */
        //alert($(".clientNames").length);
        $('#clientTable tbody tr:last').prev().find('.clientNames').append(clientOpt);
        $("#clientTable tbody tr:last").prev().find('.clientNames').find("option").each(function () {
            var id = $(this).val();
            var $this = this;
            if (id != -1) {
                $(".clientNames").each(function () {
                    if ($(this).val() == id) {
                        //alert("exists! " + $(this).val() + ", " + id);
                        $($this).prop("disabled", true);
                    }
                });
            }
        });
        $('#clientTable tr:last').prev().find(".totReg").change(function () {
            var val = parseInt($(this).val(), 10) + parseInt($(this).parent("td").next().next().find(".totDriven").val(), 10);
            $(this).parent("td").prev().find(".totSent").val(val);
            //alert(val);
        });

        $('#clientTable tr:last').prev().find(".totDriven").change(function () {
            var val = parseInt($(this).val(), 10) + parseInt($(this).parent("td").prev().prev().find(".totReg").val(), 10);
            $(this).parent("td").prev().prev().prev().find(".totSent").val(val);

            val = parseInt($(this).val(), 10) - parseInt($(this).parent("td").prev().find(".totSwing").val(), 10);
            setOverUnder($(this).parent("td").next(), val);
        });

        $('#clientTable tr:last').prev().find(".totSwing").change(function () {
            var val = parseInt($(this).parent("td").next().find(".totDriven").val(), 10) - parseInt($(this).val(), 10);
            //$(this).parent("td").next().next().find(".overUnder").val(val);
            setOverUnder($(this).parent("td").next().next(), val);
        });

        $('#clientTable tr:last').prev().find(".overUnder").change(function () {
            var val = parseInt($(this).val(), 10);
            if (val < 0) {
                $(this).addClass("negVal");
            }
            else {
                $(this).removeClass("negVal");
            }
        });
    }
    else if (typeof data == "string" && data.substring(0, 1) == "!") {
        row = "<tr><td colspan='10'><span>" + data.substring(1) + "</span></td></tr>";
        $('#clientTable tbody').append(row);
    }
    else {
        var overUnder = data.extras - data.unfilled;
        //data.transported = 0;
        row = "<tr>" +
            "<td><select class=\"clientNames\"></select></td>" +
            "<td><input readonly='readonly' class='totSent' maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"" + (data.regs + data.tempsSent) + "\"/></td>" +
            "<td><input class='totReg' maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"" + data.regs + "\" /></td>" +
            "<td><input maxlength=\"2\" class='totSwing' size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"" + data.tempsOrdered + "\" /></td>" +
            "<td><input class='totDriven' maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"" + data.tempsSent + "\" /></td>" +
            /*"<td><input maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"" + data.unfilled + "\" /></td>" +*/
            "<td><input readonly='readonly' class='overUnder' maxlength=\"2\" size=\"4\" type=\"number\" min=\"-999\" max=\"999\" value=\"" + overUnder + "\"/></td>" +
            "<td><input maxlength=\"2\" size=\"4\" type=\"number\" min=\"0\" max=\"999\" value=\"" + data.transported + "\"/></td>" +
            "<td><textarea cols=\"40\" rows=\"1\" placeholder=\"Enter any notes here\">" + data.notes + "</textarea></td>" +
        "</tr>"
        $('#clientTable tbody').append(row);
        $('#clientTable tr:last').find('.clientNames').append(clientOpt);
        $('#clientTable tr:last').find('.clientNames').val(data.clientId);

        var rowCount = $('#clientTable tr:last').prev().find(".totReg").length;

        $('#clientTable tr:last').find(".totReg").change(function () {
            var val = parseInt($(this).val(), 10) + parseInt($(this).parent("td").next().next().find(".totDriven").val(), 10);
            $(this).parent("td").prev().find(".totSent").val(val);
        })

        $('#clientTable tr:last').find(".totDriven").change(function () {
            var val = parseInt($(this).val(), 10) + parseInt($(this).parent("td").prev().prev().find(".totReg").val(), 10);
            $(this).parent("td").prev().prev().prev().find(".totSent").val(val);

            val = parseInt($(this).val(), 10) - parseInt($(this).parent("td").prev().find(".totSwing").val(), 10);
            setOverUnder($(this).parent("td").next(), val);
        });

        $('#clientTable tr:last').find(".totSwing").change(function () {
            var val = parseInt($(this).parent("td").next().find(".totDriven").val(), 10) - parseInt($(this).val(), 10);
            setOverUnder($(this).parent("td").next().next(), val);
        });

        if (parseInt($('#clientTable tr:last').find(".overUnder").val(), 10) < 0) {
            $('#clientTable tr:last').find(".overUnder").addClass("negVal");
        }
        $('#clientTable tr:last').find(".overUnder").change(function () {
            var val = parseInt($(this).val(), 10);
            if (val < 0) {
                $(this).addClass("negVal");
            }
            else {
                $(this).removeClass("negVal");
            }
        });
    }
}

function setOverUnder($td, val) {
    $td.find(".overUnder").val(val);
    if (val < 0) {
        $td.find(".overUnder").addClass("negVal");
    }
    else {
        $td.find(".overUnder").removeClass("negVal");
    }
}
function addReportRow(data) {
    var row;
    if (typeof data == "string" && data.substring(0, 1) == "!") {
        row = "<tr><td colspan='9'><span>" + data.substring(1) + "</span></td></tr>";
        $('#clientTable tbody').append(row);
    }
    else {
        var clientName = "Unknown";
        for (var i = 0; i < clientList.length; i++) {
            if (clientList[i].ClientID == data.clientId) {
                clientName = clientList[i].ClientName;
                break;
            }
        }
        var overUnder = data.extras - data.unfilled;
        row = "<tr>" +
            "<td><span>" + data.officeName + "</span></td>" +
            "<td><span>" + data.shiftType + "</span></td>" +
            "<td><span>" + clientName + "</span></td>" +
            "<td><span>" + data.totSent + "</span></td>" +
            "<td><span>" + data.regs + "</span></td>" +
            "<td><span>" + data.tempsOrdered + "</span></td>" +
            "<td><span>" + data.tempsSent + "</span></td>" +
            /*"<td><span>" + data.unfilled + "</span></td>" +*/
            /*"<td><span>" + data.extras + "</span></td>" +*/
            "<td><span>" + overUnder + "</span></td>" +
            "<td><span>" + data.transported + "</span></td>" + 
            "<td><textarea disabled=\"disabled\">" + data.notes + "</textarea></td>" +
            "<td><span>" + data.dispatchDt + "</span></td>" +
        "</tr>";
        $('#clientTable tbody').append(row);
    }
}

/*      This is info to be updated.
        public int clientId { get; set; }
        public DateTime dispatchDt { get; set; }
        public int officeId { get; set; }
        public int shiftType { get; set; }
        public int totSent { get; set; }
        public int regs { get; set; }
        public int tempsOrdered { get; set; }
        public int tempsSent { get; set; }
        public int unfilled { get; set; }
        public int extras { get; set; }
        public string notes { get; set; }
        public string createdBy { get; set; }
                <th>Client</th>
                <th>Employees Sent</th>
                <th>Regulars</th>
                <th>Temps Ordered</th>
                <th>Temps Sent</th>
                <th>Unfilled</th>
                <th>Extras</th>
                <th>Notes</th>
    var shiftType = $("#selShift").val();
    var officeId = $("#selDispatch").val();
    var dispatchDate = $("#selDate").val();
*/
function updateDailyDispatchInfo() {
    //waitIndicator(true);
    var $tr = $('#clientTable').find('tbody').find('tr');
    var count = 0;
    var list = [];

    $tr.each(function () {
        $td = $(this).find('td');
        if ($td.length > 3) {
            var cliInfo = {};
            var idx = 0;
            $($td).each(function () {
                if (idx == 0) {
                    cliInfo['clientId'] = $(this).find('select').val();
                    if (cliInfo['clientId'] <= 0) {
                        return false;
                    }
                }
                else if (idx == 1) {
                    cliInfo['totSent'] = $(this).find('input').val();
                    if (cliInfo['totSent'].length == 0) {
                        cliInfo['totSent'] = 0;
                    }
                }
                else if (idx == 2) {
                    cliInfo['regs'] = $(this).find('input').val();
                    if (cliInfo['regs'].length == 0) {
                        cliInfo['regs'] = 0;
                    }
                }
                else if (idx == 3) {
                    cliInfo['tempsOrdered'] = $(this).find('input').val();
                    if (cliInfo['tempsOrdered'].length == 0) {
                        cliInfo['tempsOrdered'] = 0;
                    }
                }
                else if (idx == 4) {
                    cliInfo['tempsSent'] = $(this).find('input').val();
                    if (cliInfo['tempsSent'].length == 0) {
                        cliInfo['tempsSent'] = 0;
                    }
                }
                else if (idx == 5) {
                    var overUnder = $(this).find('input').val();
                    if (overUnder < 0) {    //negative is really positive unfilled
                        cliInfo['unfilled'] = -1 * $(this).find('input').val();
                        cliInfo['extras'] = 0;
                    }
                    else {
                        cliInfo['extras'] = $(this).find('input').val();
                        cliInfo['unfilled'] = 0;
                    }
                }
                else if (idx == 6) {
                    cliInfo['transported'] = $(this).find('input').val();
                    if (cliInfo['transported'].length == 0) {
                        cliInfo['transported'] = 0;
                    }
                }
                else if (idx == 7) {
                    cliInfo['notes'] = $(this).find('textarea').val();
                    if (cliInfo['notes'].length == 0) {
                        cliInfo['notes'] = 0;
                    }
                }
                idx++;
            });
            if (cliInfo['clientId'] > 0) {
                cliInfo['createdBy'] = $('#userID').val();
                cliInfo['createdDt'] = getTodaysDate();
                cliInfo['officeCd'] = $("#selDispatch").val();
                cliInfo['officeId'] = 0;
                cliInfo['shiftType'] = $("#selShift").val();
                cliInfo['dispatchDt'] = $('#selDate').val();
                //for (i = 0; i < list.length; i++) {
                //    alert((list[i])['clientId']);
                //}
                list.push(cliInfo);
            }
        }
    });
    if (list.length > 0) {
        $.ajax({
            contentType: "application/json",
            type: "POST",
            async: true,
            dataType: "json",
            data: JSON.stringify(list),
            url: '../Open/UpdateDailyDispatchInfo',
            success: function (dat) {
                //alert(dat);
                if (dat.includes("Success")) {
                    alert("Information successfully updated!");
                }
                waitIndicator(false);
            },
            error: function (xhr) {
                alert("ERROR!!!" + xhr.responseText);
                waitIndicator(false);
            },
        });
    }
    else {
        alert("No data to upload!");
    }
}
function getTodaysDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    today = mm + '/' + dd + '/' + yyyy;
    return today;
}
function waitIndicator(on, titleBar) {
    $('#waiting').remove();
    if (on == true) {
        if (titleBar == null)
            titleBar = "loading";
        $("<div id='waiting'><h3 style='padding-left:88px'>please wait...</h3><img style='padding-left:88px' src='../Images/ajax-loader.gif'/></div>").dialog(
        {
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
            },
            title: titleBar
        });
    }
    else {
        $('#waiting').remove();
    }
}

function getDispatchData() {
    //var dailyDispatchData = "http://www.msiwebtrax.com/Open/DailyDispatchInfo";
    var dailyDispatchData = "../Open/DailyDispatchInfo";
    waitIndicator(true);
    var weekly = $(".weekly").find("input").prop("checked");
    //alert(weekly);
    if (weekly == true) {
        setWeekEnd();
    }
    var shiftType = $("#selShift").val();
    var officeCd = $("#selDispatch").val();
    if (officeCd.length > 1) {
        officeCd = officeCd.substring(1);
    }
    var dispatchDate = $("#selDate").val();
    //alert(shiftType + ", " + officeCd + ", " + dispatchDate);
    var data = {
        "officeCd": officeCd,
        "date": dispatchDate,
        "shiftType": shiftType,
        "weeklyReport": weekly
    };
    alert(data + ", " + data["weeklyReport"]);
    alert(JSON.stringify(data));
    $.ajax({
        contentType:"application/json",
        type: "POST",
        async: true,
        dataType: "json",
        data: JSON.stringify(data),
        url: dailyDispatchData,
        success: function (dat) {
            $("#btnExcel").prop("disabled", false);
            if (shiftType != 0 && officeCd != '1') {
                //set table header for an input form
                $("#clientTable thead").replaceWith(
            "<thead>" + 
            "<tr>" + 
                "<th>Client</th>" + 
                "<th>TOTAL SHIFT HEAD COUNT</th>" + 
                "<th>DIRECT REPORTS / REGULARS</th>" + 
                "<th>DAILY SWING ORDER</th>" +
                "<th>DAILY DISPATCH</th>" +
                //"<th>UNFILLED</th>" + 
                "<th>OVER / UNDER</th>" +
                "<th>MSI TRANSPORTED</th>" + 
                "<th>NOTES</th>" + 
            "</tr>" + 
            "</thead>"
            );
                var count = 0;
                clearTableBodyRows("clientTable");
                //var client = dat[0].clientId;
                $(dat).each(function () {
                    addRow(this);
                    count++;
                });
                addSubmitRow();
                $("#addClient").prop("disabled", false);
                $("#btnSubmit").click(function () {
                    updateDailyDispatchInfo();
                });
                $("#addClient").click(function () {
                    addRow();
                })
            }
            else {  //set table header for a report form
                $("#clientTable thead").replaceWith(
            "<thead>" +
            "<tr>" +
                "<th>Dispatch</th>" +
                "<th>Shift</th>" +
                "<th>Client</th>" +
                "<th>TOTAL SHIFT HEAD COUNT</th>" +
                "<th>DIRECT REPORTS / REGULARS</th>" +
                "<th>DAILY SWING ORDER</th>" +
                "<th>DAILY DISPATCH</th>" +
                //"<th>UNFILLED</th>" +
                "<th>OVER / UNDER</th>" +
                "<th>MSI TRANSPORTED</th>" + 
                "<th>NOTES</th>" +
                "<th>DATE</th>" + 
            "</tr>" +
            "</thead>"
            );
                var count = 0;
                var r = 0;
                var datArray = [];
                clearTableBodyRows("clientTable");
                if (Array.isArray(dat) && dat.length > 0) {
                    datArray[0] = dat[0];
                    count = 1;
                    for (var i = 1; i < dat.length; i++) {
                        if( datArray[r].clientId == dat[i].clientId &&
                            datArray[r].officeCd == dat[i].officeCd && 
                            datArray[r].dispatchDt == dat[i].dispatchDt) {
                            datArray[r].tempsSent += dat[i].tempsSent;
                            datArray[r].tempsOrdered += dat[i].tempsOrdered;
                            datArray[r].totSent += dat[i].totSent;
                            datArray[r].regs += dat[i].regs;
                            datArray[r].extras += dat[i].extras;
                            datArray[r].unfilled += dat[i].unfilled;
                            datArray[r].transported += dat[i].transported;
                            datArray[r].notes += "\n" + dat[i].notes;
                            datArray[r].shiftType += ", " + dat[i].shiftType;
                        }
                        else {
                            datArray.push(dat[i]);
                            r = r + 1;
                            count = count + 1;
                        }
                    }
                }
                if (count != 0) {
                    $(datArray).each(function () {
                        addReportRow(this);
                        count++;
                    });
                }
                if (count == 0) {
                    clearTableBodyRows("clientTable", 0);
                    var msg = "!No data available for ";
                    if ($(".weekly").find("input").attr('checked')) {
                        msg += "week ending ";
                    }
                    msg += dispatchDate + ", ";
                    if (shiftType == 0) msg += "all shifts";
                    else msg += "shift " + shiftType;
                    addRow(msg);
                    //$("#btnExcel").prop("disabled", true);
                }
            }
            waitIndicator(false);
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
            $("#btnExcel").prop("disabled", true);
            waitIndicator(false);
        },
    });
}

function getClients() {
    var clientNames = "../Client/itdept/Active";
    waitIndicator(true);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        url: clientNames,
        success: function (data) {
            clientList = data;
            if (data.length >= 1) {
                clientOpt = "<option value='-1'>Please select a client</option>";
            }
            $(data).each(function () {
                clientOpt += "<option value='" + this.ClientID + "'>" + this.ClientName + "</option>";
            });
            $(".clientNames").append(clientOpt);
            waitIndicator(false);
            //setClientTable();
            //setClientParams();
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}
function addSubmitRow() {
    $('#clientTable tbody').append("<tr id='newRow'><td colspan='7'><input type='button' id='addClient' value='ADD CLIENT'/></td><td colspan='1'>" +
        "<input type='button' id='btnSubmit' value='SUBMIT'/></td></tr>");
    
}
function clearTableBodyRows(tableId, numRows) {
    if (typeof numRows == "undefined") numRows = 1;
    tableId = "#" + tableId;
    var trCount = $(tableId + " > tBody > tr").length;
    while ($(tableId + " > tBody > tr").length > 0) {
        $(tableId + " > tBody > tr:first-child").remove();
    }
}
    function setClientTable() {
        $("#clientTable").fixedHeaderTable({
            height: '300',
            width: '800',
            footer: true,
            cloneHeadToFoot: true,
            themeClass: 'fancyTable',
            fixedColumn: true,
            autoShow: true
        });
    }
    function setClientParams() {
        $("#clientParams").fixedHeaderTable({
            width: '800',
            footer: false,
            cloneHeadToFoot: false,
            themeClass: 'fancyTable',
            fixedColumn: true,
            autoShow: true
        });
    }
