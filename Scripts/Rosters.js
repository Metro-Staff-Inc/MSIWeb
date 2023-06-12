    // Urls to access the WCF Rest service methods
    var GetActiveClients = "../Client/Active";
    var SayHello = "../Roster/Hello";
    var GetDispatchOffices = "../Roster/Dispatch";
    /* get shifts (1-7) belonging to a particular client */
    var GetClientShifts1 = "../Client/";
    var GetClientShifts2 = "/ShiftTypes";
    /* get departments belonging to a particular shift */
    var GetShiftDepts1 = "../Client/"
    var GetShiftDepts2 = "/GetDepartments?";
    /* get rosters belonging to a particular shift and department*/
    var ClientBaseURL = "../Client/";
    var ClientBaseURL2 = "http://localhost:52293/RestServiceWWW/Client/";
    var RosterBaseURL = "../Roster/";
    var RosterBaseURL2 = "http://localhost:52293/RestServiceWWW/Roster/";
    var EmployeeNameBaseURL = "../Roster/Name/";
    var EmployeeIdBaseURL = "../Roster/Id/";
    var data;

        function twoDigits(num) {
            var numStr = num.toString();
            if (numStr.length < 2)
                numStr = '0' + numStr;
            return numStr;
        }


        var optHours = "<option>01</option><option>02</option><option>03</option><option>04</option>" +
                    "<option>05</option><option>06</option><option>07</option><option>08</option><option>09</option>" +
                    "<option>10</option><option>11</option><option>12</option>";
        var optMins = "<option>00</option><option>05</option><option>10</option><option>15</option>" +
                    "<option>20</option><option>25</option><option>30</option><option>35</option><option>40</option>" +
                    "<option>45</option><option>50</option><option>55</option>";
        var optTOD = "<option>AM</option><option>PM</option>";

        function setTime(id) {
            var td = $('#' + id).find('td').eq(4);
            var sel = td.find('select');
            var inp = td.find('input');
            sel.eq(0).html(optHours);

            sel.eq(0).find('option:contains("' + inp.eq(0).val().substring(0, 2) + '")').attr('selected', 'selected');
            sel.eq(1).html(optMins);
            sel.eq(1).find('option:contains("' + inp.eq(0).val().substring(2, 4) + '")').attr('selected', 'selected');
            sel.eq(2).html(optTOD);
            sel.eq(2).find('option:contains("' + inp.eq(0).val().substring(4, 6) + '")').attr('selected', 'selected');
            sel.eq(3).html(optHours);
            sel.eq(3).find('option:contains("' + inp.eq(1).val().substring(0, 2) + '")').attr('selected', 'selected');
            sel.eq(4).html(optMins);
            sel.eq(4).find('option:contains("' + inp.eq(1).val().substring(2, 4) + '")').attr('selected', 'selected');
            sel.eq(5).html(optTOD);
            sel.eq(5).find('option:contains("' + inp.eq(1).val().substring(4, 6) + '")').attr('selected', 'selected');
        }

        function getDateFromString(dt) {
            var firstSlash = dt.indexOf("/");
            var secSlash = dt.indexOf("/", firstSlash + 1);
            dt =  new Date(dt.substring(secSlash + 1), dt.substring(0, firstSlash)-1, dt.substring(firstSlash + 1, secSlash));
            return dt;
        }
        
        function getDate(dt) {
            return new Date(parseInt(dt.replace("/Date(", "").replace(")/", ""), 10));
        }

        function formatDate(dt) {
            return "" + (dt.getMonth()+1) + "/" + dt.getDate() + "/" + dt.getFullYear();
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

        function getRosters(id, start, end) {
            var rosters = null;
            Url = RosterBaseURL + id + '/EmployeeRosters?startDate=' + start + '&endDate=' + start + '&clientID=' + $('#ddlClient').val();
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
                },
                error: function (msg) {
                }
            });
            //alert("End of get Rosters: " + data);
            return rosters;
        }

        function setTimes() {
            $('#tblCurRosters tbody tr').each(function () {
                var td = $(this).find('td').eq(4);
                var sel = td.find('select');
                var inp = td.find('input');
                sel.eq(0).html(optHours);
                sel.eq(0).find('option:contains("' + inp.eq(0).val().substring(0, 2) + '")').attr('selected', 'selected');
                sel.eq(1).html(optMins);
                sel.eq(1).find('option:contains("' + inp.eq(0).val().substring(2, 4) + '")').attr('selected', 'selected');
                sel.eq(2).html(optTOD);
                sel.eq(2).find('option:contains("' + inp.eq(0).val().substring(4, 6) + '")').attr('selected', 'selected');
                sel.eq(3).html(optHours);
                sel.eq(3).find('option:contains("' + inp.eq(1).val().substring(0, 2) + '")').attr('selected', 'selected');
                sel.eq(4).html(optMins);
                sel.eq(4).find('option:contains("' + inp.eq(1).val().substring(2, 4) + '")').attr('selected', 'selected');
                sel.eq(5).html(optTOD);
                sel.eq(5).find('option:contains("' + inp.eq(1).val().substring(4, 6) + '")').attr('selected', 'selected');
            })
        }
        function getAvailableEmployeesByName(name) {
            Url = EmployeeNameBaseURL + name;
            //alert(Url);
            $.ajax({
                cache: false,
                type: "GET",
                url: Url,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#availableEmp").html(msg);
                    $("#availableEmp").removeClass("Progress");
                    $("#tblAvailableEmployees").fixedHeaderTable({
                        width: '448',
                        footer: true,
                        cloneHeadToFoot: true,
                        altClass: 'odd',
                        themeClass: 'fancyTable',
                        footer: false,
                        cloneHeadToFoot: false,
                        fixedColumn: false,
                        autoShow: true
                    });
                },
                error: function (msg) {
                    alert("an unexpected error has occurred! " + msg.d);
                    $("#availableEmp").removeClass("Progress");
                }
            });
        }
        function getAvailableEmployeesByID() {
            id = $("#txtIdNum").val();
            Url = EmployeeIdBaseURL + id;
            $.ajax({
                cache: false,
                type: "GET",
                url: Url,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#availableEmp").html(msg);
                    $("#availableEmp").removeClass("Progress");
                    $("#tblAvailableEmployees").fixedHeaderTable({
                        width: '448',
                        footer: true,
                        cloneHeadToFoot: true,
                        altClass: 'odd',
                        themeClass: 'fancyTable',
                        footer: false,
                        cloneHeadToFoot: false,
                        fixedColumn: false,
                        autoShow: true
                    });
                },
                error: function (msg) {
                    alert("an unexpected error has occurred! " + msg.d);
                    $("#availableEmp").removeClass("Progress");
                }
            });
        }

        function dnrCheck( id, client, shift, dept, start, end, location){
            Url = ClientBaseURL + id + "/GetDnr?clientID=" + client + "&shiftID=" + shift + "&deptID=" + dept + 
                "&start=" + start + "&end=" + end + "&locationID=" + location;
            var success = "";
            $.ajax({
                cache: false,
                type: "GET",
                url: Url,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    jsonArray = msg;
                    if (jsonArray[0].AidentNumber == null)
                        success = "";
                    else
                        success = "Employee has been DNR'd: <br/><b>" + jsonArray[0].DNRReason + "</b>";
                },
                error: function (msg) {
                    alert("an unexpected error has occurred! " + msg.d);
                    $("#availableEmp").removeClass("Progress");
                }
            });
            return success;
        }

        function getReport() {
            /* add parameters to url line */
            var dept = $('#ddlDepartment').val();
            var loc = $('#ddlLoc').val();
            if ($("input:radio[name='dept']:checked").val() != 'cur') {
                dept = "0";
                loc = "0";
            }
            var url = "../Reports/roster.aspx?clientId=" + $('#ddlClient').val() +
                '&shiftType=' + $('#ddlShift').val() + '&locId=' + loc + '&deptId=' + dept + '&startDate=' + $('#startDate').val();
            window.open(url);
        }

        function printRosters() {
            $("<div id='printMenu'><h2>Print Client Roster</h2>" +
            "<hr/>" +
            "Client: " + $('#ddlClient option:selected').text() + "<span id='prnDate'>" + $('#startDate').val() + "</span>" + "<br/>" +
            "Shift: " + $('#ddlShift option:selected').text() + "<br/>" +
            "<input type='radio' name='dept' value='all'/>All Departments<br/>" +
            "<input type='radio' name='dept' checked='true' value='cur'/>Current Department Only<br/>" +
            "<input type='checkbox' disabled='true' name='temps' />Print Temporary Employees Only" +
            "</div>"
            ).dialog({
                //autoOpen: false,
                height: 300,
                width: 350,
                modal: true,
                buttons: {
                    "Print": function () {
                        getReport();
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    //allFields.val( "" ).removeClass( "ui-state-error" );
                }
                //alert("Feature not yet available for client!");
            });
        }

        function emailRosters() {
            if ($('#ddlClient').val() != 8) {
                alert("Feature not available for client!");
                return;
            }
            $("<div id='loading'><h3>please wait...</h3></div>").dialog();
            var start = getDateFromString($('#startDate').val());
            var temps = $("#temps").is(':checked');
            var shiftType = $('#ddlShift').val();
            //if( $('#
            Url = RosterBaseURL + "EmailSuncast?clientID=" + $('#ddlClient').val() + "&shiftType=" + $('#ddlShift').val() + "&deptID=0" + "&date=" + $('#startDate').val() + "&tempsOnly=" + temps;
            alert(Url);
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
                    $('#loading').remove();
                    $("<div id='" + divId + "'><h1>Roster emailed.</h1>" + msg + "</div>").dialog();
                    setTimeout(function () {
                        $('#' + divId).remove();
                    }, 20000);
                },
                error: function (msg) {
                    alert("an unexpected error has occurred! " + msg.d);
                    $("#availableEmp").removeClass("Progress");
                }
            });
            return success;
        }

        function Popup(data) {
            var mywindow = window.open('', 'my div2', 'height=400,width=600');
            mywindow.document.write('<html><head><title>my div</title>');
            /*optional stylesheet*/ //mywindow.document.write('<link rel="stylesheet" href="main.css" type="text/css" />');
            mywindow.document.write('</head><body >');
            mywindow.document.write(data);
            mywindow.document.write('</body></html>');

            mywindow.print();
            mywindow.close();

            return true;
        }

        function getCurrentDate() {
            var d = new Date();
            return (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
        }

        var conflict;
        function conflicts(id, startDate, endDate) {
            //alert(startDate + ', ' + endDate);
            var overrideOT = false;   // possibility to override conflict
            var overrideDays = false;   // possibility to override conflict

            conflict = false;
            var conflictMsg = "";

            /* is there a table? */
            if ($('#tblCurRosters').length <= 0) {
                conflictMsg = "No Roster List Available!";
                conflict = true;
            }

            /* is ID already on the table? */
            if (!conflict) {
                $('#tblCurRosters tr td:first-child').each(function () {
                    if ($(this).text().substring(2) == id) {
                        conflictMsg = "Employee already on roster!";
                        conflict = true;
                    }
                });
            }

            /* is Dispatch office selected? */
            if (!conflict && $('#ddlLocation').val() == -1) {
                conflictMsg = "No Dispatch Office Selected!";
                conflict = true;
            }

            /* is employee dnr'd at client or universally? */
            if (!conflict) {
                conflictMsg = dnrCheck(id, $('#ddlClient').val(), $('#ddlShift').val(), $('#ddlDepartment').val(), startDate, endDate, $('#ddlLoc').val());
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
                Url = ClientBaseURL + $("#ddlClient").val() +
                    "/RostersAt?aident=" + id + "&startDate=" +
                    startDateTime + "&endDate=" + endDateTime;
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: Url,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //alert(msg.length);
                        if (msg.length > 0) {
                            conflict = true;
                            conflictMsg = "Employee already on a roster at same client! -- " + msg;
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
                if ($('#ddlClient').val() == 92 || $('#ddlClient').val() == 181 || $('#ddlClient').val() == 340 ) {
                    conflictDaysMsg = getDaysWorked($('#ddlClient').val(), id);
                    if (conflictDaysMsg.length > 0 ) {
                        //alert("There were too many days...");
                        conflict = true;
                        overrideDays = true;
                    }
                }
            }

            if (conflict) {
                if (overrideDays) {
                    //alert("first the days...");
                    overrideDaysFunc(overrideOT, id, startDate, endDate, conflictDaysMsg, conflictOTMsg);
                }
                else if (overrideOT) {
                    overrideOTFunc(id, startDate, endDate, conflictOTMsg);
                }
                else {
                    $("<div>" + conflictMsg + "</div>").dialog();
                }
            }
                //alert(conflict);
            return conflict;
        }
        function overrideOTFunc(id, startDate, endDate, conflictOTMsg) {
            $("<div id='workConflict' title='Employee Already Working!'>" + conflictOTMsg + "</div>").dialog({
                close: function (event, ui) { $('#workConflict').remove(); },
                resizable: false,
                height: 320,
                width: 400,
                modal: false,
                buttons: {
                    "Add": function () {
                        addToRosterAction(id, startDate, endDate);
                        $('#workConflict').remove();
                        conflict = false;
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
        function overrideDaysFunc(overrideOT, id, startDate, endDate, conflictDaysMsg, conflictOTMsg) {
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
                        else {
                            addToRosterAction(id, startDate, endDate);
                            conflict = false;
                        }
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
                    startDate = this.formatDate(new Date(parseInt(startDate.getFullYear()) - 1, 08, 1));
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
                            retVal =  "";
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

            function placeInDB(id, loc, client, dept, office, shift, startDate, endDate) {
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
                        retVal = msg;
                    },
                    error: function (msg) {
                        alert("an unexpected error has occurred! " + msg.d);
                        //return 0;
                    }
                });
                return retVal;
            }
            function resetCount() {
                $('#totalCount').text(0);
                $('#directCount').text(0);
                $('#tempCount').text(0);
                reloadCount();
            }
            function reloadCount() {
                $('#tblCurRosters > tbody > tr').each(function () {
                    updateCount($(this).attr('id'), 1);
                });
            }

            function updateCount(id, step) {

                var el = '#' + id;

                /* get start and end times */
                start = new Date($(el + ">td:nth-child(3) input").val());
                end = new Date($(el + ">td:nth-child(4) input").val());
                //alert(start + ", " + end);
                var diff = end.getYear() - start.getYear();
                if (diff < 5)
                    updateTempCount(id, step);
                else {
                    updateDirectCount(id, step);
                }

                var totals = parseInt($('#totalCount').text(), 10);
                totals = totals + step;
                $('#totalCount').text(totals);
            }

            function updateDirectCount(id, step) {
                var directs = parseInt($('#directCount').text(), 10);
                directs = directs + step;
                $("#directCount").text(directs);
            }

            function updateTempCount(id, step) {

                var temps = parseInt($('#tempCount').text(), 10);
                temps = temps + step;
                $('#tempCount').text(temps);

            }

            function removeFromRoster(id) {
                /* get row the employee is on */
                var el = '#' + id;
                /* get start and end times */
                start = new Date($(el + ">td:nth-child(3) input").val());
                end = new Date($(el + ">td:nth-child(4) input").val());

                var sel = $(el + '>td:nth-child(5) select');
                h = sel.eq(0).val();
                m = sel.eq(1).val();
                if (sel.eq(2).val() == 'PM') {
                    //alert('PM!');
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
                if (sel.eq(5).val() == 'PM') {
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
                $(el).hide('fast', function () {
                    /* first remove from database */
                    //    alert("Start: " + start);
                    //    alert("End: " + end);
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
                            resetCount();
                            /* then remove when done */
                            $(el).remove();
                            $('#tblCurRosters tr').removeClass();
                            $("#tblCurRosters").fixedHeaderTable({
                                width: '800',
                                footer: true,
                                cloneHeadToFoot: true,
                                altClass: 'odd',
                                themeClass: 'fancyTable',
                                footer: false,
                                cloneHeadToFoot: false,
                                fixedColumn: false,
                                autoShow: true
                            });
                        },
                        error: function (msg) {
                            alert("an unexpected error has occurred! " + msg.d);
                            $("#availableEmp").removeClass("Progress");
                        }
                    });
                });
            }

            function addToRoster(id) {
                //alert("adding to roster!");
                $("<div id='loading'><h3>please wait...</h3></div>").dialog();
                var startDate = $('#startDate').val();
                /* calculate endDate based on radio buttons and drop down list value */

                var endDate = getDateFromString(startDate);
                startDate = getDateFromString(startDate);
                //endDate.setDate(startDate);
                //alert("Start Date = " + startDate + ", End Date = " + endDate);
                var interval = 0;
                interval = $("input[name='time']:checked").val();
                //alert("Interval = " + interval);
                if (interval == "day") {
                    interval = $("#ddlDays").val() * 1;
                }
                else if (interval == "week") {
                    interval = $("#ddlWeeks").val() * 7;
                }
                else {
                    interval = 365 * 200;    //200 years
                }
                if ($('#trackStart').text() < $('#trackEnd').text())
                    interval--;
                else if ($('#ddlShift').val() == 1)
                    startDate.setDate(startDate.getDate() - 1);
                //alert("Interval = " + interval + ", " + $('#trackStart').text() + ", " + $('#trackEnd').text());
                endDate.setDate(endDate.getDate() + interval);
                endDate = formatDate(endDate);
                startDate = formatDate(startDate);

                if (!conflicts(id, startDate, endDate)) {
                    addToRosterAction(id, startDate, endDate);
                }
                $('#loading').remove();
            }

            function addToRosterAction(id, startDate, endDate) {
                //alert("Add to roster: " + startDate + ", " + endDate);
                /* put employee w/id pnt onto the roster. */
                var el = "#" + id;
                var name = $(el).find("td:nth-child(2)").text();
                var dept = $('#ddlDepartment').val();
                var shift = $('#ddlShift').val();
                var office = $('#ddlLocation').val();
                var client = $('#ddlClient').val();
                var loc = $('#ddlLoc').val();

                /* add id to roster - new record */
                //alert("Place in Database: " + id + ", " + client + ", " + dept + ", " + office + ", " + shift + ", " + startDate + ", " + endDate);
                var cr_ID = placeInDB(id, loc, client, dept, office, shift, startDate, endDate);
                //alert("client roster id= " + cr_ID);
                if (cr_ID > 0) {
                    var divId = "div" + id;
                    $("<div id='" + divId + "'><h1>Added " + name + " to roster.</h1></div>").dialog();
                    setTimeout(function () {
                        $('#' + divId).remove();
                    }, 1500);

                    var s = $('#shiftStart').text();
                    var s_H = s.substring(0, 2);
                    var s_M = s.substring(3, 5);
                    var s_TOD = 'AM';
                    if (s_H >= 12) {
                        s_TOD = 'PM';
                        if (s_H > 12) {
                            s_H -= 12;
                            if (s_H <= 9)
                                s_H = '0' + s_H;
                        }
                    }
                    else if (s_H == 0) {
                        s_TOD = 'AM';
                        s_H = '12';
                    }
                    var e = $('#shiftEnd').text();
                    var e_H = e.substring(0, 2);
                    var e_M = e.substring(3, 5);
                    var e_TOD = 'AM';
                    if (e_H >= 12) {
                        e_TOD = 'PM';
                        if (e_H > 12) {
                            e_H -= 12;
                            if (e_H <= 9)
                                e_H = '0' + e_H;
                        }
                    }
                    else if (e_H == 0) {
                        e_TOD = '12';
                    }

                    /* add row */
                    var row = "<tr id='" + cr_ID + "'><td>" + $('#ddlLocation').val() + id + "</td><td>" +
                    name + "</td><td><input type='text' class='stDt' size='11' value='" + startDate +
                    "'/></td><td><input type='text' size='11' class='endDt' value='" + endDate + "'/></td>" +
                    //"<td> s:<input type='text' size='4' value='" + $('#shiftStart').text() + "'/><br/>e:<input type='text' size='4' value='" + $('#shiftEnd').text() + "'/></td>" +
                    "<td> s:<select></select>:<select></select><select></select><br/>e:<select></select>:<select></select><select></select><input type='hidden' value='" +
                    s_H + s_M + s_TOD + "'></input><input type='hidden' value='" + e_H + e_M + e_TOD + "'></input></td>";
                    if (client == 8) {
                        row += "<td><input type='text' id='sub_" + cr_ID + "' value='0' size='6' ></td>";
                    }
                    row = row +
                    "<td><input id='upd_" + cr_ID + "' type='button' value='Upd' onclick='updateLine(" + cr_ID + ")' disabled='disabled'></td>" +
                    "<td><input type='button' value='Del' onclick='deleteLine(" + cr_ID + ")' id='del_" + cr_ID + "'></td></tr>";
                    //alert(row);
                    $('#tblCurRosters > tbody:last').append(row);
                    setTime(cr_ID);
                    /* hide row */
                    $(el).hide('fast', function () {
                        /* then remove when done */
                        $(el).remove();
                        $('#tblAvailableEmployees tr').removeClass();
                        $("#tblAvailableEmployees").fixedHeaderTable({
                            width: '448',
                            footer: true,
                            cloneHeadToFoot: true,
                            altClass: 'odd',
                            themeClass: 'fancyTable',
                            footer: false,
                            cloneHeadToFoot: false,
                            fixedColumn: false,
                            autoShow: true
                        });
                    });
                    $('#tblCurRosters .stDt').datepicker();
                    $('#tblCurRosters .endDt').datepicker();

                    $('#currentEmp input').change(function () {
                        //alert("Hello!");
                        $(this).addClass('ValueChanged');
                        $(this).parent().parent().find('td input[id^="upd_"]').removeAttr('disabled');
                    });

                    $('#currentEmp select').change(function () {
                        $(this).addClass('ValueChanged');
                        $(this).parent().parent().find('td input[id^="upd_"]').removeAttr('disabled');
                    });

                    $('#tblCurRosters').fixedHeaderTable({
                        height: '300',
                        width: '800',
                        footer: true,
                        cloneHeadToFoot: true,
                        altClass: 'odd',
                        themeClass: 'fancyTable',
                        footer: false,
                        cloneHeadToFoot: false,
                        fixedColumn: false,
                        autoShow: true
                    });
                    resetCount();
                }
                else {
                    $("<div id='" + divId + "'><h1>Couldn't add " + name + " to roster. Maybe multiple AIDENT numbers.  Give Jonathan a call.</h1></div>").dialog();
                }
            }

            function getAvailableEmployeesByDept() {
                //alert("get available employees!");
                id = $("#ddlClient").val();
                Url = RosterBaseURL + id + "?numDaysStr=";
                Url += $("#ddlPrevious").val();
                Url += "&shiftType=" + $("#ddlShift").val();
                Url += "&dept=" + $("#ddlDepartment").val();
                Url += "&loc=" + $("#ddlLoc").val();
                alert(Url);
                //alert(Url);
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: Url,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $("#availableEmp").html(msg);
                        $("#availableEmp").removeClass("Progress");
                        $("#tblAvailableEmployees").fixedHeaderTable({
                            width: '448',
                            footer: true,
                            cloneHeadToFoot: true,
                            altClass: 'odd',
                            themeClass: 'fancyTable',
                            footer: false,
                            cloneHeadToFoot: false,
                            fixedColumn: false,
                            autoShow: true
                        });
                    },
                    error: function (msg) {
                        alert("an unexpected error has occurred! " + msg.d);
                        $("#availableEmp").removeClass("Progress");
                    }
                });
            }

            function getClientRosters() {
                client = $("#ddlClient").val();
                shift = $("#ddlShift").val();
                dept = $("#ddlDepartment").val();
                var loc = $("#ddlLoc").val();
                stDt = $("#startDate").val();
                endDt = stDt; //$("#endDate").val();
                Url = ClientBaseURL + client + "/Rosters?loc=" + loc + "&startDate=" + stDt + "&endDate=" + endDt + "&dept=" + dept + "&shift=" + shift;
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: Url,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $("#currentEmp").html(msg);
                        $("#currentEmp").removeClass("Progress");
                        setTimes();
                        $('#tblCurRosters').fixedHeaderTable({
                            height: '300',
                            width: '800',
                            footer: true,
                            cloneHeadToFoot: true,
                            altClass: 'odd',
                            themeClass: 'fancyTable',
                            footer: false,
                            cloneHeadToFoot: false,
                            fixedColumn: false,
                            autoShow: true
                        });
                        $('.stDt').datepicker();
                        $('.endDt').datepicker();
                        $('#currentEmp input').change(function () {
                            //alert("Hello!!!!!");
                            $(this).addClass('ValueChanged');
                            $(this).parent().parent().find('td input[id^="upd_"]').removeAttr('disabled');
                        });
                        $('#currentEmp select').change(function () {
                            //alert("Hello!!!!!");
                            $(this).addClass('ValueChanged');
                            $(this).parent().parent().find('td input[id^="upd_"]').removeAttr('disabled');
                        });
                        $('#currentEmp input[id^="upd_"]').attr('disabled', 'true');
                        $('#btnEmail').removeAttr('disabled');

                        resetCount();
                    },
                    error: function (msg) {
                        alert("an unexpected error has occurred! " + msg.d);
                        $("#currentEmp").removeClass("Progress");
                    }
                });
            }

            function setLocs(client) {
                /* populate client ddl when page is loaded */
                var jsonArray = new Array();
                var $ddlLoc = $("#ddlLoc");
                Url = ClientBaseURL + $('input[id$="userID"]').val() + "/Locations?clientID=" + client;
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
                        $($ddlLoc).empty();
                        var opt = "";
                        for (var i = 0; i < jsonArray.length; i++) {
                            opt += "<option value=\"" + jsonArray[i].LocationID + "\">" + jsonArray[i].LocationName + "</option>";
                        }
                        $ddlLoc.html(opt);
                        setShift(client, $("#ddlLoc").val());
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });
            }


            function setShift(client, location) {
                //alert(client + ", " + location);
                Url = GetClientShifts1 + client + "/ShiftsByLocation?loc=" + location;
                $("#ddlShift").find("option").remove();
                $.ajax({
                    cache: false,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    url: Url,
                    success: function (jsonData) {
                        jsonArray = jsonData;
                        /* fill out shift drop down */
                        var opt = "";
                        for (var i = 0; i < jsonArray.length; i++) {
                            opt += "<option value=\"" + jsonArray[i].ShiftTypeId + "\">" + jsonArray[i].ShiftTypeDesc + "</option>";
                        }
                        $('#ddlShift').html(opt);
                        //alert("shift ddl loaded!");
                        setDepartment(client, location, $("#ddlShift").val());
                        /* set shift accordingly */
                        //setShift($("#ddlShift").val());
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });
            }
            function setDatesAndTrim(id) {
                $('<div id="dialog-trim" title="Punch data exists!"><p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Roster can\'t be modified to given dates because of existing punches. Move ending date of roster to last punch?</p></div>'
         ).dialog({
             resizable: false,
             height: 240,
             modal: false,
             buttons: {
                 "Trim": function () {
                     retPE = punchesExist(id);
                     //alert(retPE);
                     var lp = new Date(retPE);
                     var dt = lp.getFullYear() + "-" + (lp.getMonth() + 1) + "-" + lp.getDate() + " " + lp.getHours() + ":" + lp.getMinutes();
                     Url = RosterBaseURL + "Trim/" + id;
                     $.ajax({
                         cache: false,
                         type: "GET",
                         contentType: "application/json; charset=utf-8",
                         async: false,
                         dataType: "json",
                         url: Url,
                         success: function (data) {
                             el = "#" + id;
                             $("#" + id + ">td:nth-child(3) input").val(data.substring(5, 7) + '/' + data.substring(8, 10) + '/' + data.substring(0, 4));
                             $("#" + id + ">td:nth-child(4) input").val(data.substring(15, 17) + '/' + data.substring(18, 20) + '/' + data.substring(10, 14));
                             $(el).find('input').removeClass('ValueChanged');
                             $(el).find('input[id^="upd_"]').attr('disabled', 'true');
                         },
                         error: function (xhr) {
                             alert("ERROR!!!" + xhr.responseText);
                         }
                     });

                     $(this).dialog("close");
                 },
                 Cancel: function () {
                     $(this).dialog("close");
                 }
             }
         });
            }

            function trimRoster(id, retPE) {
                $('<div id="dialog-trim" title="Punch data exists!"><p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Roster can\'t be deleted because of existing punches.Trim dates of roster to punches?</p></div>'
         ).dialog({
             resizable: false,
             height: 240,
             modal: false,
             buttons: {
                 "Trim": function () {
                     var lp = new Date(retPE);
                     var dt = lp.getFullYear() + "-" + (lp.getMonth() + 1) + "-" + lp.getDate() + " " + lp.getHours() + ":" + lp.getMinutes();
                     Url = RosterBaseURL + "Trim/" + id;
                     $.ajax({
                         cache: false,
                         type: "GET",
                         contentType: "application/json; charset=utf-8",
                         async: false,
                         dataType: "json",
                         url: Url,
                         success: function (data) {
                             //alert(data);
                             //$("#" + id + ">td:nth-child(4) input").val((lp.getMonth() + 1) + "/" + lp.getDate() + "/" + lp.getFullYear());
                             $("#" + id + ">td:nth-child(3) input").val(data.substring(5, 7) + '/' + data.substring(8, 10) + '/' + data.substring(0, 4));
                             $("#" + id + ">td:nth-child(4) input").val(data.substring(15, 17) + '/' + data.substring(18, 20) + '/' + data.substring(10, 14));
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
                //alert("DELETE!");
                var retPE = "";
                $('<div id="dialog-confirm" title="Delete Roster Row?"><p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Permanently delete roster record for ' +
            $("#" + id + ">td:nth-child(2)").text() + '?</p></div>'
         ).dialog({
             resizable: false,
             height: 240,
             modal: false,
             buttons: {
                 "Delete": function () {
                     $(this).dialog("close");
                     retPE = punchesExist(id);
                     if (retPE.length == 0) {
                         //alert(id);
                         removeFromRoster(id);
                     }
                     else {
                         trimRoster(id, retPE);
                     }
                 },
                 Cancel: function () {
                     $(this).dialog("close");
                 }
             }
         });

                //var punches = $('<div>Delete ' + 'Jonathan Murfey' + ' from roster?</div>').dialog();
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

            function updateLine(crId) {
                //alert("Update!");
                /* get row the employee is on */
                var el = '#' + crId;
                /* get start and end times */
                var start = new Date($(el + ">td:nth-child(3) input").val());
                var end = new Date($(el + ">td:nth-child(4) input").val());
                //alert( "hello!");
                var id = $(el + ">td:nth-child(1)").text().substring(2);
                //alert(id);

                var subs = 0;
                if ($('#sub_' + crId).length > 0)
                    subs = $('#sub_' + crId).val();
                //alert(sub);
                //startTime = $(el + '>td:nth-child(5) input').eq(0).val();
                //endTime = $(el + '>td:nth-child(5) input').eq(1).val();

                var sel = $(el + '>td:nth-child(5) select');
                var h = sel.eq(0).val();
                var m = sel.eq(1).val();
                if (sel.eq(2).val() == 'PM') {
                    //alert('PM!');
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
                if (sel.eq(5).val() == 'PM') {
                    if (parseInt(h, 10) < 12)
                        h = parseInt(h, 10) + 12;
                }
                else {
                    if (parseInt(h, 10) == 12)
                        h = '00';
                }
                var endTime = h + ":" + m;
                //alert(startTime + ", " + endTime);

                var tStart = $('#trackStart').text();
                var tEnd = $('#trackEnd').text();

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
                var t = dnrCheck(id, $('#ddlClient').val(), $('#ddlShift').val(), $('#ddlDepartment').val(),
                            $(el + ">td:nth-child(3) input").val(), $(el + ">td:nth-child(4) input").val(), $('#ddlLoc').val());

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
                                    $(el).find('input').removeClass('ValueChanged');
                                    $(el).find('select').removeClass('ValueChanged');
                                    $(el).find('input[id^="upd_"]').attr('disabled', 'true');
                                },
                                error: function (xhr) {
                                    alert("ERROR!!!" + xhr.responseText);
                                }
                            });
                        /*}
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });
                */
            }

            function setShiftTimes() {
                clientid = $('#ddlClient').val();
                shiftType = $('#ddlShift').val();
                dept = $('#ddlDepartment').val();
                var loc = $('#ddlLoc').val();
                Url = ClientBaseURL + clientid + "/ShiftTime?loc=" + loc + "&shiftType=" + shiftType + "&dept=" + dept;
                $.ajax({
                    cache: false,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    url: Url,
                    success: function (jsonData) {
                        //alert(jsonData);
                        start = jsonData.substring(10, 15);
                        end = jsonData.substring(15, 20);
                        $('#shiftStart').text(jsonData.substring(0, 5));
                        $('#shiftEnd').text(jsonData.substring(5, 10));
                        $('#trackStart').text(start);
                        $('#trackEnd').text(end);
                        $('#break').text(jsonData.substring(20));
                        //alert("start = " + start + ", end = " + end);
                        if (start >= end) {
                            start = $('#startDate').val();
                            end = $('#endDate').val();
                            //alert("start = " + start + ", end = " + end);
                            if (start == end) {

                                date = new Date((new Date(start)).getFullYear(), (new Date(start)).getMonth(), (new Date(start)).getDate() + 1);
                                var month = date.getMonth() + 1;
                                if (month < 10)
                                    month = '0' + month;
                                var dt = date.getDate();
                                if (dt < 10)
                                    dt = '0' + dt;
                                var strDate = (month + "/" + dt + "/" + date.getFullYear());
                                $('#endDate').val(strDate);
                                $("<div id='timeoutDiv'><h1>Two day shift!  Extending the ending date.</h1></div>").dialog();
                                setTimeout(function () {
                                    $('#timeoutDiv').remove();
                                }, 1500);
                            }
                        }
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });
            }
            var deptArray;
            function setDepartment(client, loc, shift) {
                //alert("dept = " + shift);
                Url = GetShiftDepts1 + client + GetShiftDepts2 + "locationID=" + loc + "&shiftType=" + shift;
                $.ajax({
                    cache: false,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    url: Url,
                    success: function (jsonData) {
                        deptArray = jsonData;
                        /* fill out shift drop down */
                        var opt = "";
                        for (var i = 0; i < deptArray.length; i++) {
                            opt += "<option value=\"" + deptArray[i].DepartmentID + "\">" + deptArray[i].DepartmentName + "</option>";
                        }
                        $('#ddlDepartment').html(opt);
                        setShiftTimes();
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });
            }

            jQuery.fn.center = function () {
                this.css("position", "absolute");
                this.css("top", (($(window).height() - this.outerHeight()) / 2) +
                                                $(window).scrollTop() + "px");
                this.css("left", (($(window).width() - this.outerWidth()) / 2) +
                                                $(window).scrollLeft() + "px");
                return this;
            }

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

            jQuery(document).ready(function () {
                webServiceLoc = $('input[id$="webServiceLoc"]').val();
                //alert(webServiceLoc);

                // Urls to access the WCF Rest service methods
                GetActiveClients = "../Client/Active";
                SayHello = "../Roster/Hello";
                GetDispatchOffices = "../Roster/Dispatch";
                /* get shifts (1-7) belonging to a particular client */
                GetClientShifts1 = "../Client/";
                GetClientShifts2 = "/ShiftTypes";
                /* get departments belonging to a particular shift */
                GetShiftDepts1 = "../Client/"
                GetShiftDepts2 = "/GetDepartments?";
                /* get rosters belonging to a particular shift and department*/
                ClientBaseURL = "../Client/";
                RosterBaseURL = "../Roster/";
                EmployeeNameBaseURL = "../Roster/Name/";
                EmployeeIdBaseURL = "../Roster/Id/";

                $("<div id='loading'><h3>please wait...</h3></div>").dialog();

                $('#startDate').datepicker({
                    onClose: function (dateText, event) {
                        var sDate = new Date(dateText);
                        var eDate = new Date($('#endDate').val());
                        //alert(sDate + ", " + eDate);
                        if (sDate > eDate || (sDate * 1 == eDate * 1 && $('#trackEnd').text() < $('#trackStart').text())) {
                            sDate.setDate(eDate.getDate() - 1);
                            $('#startDate').val(getTextDate(sDate));
                        }
                    }
                });
                $("#endDate").datepicker({
                    onClose: function (dateText, event) {
                        var sDate = new Date($('#startDate').val());
                        var eDate = new Date(dateText);
                        if (sDate > eDate || (sDate * 1 == eDate * 1 && $('#trackEnd').text() < $('#trackStart').text())) {
                            eDate.setDate(sDate.getDate() + 1);
                            $('#endDate').val(getTextDate(eDate));
                        }
                    }
                });

                $('#startDate').val(getTextDate(new Date()));
                $('#endDate').val(getTextDate(new Date()));

                $('#txtIdNum').keypress(function (event) {
                    if (event.which == 13)
                        $('#btnIdNum').trigger('click');
                });
                $('#txtName').keypress(function (event) {
                    if (event.which == 13)
                        $('#btnName').trigger('click');
                });

                $("#btnIdNum").click(function (e) {
                    e.preventDefault();
                    $("#availableEmp").addClass("Progress");
                    $("#availableEmp").html("");
                    getAvailableEmployeesByID();
                });

                $('#btnEmail2').click(function (e) {
                    e.preventDefault();

                    $('#tblCurRosters tbody tr').each(function (i) {
                        var empID = $(this).find('td').eq(0).text();
                        var empName = $(this).find('td').eq(1).text();
                        var row = "<tr class='printEmailEmployee'><td>" + (i + 1) + "</td><td>" + empID + "</td><td>" + empName + "</td>";
                        row += "<td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                        $('#printEmailEmployees').append(row);
                    });
                    $('#printEmail').center();
                    $('#printEmail').show();
                });

                $('#printEmailClose').click(function (e) {
                    e.preventDefault();
                    $('#printEmailEmployees').empty();
                    $('#printEmail').hide();
                });

                $('#btnName').click(function (e) {
                    e.preventDefault();
                    $('#availableEmp').addClass("Progress");
                    $('#availableEmp').html('');
                    getAvailableEmployeesByName($('#txtName').val());
                });

                $("#btnSameDept").click(function (e) {
                    e.preventDefault();
                    $("#availableEmp").addClass("Progress");
                    $("#availableEmp").html("");
                    getAvailableEmployeesByDept();
                });

                $("#btnCurEmployees").click(function (e) {
                    e.preventDefault();
                    $("#currentEmp").addClass("Progress");
                    $("#currentEmp").html("");
                    //alert("Here we are!");
                    getClientRosters();
                });

                /* set up dispatch ddl */
                //alert("adding dispatch offices!");
                $.ajax({
                    cache: false,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    dataType: "json",
                    url: GetDispatchOffices,
                    success: function (data) {
                        $("#ddlLocation").html(data);
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });

                /* populate client ddl when page is loaded */
                var jsonArray = new Array();
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
                        for (var i = 0; i < jsonArray.length; i++) {
                            opt += "<option value=\"" + jsonArray[i].ClientID + "\">" + jsonArray[i].ClientName + "</option>";
                        }
                        $('#ddlClient').html(opt);
                        $('#ddlClient').change(function () {
                            $('#currentEmp').empty();
                        });
                        $('#loading').remove();
                        /* set shift accordingly */
                        setLocs($("#ddlClient").val());
                    },
                    error: function (xhr) {
                        alert("ERROR!!!" + xhr.responseText);
                    }
                });


                $("#ddlClient").change(function () {
                    $("<div id='loading'><h3>please wait...</h3></div>").dialog();
                    setLocs($(this).val());
                    $('#loading').remove();
                });

                $("#ddlLoc").change(function () {
                    $("<div id='loading'><h3>please wait...</h3></div>").dialog();
                    setShift($("#ddlClient").val(), $(this).val());
                    $('#loading').remove();
                });

                $("#ddlShift").change(function () {
                    $("<div id='loading'><h3>please wait...</h3></div>").dialog();
                    setDepartment($("#ddlClient").val(), $("#ddlLoc").val(), $(this).val());
                    $('#loading').remove();
                });

                $('#ddlDepartment').change(function () {
                    $("<div id='loading'><h3>please wait...</h3></div>").dialog();
                    $('#currentEmp').empty()
                    setShiftTimes();
                    $('#loading').remove();
                });
                //alert();
            });
        