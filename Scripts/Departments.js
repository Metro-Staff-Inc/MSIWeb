var ClientBaseURL = "../Client/";
var RosterBaseURL = "../Roster/";
jQuery(document).ready(
    function () {
        Url = ClientBaseURL + "DepartmentInfo?clientId=" + $("input[id*='clientID']").val();
        //alert(Url);
        $.ajax({
            cache: false,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            async: true,
            dataType: "json",
            url: Url,
            success: function (depts) {
                buildTables(depts);
                $('#tblDepartments').fixedHeaderTable({
                    height: '500',
                    width: '1000',
                    footer: true,
                    cloneHeadToFoot: true,
                    altClass: 'odd',
                    themeClass: 'fancyTable',
                    footer: true,
                    cloneHeadToFoot: false,
                    fixedColumn: false,
                    autoShow: true
                });
            },
            error: function (xhr) {
                alert("ERROR!!!" + xhr.responseText);
            }
        });

        function buildTables(depts) {
            var dTable = "<table id='tblDepartments'>";
            //header info
            dTable += "<thead><tr>";
            dTable += "<th>Index</th>" + "<th>Location</th>" + "<th>Department</th>" + "<th>Shift</th>" + "<th>Tracking Start</th>" + "<th>Shift Start</th>" +
                        "<th>Shift End</th>" + "<th>Tracking End</th>" + "<th>Hourly Pay</th>";
            dTable += "</tr></thead>";
            dTable += "<tbody>";
            var indexSuffix = [".", "B.", "C.", "D."];
            for (i = 0; i < depts.length; i++) {
                for (j = 0; j < depts[i].PayRate.length; j++) {
                    if (!depts[i].Active)
                        dTable += "<tr class='inactive'>";
                    else
                        dTable += "<tr>";
                    dTable += "<td>" + (i + 1) + indexSuffix[j] + "</td><td>" + depts[i].LocationName + "</td><td>" + depts[i].DepartmentName +
                        "</td><td>" + depts[i].ShiftType + "</td><td>" + depts[i].TrackingStart + "</td><td>" + depts[i].ShiftStart + "</td><td>" + depts[i].TrackingEnd + "</td><td>" + depts[i].ShiftEnd + "</td><td>$" +
                        depts[i].PayRate[j].HourlyRate.toFixed(2) + "</td></tr>";
                }
            }
            dTable += "<tr><td colspan='8'>Create New Department</td><td><input type='button' value='NEW DEPARTMENT'></input></td></tr>";
            dTable += "</tbody>";
            dTable += "</table>";
            jQuery("#depts").append(dTable);
            jQuery("#depts > table > tbody > tr").hover(
                function () {
                    myClass = $(this).attr("class");
                    $(this).addClass("highLight").removeClass(myClass);
                }, function () {
                    $(this).removeClass("highLight").addClass(myClass);
                }
            );

            jQuery("#depts > table > tbody > tr").click(function () {
                var nr = $(this).closest('tr').next('tr');
                if (nr.hasClass("details") == false) {
                    $('<tr class="details"></tr>').insertAfter($(this).closest('tr'));

                    nr = $(this).closest('tr').next('tr');
                    var iTable = "<table style='width:660px'>";
                    iTable += "<thead><tr><th colspan='2'>Update Department Info</th><th><input type='button' value='UPD'></input></th></tr></thead>";
                    iTable += "<tbody>";
                    iTable += "<tr><td>Department Name</td><td colspan='2'><input value='" + $(this).find('td').eq(2).html() + "'></input></td></tr>";
                    iTable += "<tr><td>Shift</td><td colspan='2'><input value='" + $(this).find('td').eq(3).html() + "'></input></td></tr>";
                    iTable += "<tr><td>Tracking Start</td><td colspan='2'><input value='" + $(this).find('td').eq(4).html() + "'></input></td></tr>";
                    iTable += "<tr><td>Shift Start</td><td colspan='2'><input value='" + $(this).find('td').eq(5).html() + "'></input></td></tr>";
                    iTable += "<tr><td>Shift End</td><td colspan='2'><input value='" + $(this).find('td').eq(6).html() + "'></input></td></tr>";
                    iTable += "<tr><td>Tracking End</td><td colspan='2'><input value='" + $(this).find('td').eq(7).html() + "'></input></td></tr>";
                    iTable += "<tr><td>Pay Rate</td><td colspan='2'><input value='" + $(this).find('td').eq(8).html() + "'></input></td></tr>";
                    iTable += "</tbody>";
                    iTable += "</table>";
                    nr.html("<td colspan = '9'>" + iTable + "</td>");
                }
                else {
                    nr.remove();
                }
            });
        };
    }
);