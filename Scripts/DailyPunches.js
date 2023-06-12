function getDate(dt) {
    return new Date(parseInt(dt.replace("/Date(", "").replace(")/", ""), 10));
}

var shiftNames = new Array("Error!", "1st Shift", "2nd Shift", "3rd Shift", "Shift A", "Shift B", "Shift C", "Shift D");
var options = {
    weekday: "long", year: "numeric", month: "short",
    day: "numeric", hour: "2-digit", minute: "2-digit"
};
jQuery(document).ready(
    function () {
        //alert("Here we go!");
        var clientId = $("input[id*='clientID']").val();
        /* set selection date values */
        var $sel = $("select[id*='hcDate']");
        var dt = new Date();
        for (i = 0; i < 1; i++) {
            var dVal = dt.getFullYear() + "-";
            if ((dt.getMonth()+1) <= 9)
                dVal += "0" + (dt.getMonth()+1) + "-";
            else
                dVal += (dt.getMonth()+1) + "-";
            if (dt.getDate() <= 9)
                dVal += "0" + dt.getDate();
            else
                dVal += dt.getDate();

            $sel.append($("<option>", {
                value: dt,
                text: dVal
            }));
            dt.setDate(dt.getDate() - 1);
        }
        dt = $("select[id*='hcDate'] option:selected").text();
        $("a[id*='lnkExportExcel']").attr("href", "HeadCountExcel.aspx?clientId=" + clientId + "&startDate=" + dt + "&output=excel");
        $("a[id*='lnkExportWord']").attr("href", "HeadCountExcel.aspx?clientId=" + clientId + "&startDate=" + dt + "&output=word");
        $("select[id*='hcDate']").on("change", function () {
            var dt = $("select[id*='hcDate'] option:selected").text();
            $("a[id*='lnkExportExcel']").attr("href", "HeadCountExcel.aspx?clientId=" + clientId + "&startDate=" + dt + "&output=excel");
            $("a[id*='lnkExportWord']").attr("href", "HeadCountExcel.aspx?clientId=" + clientId + "&startDate=" + dt + "&output=word");
        });
        $("#hcGo").on("click", function () {
            $("<div id='loading'><h3>please wait...</h3></div>").dialog();
            var dt = new Date($("select[id*='hcDate'] option:selected").val());

            //var url = "http://localhost:52211/Open/PunchData/" + clientId + "?start=" + dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate() + "&end=";
            var url = "../Open/PunchData/" + clientId + "?start=" + dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate() + "&end=";
            dt.setDate(dt.getDate() + 1);
            url = url + dt.getFullYear() + "-" + (dt.getMonth()+1) + "-" + dt.getDate();

            $.ajax({
                cache: false,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                async: true,
                dataType: "json",
                url: url,
                success: function (data) {
                    var shifts = new Object();

                    for (var i = 0; i < data.length; i++) {
                        data[i].StartDate = getDate(data[i].StartDate);
                        data[i].EndDate = getDate(data[i].EndDate);
                        data[i].CreatedDate = getDate(data[i].CreatedDate);
                        data[i].PunchExact = getDate(data[i].PunchExact);
                        data[i].PunchRound = getDate(data[i].PunchRound);
                        data[i].ReportDate = getDate(data[i].ReportDate);

                        var deptNum = data[i].DepartmentId;
                        var shift = data[i].ShiftType;

                        if (shifts[shift] == null)
                        {
                            shifts[shift] = new Object();
                            shifts[shift].Name = shiftNames[data[i].ShiftType];
                            shifts[shift].Depts = new Object();
                        }
                        if (shifts[shift].Depts[deptNum] == null) {
                            shifts[shift].Depts[deptNum] = new Object();
                            shifts[shift].Depts[deptNum].Name = data[i].DepartmentName;
                            shifts[shift].Depts[deptNum].Employees = new Object();
                        }
                        var dept = shifts[shift].Depts[deptNum];
                        var id = data[i].Id;
                        /* is punch in range? */
                        if (/*data[i].PunchRound <= data[i].EndDate &&*/ data[i].PunchRound >= data[i].StartDate)
                        {
                            if (dept.Employees[id] == null) {
                                dept.Employees[id] = new Object();
                            }
                            if (dept.Employees[id].Count >= 1) {
                                dept.Employees[id].Count = dept.Employees[id].Count + 1;
                                dept.Employees[id].OnPremises = !dept.Employees[id].OnPremises;
                                if( data[i].PunchRound > dept.Employees[id].PunchTime ) {
                                    dept.Employees[id].PunchTime = data[i].PunchRound;
                                }
                            }
                            else {
                                dept.Employees[id].Count = 1;
                                dept.Employees[id].Name = data[i].LastName + ", " + data[i].FirstName;
                                dept.Employees[id].Id = data[i].Id;
                                dept.Employees[id].OnPremises = true;
                                dept.Employees[id].PunchTime = data[i].PunchRound;
                            }
                        }
                    }
                    //var months = ["Jan ", "Feb ", "Mar ", "Apr ", "May ", "Jun ", "Jul ", "Aug ", "Sep ", "Oct ", "Nov ", "Dec "];
                    //alert("# of shifts = " + shifts.length + ", # of records = " + data.length);
                    var tblDate = new Date($("select[id*='hcDate']").val());
                    tblDate = new Date();
                    //var datestring = months[tblDate.getMonth()] + tblDate.getDate() + ", "  + tblDate.getFullYear() + " " +
                    //    tblDate.getHours() + ":" + tblDate.getMinutes();
                    //var dateString = getDate(new Date());
                    var lastCout = 0;
                    var empCount = 0;
                    var count = 0;
                    var total = 0;
                    var tbl = "<table class='employeeTable' cellpadding='0' cellspacing='0'><thead><tr><th colspan='3'>Head Count: " + tblDate.toLocaleTimeString("en-us", options) + "</th></tr></thead><tbody>";
                    var tblRow = "";
                    $.each(shifts, function (shiftIdx, shiftObj) {
                        count = 0;
                        tblRow = "";
                        $.each(shiftObj.Depts, function (deptIdx, deptObj) {
                            tblRow = "";//"<tr class='deptRow'><td colspan='3'>" + shiftObj.Name + " - " + deptObj.Name + "</td></tr>";
                            count = 0;
                            $.each(deptObj.Employees, function (empIdx, empObj) {
                                if (empObj.OnPremises == true) {
                                    count = count + 1;
                                    total = total + 1;
                                    empCount += 1;
                                    tblRow += "<tr class='empRow'><td>" + empCount + ". </td><td>" + empObj.Id + "</td><td>" + 
                                        empObj.Name + "</td><td>" + empObj.PunchTime.toLocaleTimeString("en-us", options) + "</td></tr>";
                                }
                            });
                            if (count > 0) {
                                if (count == 1) {
                                    tblRow += "<tr class='deptRow'><td colspan='2'>" + count + " Employee</td><td colspan='2'>" + shiftObj.Name + " - " + deptObj.Name + "</td></tr>";
                                }
                                else {
                                    tblRow += "<tr class='deptRow'><td colspan='2'>" + count + " Employees</td><td colspan='2'>" + shiftObj.Name + " - " + deptObj.Name + "</td></tr>";
                                }
                                tbl += tblRow;
                                count = 0;
                            }
                        });
                    });
                    if (total == 0) {
                        tbl += "<tr><td>No Employees on Premises</td></tr>";
                    }
                    tbl += "</tbody></table>";
                    $(".employeeTable").remove();
                    $("div[id*='headCountControlPanel']").append(tbl);
                },
                complete: function () {
                    $("#loading").remove();
                }
            });
        });
    });

