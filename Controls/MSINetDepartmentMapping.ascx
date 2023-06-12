<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetDepartmentMapping.ascx.cs" Inherits="MSI.Web.Controls.MSINetDepartmentMapping" %>

<!--<script type="text/javascript" src="../jquery-ui-1.9.0.custom/js/jquery-1.8.2.js" ></script>-->

<script type="text/javascript">
    var ClientBaseURL = "http://localhost:52293/RestServiceWWW/Client/";
    ClientBaseURL = "../Client/";
    var url = ClientBaseURL + "GetDepartmentMapping?clientID=" + $("input[id*='clientID']").val();
    //alert(url);
    $(document).ready(function () {
        $.ajax({
            cache: false,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            async: true,
            dataType: "json",
            url: url,
            success: function (data) {
                buildTable(data);
            },
            error: function (xhr) {
                alert("ERROR!!!" + xhr.responseText);
            }
        });
    });

    function buildTable(data) {
        //alert(data.MapList.length);
        for (var i = 0; i < data.MapList.length; i = i + 1) {
            var row = "<tr>";
            /* get desc */
            row = row + "<td>" + data.MapList[i].Desc + "</td>";
            /* shift */
            row = row + "<td>" + data.MapList[i].ShiftType + "</td>";
            /* input for Tempworks ID */
            row = row + "<td><input shiftID='" + data.MapList[i].ShiftID + "' orig='" + data.MapList[i].TWMapID + "' type='text' size='6' value='" + data.MapList[i].TWMapID + "'/></td>";

            row = row + "</tr>";
            //var table.document.getElementById("rateTable");
            $(row).appendTo($("#mapTable tbody"));
        }

        $("#inpRegMult").val(data.RegRate);
        $("#inpOTMult").val(data.OTRate);
        $("#inpRegMult").attr('orig', data.RegRate);
        $("#inpOTMult").attr('orig', data.OTRate);

        $("#inpRegMult2").val(data.RegRate2);
        $("#inpOTMult2").val(data.OTRate2);
        $("#inpRegMult2").attr('orig', data.RegRate2);
        $("#inpOTMult2").attr('orig', data.OTRate2);

        $('#inpBonusMult').val(data.BonusRate);
        $('#inpVacMult').val(data.VacRate);
        $('#inpPassThruMult').val(data.PassThruRate);
        $('#inpOtherMult').val(data.OtherRate);

        $("#rateTable").fixedHeaderTable({
            width: '600',
            altClass: 'odd',
            themeClass: 'fancyTable',
            fixedColumn: true,
            autoShow: true
        });
        var h = data.MapList.length * 40;
        if (h > 280)
            h = 280;
        else if (h < 120)
            h = 120;
        $("#mapTable").fixedHeaderTable({
            width: '600',
            height: h,
            footer: true,
            cloneHeadToFoot: true,
            altClass: 'odd',
            themeClass: 'fancyTable',
            fixedColumn: false,
            autoShow: true
        });


        $("input").focus(function () {
            $(this).select();
        });
        $("#mapTable input").change(function () {
            if ($(this).val().match(/^\d+$/)) {
                $(this).css('background-color', '#FFDDDD');
                $(this).attr('update', 'true');
            }
            else {
                alert("Number entered is invalid!");
                $(this).val($(this).attr('orig'));
            }
        });
        $("#rateTable input").change(function () {
            if (!$(this).val().match(/[^0123456789.]/g)) {
                $(this).css('background-color', '#FFDDDD');
                $(this).attr('update', 'true');
            }
            else {
                alert("Number entered is invalid!");
                $(this).val($(this).attr('orig'));
            }
        });


        $("#btnUpdate").click(function () {
            var msgBox = "<div><h1>Msi Shift / Temp works Mapping</h1><p id='updMapping'></p><p id='updMultipliers'></p></div>";
            $(msgBox).dialog();
            var inps = $('#mapTable input[update="true"]');
            //alert("inps length = " + inps.length);
            var list = "";
            inps.each(function () {
                list = list + $(this).attr('shiftid') + ":" + $(this).val() + ",";
            });
            if (list.length > 0) {
                list = list.substring(0, list.length - 1); //get rid of last comma
                var clientID = $("input[id*='clientID']").val();
                var dt = new Date();
                var startDate = dt.getFullYear() + "-" + dt.getMonth() + "-" + dt.getDate();
                var userID = $("input[id*='userID']").val();
                var url = ClientBaseURL + "SetDepartmentMapping?clientID=" + clientID + "&startDate=" +
                    startDate + "&userID=" + userID + "&list=" + list;

                $.ajax({
                    cache: false,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    dataType: "json",
                    url: url,
                    success: function (data) {
                        document.getElementById("updMapping").innerHTML = data;
                        var inpsCU = $('#mapTable input[update="true"]');
                        $(inpsCU).each(function () {
                            $(this).css('background-color', '#FFFFFF');
                            $(this).removeAttr('update');
                            $(this).attr('orig', $(this).val());
                        });
                    },
                    error: function (xhr) {
                        //alert("ERROR - Mapping not set! " + xhr.responseText);
                        document.getElementById("updMapping").innerHTML = "Error - Mapping not set!";
                        cleanUp(inps, false);
                    }
                });
            }
            /* update multiplier */
            inps = $('#rateTable input[update="true"]');
            //alert(inps.length);
            var clientID = $("input[id*='clientID']").val()
            var mult = $("#inpRegMult").val();
            var mult2 = $("#inpRegMult2").val();
            var otMult = $("#inpOTMult").val();
            var otMult2 = $("#inpOTMult2").val();
            var bonusMult = $("#inpBonusMult").val();
            var vacationMult = $("#inpVacMult").val();
            var passThruMult = $("#inpPassThruMult").val();
            var otherMult = $("#inpOtherMult").val();
            var url = ClientBaseURL + "SetClientMultiplier?clientID=" + clientID + "&multiplier=" +
                    mult + "&multiplier2=" + mult2 + "&otMultiplier=" + otMult + "&otMultiplier2=" + otMult2 + "&bonusMultiplier=" + bonusMult + 
                    "&vacationMultiplier=" + vacationMult + "&passThruMultiplier=" + passThruMult + "&otherMultiplier=" + otherMult;
            $.ajax({
                cache: false,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                async: true,
                dataType: "json",
                url: url,
                success: function (data) {
                    document.getElementById("updMultipliers").innerHTML = data;
                    cleanUp(inps, true);
                },
                error: function (xhr) {
                    //alert("ERROR - Mapping not set! " + xhr.responseText);
                    document.getElementById("updMultipliers").innerHTML = "Error - Multipliers not set!";
                    cleanUp(inps, false);
                }
            });
        });
        function cleanUp(inps, success) {
            //alert("Hello! " + $(inps).length);
            $(inps).each(function () {
                $(this).css('background-color', '#FFFFFF');
                $(this).removeAttr('update');
                if (success) {
                    $(this).attr('orig', $(this).val());
                }
                else {
                    $(this).val($(this).attr('orig'));
                }
            });
        }
        function cleanUpMults(inps, success) {
            //alert("Hello! " + $(inps).length);
            $(inps).each(function () {
                $(this).css('background-color', '#FFFFFF');
                $(this).removeAttr('update');
                if (success) {
                    $(this).attr('orig', $(this).val());
                }
                else {
                    $(this).val($(this).attr('orig'));
                }
            });
        }   
    };
</script>

<br />
<div>

<table id="rateTable">
    <thead>
    </thead>
    <tbody>
    <tr>
        <td><span>1</span></td>
        <td><span>Reg. Rate: </span><input type="text" size="6" id="inpRegMult" /></td>
        <td><span>OT Rate: </span><input type="text" size="6" id="inpOTMult" /></td>
        <td rowspan='4'><span>Update Information</span><br /><input id='btnUpdate' type="button" value="GO!" /></td>
    </tr>
    <tr>
        <td><span>2</span></td>
        <td><span>Reg.Rate: </span><input type="text" size="6" id="inpRegMult2" /></td>
        <td><span>OT Rate: </span><input type="text" size="6" id="inpOTMult2" /></td>
    </tr>
    <tr>
        <td><span>3</span></td>
        <td><span>Bonus Rate: </span><input type="text" size="6" id="inpBonusMult" /></td>
        <td><span>Vacation Rate: </span><input type="text" size="6" id="inpVacMult" /></td>
    </tr>
    <tr>
        <td><span>4</span></td>
        <td><span>Pass Thru Rate: </span><input type="text" size="6" id="inpPassThruMult" /></td>
        <td><span>Other Rate: </span><input type="text" size="6" id="inpOtherMult" /></td>
    </tr>
    </tbody>
</table>

<table id="mapTable">
    <thead>
        <tr>
            <th>MSI Department Name</th>
            <th>Shift</th>
            <th>tempwerks ID</th>
        </tr>
    </thead>
    <tbody>
    </tbody>
</table>





</div>