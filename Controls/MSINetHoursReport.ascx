   <%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetHoursReport.ascx.cs" Inherits="MSI.Web.Controls.MSINetHoursReport" %>
    
    <asp:Panel ID="pnlJSCode" Visible="false" runat="server" class="w-full max-w-6xl mx-auto">
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/flexigrid.pack.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/flexigrid.pack.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox.pack.js" type="text/javascript"></script>

    <link href="../Scripts/jQuery-modalPopLite/modalPopLite.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jQuery-modalPopLite/modalPopLite.min.js" type="text/javascript"></script>
        <script type="text/javascript" src="../javascriptOOXml/linq.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml-extensions.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-load.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-inflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-deflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/FileSaver.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/openxml.js"></script>
        <script src="../javascriptOOXml/TemplateDocumentB64.js" type="text/javascript"></script>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
        <script type="text/javascript" src="../Scripts/timeline.js"></script>
        <script type="text/javascript" src="../Scripts/HoursReport.js"></script>
    <style type="text/css">
    #popup
    {
        position:absolute;
        width:auto;
        height:200px;
        border:6px double #98C73A;
        border-radius:12px; /*css3"../Client/"*/
        /*visibility:visible;*/
        display:none;
        background-color:LightYellow;
    }
    .ttBtn
    {
        font-size:13px;
        background-color: #98C73A;
        color: white;
        font-weight: 500;
        border-radius: 0.375rem;
        padding: 0.5rem 1rem;
    }
    .ttBtn:hover {
        opacity: 0.9;
    }
    .ttDate
    {
        font-size:13px;
        width: 100%;
        border-radius: 0.375rem;
        border-color: #d1d5db;
        box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
        padding: 0.5rem 0.75rem;
    }
    .ttDate:focus {
        --tw-ring-color: #98C73A;
        --tw-ring-offset-shadow: var(--tw-ring-inset) 0 0 0 var(--tw-ring-offset-width) var(--tw-ring-offset-color);
        --tw-ring-shadow: var(--tw-ring-inset) 0 0 0 calc(2px + var(--tw-ring-offset-width)) var(--tw-ring-color);
        box-shadow: var(--tw-ring-offset-shadow), var(--tw-ring-shadow), var(--tw-shadow, 0 0 #0000);
        border-color: #98C73A;
    }
    .underLine
    {
        text-decoration:underline;
        color: #98C73A;
    }
    #popup h1
    {
        margin-left:12px;
        margin-right:12px;
        font-size:1.5rem;
        font-weight:500;
        color:#222222;
        border-bottom:1px solid #e5e7eb;
        padding-bottom:0.5rem;
        margin-bottom:0.5rem;
    }
    #popup h2
    {
        margin-left:12px;
        margin-right:12px;
        font-size:1.25rem;
        font-weight:500;
        color:#222222;
        margin-bottom:0.5rem;
    }
    #addPunchDetail
    {
        background-color:#98C73A;
        color:white;
        font-size:1.25rem;
        font-weight:500;
        margin-left:16px;
        padding:0.5rem 1rem;
        border-radius:0.375rem;
    }
    .popupClose
    {
        padding:8px;
        float:right;
        cursor:pointer;
    }
    .tooltip
    {
        position: absolute;
        border: 4px solid #666666;
        z-index: 2;
        background-color: #f9fafb;
        padding: 0.5rem 1rem;
        border-radius: 0.375rem;
        box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
    }
    .popupClose
        .popupCloseBtn
        {
            width: 16px;
            height: 16px;
            border-radius: 9999px;
            padding: 2px;
            background-color: #f3f4f6;
            transition: all 0.2s;
        }
        
    .popupClose .popupCloseBtn:hover {
        background-color: #e5e7eb;
        transform: scale(1.1);
    }
    .hover
    {
        border:2px solid #98C73A;
        border-radius: 0.375rem;
    }
    .Detail
    {
        width:1000px;
        max-width: 100%;
        margin: 0 auto;
    }
    
    .tblDetail {
        width: 100%;
        max-width: 6xl;
        margin: 0 auto;
        border-radius: 0.5rem;
        box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
        overflow: hidden;
        border: 1px solid #e5e7eb;
    }
    
    .tblDetail th {
        background-color: #666666;
        color: white;
        font-size: 0.875rem;
        font-weight: 500;
        text-transform: uppercase;
        letter-spacing: 0.05em;
        padding: 0.75rem 1.5rem;
        text-align: left;
    }
    
    .tblDetail tr {
        border-bottom: 1px solid #e5e7eb;
    }
    
    .tblDetail tr:last-child {
        border-bottom: 0;
    }
    
    .tblDetail tr:nth-child(even) {
        background-color: #f9fafb;
    }
    
    .tblDetail tr:hover {
        background-color: #f3f4f6;
    }
    
    .tblDetail td {
        padding: 1rem 1.5rem;
        font-size: 0.875rem;
        color: #222222;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
    #tblApprove th
    {
        font-weight:bolder;
        background-color: #666666;
        color: white;
        text-transform: uppercase;
        letter-spacing: 0.05em;
        font-size: 0.875rem;
        padding: 0.75rem 1.5rem;
        text-align: left;
    }
    #tblApprove, #tblApprove th, #tblApprove td
    {
        border: 1px solid #e5e7eb;
        border-collapse: collapse;
    }
    #tblApprove th, #tblApprove td
    {
        padding: 1rem 1.5rem;
    }
    #tblApprove tr:nth-child(even) {
        background-color: #f9fafb;
    }
    #tblApprove tr:hover {
        background-color: #f3f4f6;
    }
    #tblApprove tr:last-child {
        border-bottom: 0;
    }
    </style>
    
    <script type="text/javascript" id="\">

        var ClientBaseURL = "../Client/";
        var ClientBaseURL2 = "http://localhost:52293/RestServiceWWW/Client/";

        var RosterBaseURL = "../Roster/";
        var RosterBaseURL2 = "http://localhost:52293/RestServiceWWW/Roster/";

        var OpenBaseURL = "../Open/";

        var UpdatePunchBaseURL = "../Roster/Name/";
        var updateAuthorized;

        var shiftData = null;

        var HeaderStyle = 6;
        var RowStyle = 3;
        var RowStyleOdd = 1;
        var FooterStyle = 5;
        var WarningStyle = 2;

        function getShiftData() {
            var clientId = $("input[id*='clientID']").val();
            var Url = ClientBaseURL + "ShiftDepartments?clientId=" + clientId;
            $.ajax({
                cache: false,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                async: false,
                crossDomain: true,
                dataType: "json",
                url: Url,
                success: function (data) {
                    shiftData = data;
                },
                error: function (error) {
                    alert(JSON.stringify(error));
                }
            });
        }

        function createAddPopup() {
            //alert("create popup no params!");
            var d = new Date();
            var mnt = d.getMonth();
            var year = d.getFullYear();
            var day = d.getDay();
            var h = d.getHours();
            if (h >= 12) {
                am = "PM";
                h -= 12;
                if (h == 0)
                    h = 12;
            }
            else {
                am = "AM";
                if (h == 0)
                    h = 12;
            }
            var m = d.getMinutes();
            if (m <= 7)
                m = 0;
            else if (m <= 22)
                m = 15;
            else if (m <= 37)
                m = 30;
            else if (m <= 52)
                m = 45;
            else {
                m = 0;
                h++;
            }
            var popup = "<div class='popupClose' onclick='popupClose()'>" +
                    "<img alt='Close' title='Click to close' class='popupCloseBtn' src='../Images/Close.png' /></div>" +
                    "<h1 class='text-xl font-medium text-brand-black border-b border-gray-200 pb-2 mb-2'><span>Add Punch</span></h1>" +
                    "<h2 class='text-lg font-medium text-brand-black mb-2'>Set Time: ";
            //alert(popup);
            /* date */
            popup += "<input type='text' id='punchDate' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm' size='8' value='" + mnt + "/" + day + "/" + year + "'/>";
            /* hour */
            popup += "<select id='punchHour' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
            for (i = 1; i <= 12; i++) {
                var val = i;
                if (i < 10)
                    val = "0" + val;
                //alert(i + ", " + h);
                if (i != h)
                    popup += "<option value='" + val + "'>" + val + "</option>";
                else
                    popup += "<option selected='selected' value='" + val + "'>" + val + "</option>";
            }
            popup += "</select>";

            /* minutes */
            popup += "<select id='punchMin' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
            for (i = 0; i < 60; i += 5) {
                var val = i;
                if (i < 10)
                    val = "0" + val;
                if (i != m) {
                    popup += "<option value='" + val + "'>" + val + "</option>";
                }
                else {
                    popup += "<option selected='selected' value='" + val + "'>" + val + "</option>";
                }
            }
            popup += "</select>";

            /* am / pm */
            popup += "<select id='punchAMPM' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
            if (am == 'AM') {
                popup += "<option selected='selected' value='0'>AM</option>";
                popup += "<option value='12'>PM</option>";
            }
            else {
                popup += "<option selected='selected' value='12'>PM</option>";
                popup += "<option value='0'>AM</option>";
            }
            popup += "</select>";
            //popup += "<input type='button' id='updateBtn' onclick='updatePunch(" + id + ")' value='Update'/>";
            popup += "</h2>";
            popup += "<div id='punchResults'></div>";

            return popup;
        }

        function createPopup(mnt, dy, year, h, m, am, exact, id) {
            //alert("create popup with params!");
            var rounded = mnt + "/" + dy + "/" + year + " " + h + ":" + m + " " + am;
            var popup = "<div class='popupClose' onclick='popupClose()'>" +
                    "<img alt='Close' title='Click to close' class='popupCloseBtn' src='../Images/Close.png' /></div>" +
                    "<h1 class='text-xl font-medium text-brand-black border-b border-gray-200 pb-2 mb-2'><span>Modify Punch</span></h1>" +
                    "<h2 class='text-lg font-medium text-brand-black mb-2'><span>Exact Punch Time: " + exact + "</span></h2>" +
                    "<h2 class='text-lg font-medium text-brand-black mb-2'>Change Time: ";
            /* date */
            popup += "<input type='text' id='punchDate' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm' size='8' value='" + mnt + "/" + dy + "/" + year + "'/>";
            /* hour */
            popup += "<select id='punchHour' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
            for (i = 1; i <= 12; i++) {
                var val = i;
                if (i < 10)
                    val = "0" + val;
                if( i != h )
                    popup += "<option value='" + val + "'>" + val + "</option>";
                else
                    popup += "<option selected='selected' value='" + val + "'>" + val + "</option>";
            }
            popup += "</select>";

            /* minutes */
            popup += "<select id='punchMin' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
            for (i = 0; i < 60; i += 5) {
                var val = i;
                if (i < 10)
                    val = "0" + val;
                if (i != m) {
                    popup += "<option value='" + val + "'>" + val + "</option>";
                }
                else {
                    popup += "<option selected='selected' value='" + val + "'>" + val + "</option>";
                }
            }
            popup += "</select>";

            /* am / pm */
            popup += "<select id='punchAMPM' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
            if (am == 'AM') {
                popup += "<option selected='selected' value='0'>AM</option>";
                popup += "<option value='12'>PM</option>";
            }
            else {
                popup += "<option selected='selected' value='12'>PM</option>";
                popup += "<option value='0'>AM</option>";
            }
            popup += "</select>";
            popup += "<input type='button' id='updateBtn' onclick='updatePunch(" + id +  ")' value='Update' class='bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2'/>";
            popup += "</h2>";
            popup += "<div id='punchResults'></div>";
            return popup;
        }

        function updatePunch(id) {
            var e = document.getElementById("punchDate");
            var dt = e.value;

            var month = dt.substring(0, 2);
            var day = dt.substring(3, 5);
            var year = dt.substring(6, 10);

            e = document.getElementById("punchHour");
            var hour = e.options[e.selectedIndex].value;
            e = document.getElementById("punchMin");
            var min = e.options[e.selectedIndex].value;
            e = document.getElementById("punchAMPM");
            var am = e.options[e.selectedIndex].value;

            var retDate;
            if (am == 0)
                retDate = month + "/" + day + "/" + year + " " + hour + ":" + min + " AM";
            else
                retDate = month + "/" + day + "/" + year + " " + hour + ":" + min + " PM";

            if (am == '12') { /* PM */
                if (hour < 12)
                    hour = parseInt(hour) + 12;
                // if hour == 12 remain 12
            }
            else {
                if (hour == 12)
                    hour = 0;
            }
            //var url = RosterBaseURL + id + "/UpdatePunch?month=" + month + "&day=" + day + "&year=" + year + "&hour=" + hour + "&min=" + min;
            //alert("Upate punch: " + url);
            var url = RosterBaseURL + id + "/UpdatePunch";
            var postData = "month=" + month + "&day=" + day + "&year=" + year + "&hour=" + hour + "&min=" + min;
            $.ajax({
                cache: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                data: postData,
                dataType: "json",
                url: url,
                success: function (data) {
                    //alert(data);
                    retDate = data;
                },
                error: function (xhr) {
                    alert("ERROR!!!" + xhr.responseText);
                }
            });

            /* invalidate page totals */
            $("span[id *= 'OTHours']").each(function () {
                $(this).text("--.--");
                $(this).css('color', 'red');
            });
            $("span[id *= 'TotalHours']").each(function () {
                $(this).text("--.--");
                $(this).css('color', 'red');
            });
            $("span[id *= 'TotWeek']").each(function () {
                $(this).text("--.--");
                $(this).css('color', 'red');
            });
            $("span[id *= 'GrandWeek']").each(function () {
                $(this).text("--.--");
                $(this).css('color', 'red');
            });
            $('input[value=' + id + ']').closest('td').find('span').text(retDate);
            $('input[value=' + id + ']').closest('td').parent().find('td:eq(4)').find('span').text("--.--");
            $('input[value=' + id + ']').closest('td').parent().find('td:eq(5)').find('span').text("--.--");
            $('input[value=' + id + ']').closest('td').parent().find('td:eq(4)').find('span').css('color', 'red');
            $('input[value=' + id + ']').closest('td').parent().find('td:eq(5)').find('span').css('color', 'red');
            return retDate;
        }

        function popupAdd(e, elem) {
            //alert("popup Add!");
            if (!e)
                e = window.event;
            var ele = document.getElementById("popup");
            //alert(ele);
            //alert(ele.style.display);
            if (ele.style.display == "block") {
                ele.innerHTML = "";
                ele.style.display = "none";
            }
            else {
                ele.style.display = "block";
                var html = createAddPopup();

                //alert(html);
                ele.innerHTML = html;
                $(ele).offset({ top: e.pageY, left: e.pageX });
                $(".popupCloseBtn").hover(
                    function () {
                        $(this).addClass("hover");
                    },
                    function () {
                        $(this).removeClass("hover");
                    }
                );
            }
        }

        function clearChars(id) {  // remove any non-digit characters from id
            var retId = "";
            id = "" + id;
            for (var i = 0; i < id.length; i++) {
                if (id.charAt(i) >= '0' && id.charAt(i) <= '9')
                    retId += id.charAt(i);
            }
            //alert(retId);
            return retId;
        }

        function popupModify(e, elem) {
            if (!e)
                e = window.event;
            //alert("popup Modify!");
            var id = $(elem).closest('td').find('input').val();
            var exact = $(elem).closest('td').prev('td').find('span').text();

            var ele = document.getElementById("popup");
            if (ele.style.display == "block") {
                ele.innerHTML = "";
                ele.style.display = "none";
            }
            else {
                ele.style.display = "block";
                var dateTime = elem.innerHTML;
                var dt = dateTime.substring(0, dateTime.indexOf(' '));
                var tm = dateTime.substring(dateTime.indexOf(' ')+1);
                var mn = dt.substring(0, dt.indexOf("/"));
                var dy = dt.substring(dt.indexOf("/") + 1, dt.indexOf("/") + 3);
                var yr = dt.substring(dt.indexOf("/") + 4);

                var h = tm.substring(0, tm.indexOf(':'));
                var m = tm.substring(tm.indexOf(':') + 1, tm.indexOf(' '));
                var am = tm.substring(tm.indexOf('M') - 1);  //am or pm

                var html = createPopup(mn, dy, yr, h, m, am, exact, id);
                ele.innerHTML = html;
                $(ele).offset({ top: e.pageY, left: e.pageX });
                $(".popupCloseBtn").hover(
                    function () {
                        $(this).addClass("hover");
                    },
                    function () {
                        $(this).removeClass("hover");
                    }
                );
            }
        }

        function objToString(obj) {
            var str = '';
            for (var p in obj) {
                if (obj.hasOwnProperty(p)) {
                    str += p + '::' + obj[p] + '\n';
                }
            }
            return str;
        }

        function popupClose() {
            $('#popup').hide();
        }



        function updateBonuses() {
            //alert($('input[updbonus="true"]').length);
            var inps = $('input[updbonus="true"]');
            var list = "";
            inps.each(function () {
                $tr = $(this).parent().parent();
                $dept = $tr.prevAll("tr[id*='trDepartment_']");
                list = list + $(this).val() + ":" + clearChars($tr.find("span[id*='lblBadgeNumber']").text()) +
                    ":" + $dept.find("input[id*='hdnDept']").val() + ",";
            });
            list = list.substring(0, list.length - 1); //get rid of last comma
            //alert(list);
            var clientID = $("input[id*='clientID']").val();
            var weekEnd = $("select[id*='txtCalendar']").find(":selected").text();
            var userID = $("input[id*='userID']").val();
            var url = ClientBaseURL + "UpdateBonuses?clientID=" + clientID + "&weekEnd=" + weekEnd + "&list=" + list + "&userID=" + userID;
            //alert(url);
            $.ajax({
                cache: false,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                url: url,
                success: function (data) {
                    //alert($('input[updbonus="true"]').length);
                    $('<div><p>' + data + '</p></div>').dialog({
                        async: "false",
                        title: "Bonuses Updated",
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $("input[id*='UpdBonus']").attr('disabled', 'disabled');
                    $('input[updbonus="true"]').css("background-color", "#FFFFFF");
                    //alert($('input[updbonus="true"]').length);
                    $('input[updbonus="true"]').removeAttr("updbonus");

                    // $('input[updbonus="true"]').each(function () {
                    //     this.css("background-color", "#FFFFFF");
                    //     this.remAttr("updbonus");
                    // });
                },
                error: function (xhr) {
                    $('<div><p>' + xhr.responseText + '</p></div>').dialog({
                        title: "Bonus Update Error"
                    });
                }
            });
        }

        var grandTotal = 0;
        function print(d) {
            exportToXlsx(d);
            return true;
        }

        function validateBonus(inp){
            if ($.isNumeric(inp.val()) == false) {
                alert("Number entered is not a valid number!");
                inp.val(inp.attr("store"));
            }
            else {

                inp.css("background-color", "#FFDDDD");
                $("input[id*='UpdBonus']").removeAttr('disabled');

                var tr = inp.parent(); //inp.closest("tr[id*='trDepartment']");
                tr = tr.parent().prev();

                /* recalculate columns */
                var totSpan = tr.nextAll("tr[id*='trDepartmentTotals']").first().find("span[id*='lblDeptTotBonus']");

                var totSpanVal = totSpan.text().replace(/[$, ]/g,'');
                //alert(totSpanVal);

                var store = inp.attr("store");
                var newAdd = parseFloat(inp.val()) - parseFloat(store);
                //alert(newAdd);
                totSpanVal = parseFloat(totSpanVal) + newAdd;
                totSpan.text('$ ' + totSpanVal.toFixed(2));

                totSpan = tr.nextAll("tr[id*='trGrandTot']").first().find("span[id*='lblGrandTotBonus']");
                totSpanVal = parseFloat( totSpan.text().replace(/[$, ]/g,'')) + newAdd;
                //alert(totSpanVal);

                totSpan.text('$ ' + totSpanVal.toFixed(2));
                inp.attr("updBonus", "true");
                inp.attr("store", parseFloat(inp.val()).toFixed(2));
                inp.val(parseFloat(inp.val()).toFixed(2));
            }
        }
        var approveCount = 0;

        function approvalStatus(rowId, punchInfo) {
            if ($("#approveDialog").length == 0 ) {
                approveCount = 0;
                $("<div id='approveDialog' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200'><h2 class='text-xl font-medium text-brand-black mb-2'>Punch Approval</h2><hr/>" +
                    "<table id='tblApprove' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200'><thead><tr><th>Name</th><th>ID #</th><th>Total Punches</th><th>Status</th><th>Supervisor</th></tr></thead>" +
                    "<tbody></tbody>" +
                    "</div>").dialog({
                    height: 400,
                    width: 600,
                    draggable:true,
                    title: "Punch Approval",
                    close: function () {
                        var c = $('#approveDialog');
                        if (c.length >= 1) {
                            c.remove();
                        }
                    }
                });
            }
            if (rowId == null) {
                rowId = "id" + parseInt(Math.random() * 1000000, 10);
                $tr = $("<tr id='" + rowId + "'><td class='name'>" + punchInfo["name"] + "</td><td class='badge'>" + punchInfo["badge"] + "</td><td class='punchCount'>" + punchInfo["punchCount"] + "</td><td class='status'>" + punchInfo["status"] + "</td><td>" + punchInfo["super"] + "</td></tr>");
                $("#tblApprove").find("tbody").append($tr);
            }
            else {
                $("tr[id='" + rowId + "'").find("td[class='status']").text(punchInfo["status"]).closest("tr").css("backgroundColor", punchInfo["color"]);
            }
            approveCount++;
            return rowId;
        }
        function waitIndicator(on, titleBar) {
            $('#waiting').remove();
            if (on == true) {
                if (titleBar == null)
                    titleBar = "loading";
                $("<div id='waiting' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 p-4'><h3 class='text-lg font-medium text-brand-black mb-2 text-center'>please wait...</h3><div class='flex justify-center'><img src='../Images/ajax-loader.gif'/></div></div>").dialog(
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

        jQuery(document).ready(
            function () {
                $("input[id*='btnExportHTML']").addClass("bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2").click(function () {
                    exportHRToXlsx("tblHoursReport");
                });
                $("span[id*='999lblFirstPunch'").click(function () {
                    var tr = $(this).closest('tr');
                    var name = $(tr).find("span[id*='lblLastName']").text() + ", " + $(tr).find("span[id*='lblFirstName']").text();
                    var badge = $(tr).find("span[id*='lblBadge']").text();
                    var firstPunch = $(this).text();
                    var tableWidth = 520;
                    var $firstPunchDialog =
                    $("<div id='firstPunchDialog' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200'>" +
                        "<table class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200' style='width:" + tableWidth + "px'><thead><tr class='bg-brand-grey text-white text-sm font-medium uppercase tracking-wider px-6 py-3 text-left'>" +
                        "<th><span>Badge #</span></th><th><span>Name</span></th><th><span>Start Date</span></th><th><span>New Start Date</span></th>" +
                        "</tr></thead>" +
                        "<tbody class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><tr>" +
                        "<td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span>" + badge + "</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span>" + name + "</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span>" + firstPunch +
                        "</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span><input id='newFirstPunch' type='text' size='8' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm'/></span></td>" +
                        "</tr>" +
                        "<tr id='trResults'><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'>" + '<img style="height:40px;width:40px;" src="../Images/ajax-loader.gif" />' + "</td><td colspan='3' class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><h1 id='firstPunchResults' style='color:Blue'></h1></td></tr>" +
                        "</tbody>" +
                        "</table>" +
                    "</div>");
                    $firstPunchDialog.dialog({
                        close: function (event, ui) { $('#firstPunchDialog').remove(); },
                        resizable: false,
                        height: 360,
                        width: (tableWidth + 40),
                        open: function (event, ui) {
                            var d = $('#firstPunchDialog');
                            d.find('img').hide();
                            d.find('#firstPunchResults').hide();
                        },
                        title: 'Update Start Date',
                        draggable: true,
                        modal: true,
                        buttons: {
                            "Update Start Date": function () {
                                $.ajax({
                                    cache: false,
                                    type: "GET",
                                    contentType: "application/json; charset=utf-8",
                                    async: false,
                                    dataType: "json",
                                    url: Url,
                                    success: function (d) {
                                    },
                                    error: function (xhr) {
                                        alert("ERROR!!!" + xhr.responseText);
                                    },
                                    complete: function (xhr) {
                                    }
                                });
                            }
                        }
                    });
                });
                getShiftData();

                if ($('input[id*="hdnUpdPay"]').val() == 'true') {
                    $("span[id*='lblPayRate']").hover(
                    function () { $(this).addClass("underLine"); },
                    function () { $(this).removeClass("underLine"); }
                ).click(function () {
                    var payLink = $(this);
                    var tr = $(this).closest('tr');
                    var name = $(tr).find("span[id*='lblLastName']").text() + ", " + $(tr).find("span[id*='lblFirstName']").text();
                    var badge = $(tr).find("span[id*='lblBadge']").text();
                    var payRate = $(tr).find("input[id*='hdnPayRate']").val();//$(this).text();
                    var payRateRatio = $(tr).find("input[id*='hdnBillRateRatio").val();
                    //alert(payRateRatio);
                    var deptRow = $(tr).prevAll('tr[id*="trDepartment_"]').first();
                    var dept = $(deptRow).find('input[id*="hdnDept"]').val();
                    var punchDt = $(this).closest('tr').next('tr').find('.tblDetail').find('tr[id*="tableRow_0"]').find('span[id*="lblCheckIn_0"]').attr("exact");
                    var shift = $(deptRow).find("input[id*='hdnShift']").val();
                    var $payRateDialog =
                    $("<div id='payrate' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200'>" +
                        "<table class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200'><thead><tr class='bg-brand-grey text-white text-sm font-medium uppercase tracking-wider px-6 py-3 text-left'>" +
                        "<th><span>Badge #</span></th><th><span>Name</span></th><th><span>Pay Rate</span></th><th><span>New Pay Rate</span></th><th><span>ALL Depts (Same Shift)</span></th>" +
                        "</tr></thead>" +
                        "<tbody class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><tr>" +
                        "<td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span>" + badge + "</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span>" + name + "</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span>" + payRate +
                        "</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span>$<input id='newPayRate' type='text' size='4' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm'/></span></td>" +
                        "<td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><input type='checkbox' id='allDepts' class='rounded border-gray-300 text-brand-green focus:ring-brand-green'/>" +
                        "</tr>" +
                        "<tr><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span id='spanSelectDt' class='block text-sm font-medium text-brand-black mb-1'>Starting Date For New Pay Rate: </span><input type='text' width='10' id='selectDt' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm'></input><input type='hidden' name='ttDateHidden'></input></td></tr>" +
                        "<tr><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'>" + '<img style="height:40px;width:40px;" src="../Images/ajax-loader.gif" />' + "</td><td colspan='3' class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><h1 id='payRateResults' style='color:Blue'></h1></td></tr>" +
                        "</tbody>" +
                        "</table>" +
                    "</div>");
                    $payRateDialog.find("input[id*='selectDt']").datepicker({
                    });
                    var payDate = new Date($("select[id*='txtCalendar']").val());
                    payDate.setDate(payDate.getDate() - 6);
                    //alert(payDate);
                    var month = payDate.getMonth() + 1;//startDt.substring(0, 2);
                    if (month <= 9) month = '0' + month;
                    var day = payDate.getDate();//startDt.substring(3, 5);
                    if (day <= 9) day = '0' + day;
                    var year = payDate.getFullYear();//startDt.substring(6, 10);

                    startDt = month + "/" + day + "/" + year;
                    $payRateDialog.find("input[id*='selectDt']").val(startDt);
                        $payRateDialog.dialog({
                        close: function (event, ui) { $('#payrate').remove(); },
                        resizable: false,
                        height: 360,
                        width: 560,
                        open: function (event, ui) {
                            var d = $('#payrate');
                            d.find('img').hide();
                            d.find('#payRateResults').hide();
                            $('div[class*="ui-dialog"]').find('button').prop("disabled", true);
                            d.find('input[id*="newPayRate"]').change(function () {
                                var vl = $(this).val();
                                if (isNumber(vl)) {
                                    $('div[class*="ui-dialog"]').find('button').prop("disabled", false);
                                }
                                else {
                                    $('div[class*="ui-dialog"]').find('button').prop("disabled", true);
                                }
                            });
                        },
                        title: 'Update Pay Rate',
                        draggable: true,
                        modal: true,
                        buttons: {
                            "Change Pay Rate": function () {
                                if ($("#allDepts").attr("checked"))
                                    dept = 0;
                                $('div[class*="ui-dialog"]').find('button').prop("disabled", true);
                                $('#payrate').find('img').show();
                                var startDt = $('#selectDt').val();
                                var newDate = new Date(startDt);
                                //newDate.setDate(newDate.getDate() - 7);
                                //alert(newDate);
                                var month = newDate.getMonth() + 1;//startDt.substring(0, 2);
                                if (month <= 9) month = '0' + month;
                                var day = newDate.getDate();//startDt.substring(3, 5);
                                if (day <= 9) day = '0' + day;
                                var year = newDate.getFullYear();//startDt.substring(6, 10);

                                startDt = year + "-" + month + "-" + day;
                                //alert(startDt);
                                //alert(year + ", " + month + ", " + day + ", " + startDt);
                                var curDt = $("select[id*='txtCalendar']").val();
                                //alert( curDt);
                                var month = curDt.substring(0, 2);
                                var day = curDt.substring(3, 5);
                                var year = curDt.substring(6, 10);

                                curDt = year + "-" + month + "-" + day;
                                //alert(year + ", " + month + ", " + day + ", " + curDt);

                                Url = RosterBaseURL + "PayRate?aident=" + badge + "&startDate=" + startDt +
                                    "&endDate=9999-01-01&clientId=" + $("input[id*='clientID']").val() + "&dept=" + dept + "&shift=" + shift + 
                                    "&payRate=" + $("#newPayRate").val() + "&user=" + $('input[id$="userID"]').val() + "&punchDate=" + $payRateDialog.find("#selectDt").val();
                                //alert(Url);
                                $.ajax({
                                    cache: false,
                                    type: "GET",
                                    contentType: "application/json; charset=utf-8",
                                    async: false,
                                    dataType: "json",
                                    url: Url,
                                    success: function (d) {
                                        var newPayRate = $("#newPayRate").val() * payRateRatio;
                                        var clientId = $("input[id*='clientID']").val();
                                        if (clientId == 309)
                                        {
                                            newPayRate += .15;
                                        }
                                        //alert( newPayRate );
                                        $("#payrate").find('#payRateResults').text(d.toString());
                                        $("#payrate").find("#payRateResults").show();
                                        //alert(curDt + ", " + curDt);
                                        if( startDt <= curDt )
                                            $(payLink).text("$" + newPayRate.toFixed(2) );
                                        if (dept == 0) {
                                            var rows = $('tr[class*="DlyHrs"]');
                                            rows = $(rows).find('span[id*="BadgeNum"]');
                                            badge = clearChars(badge);
                                            $(rows).each(function () {
                                                if (clearChars($(this).text()) == badge) {
                                                    /* get shift */
                                                    var topRow = $(this).closest('tr');
                                                    topRow = $(topRow).prevAll("tr[id*='trDepartment_']").first();
                                                    var newShift = $(topRow).find("span[id*='lblDepartment']").text();
                                                    newShift = newShift.split(" - ");
                                                    newShift = newShift[newShift.length - 1];
                                                    if (newShift.indexOf("1st") != -1)
                                                        newShift = 1;
                                                    else if (newShift.indexOf("2nd") != -1)
                                                        newShift = 2;
                                                    else
                                                        newShift = 3;
                                                    if (newShift == shift) {
                                                        $(this).closest('tr').find('span[id*="PayRate"]').text("$" + newPayRate.toFixed(2));
                                                    }
                                                }
                                            });
                                        }
                                    },
                                    error: function (xhr) {
                                        alert("ERROR!!!" + xhr.responseText);
                                    },
                                    complete: function (xhr) {
                                        $("#payrate").find('img').hide();
                                    }
                                });
                            }
                        }
                    });
                });
                }

                function isNumber(n) {
                    return !isNaN(parseFloat(n)) && isFinite(n);
                }
                $("#btnApprove").click(function () {
                    $("<div class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 p-4'><p class='text-sm text-brand-black'>Please approve hours by clicking the \"APR\" button on each employee's line</p></div>").dialog({
                        async: "false",
                        title: "Employee Hours Approval",
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                });
                $(".btnTimeline").addClass("bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2").click(function () {
                    var tr = $(this).parent().parent(); // btn's row
                    var timeline = tr.next('tr');
                    if( timeline.hasClass('trTimeline')) {
                        timeline.remove();
                        return;
                    }
                    var colspan = 0;
                    tr.find("td").each(function () {
                        colspan += $(this).prop("colspan");
                    });
                    tr.after("<tr class='trTimeline'><td colspan='" + colspan + "'><div id='punchTimeline' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 p-4'></div></td></tr>");
                    setTimeline(tr);
                });
                var pl = new Array($("span[id*='lblBadgeNumber']").length);
                $("input[id*='peekABoo']").click(function () {
                    $emps = $("span[id*='lblBadgeNumber']");
                    $emps.each(function (i) {
                        var empObj = new Object();
                        empObj.id = $(this).text();
                        empObj.name = '"' + $(this).closest('td').next('td').find('span[id*="LastName"]').text() + "," +
                            $(this).closest('td').next('td').find('span[id*="FirstName"]').text() + '"';

                        pl[i] = empObj;

                        var hours = new Array($(this).closest('tr').next('tr').find(".tblDetail").find('tr[id*="Detail"]').length);
                        $det = $(this).closest('tr').next('tr').find(".tblDetail").find('tr[id*="Detail"]');
                        $det.each(function (j) {
                            empHours = new Object();
                            empHours.start = new Date($(this).find("span[id*='CheckIn']").text());
                            empHours.end = new Date($(this).find("span[id*='CheckOut']").text());
                            empHours.hours = $(this).find("span[id*='CheckHours']").text();
                            //if( j < 2 )
                            //    alert("# of hours = " + empHours.hours);
                            if (empHours.hours > 5)
                                empHours.hours -= .5;
                            empHours.approvedBy = $(this).find("span[id*='ApprovedBy']").text();
                            hours[j] = empHours;
                        });
                        pl[i].hours = hours;
                    });
                    //alert($emps.length + ", " + pl.length);
                    for (var i = 0; i < pl.length; i++) {
                        for (var j = i + 1; j < pl.length; j++) {
                            if (pl[j].id != 0 && pl[i].id != 0 && pl[i].id == pl[j].id) {
                                var na = new Array(pl[i].hours.length + pl[j].hours.length);
                                for (var k = 0; k < pl[i].hours.length; k++) {
                                    var hr = new Object();
                                    hr.start = pl[i].hours[k].start;
                                    hr.end = pl[i].hours[k].end;
                                    hr.hours = pl[i].hours[k].hours;
                                    hr.approvedBy = pl[i].hours[k].approvedBy;
                                    na[k] = hr;
                                }
                                for (var k = 0; k < pl[j].hours.length; k++) {
                                    var hr = new Object();
                                    hr.start = pl[j].hours[k].start;
                                    hr.end = pl[j].hours[k].end;
                                    hr.hours = pl[j].hours[k].hours;
                                    hr.approvedBy = pl[j].hours[k].approvedBy;
                                    na[k + pl[i].hours.length] = hr;
                                }
                                pl[j].id = 0;
                                pl[i].hours = na;
                            }
                        }
                    }
                    for (var i = 0; i < pl.length; i++) {
                        if (pl[i].id == 0)
                            continue;
                        /* sort pl[i].hours... */
                        for (var j = 0; j < pl[i].hours.length; j++) {
                            for (var k = j + 1; k < pl[i].hours.length; k++) {
                                if (pl[i].hours[j].start > pl[i].hours[k].start) {
                                    var temp = pl[i].hours[j];
                                    pl[i].hours[j] = pl[i].hours[k];
                                    pl[i].hours[k] = temp;
                                }
                            }
                        }
                    }
                    print(pl);
                });
                updateAuthorized = ($("input[id*='hdnUpdPunches']").val() == 'True') && $("span[id*='lblApprovalHeader']").length == 0;
                $('.DlyHrs:even').addClass('DlyHrsAlt bg-gray-50');
                $('.DlyHrs').addClass('hover:bg-gray-50 border-b border-gray-200');
                var myClass;
                $('tr[class*="DlyHrs"]').addClass('hover:bg-gray-50 border-b border-gray-200').hover(
                    function () {
                        myClass = $(this).attr("class");
                        $(this).addClass("bg-gray-100").removeClass(myClass);
                    }, function () {
                        $(this).removeClass("bg-gray-100").addClass(myClass);
                    }
                );
                $('span[id*="lblSwipeDateTime"]').hover(
                    function () {
                        myClass = $(this).attr("class");
                        $(this).addClass("underline text-brand-green").removeClass(myClass);
                    }, function () {
                        $(this).removeClass("underline text-brand-green").addClass(myClass);
                    }).click(function () {
                        alert("You clicked me! " + $(this).text());
                    });

                $("span[id*='CheckInExact_']").attr("title", "Click to view image");
                $("span[id*='CheckOutExact_']").attr("title", "Click to view image");

                var btn = "<input type='button' onclick='updateBonuses()' disabled='disabled' id='inpUpdBonuses' value='Bonus' class='bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2' />";
                $("span[id*='spanUpdBonuses']").html(btn);

                if ($("input[id*='hdnUpdBonuses']").val() == 'true') {
                    //alert("Bonuses!");
                    $("td[id*='onusMult']").show();
                    $("input[id*='onusMult']").show();
                    $("input[id*='txtBonusM']").each(function (index) {
                        $(this).change(
                            function (e) {
                                var mult = parseFloat($(this).val());
                                //alert($(this).closest('tr').next('tr').length);
                                $(this).closest('tr').nextAll('tr[id*="trDepartment_"]').eq(0).find('input[id*="BonusMult"]').focus().select();
                                $(this).closest('tr').nextUntil('tr[id*="trDepartmentTot"]').each(function () {
                                    var hrs = parseFloat($(this).find("span[id*='lblTotalHours']").text()) +
                                        parseFloat($(this).find("span[id*='lblOTHours']").text());
                                    if (hrs > 0) {
                                        $(this).find("input[id*='bonusPay']").val((hrs * mult).toFixed(2));
                                        $(this).find("input[id*='bonusPay']").trigger("change");
                                    }
                                });
                            });
                    });
                    $("span[id*='BonusItem']").each(function (index) {
                        //console.log(index + ": " + $(this).text());
                        var inp = "$<input type='text' onClick='this.select();' size='3' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm text-right' id='bonusPay" +
                        index + "' value='" + $(this).text() + "' store='" + $(this).text() + "'/>";
                        $(this).parent().html(inp).change(
                        function (e) {
                            validateBonus($(this).find("input"));
                        }
                    )
                    });
                }
                $('tr[id*="exception"]').hover(
                    function () {
                        $(this).find('td').addClass("bg-gray-100");
                    },
                    function () {
                        $(this).find('td').removeClass("bg-gray-100");
                    }
                ).click(
	                function (e) {
	                    var nxtTr = $(this).nextAll('tr');
	                    nxtTr.toggle(100);
	                });
                $('tr[class*="DlyHrs"]').find('td:nth-child(2)').hover(
                    function () {
                        $(this).addClass("underline text-brand-green");
                    },
                    function () {
                        $(this).removeClass("underline text-brand-green");
                    }
                ).click(
	                function (e) {
	                    var nxtTr = $(this).parent('tr').next('tr');
	                    nxtTr.toggle(100);
	                    var fg = nxtTr.find('td').find('div');
	                    if (fg.length == 0) {
	                        if (updateAuthorized) {
	                            nxtTr.find('td').find('table').find('tbody').append(
	                               '<tr noBadge="true" class="hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0"><td id="A' + parseInt(Math.random() * 100000000) +
	                               '" class="time px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span>Add a punch</span></td><td id="B' +
	                               parseInt(Math.random() * 100000000) +
	                               '" class="time px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span>Add a punch</span></td><td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span></span></td></tr>' +
	                               '<tr noBadge="true" class="hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0"><td id="A' + parseInt(Math.random() * 100000000) +
	                               '" class="time px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span>Add a punch</span></td><td id="B' +
	                               parseInt(Math.random() * 100000000) +
	                               '" class="time px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span>Add a punch</span></td><td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span></span></td></tr>' +
	                               '<tr noBadge="true" class="hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0"><td id="A' + parseInt(Math.random() * 100000000) +
	                               '" class="time px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span>Add a punch</span></td><td id="B' +
	                               parseInt(Math.random() * 100000000) +
	                               '" class="time px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis" split="true"><span>Split Shift</span></td><td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><span></span></td></tr>'
                                );
	                        }
	                        nxtTr.find('td').find('table').addClass('w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 tblDetail').flexigrid({ showToggleBtn: false, colResize: false });

	                        $(nxtTr).find(".unassignedDisplay").click(function () {
	                            $("<div id='unassigned' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 p-4'></div>").dialog({
	                                close: function (event, ui) { $('#unassigned').remove(); },
	                                resizable: false,
	                                height: 320,
	                                width: 400,
	                                draggable: true,
	                                modal: true,
	                                buttons: {
	                                    "Don't Add": function () {
	                                        $('#unassigned').remove();
	                                    }
	                                }
	                            });
	                        });

	                        $(nxtTr).find(".punchDisplay").addClass("bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2").click(function () {
	                            var hDiv = $(this).closest('div[class="hDiv"]');
	                            var cDrag = $(hDiv).next('div[class="cDrag"]');
	                            var bDiv = $(cDrag).next('div[class="bDiv"]');
	                            var spans = $(bDiv).find('span[id*="lblCheck"]');
	                            if ($(this).attr("value") == "Exact Time") {
	                                $(spans).each(function () {
	                                    $(this).text($(this).attr("exact"));
	                                });
	                                $(this).attr("value", "Round Time");
	                            }
	                            else {
	                                $(spans).each(function () {
	                                    $(this).text($(this).attr("round"));
	                                });
	                                $(this).attr("value", "Exact Time");
	                            }
	                        });
	                        if (updateAuthorized) {
	                            nxtTr.find('td').find('table').find('tbody').find('tr[nobadge="true"]').find('td[class*="time"]').hover(
	                               function () {
	                                   $(this).addClass("underline text-brand-green");
	                               },
	                               function () {
	                                   $(this).removeClass("underline text-brand-green");
                                }).click(function (e) {
                                    if ($(this).attr("split") != "true")
                                        adjustPunches(e, $(this));
                                    else {
                                        splitShift(e, $(this));
                                    }
                                });
	                        }
	                    }
	                }
                );
                var $title;
                if (updateAuthorized) {
                    $title = "Click to change punch";
                }
                else {
                    $title = "Click for punch info";
                }
                $('.tblDetail').find('tr').addClass('hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0').find('td[class*="time"]').addClass('px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis').find('span[punchid]').closest('td').attr("title", $title).hover(
                    function () {
                        $(this).addClass("underline text-brand-green");
                    },
                    function () {
                        $(this).removeClass("underline text-brand-green");
                    }
                ).click(function (e) {

                    adjustPunches(e, $(this));
                });
                $('td[id*="tdDepartmentHead"]').hover(
                    function () {
                        $(this).addClass("underline text-brand-green");
                    },
                    function () {
                        $(this).removeClass("underline text-brand-green");
                    }
                ).click(function () {
                    $(this).parent('tr').nextUntil('tr[id*="trDepartmentTot"]').each(function () {
                        if ($(this).find('td[id*="tdEmpCnt"]').length == 0)
                            $(this).hide();
                        else
                            $(this).toggle();
                    });
                });
                $("input[id*='btnLineApprove']").addClass("bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2").click(function () {
                    var punchInfo = {};
                    punchInfo["badge"] = $(this).closest("tr").find("span[id*='lblBadge']").text();
                    punchInfo["name"] = $(this).closest("tr").find("span[id*='lblLastName']").text() + ", " +
                        $(this).closest("tr").find("span[id*='lblFirstName']").text();
                    var punchCount = $(this).closest("tr").next("tr").find("td[class='time']");
                    //alert(punchCount.length);
                    var punchIds = [];
                    var userName = $("input[id*='userID']").val();
                    punchInfo["super"] = userName;
                    punchInfo["punchCount"] = punchCount.length;
                    punchIds.push(userName);
                    $(punchCount).each(function () {
                        var punchId = $(this).find("span[punchid]");
                        if (punchId.length > 0) {
                            //alert($(punchId).attr("punchid"));
                            punchIds.push($(punchId).attr("punchid"));
                        }
                    });
                    if (punchIds.length <= 1) {
                        alert("No punch data found!");
                        return;
                    }
                    //upload punchIds
                    Url = OpenBaseURL + 'LineApprove';
                    var btn = this;
                    $(btn).prop("disabled", "true");
                    punchInfo["status"] = "Submitting punches";
                    var rowId = approvalStatus(null, punchInfo);
                    //alert(rowId);
                    $.ajax({
                        cache: false,
                        type: "POST",
                        url: Url,
                        dataType: "json",
                        data: JSON.stringify(punchIds),
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //waitIndicator(false);
                            $(btn).prop("visible", "false");
                            $(btn).closest("tr").find("span[id*='lblUnApprov']").css('color', 'black').hide();
                            $(btn).closest("tr").find("span[id*='lblBadge']").css('color', 'black');
                            $(btn).closest("tr").find("span[id*='lblLastName']").css('color', 'black');
                            $(btn).closest("tr").find("span[id*='lblFirstName']").css('color', 'black');
                            approveCount--;
                            //alert(rowId);
                            punchInfo["status"] = "Punches Approved";
                            punchInfo["color"] = "LightGreen";
                            approvalStatus(rowId, punchInfo);
                        },
                        error: function (msg) {
                            //waitIndicator(false);
                            msg = eval(msg);
                            //alert(msg);
                            approveCount--;
                            punchInfo["status"] = "Error - Punches Not Approved";
                            punchInfo["color"] = "OrangeRed";
                            approvalStatus(rowId, punchInfo);
                        },
                        complete: function (e) {
                        }
                    });
                });
            });

            function splitShift(e, td) {
                alert(e + ", " + td.length);
            }

            function adjustPunches(e, td) {
                var punchID = null;
                var c = $('div[class*="img"]');
                var c2 = $("#addPunch");

                if (c.length > 0) {
                    c.remove();
                }
                else if (c2.length > 0) {
                    c2.remove();
                }
                else {
                    var $targ = $(td).find('span');
                    punchID = $targ.attr("punchID");
                    
                    if (punchID == null || punchID.length == 0) {
                        var id = $targ.closest('table').closest('tr').prev('tr').find('span[id*="Badge"]').text().substring(2);
                        addPunch(e, $(td), id);
                    }
                    else {
                        var d = $('<div class="img w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200" updTime="false" updMove="false"></div>', {});
                        var sY = $(window).scrollTop() + $(window).height();
                        var topTR = $targ.closest('table').closest('tr').prev('tr');
                        var name = topTR.find('span[id*="LastName"]').text() + ", " + topTR.find('span[id*="FirstName"]').text();
                        var badge = topTR.find('span[id*="Badge"]').text().substring(2);
                        var exact = new Date($targ.attr('exact'));
                        var year = parseInt(exact.getFullYear());
                        var month = parseInt(exact.getMonth()) + 1;
                        if (month < 10) month = '0' + month;
                        var date = parseInt(exact.getDate(), 10);
                        if (date < 10) date = '0' + date;
                        var hours = parseInt(exact.getHours(), 10);
                        if (hours < 10) hours = '0' + hours;
                        var minutes = parseInt(exact.getMinutes(), 10);
                        if (minutes < 10) minutes = '0' + minutes;
                        var seconds = parseInt(exact.getSeconds(), 10);
                        if (seconds < 10) seconds = '0' + seconds;
                        var im = badge + '__' + year + month + date + '_' + hours + minutes + seconds + '.jpg';
                        var htm = addHtml(im, name, $targ);
                        if ((e.pageY + 400) < sY)
                            d.appendTo('body').html(htm).addClass("tooltip").css('top', (e.pageY - 80) + 'px').css('left', (e.pageX + 10) + 'px');
                        else
                            d.appendTo('body').html(htm).addClass("tooltip").css('top', (e.pageY - 400) + 'px').css('left', (e.pageX + 10) + 'px');

                        //setVideoSrc(326, "2016-01-18 21:48:32");
                        if (!updateAuthorized)
                            return;

                        setDatePicker();
                        $('#punchTime > input').change(function () {
                            var ePunchId = punchID;
                            enableUpdate(ePunchId, td);
                        });
                        $('#punchTime > select').change(function () {
                            var ePunchId = punchID;
                            enableUpdate(ePunchId, td);
                        });
                        $('.moveDepts').change(function () {
                            var ePunchId = punchID;
                            enableMove(ePunchId, td)
                        });
                        $('#moveShifts').change(function () {
                            var ePunchId = punchID;
                            enableMove(ePunchId, td);
                        });

                        document.getElementById("ttDel").onclick = function () {
                            var userID = $('input[id$="userID"]').val();
                            //alert("punch id = " + punchID);
                            $("<div id='delPrompt' class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 p-4'><p class='text-sm text-brand-black'>Are you sure you want to delete this punch?</p></div>").dialog({
                                height: 200,
                                width: 350,
                                title: "Delete punch for " + name + "<br/>Actual: " + $targ.attr("exact") +
                                        "<br/>Rounded: " + $targ.attr("round"),
                                modal: true,
                                buttons: {

                                    "Delete": function () {
                                        Url = RosterBaseURL + 'TTDelete?id=' + userID + '&punchID=' + punchID;
                                        $.ajax({
                                            cache: false,
                                            type: "GET",
                                            url: Url,
                                            async: false,
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            success: function (d) {
                                                if (d == false) {
                                                    //alert($('#delPrompt').text());
                                                    $('#delPrompt').append("<h2>Error - Punch NOT Deleted!</h2>");
                                                }
                                                else {
                                                    //alert($('#delPrompt').text());
                                                    $('#delPrompt').append("<h2>Punch Deleted!</h2>");
                                                    td.text("");
                                                    td.parent().find('td > span[id*="lblPunchStatus"]').text("*MODIFIED*");
                                                    var span = td.parent().find('td > span[id*="lblPunchStatus"]');
                                                    $(span).attr("title", "Refresh to see status");
                                                }
                                                $('#delPrompt').dialog("option",
                                                "buttons", [
                                                    {
                                                        text: "Ok", click: function () {
                                                            $(this).dialog("close");
                                                            element = document.getElementById("delPrompt");
                                                            element.parentNode.removeChild(element);
                                                        }
                                                    }
                                                ]);
                                            },
                                            error: function (msg) {
                                                msg = eval(msg);
                                                alert(msg);
                                            }
                                        });
                                    },
                                    Cancel: function () {
                                        $(this).dialog("close");
                                        element = document.getElementById("delPrompt");
                                        element.parentNode.removeChild(element);
                                    }
                                },
                                close: function () {
                                    var c = $('div[class*="img"]');
                                    if (c.length >= 1) {
                                        c.remove();
                                    }
                                }
                            });
                        };
                    }
                }
            }
            function movePunch(punchId, td) {
                var shiftType = $('#moveShifts option:selected').val();
                var dept = $('.moveDepts:visible option:selected').val();
                var shiftName = $('#moveShifts option:selected').text();
                var deptName = $('.moveDepts:visible option:selected').text();
                var userId = $('input[id*="userID"]').val();
                td.text("Punch Moved");
                Url = RosterBaseURL + "MovePunch/" + punchId + "?departmentId=" + dept + "&shiftType=" + shiftType + "&userName=" + userId;
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: Url,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (d) {
                        td.text('*' + shiftName + ' ' + deptName + '*');
                        td.attr('style', 'font-size:14px; color:Green;padding:4px;');
                        td.off('click');
                        td.off('hover');
                        var c = $("div[class*='img']");
                        if (c.length > 0 && c.attr("updTime") == "false") {
                            c.remove();
                        }
                        else {
                            c.attr("updMove", "false");
                        }
                    },
                    error: function () {
                        alert("error!!!!");
                    }
                });
            }
                        function updatePunchTime(punchID, td) {
                            //alert($('#punchTime > select').first().length);
                            //var sel = $(#punchTime
                            var $time = $('#punchTime > select option:selected');
                            var $date = $('.ttDateHidden').val() + ' ' +
                            $time.eq(0).text() + ':' + //HOURS
                            $time.eq(1).text() + ' ' + //MINS
                            $time.eq(2).text(); //AM-PM
                            var dtRnd = new Date($date);
                            $date = $('table[class*="img"]').find("input[id='exactDate']");

                            //Mon, Jan. 30, 2017 04:54 AM

                            var dtEx = new Date($date.val());
                            //alert($date.val() + " --- "+ dtEx);
                            var timeDiff = Math.abs(dtEx.getTime() - dtRnd.getTime());
                            //var diffDays = Math.ceil( /*parseInt(timeDiff,10) / (1000 * 3600 * 24)*/ 24.545 );
                            var diffMins = Math.ceil(timeDiff / (1000 * 60));
                            var count = 1;
                            var warning = "";
                            if (diffMins > 300) {
                                warning = "(" + count + ") <b>Warning!</b> There are " + diffMins + " minutes difference between the actual punch time and the rounded punch time.";
                                warning += "<br/>(Typically the maximum difference is less than 30 minutes)";
                                count++;
                            }
                            var start = new Date($('input[id*="weekStartDate"]').val());
                            var end = new Date($('input[id*="weekEndDate"]').val());

                            //alert(start + ", " + end);

                            if (dtRnd < start || dtRnd > end) {
                                warning += "<br/><hr/>(" + count + ") <b>Warning!</b> Date is out of page range.  It won't be visible on this page!" +
                                        "<br/>The only punches visible on this page are between: <br/><b>" + start.toLocaleString() +
                                        "</b><br/> and <br/><b>" + end.toLocaleString() + "</b>";
                            }
                            if (warning.length > 0) /* warning dialog */{
                                $('<div class="w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 p-4">' + warning + '</div>').dialog({
                                    title: "<b>Punch adjust Warning!</b>",
                                    width: "460px",

                                    draggable: true,
                                    buttons: {
                                        Cancel: function () {
                                            $(this).dialog("close");
                                        }
                                    },
                                    close: function () {
                                        var c = $('.img');
                                        if (c.length >= 1) {
                                            c.remove();
                                        }
                                    }
                                });
                            }
                            else {
                                updateTime(punchID, dtRnd, td);
                            }
                        };

            function updateTime(punchID, dtRnd, td) {
                Url = RosterBaseURL + punchID + "/UpdatePunch?userID=" + $('input[id$="userID"]').val() + "&month=" + (1 + parseInt(dtRnd.getMonth())) +
                                            "&day=" + dtRnd.getDate() + "&year=" + dtRnd.getFullYear() + "&hour=" + dtRnd.getHours() + "&min=" + dtRnd.getMinutes();
                //alert(Url);
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: Url,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (d) {
                        td.text(d + ' *MODIFIED* ');
                        td.attr('style', 'font-size:14px; color:Green;padding:4px;');
                        td.off('click');
                        td.off('hover');
                        var c = $('div[class*="img"]');
                        if (c.length >= 1 && c.attr("updMove") == "false") {
                            c.remove();
                        }
                        else {
                            c.attr("updTime", "false");
                        }
                    },
                    error: function () {
                        alert("error!!!!");
                    }
                });
            }

            function enableUpdate(ePunchId, td) {
                document.getElementById('ttUpd').disabled = false;
                //alert($(".img").attr("updTime"));
                $("div[class*='img']").attr("updTime", "true");
                $('#ttUpd:not(".boundTime")').addClass("boundTime").click(function () {
                    updatePunchTime(ePunchId, td);
                });
                $('#punchTime > input').css({
                    "background-color": "#FFDDDD"
                })
                $('#punchTime > select').css({
                    "background-color": "#FFDDDD"
                })
            }

            function enableMove(ePunchId, td) {
                document.getElementById('ttUpd').disabled = false;
                //alert($(".img").attr("updMove"));
                $("div[class*='img']").attr("updMove", "true");
                $('#ttUpd:not(".boundMove")').addClass('boundMove').click(function () {
                    movePunch(ePunchId, td);
                });
                $('#moveShifts').css({
                    "background-color": "#FFDDDD"
                })
                $('.moveDepts').css({
                    "background-color": "#FFDDDD"
                })

                var shiftOpts = "";
                var curShift = $("#moveShifts").val();

                var $depts = $(".moveDepts");
                $.each($depts, function (i, l) {
                    if ($(l).parent().attr("dept") == curShift) {
                        $(l).parent().show();
                    }
                    else {
                        $(l).parent().hide();
                    }
                });
            }

            var shiftNames = ["", "1st Shift", "2nd Shift", "3rd Shift", "Shift A", "Shift B", "Shift C", "Shift D"];
            function addMoveHtml(curShift, curDept) {
                var deptMain = "";
                var moveRows = "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td class='ttBtn px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis' colspan='4'><hr/><span class='block text-sm font-medium text-brand-black mb-1'>Move:</span></td></tr><tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'>";
                var shiftOpts = "<td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis' colspan='1'><select id='moveShifts' class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
                for (var i = 0; i < shiftData.length; i = i + 1) {
                    shiftOpts += "<option value='" + shiftData[i].Type + "' ";
                    if ((i + 1) == curShift)
                        shiftOpts += "selected ";
                    shiftOpts += ">" + shiftNames[shiftData[i].Type] + "</option>";
                    var deptOpts = "<td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis' colspan='3' dept='" + shiftData[i].Type + "' style='";
                    if (curShift != shiftData[i].Type)
                        deptOpts += "display:none;";
                    else {
                        deptOpts += "display:block;";
                    }
                    deptOpts += "'><select class='moveDepts w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green'>";
                    for (var j = 0; j < shiftData[i].Department.length; j = j + 1) {
                        deptOpts += "<option value='" + shiftData[i].Department[j].Id + "' ";
                        if( curDept == shiftData[i].Department[j].Id )
                            deptOpts += "selected";
                        deptOpts += ">" + shiftData[i].Department[j].Name + "</option>";
                    }
                    deptOpts += "</select></td>";
                    deptMain += deptOpts;
                }
                shiftOpts += "</select></td>";
                moveRows += shiftOpts + deptMain;
                return moveRows;
            }

            function setVideoSrc(clientId, punchDate) {
                alert("Client = " + clientId + ", punchDate = " + punchDate);
                Url = RosterBaseURL + "/VideoUrl?clientID=" + clientId + "&date=" + punchDate;
                alert("Here we go!");
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: Url,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (d) {
                        //alert(d);
                        if (d.length > 0)
                        {
                            //alert(d);
                            var video = document.getElementById('video');
                            var source = document.createElement('source');
                            alert(video + ", " + source + ", " + d);
                            source.setAttribute('src', d);
                            source.setAttribute('type', "video/mp4");

                            $("#videoBox").show();
                            video.appendChild(source);
                            video.play();
                        }
                        else {
                            //alert("No video available for that time!");
                        }
                    },
                    error: function (e) {
                        //alert("error!!!!" + e);
                    }
                });
            }

        var months = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13"];
        var days = ["00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18",
            "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33"];

        function getFilePath(idStr, clientId, dt) {
            var dt2 = new Date(dt);
            dt = dt2;
            var id = parseInt(idStr.substring(1));

            id = parseInt(id / 100);
            var dirPath = "";
            for (var i = 0; i < 4; i++) {
                dirPath = "/" + (id % 10) + dirPath;
                id = parseInt(id / 10);
            }
            dirPath += "/" + dt.getFullYear() + ("0" + (dt.getMonth() + 1)).slice(-2);

            dirPath = "http://msiwebtrax.com/EmployeeImages/" + clientId + dirPath;
            return dirPath;
        }

        function addHtml(im, nm, span) {
            var storageLoc = "s3-us-west-2.amazonaws.com/"
            var $flexDiv = span.closest('div[class*="flexigrid"]');
            var $deptTr = $flexDiv.closest('tr');
            $deptTr = $deptTr.prevAll('tr[id*="trDepartment"]').first();
            var curDept = $deptTr.find("input[id*='hdnDept']").val();
            var curShift = $deptTr.find("input[id*='hdnShift']").val();
            var badgeNumber = $flexDiv.closest("tr").prev("tr[id*='emp']").find("span[id*='BadgeNumber']");
            badgeNumber = "/" + badgeNumber.text().substring(2);
            //alert(badgeNumber);
            var dtEx = span.attr("exact");
            var dtRnd = span.attr("round");

            var ticks = span.attr("ticks");
            var gps = span.attr("loc");
            var bDate = new Date(dtEx);
            var bucketName = bDate.getUTCFullYear() + "-" + months[bDate.getUTCMonth()] + "-" +
                days[bDate.getUTCDate()] + "-punchpics.";

            var createdBy = span.attr("createdBy");
            var createdDt = span.attr("createdDt");
            var updatedBy = span.attr("updatedBy");
            var updatedDt = span.attr("updatedDt");
            var manual = span.attr("manual");
            var clientId = $("input[id*='clientID']").val();
            var dir = $('input[id*="_clientDir"]').val();

            var ret = "<table class=\"img w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200\">";
            //ret += "<tr><td colspan='4'>" + nm + "</td><td id=\"videoBox\" hidden='true' rowspan='4'><video id=\"video\" width=\"400\" controls>"
            //    + "Your browser does not support HTML5 video.</video></tr>";
            ret += "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td colspan='4' class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'>" + htmlDate(dtEx, false) + "<input id='exactDate' type='hidden' value='" + dtEx + "'/></td></tr>";
            ret += "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td colspan='4' class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'>" + htmlDate(dtRnd, false) + "<input id='roundedDate' type='hidden' value='" + dtRnd + "'/></td></tr>";
            ret += "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td colspan='4' class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis' width=\"320\" height=\"246\">";

            if (manual == 'False') {
                if (dir != 165 && dir != 185 && dir != 399) {
                    var tmpDir = dir;
                    if (tmpDir == 415) tmpDir = 414;
                    ret += "<img alt=\"Picture Not Available\" src=\"http://msiwebtrax.com/dropbox/images/" + tmpDir + "/" +
                        im + "\"/>";
                }
                else if (dir != 165 && dir != 399) {
                    ret +=
                        "" + 
                        "<img alt=\"Picture Not Available\" src=\"https://" + bucketName + storageLoc
                        + badgeNumber + "_" + ticks + ".jpg\"" + "/>";
                }
                else {
                    var filePath = getFilePath(badgeNumber, dir, bDate);
                    ret += "<img  height=\"320\" width=\"240\"  alt=\"Picture Not Available\" src=\"" + filePath +
                        badgeNumber + bDate.getFullYear() + ("0" + (bDate.getMonth() + 1)).slice(-2) + ("0" + bDate.getDate()).slice(-2) + "_" +
                        ("0" + bDate.getHours()).slice(-2) + ("0" + bDate.getMinutes()).slice(-2) +
                        ("0" + bDate.getSeconds()).slice(-2) + ".jpg\"/>";
                    ret += "<td colspan='4' width=\"320\" height=\"246\">";
                    ret += "<img src=\"https:" + "//maps.googleapis.com/maps/api/staticmap?" +
                        "markers=" + gps + "&" +  
                        "size=300x300&maptype=roadmap&" + 
                        "key=AIzaSyAKVZCCbme2S6fnAh7vfRFUyq4QvC6Fc34\"/>";
                }
            }
            else {
                ret += "<table class='w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200'><thead><tr class='bg-brand-grey text-white text-sm font-medium uppercase tracking-wider px-6 py-3 text-left'><th colspan='4'>Manual Override</th></thead></tr>" +
                    "<tbody class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><tr><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm font-medium text-brand-black'>Created By:</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm'>" + createdBy + "</span></td></tr>" +
                    "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm font-medium text-brand-black'>Created On:</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm'>" + createdDt + "</span></td></tr>";
                if (updatedBy.length > 1) {
                    ret +=
                        "<tbody class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><tr><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm font-medium text-brand-black'>Updated By:</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm'>" + updatedBy + "</span></td></tr>" +
                        "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm font-medium text-brand-black'>Updated On:</span></td><td class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><span class='text-sm'>" + updatedDt + "</span></td></tr>";
                }
                ret += "</tbody></table>";
            }
            ret += "</td></tr>";
            if (updateAuthorized) {
                /* update punches */
                ret += "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td colspan='4' class='ttBtn px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><hr/>Update:<span style='float:right; padding-right:8px' onclick='closePopUps()'>close</span></td></tr>";
                ret += "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td id='punchTime' colspan='4' class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'>" + htmlDate(dtRnd, true) + "</td></tr>";
                /* move punches */
                ret += addMoveHtml(curShift, curDept);
                ret += "<tr class='hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0'><td colspan='4' class='px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis'><input id='ttUpd' class='ttBtn bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2' type='button' disabled='true' value='Update Punch Record'/>";
                ret += "<input id='ttDel' class='ttBtn bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2 ml-2' type='button' value='Delete Punch Record'/></td>";
                /* split shift */
                //ret += 
            }

            ret += "</tr></table>";
            return ret;
        }

        function htmlDate(dt, inp) {
            dt = new Date(dt);
            var Days = new Array("Sun", "Mon", "Tues", "Wed", "Thu", "Fri", "Sat", "Sun");
            var Months = new Array("Trouble ", "Jan. ", "Feb. ", "Mar. ", "Apr. ", "May ", "June ", "July ", "Aug. ", "Sept. ", "Oct. ", "Nov. ", "Dec. ");

            var mnth = parseInt(dt.getMonth(), 10) + 1;

            var day = dt.getDate(); // .slice(0, s);

            var yr = dt.getFullYear(); // .slice(0, s);

            var amPm = 'AM';

            var hrs = dt.getHours(); // .slice(0, s);
            if (hrs >= 12) {
                hrs -= 12;
                amPm = 'PM';
            }
            if (hrs == 0)
                hrs += 12;
            if (hrs <= 9)
                hrs = '0' + hrs;
            //dt = dt.slice(s + 1);

            //s = dt.indexOf(' ');
            var mins = dt.getMinutes(); //dt.slice(0, s);
            if (mins <= 9)
                mins = '0' + mins;

            if (inp == false) {
                return Days[dt.getDay()] + ", " + Months[parseInt(mnth, 10)] + day + ", " + yr +
                    "  " + hrs + ":" + mins + " " + amPm;
            }
            else {
                var $inp, $inpClose;
                var $sel, $selClose;
                var $disabled = " disabled='disabled'";
                if (updateAuthorized)
                    $disabled = "";
                $inp = "<input";
                $inpClose = "</input>";
                $sel = "<select";
                $selClose = "</select>";

                var ret = $inp + $disabled + " class='ttDate w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm' type='text' size='14' value='" + Months[parseInt(mnth, 10)] + day + ", " + yr;
                ret += "'>" + $inpClose + $inp + " class='ttDateHidden' type='hidden' value='" + mnth + " " + day + " " + yr + "'>" + $inpClose;

                ret += $sel + $disabled + " class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green' name='hours'>";
                for (var i = 1; i <= 12; i++) {
                    ret += "<option value='" + i + "'";
                    if (i != hrs)
                        ret += ">" + i + "</option>";
                    else
                        ret += " selected>" + i + "</option>";
                }
                ret += $selClose + ":";
                ret += $sel + $disabled + " class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green' name='mins'>";
                for (var i = 0; i < 60; i += 5) {
                    var v = i;
                    if (i < 10)
                        v = '0' + i;
                    ret += "<option value='" + v + "'";
                    if (i != mins)
                        ret += ">" + v + "</option>";
                    else
                        ret += "selected>" + v + "</option>";
                }
                ret += $selClose + " ";

                ret += $sel + $disabled + " class='w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green' name='amPm'>";
                if (amPm == "AM")
                    ret += "<option value='AM' selected>AM</option><option value='PM'>PM</option>";
                else
                    ret += "<option value='PM' selected>PM</option><option value='AM'>AM</option>";
                ret += $selClose;
                return ret;
            }
        }
        function closePopUps() {
            $(".img").remove();
            $("#addPunch").remove();
        }

        function addPunch(e, $td, id) {
            var dtRnd = new Date();
            var crID = 0;
            var sY = $(window).scrollTop() + $(window).height();

            var topTR = $td.closest('table').closest('tr').prev('tr');
            var name = topTR.find('span[id*="LastName"]').text() + ", " + topTR.find('span[id*="FirstName"]').text();
            var badge = topTR.find('span[id*="Badge"]').text().substring(2);
            var topSect = topTR.prevAll('tr[id*="trDepartment"]').first();
            var shiftType = topSect.find('input[id*="hdnShift"]').val();
            var dept = topSect.find('input[id*="hdnDept"]').val();
            //alert(dept + ", " + topSect.find('p[id*="pDeptStart"]').length + ", " + topSect.find('p[id*="pDeptStart"]').text());
            var weekStart = topSect.find('p[id*="pDeptStart"]').text();
            var weekEnd = topSect.find('p[id*="pDeptEnd"]').text();
            var d = $("<div id='addPunch' class='tooltip w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200'>" +
                "<div id='adder' cr_id='" + crID + "' td='" + $td.attr('id') + "' class='p-4'>Add punch for:<span style='float:right; padding-right:8px' onclick='closePopUps()'>close</span><hr/>" +
                "<span class='text-lg font-medium text-brand-black'>" + name + '</span><br/>' +
                "ID# <span id='apBadge' class='text-lg font-medium text-brand-black'>" + badge + "</span>" +
                "<p>" + htmlDate(dtRnd, true) + "</p><hr/>" +
                "<input type='button' style='float:right; padding-right:8px;' onclick='createPunch()' value='ADD' class='bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2'/>" +
                "<input type='hidden' value='" + dept + "' class='hdnDept'/>" +
                "<input type='hidden' value='" + shiftType + "' class='hdnShift'/>" +
                "<input type='hidden' value='" + weekStart + "' class='hdnStart'/>" +
                "<input type='hidden' value='" + weekEnd + "' class='hdnEnd'/>" +
                "</div>" +
                "</div>");
            d.appendTo('body').addClass("tooltip").css('top', (e.pageY - 80) + 'px').css('left', (e.pageX + 10) + 'px');
            setDatePicker();
        }
        function setDatePicker() {
            var d = new Date($(".ttDateHidden").val());
            $('.ttDate').datepicker({
                altFormat: "mm dd yy",
                altField: ".ttDateHidden",
                dateFormat: "M dd, yy",
                defaultDate: d,
                monthNamesShort: ["Jan. ", "Feb. ", "Mar. ", "Apr. ", "May ", "June ", "July ", "Aug. ", "Sept. ", "Oct. ", "Nov. ", "Dec. "]
            });
            //alert("set!");
        }

        $(function () {
            $('#popup-wrapper').modalPopLite({ openButton: 'tzz[id*="ZAction"]', closeButton: '#close-btn' });
        });

            function createPunch() {
                var $time = $('#adder > p > select option:selected');
                var $date = $('.ttDateHidden').val() + ' ' +
                            $time.eq(0).text() + ':' + //HOURS
                            $time.eq(1).text() + ' ' + //MINS
                            $time.eq(2).text(); //AM-PM
                var dtRnd = new Date($date);

                var start = new Date($('input[id*="weekStartDate"]').val());
                start = new Date($('p[id*="pDeptStart"]').val());
                //alert(start);
                var end = new Date($('input[id*="weekEndDate"]').val());
                end = new Date($('p[id*="pDeptEnd"]').val());
                //alert(end);
                var warning = "";

                if (dtRnd < start || dtRnd > end) {
                    warning += "<br/><hr/><b>Warning!</b> Date is out of page range.  It WON'T be visible on this Weekly Report!" +
                                        "<br/>The only punches visible on this page are between: <br/><b>" + start.toLocaleString() +
                                        "</b><br/> and <br/><b>" + end.toLocaleString() + "</b>";
                }
                if (warning.length > 0) /* warning dialog */{
                    $('<div class="w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 p-4">' + warning + '</div>').dialog({
                        title: "<b>Add Punch Warning!</b>",
                        width: "460px",
                        draggable: true,
                        buttons: {
                            Cancel: function () {
                                $(this).dialog("close");
                            }
                        },
                        close: function () {
                            //nothing happens on close...
                        }
                    });
                }
                else {
                    createPunchOK(dtRnd);
                }
            }

            function createPunchOK($dtRnd) {
                var m = $dtRnd.getMonth() + 1;
                if (m < 10) m = '0' + m;
                var d = $dtRnd.getDate();
                if (d < 10) d = '0' + d;
                var amPm = 'AM';
                var h = $dtRnd.getHours();
                if (h > 12) {
                    h -= 12;
                    amPm = 'PM';
                }
                if (h == 12)
                    amPm = 'PM';
                var mins = $dtRnd.getMinutes();
                if (mins < 10)
                    mins = '0' + mins;
                $date = m + "/" + d + "/" + $dtRnd.getFullYear() + " " + h + ":" + mins + ' ' + amPm;
                var badge = $("span[id='apBadge']").text();
                var userID = $("input[id*='userID']").val();
                var clientID = $("input[id*='clientID']").val();
                var rosterID = -1;
                var deptID = $("input[class='hdnDept']").val();
                var shiftType = $("input[class='hdnShift']").val();

                /* retreive roster id for given punch value and client id */
                Url = RosterBaseURL + badge + '/EmployeeRosters?startDate=' +  $date + '&endDate=' + $date + '&clientID=' + clientID;
                //alert( Url);
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: Url,
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (d) {
                        for (var i = 0; i < d.length; i++) {
                            if (d[i].ClientID == clientID) {
                                rosterID = d[i].ID;
                                //deptID = d[i].Dept;
                                break;
                            }
                        }
                        if (rosterID == -1) //no roster
                        {
                            alert("No roster exists for this punch.  Make sure the roster is set up before adding a punch in that range.");
                        }
                        //else if( deptID != currentDeptID )
                        else {
                            Url = ClientBaseURL + badge + '/AddPunch?crID=' + rosterID + '&punchDate=' + $date + '&deptID=' + deptID + '&userID=' + userID + '&shiftType=' + shiftType;
                            $.ajax({
                                cache: false,
                                type: "GET",
                                url: Url,
                                async: false,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (d) {
                                    var tdID = '#' + $('#adder').attr('td');
                                    var tdNew = $('#adder').attr('td');
                                    $(tdID).html("<span style='font-size:14px; color:Green;padding:4px;'>" + $date + " *ADDED*</span>");
                                    $('#addPunch').remove();
                                    $(tdID).off('click');
                                    $(tdID).off('hover');
                                    $(tdID).removeAttr("class");
                                },
                                error: function (msg) {
                                    msg = eval(msg);
                                    alert(msg);
                                }
                            });
                        }
                    },
                    error: function (msg) {
                        msg = eval(msg);
                        alert(msg);
                    }
                });
            }

            (function (root) {  // root = global
                "use strict";
                root.exportToXlsx = function (tableId) {
                    // open the document
                    openXml.util.bucketTimer.init();
                    openXml.util.bucketTimer.bucket("OpenAndSave");

                    //var doc = new openXml.OpenXmlPackage(openedFileData);
                    var doc = new openXml.OpenXmlPackage(excelTemplate);
                    var workbook = doc.workbookPart();
                    var workbookXDoc = workbook.getXDocument();
                    var sheets = workbookXDoc.root.element(S.sheets);
                    var sheet = null;
                    var relId = null;
                    var sheetName = "MySheet";
                    for (var i = 0; i < sheets.nodesArray.length; i++) {
                        sheet = sheets.nodesArray[i];
                        if (sheet.nodeType != 'Element')
                            continue;
                        if (sheet.attribute('name').value == sheetName) {
                            relId = sheet.attribute(R.id).value;
                            break;
                        }
                    }

                    if (relId == null)
                        alert(sheetName + " not found!");
                    else {
                        var worksheet = workbook.getPartById(relId);
                        var worksheetXDoc = worksheet.getXDocument();
                        var dimensions = worksheetXDoc.root.element(S.dimension);
                        var coords = dimensions.attribute('ref').value;
                        var sheetData = worksheetXDoc.root.element(S.sheetData);
                        sheetData.removeAll();

                        /* set filename */
                        var dt = new Date;
                        var fileName = '' + dt.getFullYear() + (dt.getMonth() + 1) + dt.getDate() + '_';
                        /* lets add some data */
                        peekABooToExcel(tableId, sheetData, worksheetXDoc);
                        fileName += 'peekABoo';

                        // serialize it to a blob
                        var openedFileData = doc.saveToBlob();
                        //openedFileData = doc.saveToFlatOpc();

                        openXml.util.bucketTimer.bucket("Done");
                        var s = openXml.util.bucketTimer["OpenAndSave"];
                        //alert("Document modified.  Elapsed time: " + (s.time / 1000).toString() + " seconds");

                        saveAs(openedFileData, fileName + '.xlsx');
                        /******************************************************************************/
                    }
                }
                root.exportHRToXlsx = function (tableId) {
                    // open the document
                    openXml.util.bucketTimer.init();
                    openXml.util.bucketTimer.bucket("OpenAndSave");

                    //var doc = new openXml.OpenXmlPackage(openedFileData);
                    var doc = new openXml.OpenXmlPackage(excelTemplate);
                    var workbook = doc.workbookPart();
                    var workbookXDoc = workbook.getXDocument();
                    var sheets = workbookXDoc.root.element(S.sheets);
                    var sheet = null;
                    var relId = null;
                    var sheetName = "MySheet";
                    for (var i = 0; i < sheets.nodesArray.length; i++) {
                        sheet = sheets.nodesArray[i];
                        if (sheet.nodeType != 'Element')
                            continue;
                        if (sheet.attribute('name').value == sheetName) {
                            relId = sheet.attribute(R.id).value;
                            break;
                        }
                    }

                    if (relId == null)
                        alert(sheetName + " not found!");
                    else {
                        var worksheet = workbook.getPartById(relId);
                        var worksheetXDoc = worksheet.getXDocument();
                        var dimensions = worksheetXDoc.root.element(S.dimension);
                        var coords = dimensions.attribute('ref').value;
                        var sheetData = worksheetXDoc.root.element(S.sheetData);
                        sheetData.removeAll();

                        /* set filename */
                        var weDate = $("select[id*='txtCalendar']").find(":selected").text();
                        var fnM = weDate.substring(0, weDate.indexOf("/"));
                        var fnD = weDate.substring(weDate.indexOf("/") + 1, weDate.length - 5);
                        if (fnD.length == 1) fnD = "0" + fnD;
                        if (fnM.length == 1) fnM = "0" + fnM;
                        var fileName = weDate.substring(weDate.length - 4) + "_" + fnM + "_" + fnD + "_";
                        var dt = new Date;
                        //var fileName = '' + dt.getFullYear() + (dt.getMonth() + 1) + dt.getDate() + '_';
                        /* lets add some data */
                        //peekABooToExcel(tableId, sheetData, worksheetXDoc);
                        hoursReportToExcel(tableId, sheetData, worksheetXDoc);
                        fileName += 'hoursReport';

                        // serialize it to a blob
                        var openedFileData = doc.saveToBlob();
                        //openedFileData = doc.saveToFlatOpc();

                        openXml.util.bucketTimer.bucket("Done");
                        var s = openXml.util.bucketTimer["OpenAndSave"];
                        //alert("Document modified.  Elapsed time: " + (s.time / 1000).toString() + " seconds");

                        saveAs(openedFileData, fileName + '.xlsx');
                        /******************************************************************************/
                    }
                }


                function peekABooToExcel(recs, sheetData, worksheetXDoc, freezePane) {
                    /* cost centers first! */
                    /* cost centers */
                    var ccs = {};
                    var $rows = $("tr[class*='DlyHrs']");

                    alert($rows.length);
                    var i = 0;
                    var regTotal = 0;
                    var otTotal = 0;
                    var hoursTotal = 0;
                    var regBillTotal = 0;
                    var otBillTotal = 0;
                    var billTotal = 0;
                    var acaTotal = 0;
                    $rows.each(function (i, r) {
                        var deptName = $(r).prevAll('tr[id*="trDepartment"]').first().find('span[id*="lblDepartment"]').text();
                        //alert( "Row " + i + ", " + deptName );
                        deptName = deptName.substring(deptName.indexOf(" - ") + 3, deptName.lastIndexOf(" - "));
                        var reg = 0;
                        var ot = 0;
                        var payRate = 0;
                        var billReg = 0;
                        var billOT = 0;
                        if (!(deptName in ccs)) {
                            /* initialize */
                            //alert("Row " + i + ": " + deptName + " is not there!");
                            reg = parseFloat($(r).find('span[id*="lblTotalHours"]').text());
                            ot = parseFloat($(r).find('span[id*="lblOTHours"]').text());
                            payRate = $(r).find('input[id*="hdnPayRate"]').val();
                            payRate = parseFloat(payRate.substring(payRate.indexOf('$') + 1));
                            billReg = reg * payRate;
                            billOT = ot * payRate * 1.5;

                            ccs[deptName] = { 'name': deptName, 'count': 1, 'reg': reg, 'ot': ot, 'billReg': billReg, 'billOT': billOT };
                        }
                        else {
                            reg = parseFloat($(r).find('span[id*="lblTotalHours"]').text());
                            ot = parseFloat($(r).find('span[id*="lblOTHours"]').text());
                            payRate = $(r).find('input[id*="hdnPayRate"]').val();
                            payRate = parseFloat(payRate.substring(payRate.indexOf('$') + 1));
                            billReg = reg * payRate;
                            billOT = ot * payRate * 1.5;
                            ccs[deptName].reg += reg;
                            ccs[deptName].ot += ot;
                            ccs[deptName].count++;
                            ccs[deptName].billReg += billReg;
                            ccs[deptName].billOT += billOT;
                        }
                    });
                    var mult = $('input[id*="multiplier"]').val();
                    var outpt = "Cost center ids:";
                    //alert(ccs.length);
                    var ccList = getKeys(ccs);

                    //Badge	Employee	Reg	OT	Total	Approved By
                    /* header elements we want.. */
                    setCell(1, 1, sheetData, 0, null, 'ID #', HeaderStyle, 1, 1);
                    setCell(1, 2, sheetData, 0, null, 'Employee', HeaderStyle, 1, 1);
                    setCell(1, 3, sheetData, 0, null, 'Reg Hrs', HeaderStyle, 1, 1);
                    setCell(1, 4, sheetData, 0, null, 'OT Hrs', HeaderStyle, 1, 1);
                    setCell(1, 5, sheetData, 0, null, 'Total Hrs', HeaderStyle, 1, 1);
                    setCell(1, 6, sheetData, 0, null, 'Approved By', HeaderStyle, 1, 1);

                    //Cost Center	Head Count	Reg. Hours	OT Hours	Total Hours	Reg. Bill Hours	OT Bill Hours	Total Bill Hours
                    setCell(1, 8, sheetData, 0, null, 'Cost Center', HeaderStyle, 1, 1);
                    setCell(1, 9, sheetData, 0, null, 'Head Count', HeaderStyle, 1, 1);
                    setCell(1, 10, sheetData, 0, null, 'Reg. Hours', HeaderStyle, 1, 1);
                    setCell(1, 11, sheetData, 0, null, 'OT Hours', HeaderStyle, 1, 1);
                    setCell(1, 12, sheetData, 0, null, 'Total Hours', HeaderStyle, 1, 1);
                    setCell(1, 13, sheetData, 0, null, 'Reg. Bill Hours', HeaderStyle, 1, 1);
                    setCell(1, 14, sheetData, 0, null, 'OT Bill Hours', HeaderStyle, 1, 1);
                    setCell(1, 15, sheetData, 0, null, 'Total Bill Hours', HeaderStyle, 1, 1);
                    //setCell(1, 16, sheetData, 0, null, 'ACA Cost', HeaderStyle, 1, 1);

                    var widths = [];
                    widths[0] = 2;
                    widths[1] = 12;
                    widths[2] = 10;
                    widths[3] = 10;
                    widths[4] = 12;
                    widths[5] = 16;
                    widths[6] = 1;
                    widths[7] = 16;
                    widths[8] = 16;
                    widths[9] = 16;
                    widths[10] = 16;
                    widths[11] = 16;
                    widths[12] = 16;
                    widths[13] = 16;
                    widths[14] = 16;
                    widths[15] = 16;
                    //widths[16] = 16;

                    var row = 2;
                    grandTotal = 0;
                    var rStyle;
                    var tot = 0;

                    for (var i = 0; i < recs.length; i++) {
                        if (recs[i].id == 0) continue;
                        var dailyTotal = 0;
                        var otTrack = 0;
                        var numRows = 0;
                        var r = 0;
                        var o = 0;
                        var h = 0;
                        for (var j = 0; j < recs[i].hours.length; j++) {
                            var t1 = 0;
                            var t2 = 0;
                            if (numRows == 0)
                                dailyTotal = 0;

                            // use 2 rows?  or just 1? 
                            if (j < recs[i].hours.length - 1 && numRows == 0) {
                                var timeDiff = Math.abs(recs[i].hours[j + 1].end - recs[i].hours[j].start);
                                var diffMins = Math.ceil(timeDiff / (1000 * 60));
                                if (diffMins > 18 * 60)
                                    numRows = 1;
                                else
                                    numRows = 2;
                            }
                            else
                                numRows = 1;

                            r = 0;
                            o = 0;
                            h = Math.round(parseFloat(recs[i].hours[j].hours) * 100) / 100;
                            grandTotal += h;
                            dailyTotal += h;
                            if ((otTrack + h) > 40.0) {
                                if (otTrack >= 40.0) {
                                    r = 0;
                                    o = h;
                                }
                                else {
                                    r = 40.0 - otTrack;
                                    o = (h + otTrack) - 40.0;
                                }
                            }
                            else {
                                o = 0;
                                r = h;
                            }
                            otTrack += h;
                            if (row % 2 == 0)
                                rStyle = RowStyle;
                            else
                                rStyle = RowStyleOdd;
                            if (numRows == 1) {
                                setCell(row, 1, sheetData, null, null, recs[i].id, rStyle, 1, 1);
                                setCell(row, 2, sheetData, null, null, recs[i].name, rStyle, 1, 1);
                                setCell(row, 3, sheetData, dec2(r), null, null, rStyle, 1, 1);
                                setCell(row, 4, sheetData, dec2(o), null, null, rStyle, 1, 1);
                                setCell(row, 5, sheetData, dec2(dailyTotal), null, null, rStyle, 1, 1);
                                setCell(row, 6, sheetData, null, null, recs[i].hours[j].approvedBy, rStyle, 1, 1);
                                setCell(row, 7, sheetData, null, null, ' ', rStyle, 1, 1);
                                if( (row-2) < ccList.length )
                                {
                                    var reg = parseFloat(ccs[ccList[(row-2)]].reg);
                                    var ot = parseFloat(ccs[ccList[(row-2)]].ot);
                                    var regBill = parseFloat(ccs[ccList[(row-2)]].billReg);
                                    var otBill = parseFloat(ccs[ccList[(row - 2)]].billOT);

                                    regTotal += reg;
                                    otTotal += ot;
                                    hoursTotal += (reg + ot);
                                    regBillTotal += regBill;
                                    otBillTotal += otBill;
                                    billTotal += (regBill + otBill);
                                    acaTotal += (reg * 0.19);
                                    setCell(row, 8, sheetData, ccs[ccList[(row-2)]].name, null, null, rStyle, 1, 1);
                                    setCell(row, 9, sheetData, dec2(ccs[ccList[(row-2)]].count), null, null, rStyle, 1, 1);
                                    setCell(row, 10, sheetData, dec2(reg), null, null, rStyle, 1, 1);
                                    setCell(row, 11, sheetData, dec2(ot), null, null, rStyle, 1, 1);
                                    setCell(row, 12, sheetData, dec2(reg + ot), null, null, rStyle, 1, 1);
                                    setCell(row, 13, sheetData, dec2(regBill), null, null, rStyle, 1, 1);
                                    setCell(row, 14, sheetData, dec2(otBill), null, null, rStyle, 1, 1);
                                    setCell(row, 15, sheetData, dec2(regBill + otBill), null, null, rStyle, 1, 1);
                                    //setCell(row, 16, sheetData, dec2(reg*0.19), null, null, rStyle, 1, 1);
                                }
                                else if( (row-2) == ccList.length )
                                {
                                        setCell(row, 8, sheetData, 0, null, 'Totals:', rStyle, 1, 1);
                                        setCell(row, 9, sheetData, " ", null, null, rStyle, 1, 1);
                                        setCell(row, 10, sheetData, dec2(regTotal), null, null, rStyle, 1, 1);
                                        setCell(row, 11, sheetData, dec2(otTotal), null, null, rStyle, 1, 1);
                                        setCell(row, 12, sheetData, dec2(hoursTotal), null, null, rStyle, 1, 1);
                                        setCell(row, 13, sheetData, dec2(regBillTotal), null, null, rStyle, 1, 1);
                                        setCell(row, 14, sheetData, dec2(otBillTotal), null, null, rStyle, 1, 1);
                                        setCell(row, 15, sheetData, dec2(billTotal), null, null, rStyle, 1, 1);
                                        //setCell(row, 16, sheetData, dec2(acaTotal), null, null, rStyle, 1, 1);
                                    }

                            }
                            else {
                                setCell(row, 1, sheetData, null, null, recs[i].id, rStyle, 1, 1);
                                setCell(row, 2, sheetData, null, null, recs[i].name, rStyle, 1, 1);
                                setCell(row, 3, sheetData, dec2(r), null, null, rStyle, 1, 1);
                                setCell(row, 4, sheetData, dec2(o), null, null, rStyle, 1, 1);
                                setCell(row, 5, sheetData, "", null, null, rStyle, 1, 1);
                                setCell(row, 6, sheetData, null, null, recs[i].hours[j].approvedBy, rStyle, 1, 1);
                                setCell(row, 7, sheetData, null, null, " ", rStyle, 1, 1);
                                if ((row-2) < ccList.length) {
                                        var reg = parseFloat(ccs[ccList[(row-2)]].reg);
                                        var ot = parseFloat(ccs[ccList[(row-2)]].ot);
                                        var regBill = parseFloat(ccs[ccList[(row-2)]].billReg);
                                        var otBill = parseFloat(ccs[ccList[(row - 2)]].billOT);

                                        regTotal += reg;
                                        otTotal += ot;
                                        hoursTotal += (reg + ot);
                                        regBillTotal += regBill;
                                        otBillTotal += otBill;
                                        billTotal += (regBill + otBill);
                                        acaTotal += reg * 0.19;

                                        setCell(row, 8, sheetData, ccs[ccList[(row - 2)]].name, null, null, rStyle, 1, 1);
                                        setCell(row, 9, sheetData, dec2(ccs[ccList[(row - 2)]].count), null, null, rStyle, 1, 1);
                                        setCell(row, 10, sheetData, dec2(reg), null, null, rStyle, 1, 1);
                                        setCell(row, 11, sheetData, dec2(ot), null, null, rStyle, 1, 1);
                                        setCell(row, 12, sheetData, dec2(reg + ot), null, null, rStyle, 1, 1);
                                        setCell(row, 13, sheetData, dec2(regBill), null, null, rStyle, 1, 1);
                                        setCell(row, 14, sheetData, dec2(otBill), null, null, rStyle, 1, 1);
                                        setCell(row, 15, sheetData, dec2(regBill + otBill), null, null, rStyle, 1, 1);
                                        //setCell(row, 16, sheetData, dec2(reg * 0.19), null, null, rStyle, 1, 1);
                                    }
                                else if( (row-2) == ccList.length )
                                {
                                    setCell(row, 8, sheetData, 0, null, 'Totals:', rStyle, 1, 1);
                                    setCell(row, 9, sheetData, " ", null, null, rStyle, 1, 1);
                                    setCell(row, 10, sheetData, dec2(regTotal), null, null, rStyle, 1, 1);
                                    setCell(row, 11, sheetData, dec2(otTotal), null, null, rStyle, 1, 1);
                                    setCell(row, 12, sheetData, dec2(hoursTotal), null, null, rStyle, 1, 1);
                                    setCell(row, 13, sheetData, dec2(regBillTotal), null, null, rStyle, 1, 1);
                                    setCell(row, 14, sheetData, dec2(otBillTotal), null, null, rStyle, 1, 1);
                                    setCell(row, 15, sheetData, dec2(billTotal), null, null, rStyle, 1, 1);
                                    //setCell(row, 16, sheetData, dec2(acaTotal), null, null, rStyle, 1, 1);
                                }
                            }
                            numRows--;
                            row++;
                            if (widths[0] < recs[i].id.length)
                                widths[0] = recs[i].id.length;
                            if (widths[1] < recs[i].name.length)
                                widths[1] = recs[i].name.length;
                            if (widths[5] < recs[i].hours[j].approvedBy.length)
                                widths[5] = recs[i].hours[j].approvedBy.length;
                        }
                    }
                    setColumnWidths(widths, worksheetXDoc);
                    setFreezePane(1, worksheetXDoc);
                }

                var getKeys = function (obj) {
                    var keys = [];
                    for (var key in obj) {
                        keys.push(key);
                    }
                    return keys;
                }
                var dec2 = function (obj){
                    return parseFloat(Math.round(obj * 100) / 100).toFixed(2);
                }
            } (this));
            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            function getRef(c, r) {
                var ref = '';
                c--;
                while (c > 25) {
                    ref = alpha[c % 26] + ref;
                    c = (c / 26) | 0;
                    c--;
                }
                ref = alpha[c] + ref + r;
                return ref;
            }
            function setColumnWidths(widths, worksheetXDoc) {
                /* set column widths */
                var cols = worksheetXDoc.root.element(S.cols);
                cols.removeAll();
                for (var i = 0; i < widths.length; i++) {
                    var col = new XElement(S.col);
                    col.add(new XAttribute('max', (i + 1)));
                    col.add(new XAttribute('min', (i + 1)));
                    col.add(new XAttribute('width', (widths[i] * 1.3)));
                    col.add(new XAttribute('customWidth', 1));
                    cols.add(col);
                }
            }

            function setFreezePane(row, worksheetXDoc) {
                var sheetViews = worksheetXDoc.root.element(S.sheetViews);
                sheetViews.removeAll();

                var sheetView = new XElement(S.sheetView);
                sheetView.add(new XAttribute('tabSelected', '1'));
                sheetView.add(new XAttribute('workbookViewId', '0'));
                var pane = new XElement(S.pane);
                pane.add(new XAttribute('activePane', 'bottomLeft'));
                pane.add(new XAttribute('state', 'frozen'));
                pane.add(new XAttribute('topLeftCell', 'A4'));
                pane.add(new XAttribute('ySplit', row));
                sheetView.add(pane);
                sheetViews.add(sheetView);
            }

    </script>
    </asp:Panel>

        <asp:Menu ID="mnuHoursReport" runat="server" CssClass="w-full bg-white rounded-lg shadow-md overflow-hidden mb-6"
            Orientation="Horizontal" EnableViewState="false">
            <StaticMenuItemStyle CssClass="inline-block px-6 py-3 text-brand-grey hover:bg-brand-green hover:text-white transition-colors duration-200" />
            <DynamicHoverStyle CssClass="bg-brand-green text-white" />
            <DynamicMenuStyle CssClass="bg-white rounded-md shadow-lg" />
            <StaticSelectedStyle CssClass="bg-brand-green text-white font-medium" />
            <DynamicSelectedStyle CssClass="bg-brand-green text-white" />
            <DynamicMenuItemStyle CssClass="px-6 py-3 hover:bg-brand-green hover:text-white transition-colors duration-200" />
            <Items>
                <asp:MenuItem Selected="True" Text="Roster Tracking" Value="Roster"></asp:MenuItem>
                <asp:MenuItem Text="Day Labor Tracking" Value="Day Labor"></asp:MenuItem>
            </Items>
            <StaticHoverStyle CssClass="bg-brand-green text-white" />
        </asp:Menu>
        <asp:Panel ID="pnlHeader" runat="server" CssClass="w-full max-w-6xl mx-auto bg-gray-100 rounded-lg shadow-md p-4 mb-6">
            <asp:HiddenField ID="hdnUpdBonuses" runat="server" Value="false" />
            <asp:HiddenField ID="hdnUpdPunches" runat="server" Value="false" />
            <asp:HiddenField ID="hdnUpdPayRates" runat="server" Value="false" />
            <asp:Table ID="tblPeriod" runat="server" CssClass="w-full">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><b><asp:Label ID="lblHoursConfirmation" runat="server" visible="false" CssClass="text-red-600 font-bold text-lg" Text="Hours have been successfully submitted."></asp:Label></b></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="w-1/3">
                        <asp:Table runat="server" CssClass="w-full">
                            <asp:TableRow runat="server">
                                <asp:TableCell runat="server">
                                <div><asp:Label runat="server" CssClass="block text-sm font-medium text-brand-black mb-1" Text="Week Ending:"/></div>
                                <div><asp:DropDownList runat="server" ID="txtCalendar" CssClass="w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green" /><input runat="server" type="hidden" id="weekEndDate" /><input runat="server" type="hidden" id="weekStartDate"/></div>
                                <div class="mt-2"><select runat="server" id="selectEmployees" CssClass="w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green">
                                    <option value="10000" selected="selected">All Employees</option>
                                    <option value="90">Less Than 90 Days</option>
                                    <option value="60">Less Than 60 Days</option>
                                    <option value="30">Less Than 30 Days</option>
                                    <%--<option value="-1">All Employees Incl. Invalid Punches</option>--%> 
                                    </select>
                                </div>
                                <div class="mt-4">
                                <asp:CheckBox Enabled="true" runat="server" ID="chkboxUnassigned" CssClass="rounded text-brand-green focus:ring-brand-green mr-2" /><asp:Label ID="Label10" runat="server" CssClass="text-sm text-brand-black" Text="Include Unassigned Punches"/>
                                </div>
                                <div class="mt-2">
                                <asp:CheckBox Visible="false" Enabled="true" runat="server" ID="chkboxSupervisor" CssClass="rounded text-brand-green focus:ring-brand-green mr-2" /><asp:Label Visible="false" ID="lblSupervisor" runat="server" CssClass="text-sm text-brand-black" Text="Show Supervisors / Cost Centers"/>
                                </div>
                                </asp:TableCell>
                                <asp:TableCell runat="server" ID="deptChooser" Visible="false" CssClass="p-2">
                                    <div class="mb-2"><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnDepts" ID="rbtnDept" runat="server" CssClass="rounded text-brand-green focus:ring-brand-green mr-2" Text="Berlin" /></div>
                                    <div class="mb-2"><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnDepts" ID="rbtnDept2" runat="server" CssClass="rounded text-brand-green focus:ring-brand-green mr-2" Text="Amer.Litho" /></div>
                                    <div class="mb-2"><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnDepts" Checked="true" ID="rbtnDept3" runat="server" CssClass="rounded text-brand-green focus:ring-brand-green mr-2" Text="Both" /></div>
                                </asp:TableCell>
                                <asp:TableCell CssClass="p-2">
                                    <div class="mb-2"><label class="block text-sm font-medium text-brand-black">Sort By:</label></div>
                                    <div class="mb-2"><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnSortOrder" ID="rbtnShifts" runat="server" CssClass="rounded text-brand-green focus:ring-brand-green mr-2" Text="Shift" /></div>
                                    <div class="mb-2"><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnSortOrder" ID="rbtnDepts" Checked="true" runat="server" CssClass="rounded text-brand-green focus:ring-brand-green mr-2" Text="Dept." /></div>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="w-1/3 p-2">
                        <asp:Image ID="Image1" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <div class="space-y-2">
                            <asp:Button id="btnGo" onclick="btnGo_Click" runat="server" CssClass="bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2" Text="View Hours Report"></asp:Button>
                            <asp:Button id="btnCSV" onclick="btnGo_Click" runat="server" CssClass="bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2" CommandName="GenerateCSV" Text="Generate CSV"></asp:Button>
                            <input runat="server" type="button" id="peekABoo" CssClass="bg-brand-grey hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2" value="peek"/>
                        </div>
                        
                        <div class="mt-4">
                        <asp:DropDownList id="btnOffice" runat="server" CssClass="w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green">
                            <asp:ListItem Selected="True" Text="All Offices" Value="*"></asp:ListItem>
                            <asp:ListItem Text="Elgin" Value="E"></asp:ListItem>
                            <asp:ListItem Text="Aurora" Value="R"></asp:ListItem>
                            <asp:ListItem Text="West Chicago" Value="W"></asp:ListItem>
                            <asp:ListItem Text="Bolingbrook" Value="O"></asp:ListItem>
                            <asp:ListItem Text="Bartlett" Value="B"></asp:ListItem>
                            <asp:ListItem Text="Schaumburg" Value="S"></asp:ListItem>
                            <asp:ListItem Text="Addison" Value="A"></asp:ListItem>
                            <asp:ListItem Text="Villa Park" Value="V"></asp:ListItem>
                            <asp:ListItem Text="Wheeling" Value="H"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                        <input hidden="hidden" runat="server" type="button" id="btnExportHTML" CssClass="bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2" value="Export HTML to Excel" />
                        <div class="mt-4">
                        <asp:Button id="btnSubmitApproved" onclick="btnGo_Click" runat="server" CssClass="bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2 w-full" Text="Submit Approved Hours" CommandName="SubmitApproved"></asp:Button>
                        </div>
                        <input type="hidden" id="hdnApproveList" runat="server" enableviewstate="false" value="" />
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="w-1/3 p-2">
                        <asp:Image ID="Image2" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <div class="space-y-2">
                            <asp:HyperLink ID="lnkExport" Target="_blank" NavigateURL="~/auth/HoursReportExcel.aspx?date=" runat="server" CssClass="block text-brand-green hover:underline" Text="Export Summary to Excel"></asp:HyperLink>
                            <asp:HyperLink ID="lnkExportDetail" Target="_blank" NavigateURL="~/auth/HoursReportExcel.aspx?date=" runat="server" CssClass="block text-brand-green hover:underline" Text="Export Detail to Excel"></asp:HyperLink>
                            <asp:HyperLink ID="lnkExportPeekABoo" Target="_blank" NavigateURL="~/auth/HoursReportExcel.aspx?date=" runat="server" CssClass="block text-brand-green hover:underline" Text="Export Data For PeekABoo"></asp:HyperLink>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" CssClass="p-2">
                        <asp:Panel ID="pnlTotals" runat="server" CssClass="bg-white rounded-lg shadow-md p-4 text-center">
                            <div class="flex justify-between items-center mb-2">
                                <asp:Label ID="Label2" runat="server" Text="Total Regular Hours:" CssClass="text-sm font-medium text-brand-black"></asp:Label>
                                <asp:Label ID="lblRegularHrs" runat="server" CssClass="text-sm font-medium text-brand-black" Text="0.00"></asp:Label>
                            </div>
                            <div class="flex justify-between items-center">
                                <asp:Label ID="lblTTest" runat="server" Text="Total Overtime Hours:" CssClass="text-sm font-medium text-brand-black"></asp:Label>
                                <asp:Label ID="lblOTHrs" runat="server" CssClass="text-sm font-medium text-brand-black" Text="0.00"></asp:Label>
                            </div>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
     </asp:Panel>
     <asp:Panel runat="server" ID="pnlApprovalInfo" Visible="false" CssClass="w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 mb-6">
        <table class="w-full">
            <tr class="bg-brand-green text-white">
                <td colspan="3" class="px-6 py-3 text-left"><asp:Label runat="server" ID="lblApprovalHeader" CssClass="text-lg font-medium" Text="Hours for this week have been approved."></asp:Label></td>
            </tr>
            <tr class="border-b border-gray-200">
                <td class="px-6 py-4 text-right w-1/6"><strong><asp:Label runat="server" ID="lblApprovalDateHead" Text="Approval Date/Time:" CssClass="text-sm text-brand-black"></asp:Label></strong></td>
                <td class="px-6 py-4 text-left" colspan="2"><asp:Label runat="server" ID="lblApprovalDate" CssClass="text-sm text-brand-black"></asp:Label>
                    <asp:Label ID="Label1" runat="server" Text="."></asp:Label></td>
            </tr>
            <tr class="border-b border-gray-200">
                <td class="px-6 py-4 text-right"><strong><asp:Label runat="server" ID="lblApprovedByHead" Text="Approved By:" CssClass="text-sm text-brand-black"></asp:Label></strong></td>
                <td class="px-6 py-4 text-left w-1/3"><asp:Label runat="server" ID="lblApprovedBy" Text="" CssClass="text-sm text-brand-black"></asp:Label></td>
                <td class="px-6 py-4 text-left">
                        <asp:LinkButton ID="lnkCreateInvoice" runat="server" CssClass="bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2 inline-block" Text="Generate Invoice and Finalize Week" OnClick="lnkCreateInvoice_Click"></asp:LinkButton>
                        <asp:HiddenField ID="hdnClientApprovalId" runat="server" />
                        <asp:HiddenField ID="hdnWeekEndDate" runat="server" />
                </td>
            </tr>
        </table>
     </asp:Panel>
     <asp:Panel runat="server" ID="PanelNoApproval" Visible="false" CssClass="w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 mb-6">
        <table class="w-full">
            <tr class="bg-brand-green text-white">
                <td colspan="3" class="px-6 py-3 text-left"><asp:Label runat="server" ID="Label3" CssClass="text-lg font-medium" Text="Hours for this week have been approved."></asp:Label></td>
            </tr>
            <tr class="border-b border-gray-200">
                <td class="px-6 py-4 text-right w-1/6"><strong><asp:Label runat="server" ID="Label5" Text="Approval Date/Time:" CssClass="text-sm text-brand-black"></asp:Label></strong></td>
                <td class="px-6 py-4 text-left" colspan="2"><asp:Label runat="server" ID="Label6" CssClass="text-sm text-brand-black"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text="."></asp:Label></td>
            </tr>
            <tr class="border-b border-gray-200">
                <td class="px-6 py-4 text-right"><strong><asp:Label runat="server" ID="Label8" Text="Approved By:" CssClass="text-sm text-brand-black"></asp:Label></strong></td>
                <td class="px-6 py-4 text-left w-1/3"><asp:Label runat="server" ID="Label9" Text="" CssClass="text-sm text-brand-black"></asp:Label></td>
                <td class="px-6 py-4 text-left">
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2 inline-block" Text="Generate Invoice and Finalize Week" OnClick="lnkCreateInvoice_Click"></asp:LinkButton>
                </td>
            </tr>
        </table>
     </asp:Panel>

     <asp:Panel Visible="false" runat="server" ID="pnlUnassignedPunches" CssClass="w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200 mb-6">
         <asp:Repeater id="rptrTicketTrackerException" OnItemDataBound="rptrTicketTrackerException_ItemDataBound" runat="server">
          
          <HeaderTemplate>
             <table class="w-full">
                <tr id="exceptions" class="bg-brand-grey text-white text-sm font-medium uppercase tracking-wider">
                    <td id="tdBadgeNumberHeader" runat="server" class="px-6 py-3 text-left">Badge #</td>
                    <td id="tdNameHeader" runat="server" class="px-6 py-3 text-left">Employee Name</td>
                    <td class="px-6 py-3 text-left">Swipe Date/Time</td>
                    <td class="px-6 py-3 text-left">Exception Message</td>
                </tr>
          </HeaderTemplate>
             
          <ItemTemplate>
             <tr class="hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0">
                   <td id="tdBadgeNumber" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label ID="lblItem" Text='' runat="server"></asp:Label><asp:Label ID="lblBadgeNumber" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/></td>
                   <td id="tdName" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label ID="lblFullName" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>' Runat="server"/></td>
                   <td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label ID="lblSwipeDateTime" Text='' Runat="server"/></td>
                   <td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label ID="lblExceptionMessage" Text='' Runat="server"/></td>
            </tr>
          </ItemTemplate>
          <FooterTemplate>
            </table>
          </FooterTemplate>

       </asp:Repeater>
     </asp:Panel>
<asp:Repeater ID="rptrSupervisorList" OnItemDataBound="rptrSupervisorList_ItemDataBound" runat="server">
    <HeaderTemplate>
    <table class="w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200">
        <thead>
        <tr runat="server" id="top1" class="bg-brand-grey text-white">
        <td colspan="16"></td>
        </tr>
        <tr runat="server" id="top2" class="bg-brand-grey text-white">
        <td colspan="16"></td>
        </tr>
        <tr runat="server" id="top3" class="bg-brand-grey text-white">
        <td colspan="16"></td>
        </tr>
        <tr class="bg-brand-grey text-white text-sm font-medium uppercase tracking-wider">
            <td class="px-6 py-3 text-left">
                <b>#</b>
            </td>
            <td class="px-6 py-3 text-left">
                <b>Badge #</b>
            </td>
            <td class="px-6 py-3 text-left">
                <b>Name</b>
            </td>
            <td class="px-6 py-3 text-left">
                <b>Department</b>
            </td>
            <td class="px-6 py-3 text-left">
                <b>Shift</b>
            </td>
            <td class="px-6 py-3 text-left">
                <b>Pay Rate</b>
            </td>
            <td class="px-6 py-3 text-left">
                <b>Cost Center</b>
            </td>
            <td class="px-6 py-3 text-left">
                <b>Supervisor</b>
            </td>
            <td runat="server" id="hdrOne" class="px-6 py-3 text-left">
            </td>
            <td runat="server" id="hdrTwo" class="px-6 py-3 text-left">
            </td>
            <td runat="server" id="hdrThree" class="px-6 py-3 text-left">
            </td>
            <td runat="server" id="hdrFour" class="px-6 py-3 text-left">
            </td>
            <td runat="server" id="hdrFive" class="px-6 py-3 text-left">
            </td>
            <td runat="server" id="hdrSix" class="px-6 py-3 text-left">
            </td>
            <td runat="server" id="hdrSeven" class="px-6 py-3 text-left">
            </td>
            <td class="px-6 py-3 text-left">Reg Hours
            </td>
            <td class="px-6 py-3 text-left">OT Hours
            </td>
        </tr>
    </thead>
    </HeaderTemplate>

    <ItemTemplate>
    <tr runat="server" class="hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0">
        <td id="cellEmpCount" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellBadge" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellName" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellDept" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellShift" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellPayRate" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellCostCenter" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellSupervisor" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellOne" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellTwo" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellThree" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellFour" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellFive" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellSix" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellSeven" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellReg" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
        <td id="cellOT" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"></td>
    </tr>
   </ItemTemplate>
    <FooterTemplate>
    </table>
    </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptrHoursReport" OnItemDataBound="rptrHoursReport_ItemDataBound"  OnItemCommand="rptrHoursReport_ItemCommand" runat="server">
        <HeaderTemplate>
             <table id="tblHoursReport" class="w-full max-w-6xl mx-auto rounded-lg shadow-md overflow-hidden border border-gray-200">
                <tr>
                    <td ID="tdClientNameSpc" runat="server" Visible="true"></td>
                    <td colspan="2" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblClientName" Runat="server" Text=""/></td>
                </tr>
                <tr id="trExcelTitle" runat="server">
                    <%-- original code for td element, added empty cell <td>
                    <td colspan="11" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelTitle" Runat="server" Text="MSI WebTrax Hours Report"/></td>
                    --%>
                    <td id="tdExcelTitleSpc" runat="server" style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                    <td colspan="2" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelTitle" Runat="server" Text="MSI WebTrax Hours Report"/></td>
                </tr>                
                <tr id="trExcelWeekEnding" runat="server">
                    <%-- inserted empty td immediately below --%>
                    <td id="tdDateSpc" runat="server" style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                    <td id="tdExcelWeekEnding" runat="server" colspan="3" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekEnding" Runat="server" Text=""/></td>
                    <td ID="exlDept" Visible = "false" runat="server" style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                    <td ID="exlLoc" Visible = "false"  runat="server" style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                    <td ID="exlShift" Visible = "false"  runat="server" style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay1" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay2" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay3" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay4" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay5" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay6" Runat="server" Text=""/></td>
                    <td colspan="1" style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay7" Runat="server" Text=""/></td>
                    <td id="exlApproved"  Visible = "false" runat="server" style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                </tr>
                <tr id="tempp" runat="server" class="bg-brand-grey text-white text-sm font-medium uppercase tracking-wider">
                    <td ID="seqNum" Visible = "true" runat="server" class="px-6 py-3 text-left">Seq #</td>
                    <td ID="hdrDept" Visible = "false" runat="server" class="px-6 py-3 text-left">Department</td>
                    <td ID="hdrLoc" Visible = "false" runat="server" class="px-6 py-3 text-left">Location</td>
                    <td ID="hdrShift" Visible = "false" runat="server" class="px-6 py-3 text-left">Shift</td>
                    <td class="px-6 py-3 text-left">Badge #</td>
                    <td id="hdrDispatch" visible="false" runat="server" class="px-6 py-3 text-left">Dispatch</td>
                    <td class="px-6 py-3 text-left">Employee Name</td>
                    <td runat="server" ID="tdJobCodeHead" class="px-6 py-3 text-left">
                        <asp:Label ID="lblJobCodeHead" runat="server">Job Code</asp:Label>
                    </td>
                    <td ID="tdShiftHead" runat="server" Visible="false" style="padding:2pt 2pt 2pt 2pt; width:80px; height:30px; color:#ffffff; background-color:#003776; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Shift</td>
                    <td ID="tdDeptHead" runat="server" Visible="false" style="padding:2pt 2pt 2pt 2pt; width:80px; height:30px; color:#ffffff; background-color:#003776; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Dept</td>
                    <td runat="server" ID="tdCostCenterHead" Visible="false" style="padding:2pt 2pt 2pt 2pt; width:80px; height:30px; color:#ffffff; background-color:#003776; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Cost Center</td>
                    <td Visible="false" runat="server" id="tdPayRateHead" style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Pay Rate</td>
                    <td ID="tdFirstPunchHead" runat="server" Visible="true" style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Start Date</td>
                    <td id="tdMonHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay1" runat="server" /></td>
                    <td id="tdTueHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay2" runat="server" /></td>
                    <td id="tdWedHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay3" runat="server" /></td>
                    <td id="tdThuHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay4" runat="server" /></td>
                    <td id="tdFriHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay5" runat="server" /></td>
                    <td id="tdSatHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay6" runat="server" /></td>
                    <td id="tdSunHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay7" runat="server" /></td>
                    <td id="tdRegHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:70px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Reg</td>
                    <td id="tdOTHeader"  runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">OT</td>
                    <td id="tdBonusPay" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <span runat="server" id="spanUpdBonuses">Bonus</span></td>
                    <td visible="false" id="tdApproveHeader" runat="server" style="background-color:#003776; text-align:center;"><input type="Button" ID="btnApprove" value="Approve" /></td>
                    <td id="tdTotalHours"  runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Multi Dept. Hours</td>
                    <td ID="tdApprovedBy" Visible = "false" runat="server" style="padding:2pt 2pt 2pt 2pt; width:120px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Approved By</td>
                    <td id="tdClientNum" Visible="false" runat="server" style="padding:2pt 2pt 2pt 2pt; width:120px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Client Number</td>
                </tr>
          </HeaderTemplate>
              
          <ItemTemplate>
             <tr id="trDepartment" runat="server" class="bg-gray-100">
                   <%-- empty td element added and colspan reduced 12 -> 11 --%>
                   <td id="tdDeptSpc" runat="server" class="px-6 py-4"></td>
                   <td id="tdDepartmentHead" runat="server" colspan="4" class="px-6 py-4 text-left font-bold text-lg text-brand-black"><asp:Label ID="lblDepartment" Runat="server"/><asp:HiddenField ID="hdnDept" runat="server" /><asp:HiddenField ID="hdnShift" runat="server" /></td>
                   <td style="display:none" id="bonusMultiplyLabel"><span class="text-sm font-medium text-brand-black">Bonus Multiplier:</span></td>
                   <td runat="server" style="display:none" id="bonusMultiplier"><input runat="server" id="txtBonusMultiplier" onClick='this.select();' class="w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green px-3 py-2 text-sm text-right" style="display:none;" type="text" size="6" value="0.00" /></td>
                   <td id="tdDeptStart" style="display:none"><p style="display:none" runat="server" id="pDeptStart"></p><p style="display:none" runat="server" id="pDeptEnd"></p></td>
             </tr>
             <tr visible="false" id="trPeek" runat="server">
                <td id="tdPeekIdx" runat="server"><span runat="server" id="spPeekIdx"></span></td>
                <td id="tdPeekName" runat="server"><span runat="server" id="spPeekName"></span></td>
                <td id="tdPeekReg" runat="server"><span runat="server" id="spPeekReg"></span></td>
                <td id="tdPeekStart" runat="server"><span runat="server" id="spPeekStart"></span></td>
                <td id="tdPeekEnd" runat="server"><span runat="server" id="spPeekEnd"></span></td>
                <td id="tdPeekApproved" runat="server"><span runat="server" id="spPeekApproved"></span></td>
             </tr>
             <tr id="emp" class="DlyHrs hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0" runat="server">
                     <td id="tdEmpCntSpc" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis">
                     <asp:Label ID="lblEmpCnt" Runat="server"></asp:Label>
                     </td>
                    <td ID="rowDept" visible="false" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis">Breathing Hose</td>
                    <td ID="rowLoc" visible="false" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis">Schaumburg</td>
                    <td ID="rowShift" visible="false" runat="server" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis">1st Shift</td>
                    
                    <td runat="server" visible="true" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis cursor-move">
                        <asp:Panel Visible="true" runat="server" ID="pnlPlusMinus">
                            <img id="i<%# GetBoundEmployeeID(false) %>" src="../Images/plus.gif" alt="" />
                            <asp:Label ID="lblBadgeNumber" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/>
                        </asp:Panel>
                        <asp:Label ID="lblBadgeNumberExcel" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/>
                    </td>
                    
                    <td id="dispatch" runat="server" visible="false">
                        <asp:Label ID="lblDispatchExcel" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/>
                    </td>
                    
                    <td style="width:250px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblLastName" Text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>' Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>' Runat="server"/>
                        &nbsp;<asp:Label ID="lblUnApproved" Text='**' Visible="false" Runat="server"/>
                    </td>

                    <td runat="server" id="tdJobCode" visible="false" style="width:80px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;">
                        <asp:Label ID="lblJobCode" Runat="server"/>
                    </td>

                    <td ID="tdShift" runat="server" Visible="false" style="width:80px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblShift" Runat="server"/></td>
                    <td ID="tdDept" runat="server" Visible="false" style="width:80px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblDept" Runat="server"/></td>

                    <td ID="tdCostCenter" runat="server" Visible="false" style="width:80px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblCostCenter" Runat="server"/></td>

                    <td ID="tdPayRate" runat="server" Visible="false" style="width:50px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:HiddenField ID="hdnPayRate" runat="server" /><asp:HiddenField ID="hdnBillRateRatio" runat="server" /><asp:Label ID="lblPayRate" Runat="server"/></td>

                    <td ID="tdFirstPunch" runat="server" Visible="true" style="width:50px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblFirstPunch" Runat="server"/></td>

                    <td id="tdMon" runat="server" ><asp:Label ID="lblWeekDay1Hours" Runat="server"/></td>
                    <td id="tdTue" runat="server" ><asp:Label ID="lblWeekDay2Hours" Runat="server"/></td>
                    <td id="tdWed" runat="server" ><asp:Label ID="lblWeekDay3Hours" Runat="server"/></td>
                    <td id="tdThu" runat="server" ><asp:Label ID="lblWeekDay4Hours" Runat="server"/></td>
                    <td id="tdFri" runat="server" ><asp:Label ID="lblWeekDay5Hours" Runat="server"/></td>
                    <td id="tdSat" runat="server" ><asp:Label ID="lblWeekDay6Hours" Runat="server"/></td>
                    <td id="tdSun" runat="server" ><asp:Label ID="lblWeekDay7Hours" Runat="server"/></td>
                    <td id="tdReg" runat="server" ><asp:Label ID="lblTotalHours" Runat="server"/></td>
                    <td id="tdOT" runat="server" ><asp:Label ID="lblOTHours" Runat="server"/></td>
                    <%-- <!--<td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdTotal" runat="server" ><asp:Label ID="lblTotalHoursAllDepts" Runat="server"/></td>-->--%>
                    <td id="tdTotalWeek" visible="false" runat="server" ><asp:Label ID="lblTotalWeek" Runat="server"/></td>                   
                    <td visible="false" id="tdBonusItem" runat="server" ><asp:Label ID="lblBonusItem" Runat="server"/></td>                   
                    <td id="tdApproveHours" visible="false" enableviewstate="true" Runat="server">
                        <!--<%--<asp:CheckBox runat="server" Visible="false" ID="chkBoxApprove"></asp:CheckBox><asp:HiddenField ID="hdnBadgeNumber" runat="server" /><asp:HiddenField ID="hdnEmployeePunchList" runat="server" />--%>-->
                        <input runat="server" id="btnLineApprove" class="lineApproveBtn" visible="false" disabled="disabled" type="button" value="APR" /><!--<%--<asp:HiddenField ID="hdnBadgeNumber" runat="server" /><asp:HiddenField ID="hdnEmployeePunchList" runat="server" />--%>-->
                    </td>
                    <td ID="exlApprovedBy" visible="false" runat="server" style="width:120px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"></td>
                    <td ID="exlClientNum" runat="server" visible="false" style="width:120px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"></td>
                </tr>
                    <asp:Repeater id="rptrHoursReportDetail" OnItemDataBound="rptrHoursReportDetail_ItemDataBound" runat="server" >
                        <HeaderTemplate>
                        <tr style="display:none;">
                            <td colspan="11">
                                <table class="tblDetail w-full rounded-lg shadow-md overflow-hidden border border-gray-200">
                                    <thead>
                                    <tr class="bg-brand-grey text-white text-sm font-medium uppercase tracking-wider">
                                        <th class="px-6 py-3 text-left"><asp:Label runat="server" CssClass="text-sm">Check In</asp:Label></th>
                                        <th class="px-6 py-3 text-left"><asp:Label runat="server" CssClass="text-sm">Type</asp:Label></th>
                                        <th class="px-6 py-3 text-left"><asp:Label runat="server" CssClass="text-sm">Check Out</asp:Label></th>
                                        <th class="px-6 py-3 text-left"><asp:Label runat="server" CssClass="text-sm">Type</asp:Label></th>
                                        <th class="px-6 py-3 text-left"><asp:Label runat="server" CssClass="text-sm">Hours</asp:Label></th>
                                        <th runat="server" id="tdBreak" class="px-6 py-3 text-left"><asp:Label ID="lblBreak" runat="server" CssClass="text-sm">Break</asp:Label></th>
                                        <th runat="server" id="tdFirstPunch" class="px-6 py-3 text-left"><asp:Label ID="lbl1stPunch" runat="server" CssClass="text-sm">1st Punch</asp:Label></th>
                                        <th class="px-6 py-3 text-left"><input type='button' class='punchDisplay bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2' value='Exact Time' /></th>
                                        <th class="px-6 py-3 text-left"><asp:DropDownList runat="server" ID="moveDept" CssClass="w-full rounded-md border-gray-300 shadow-sm focus:ring-2 focus:ring-brand-green focus:border-brand-green"><asp:ListItem Value="424" Text="Department Number 1"></asp:ListItem><asp:ListItem Value="428" Text="Department Number 2"></asp:ListItem></asp:DropDownList></th>
                                        <th class="px-6 py-3 text-left"><input type='button' class='punchDisplay bg-brand-grey hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2' disabled="disabled" value='Move' /></th>
                                        <!--<%--<th><input disabled="disabled" type='button' class='unassignedDisplay' value='Unassigned Punches' /></th>--%>-->
                                    </tr>
                                    </thead>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr id="tableRow" runat="server" class="hover:bg-gray-50 even:bg-gray-50 odd:bg-white border-b border-gray-200 last:border-b-0">
                                <td id="tdCheckIn" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis time" crt_by='' crt_dt=''><asp:Label CssClass="text-sm" ID="lblCheckIn" Runat="server"/></td>
                                <td id="tdCheckInType" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis time" crt_by='' crt_dt=''><asp:Label CssClass="text-sm" ID="lblCheckInType" Runat="server"/></td>
                                <td id="tdCheckOut" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis time" crt_by='' crt_dt=''><asp:Label CssClass="text-sm" ID="lblCheckOut" Runat="server"/></td>
                                <td id="tdCheckOutType" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis time" crt_by='' crt_dt=''><asp:Label CssClass="text-sm" ID="lblCheckOutType" Runat="server"/></td>
                                <td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label CssClass="text-sm" ID="lblCheckHours" Runat="server"/></td>
                                <td id="tdCheckBreak" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label CssClass="text-sm" ID="lblCheckBreak" runat="server"></asp:Label></td>
                                <td id="tdLatePunch" class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label runat="server" ID="lblLatePunch" CssClass="text-sm"></asp:Label></td>
                                <td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis"><asp:Label ID="lblApprovedBy" runat="server"></asp:Label></td>
                                <td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis">&nbsp</td>
                                <td class="px-6 py-4 text-sm text-brand-black whitespace-nowrap overflow-hidden text-ellipsis">&nbsp</td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </table> 
                            </td>
                        </tr>
                        </FooterTemplate>    
                    </asp:Repeater>
                <!--<%--</asp:Panel>--%>-->
                    <asp:Repeater id="rptrHoursReportDetail_Excel" OnItemDataBound="rptrHoursReportDetail_ItemDataBound" runat="server" >
                        <HeaderTemplate>
                            <tr >
                                <td colspan="2">
                                    <table width="100%" cellpadding="0" cellspacing="0" style="border:none;">
                                        <tr class="Detail">
                                            <td>Check In</td>
                                            <td>Punch Type</td>
                                            <td>Check Out</td>
                                            <td>Punch Type</td>
                                            <td>Hours</td>
                                            <td><asp:Label runat="server" ID="lblBreak">Break</asp:Label></td>
                                            <td><asp:Label runat="server" ID="lbl1stPunch">1st Punch</asp:Label></td>
                                            <td><asp:Label runat="server" ID="lblPunchTypeIn"></asp:Label></td>
                                        </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="HrsDetail" runat="server">
                                <td id="tdCheckIn" style='width:200px;' runat="server"><asp:Label ID="lblCheckIn" Runat="server"/></td>
                                <td id="tdCheckInType" style='width:200px;' runat="server"><asp:Label ID="lblCheckInType" Runat="server"/></td>
                                <td id="tdCheckOut" style='width:200px;' runat="server"><asp:Label ID="lblCheckOut" Runat="server"/></td>
                                <td id="tdCheckOutType" style='width:200px;' runat="server"><asp:Label ID="lblCheckOutType" Runat="server"/></td>
                                <td><asp:Label ID="lblCheckHours" Runat="server"/></td>
                                <td><asp:Label ID="lblCheckBreak" Runat="server"/></td>
                                <td><asp:Label ID="lblLatePunch" Runat="server"/></td>

                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td style="background-color:#ffffff; color:Black; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;" colspan="5">&nbsp;</td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                        </FooterTemplate> 
                    </asp:Repeater>
            <%-- department totals line --%>
                <tr id="trDepartmentTotals" runat="server" class="bg-gray-100 font-medium">
                    <td id="tdDepartmentTotalsSpc" runat="server" class="px-6 py-3"><input runat="server" visible="false" id="btnTimeline" class="btnTimeline bg-brand-green hover:bg-opacity-90 text-white font-medium rounded-md px-4 py-2" type="button" value="Timeline" /></td>
                    <td id="tdDepartmentTotalLabel" runat="server" colspan="2" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300"><asp:Label ID="lblDepartmentTotalLabel" Runat="server"/></td>
                    <td id="tdDeptSpacer" runat="server" visible="false" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300">---</td>
                    <td id="ftDept" Visible ="false" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"></td>
                    <td id="ftLoc" Visible ="false" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"></td>
                    <td id="ftShift" Visible ="false" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"></td>
                    <td id="tdDeptMonTotal" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotWeekDay1Hours" Runat="server"/></td>
                    <td id="tdDeptTueTotal" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotWeekDay2Hours" Runat="server"/></td>
                    <td id="tdDeptWedTotal" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotWeekDay3Hours" Runat="server"/></td>
                    <td id="tdDeptThuTotal" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotWeekDay4Hours" Runat="server"/></td>
                    <td id="tdDeptFriTotal" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotWeekDay5Hours" Runat="server"/></td>
                    <td id="tdDeptSatTotal" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotWeekDay6Hours" Runat="server"/></td>
                    <td id="tdDeptSunTotal" runat="server" class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotWeekDay7Hours" Runat="server"/></td>
                    <td class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotTotalHours" Runat="server"/></td>
                    <td class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotOTHours" Runat="server"/></td>
                    <td class="px-6 py-3 text-right font-medium text-brand-black border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblDeptTotBonus" Text="99.99" Runat="server"/></td>
                </tr>
             <%-- white shift totals --%>
             <tr id="trShiftTotals" runat="server" visible="false">
                   <td />
                   <td colspan="2" style="border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblShiftTotalLabel" Runat="server"/></td>
                   <td id="tdShiftSpacer" runat="server" visible="false" style="height:17pt; width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                   <td id="tdMonTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotMonHours" Runat="server"/></td>
                   <td id="tdTueTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotTueHours" Runat="server"/></td>
                   <td id="tdWedTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotWedHours" Runat="server"/></td>
                   <td id="tdThuTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotThuHours" Runat="server"/></td>
                   <td id="tdFriTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotFriHours" Runat="server"/></td>
                   <td id="tdSatTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotSatHours" Runat="server"/></td>
                   <td id="tdSunTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotSunHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotTotalHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotOTHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblShiftTotHours" Text="99.99" Runat="server"/></td>
             </tr>
          </ItemTemplate>
          
          <FooterTemplate>
                <tr id="trGrandTotals" runat="server" class="bg-brand-green text-white font-medium">
                   <td id="tdGrandTotalsSpc" runat="server" class="px-6 py-3 text-right border-t border-b-2 border-gray-300"></td>
                   <td id="totDept" Visible="false" runat="server" class="px-6 py-3"></td>
                   <td id="totLoc" Visible="false" runat="server" class="px-6 py-3"></td>
                   <td id="totShift" Visible="false" runat="server" class="px-6 py-3"></td>
                    <td id="tdGrandTotalLabel" runat="server" colspan="2" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300">Grand Totals:</td>
                   <td id="tdGTSpacer" runat="server" visible="false" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300">---</td>
                   <td id="tdMonGrandTotal" runat="server" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandWeekDay1Hours" Runat="server"/></td>
                   <td id="tdTueGrandTotal" runat="server" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandWeekDay2Hours" Runat="server"/></td>
                   <td id="tdWedGrandTotal" runat="server" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandWeekDay3Hours" Runat="server"/></td>
                   <td id="tdThuGrandTotal" runat="server" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandWeekDay4Hours" Runat="server"/></td>
                   <td id="tdFriGrandTotal" runat="server" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandWeekDay5Hours" Runat="server"/></td>
                   <td id="tdSatGrandTotal" runat="server" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandWeekDay6Hours" Runat="server"/></td>
                   <td id="tdSunGrandTotal" runat="server" class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandWeekDay7Hours" Runat="server"/></td>
                   <td class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandTotalHours" Runat="server"/></td>
                   <td class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandOTHours" Runat="server"/></td>
                   <td class="px-6 py-3 text-right font-medium border-t border-b-2 border-gray-300" style="mso-number-format:\#\,\#\#0\.00;"><asp:Label ID="lblGrandTotBonus" Text="9999.99" Runat="server"/></td>
                </tr>
                
             </table>
          </FooterTemplate>
       </asp:Repeater>
       <div id='popup'></div>
