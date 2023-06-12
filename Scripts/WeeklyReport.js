/* shift data */
    var shiftData;
    var ClientBaseURL =  "../Client/";
    var ClientBaseURL2 = "http://localhost:52293/RestServiceWWW/Client/";
    var RosterBaseURL2 = "../Roster/"; // "http://localhost:52293/RestServiceWWW/Roster/";
    var Url;
    var clientID;
    var userID;
    var report;

    function loadWeeklyReport() {
         //alert("Here we go!");
         Url = ClientBaseURL2 + clientID + "/WeeklyReport?id=" + userID + "&start=" + $('#weekStart').val() + "&end=" + $('#weekEnd').val();
         //alert("URL = " + Url);
         $.ajax({
             cache: false,
             type: "GET",
             contentType: "application/json; charset=utf-8",
             async: false,
             dataType: "json",
             url: Url,
             success: function (reportIn) {
                 report = reportIn;
                 $("#report").html(buildMainReport(report));
                 //$("#reportTable").flexigrid();
             },
             error: function () {
                 alert("Failure!");
             }
         });
     }

     function empExpand(sId, eId) {
         if ($('#punchDiv' + eId).length > 0) {
             toggle('punchDiv' + eId);
             return;
         }
         var punchTable = '<div id="punchDiv' + eId + '" style="width:1000px">';
         punchTable += '<table id="punchTable" border="1"><thead><tr></tr></thead>';
         punchTable += '<tbody>';
         for (i = 0; i < report.shifts[sId].Employees[eId].Days.length; i++) {
             for (j = 0; j < report.shifts[sId].Employees[eId].Days[i].Punches.length; j+=2) {
                 punchTable += '<tr>';
                 punchTable += '<td>Check In:</td><td>' + report.shifts[sId].Employees[eId].Days[i].Punches[j].RoundedDate + '</td>';
                 punchTable += '<td>Check In Exact:</td><td>' + report.shifts[sId].Employees[eId].Days[i].Punches[j].ExactDate + '</td>';
                 punchTable += '<td>Check Out:</td><td>' + report.shifts[sId].Employees[eId].Days[i].Punches[j+1].RoundedDate + '</td>';
                 punchTable += '<td>Check Out Exact:</td><td>' + report.shifts[sId].Employees[eId].Days[i].Punches[j+1].ExactDate + '</td>';
                 punchTable += '</tr>';
             }
         }
         punchTable += '</tbody></table></div>';
         //alert(punchTable);
         //$('#emp' + sId + '_' + eId).after('<tr><td colspan="11">' + punchTable + '</td></tr>');
         $('#emp' + sId + '_' + eId).after(punchTable);
     }
     
     function toggle(d) {
         var o = document.getElementById(d);
         o.style.display = (o.style.display == 'none') ? 'block' : 'none';
     }
     
     function shiftExpand(idx) {
         
         if ($('#empDiv' + idx).length > 0) {
             toggle('empDiv' + idx);
             return;
         }

         var employeeTable = '<div id="empDiv' + idx + '" style="width:1000px">';
         employeeTable += '<table id="empTable" border="1"><thead><tr><th>Badge</th><th>Name</th><th>Mon</th><th>Tue</th><th>Wed</th><th>Thr</th><th>Fri</th><th>Sat</th><th>Sun</th><th>Reg</th><th>OT</th><th>Total</th></tr></thead>';
         employeeTable += '<tbody>';
         for (i = 0; i < report.shifts[idx].Employees.length; i++) {
             
             employeeTable += '<tr id="emp' + idx + '_' + i + '"><td>' + report.shifts[idx].Employees[i].Badge + '</td><td onclick="empExpand(' + idx + ', ' + i + ')">' + report.shifts[idx].Employees[i].Name + '</td>';
             
             for (j = 0; j < report.shifts[idx].Employees[i].Days.length; j++) {
                 employeeTable += '<td title=' + report.shifts[idx].Employees[i].Days[j].Exact + '>' + report.shifts[idx].Employees[i].Days[j].Rounded + '</td>';
             }

             employeeTable += '<td>' + report.shifts[idx].Employees[i].Reg + '</td>';
             employeeTable += '<td>' + report.shifts[idx].Employees[i].OT + '</td>';
             employeeTable += '<td>' + report.shifts[idx].Employees[i].Total + '</td>';

             employeeTable += '</tr>';
         }
         employeeTable += '</tbody></table></div>';
         //alert($("#reportTable > tbody > tr #rpt" + idx));
         $("#rpt" + idx).after('<tr><td>' + employeeTable + '</td></tr>');
     }

    function buildMainReport(report) {
        alert("building report!");
        var shiftHTML = "<div id=\'reportDiv\'>";
        shiftHTML += '<ul id="mainReport"><span>Weekly Report</span>';
        /* build shift sections */
        var count = 1;
        for (i in report.shifts) {
            shiftHTML += '<li id="rpt' + i + '" onclick="shiftExpand(' + i + ')">' + report.shifts[i].Desc + '</li>';
        }
        shiftHTML += '</ul>';
        shiftHTML += '</div>';
        //alert(shiftHTML);
        return shiftHTML;
    }

function setDate(dt) {
    var m = dt.getMonth() + 1;
    if (m < 10) m = "0" + m;
    var d = dt.getDate();
    if (d < 10) d = "0" + d;
    var y = dt.getFullYear();
    return m + "/" + d + "/" + y;
}

jQuery(document).ready(function () {
    //alert("Starting up!");
    $('#btnWeekEnd').click(function () {
        loadWeeklyReport();
    });
    userID = $('input[id*="userID"]').val();
    clientID = $('input[id*="clientID"]').val();
    //alert(userID + ", " + clientID);
    $('#weekEnd').datepicker({
        onSelect: function (dateText, inst) {
            var dt = new Date(dateText);
            while (dt.getDay() != 0) {
                dt.setDate(dt.getDate() + 1);
            }
            $(this).val(setDate(dt));
        }
    });




    $('h3').click(function () {
        $(this).next('table').toggle();
    });

    $('.level3').click(function () {
        //alert("Hello!");
        $(this).next('tr').toggle();
    });
    $('#shifts h3').css('background-color', 'LightBlue');
});
