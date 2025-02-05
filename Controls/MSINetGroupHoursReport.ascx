﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetGroupHoursReport.ascx.cs" Inherits="MSI.Web.Controls.MSINetGroupHoursReport" %>

    <asp:Panel ID="pnlJSCode" Visible="false" runat="server">
    <link href="../Includes/flexigrid.pack.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/flexigrid.pack.js" type="text/javascript"></script>
    <style type="text/css">
    #popup
    {
        position:absolute;
        width:auto;
        height:200px;
        border:6px double Blue;
        border-radius:12px; //css3"../Client/"
        //visibility:visible;
        display:none;
        background-color:LightYellow;
    }
    .underLine
    {
        text-decoration:underline;
    }
    #popup h1
    {
        margin-left:12px;
        margin-right:12px;
        font-size:2.5em;
        border-bottom:1px solid Black;
    }
    #popup h2
    {
        margin-left:12px;
        margin-right:12px;
        font-size:1.75em;
    }
    #addPunchDetail
    {
        background-color:Yellow;
        font-size:1.75em;
        margin-left:16px;
    }
    .popupClose
    {
        padding:8px;
        float:right;
    }
    
    .popupClose
        .popupCloseBtn
        {
            width: 16px;
            height: 16px;
        }
        
    .hover
    {
        border:2px solid LightYellow;
    }
    .Detail
    {
        width:1000px;
    }
    </style>
    
    <script type="text/javascript" id="\">

        var ClientBaseURL = "../Client/";
        var ClientBaseURL2 = "http://localhost:52293/RestServiceWWW/Client/";

        var RosterBaseURL = "../Roster/";
        var RosterBaseURL2 = "http://localhost:52293/RestServiceWWW/Roster/";

        var UpdatePunchBaseURL = "../Roster/Name/";

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
                    "<h1><span>Add Punch</span></h1>" +
                    "<h2>Set Time: ";
            //alert(popup);
            /* date */
            popup += "<input type='text' id='punchDate' size='8' value='" + mnt + "/" + day + "/" + year + "'/>";
            /* hour */
            popup += "<select id='punchHour'>";
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
            popup += "<select id='punchMin'>";
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
            popup += "<select id='punchAMPM'>";
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
                    "<h1><span>Modify Punch</span></h1>" +
                    "<h2><span>Exact Punch Time: " + exact + "</span></h2>" +
                    "<h2>Change Time: ";
            /* date */
            popup += "<input type='text' id='punchDate' size='8' value='" + mnt + "/" + dy + "/" + year + "'/>";
            /* hour */
            popup += "<select id='punchHour'>";
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
            popup += "<select id='punchMin'>";
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
            popup += "<select id='punchAMPM'>";
            if (am == 'AM') {
                popup += "<option selected='selected' value='0'>AM</option>";
                popup += "<option value='12'>PM</option>";
            }
            else {
                popup += "<option selected='selected' value='12'>PM</option>";
                popup += "<option value='0'>AM</option>";
            }
            popup += "</select>";
            popup += "<input type='button' id='updateBtn' onclick='updatePunch(" + id + ")' value='Update'/>";
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
            var url = RosterBaseURL + id + "/UpdatePunch?month=" + month + "&day=" + day + "&year=" + year + "&hour=" + hour + "&min=" + min;
            //alert(url);
            $.ajax({
                cache: false,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                async: false,
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

            //alert(retDate);
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
                var tm = dateTime.substring(dateTime.indexOf(' ') + 1);
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
                alert($tr + ", " + $dept.length + ", " + $dept.find("input[id*='hdnDept']").val());
                list = list + $(this).val() + ":" + $tr.find("span[id*='lblBadgeNumber']").text().substring(2) +
                    ":" + $dept.find("input[id*='hdnDept']").val() + ",";

            });
            list = list.substring(0, list.length - 1); //get rid of last comma
            alert(list);
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

        function validateBonus(inp) {
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

                var totSpanVal = totSpan.text().substring(2);
                //alert(totSpan.text() + ', ' + totSpanVal);
                var store = inp.attr("store");
                var newAdd = parseFloat(inp.val()) - parseFloat(store);
                alert(newAdd);
                totSpanVal = parseFloat(totSpanVal) + newAdd;
                totSpan.text('$ ' + totSpanVal.toFixed(2));

                totSpan = tr.nextAll("tr[id*='trGrandTot']").first().find("span[id*='lblGrandTotBonus']");
                totSpanVal = parseFloat(totSpan.text().substring(2)) + newAdd;

                totSpan.text('$ ' + totSpanVal.toFixed(2));
                inp.attr("updBonus", "true");
                inp.attr("store", parseFloat(inp.val()).toFixed(2));
                inp.val(parseFloat(inp.val()).toFixed(2));
            }
        }

        jQuery(document).ready(
            function () {
                $('.DlyHrs:even').addClass('DlyHrsAlt');
                var myClass;
                $('tr[class*="DlyHrs"]').hover(
                    function () {
                        myClass = $(this).attr("class");
                        $(this).addClass("highLight").removeClass(myClass);
                    }, function () {
                        $(this).removeClass("highLight").addClass(myClass);
                    }
                ); //tr hover

                $("span[id*='CheckInExact_']").attr("title", "Click to view image");
                $("span[id*='CheckOutExact_']").attr("title", "Click to view image");

                var btn = "<input type='button' onclick='updateBonuses()' disabled='disabled' id='inpUpdBonuses' value='Bonus' />";
                $("span[id*='spanUpdBonuses']").html(btn);

                if ($("input[id*='_temp_hdnUpdBonuses']").val() == 'true') {
                    $("span[id*='BonusItem']").each(function (index) {
                        //console.log(index + ": " + $(this).text());
                        var inp = "$<input type='text' onClick='this.select();' size='2' style='text-align:right' id='bonusPay" +
                        index + "' value='" + $(this).text() + "' store='" + $(this).text() + "'/>";
                        $(this).parent().html(inp).change(
                        function (e) {
                            validateBonus($(this).find("input"));
                        }
                    )
                    });
                }

                //alert($("span[id*='BonusItem']").length);
                $("spanr[id*='CheckIn_']:contains('M')").attr("title", "Click to change punch").click(
	                function (e) {
	                    popup2Modify(e, this);
	                });
                $("span[id*='CheckIn_']:contains('N/A')").attr("title", "Click to add a punch").click(
	                function (e) {
	                    popup2Add(e, this);
	                });
                $("spanr[id*='CheckOut_']:contains('M')").attr("title", "Click to change punch").click(
	                function (e) {
	                    popup2Modify(e, this);
	                });
                $("span[id*='CheckOut_']:contains('N/A')").attr("title", "Click to add a punch").click(
	                function (e) {
	                    popup2Add(e, this);
	                });
                $('tr[class*="DlyHrs"]').find('td:nth-child(2)').hover(
                    function () {
                        $(this).addClass("underLine");
                    },
                    function () {
                        $(this).removeClass("underLine");
                    }
                ).click(
	                function (e) {
	                    var nxtTr = $(this).parent('tr').next('tr');
	                    nxtTr.toggle(100);
	                    var fg = nxtTr.find('td').find('div');
	                    if (fg.length == 0)
	                        nxtTr.find('td').find('table').flexigrid({ showToggleBtn: false });
	                }
                );
            });
            
    </script>
    </asp:Panel>

        <asp:Menu ID="mnuHoursReport" runat="server" BackColor="#E3EAEB" DynamicHorizontalOffset="2"
            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#666666" Orientation="Horizontal"
            StaticSubMenuIndent="10px" EnableViewState="false">
            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <DynamicHoverStyle BackColor="#666666" ForeColor="White" />
            <DynamicMenuStyle BackColor="#E3EAEB" />
            <StaticSelectedStyle BackColor="Blue" ForeColor="White" />
            <DynamicSelectedStyle BackColor="Blue" BorderColor="White" />
            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <Items>
                <asp:MenuItem Selected="True" Text="Roster Tracking" Value="Roster"></asp:MenuItem>
                <asp:MenuItem Text="Day Labor Tracking" Value="Day Labor"></asp:MenuItem>
            </Items>
            <StaticHoverStyle BackColor="#666666" ForeColor="White" />
        </asp:Menu>
        <asp:Panel ID="pnlHeader" runat="server" BackColor="#E0E0E0" Width="1100px">
            <asp:HiddenField ID="hdnUpdBonuses" runat="server" Value="false" />
            <asp:Table ID="tblPeriod" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><b><asp:Label ID="lblHoursConfirmation" runat="server" visible="false" Font-Bold="true" ForeColor="red" Font-Names="Arial" Font-Size="12pt" Text="Hours have been successfully submitted."></asp:Label></b></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="320px">
                        <asp:Table ID="Table1" runat="server">
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell ID="TableCell1" runat="server">
                                <div><asp:Label ID="Label1" runat="server" Text="Week of:"/></div>
                                <div><asp:DropDownList  runat="server" ID="txtCalendar" /></div>
                                <asp:CheckBox Enabled="false" runat="server" ID="chkBoxExact" /><asp:Label ID="Label2" runat="server" Text="Exact Times"/>
                                </asp:TableCell>
                                <asp:TableCell runat="server" ID="deptChooser" Visible="false">
                                    <div><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnDepts" ID="rbtnDept" runat="server" Text="Berlin" /></div>
                                    <div><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnDepts" ID="rbtnDept2" runat="server" Text="Amer.Litho" /></div>
                                    <div><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnDepts" Checked="true" ID="rbtnDept3" runat="server" Text="Both" /></div>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <div><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnSortOrder" ID="rbtnShifts" runat="server" Text="Sort By Shift" /></div>
                                    <div><asp:RadioButton ViewStateMode="Enabled" GroupName="rbtnSortOrder" ID="rbtnDepts" Checked="true" runat="server" Text="Sort By Dept." /></div>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="230px"><asp:Image ID="Image1" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Text="View Hours Report"></asp:Button>
                        <asp:Button id="btnCSV" onclick="btnGo_Click" runat="server" CommandName="GenerateCSV" Text="Generate CSV"></asp:Button>
                        <!--<asp:DropDownList ID="ddlMembers" runat="server" >
                            <asp:ListItem Text="All Creative Locations" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Creative Werks" Value="178"></asp:ListItem>
                            <asp:ListItem Text="Creative Werks Brummell" Value="292"></asp:ListItem>
                            <asp:ListItem Text="Creative Werks Kirk" Value="280"></asp:ListItem>
                        </asp:DropDownList>-->
                        <asp:HiddenField Value="0" runat="server" ID="hdnClientLocation" />
                        <br />
                        <asp:Button id="btnSubmitApproved" onclick="btnGo_Click" runat="server" Text="Submit Approved Hours" CommandName="SubmitApproved"></asp:Button>
                        <input type="hidden" id="hdnApproveList" runat="server" enableviewstate="false" value="" />
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="180px">
                        <asp:Image ID="Image2" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:HyperLink ID="lnkExport" Target="_blank" NavigateURL="~/auth/GroupHoursReportExcel.aspx?date=" runat="server" Text="Export Summary to Excel"></asp:HyperLink>
                        <br /><asp:HyperLink ID="lnkExportDetail" Target="_blank" NavigateURL="~/auth/GroupHoursReportExcel.aspx?date=" runat="server" Text="Export Detail to Excel"></asp:HyperLink>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                        <asp:Panel ID="pnlTotals" runat="server" BackColor="White" BorderStyle="Inset" HorizontalAlign="Center" Width="260px">
                            <asp:Label ID="Label3" runat="server" Text="Total Regular Hours:" Width="130px" CssClass="MSINetBodyText_Right"></asp:Label>
                            <asp:Label ID="lblRegularHrs" runat="server" CssClass="MSINetBodyText_Right" Text="0.00" Width="80px"></asp:Label><br />
                            <asp:Label ID="lblTTest" runat="server" Text="Total Overtime Hours:" Width="130px" CssClass="MSINetBodyText_Right"></asp:Label>
                            <asp:Label ID="lblOTHrs" runat="server" CssClass="MSINetBodyText_Right" Text="0.00" Width="80px"></asp:Label>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
     </asp:Panel>
     <asp:Panel runat="server" ID="pnlApprovalInfo" Visible="false" style="border:solid 1px #cccccc;" Width="100%">
        <table width="100%" border="0" cellpadding="5" cellspacing="0">
            <tr>
                <td colspan="3" align="left"><asp:Label runat="server" ID="lblApprovalHeader" CssClass="MSINetSectionHeading" Text="Hours for this week have been approved."></asp:Label></td>
            </tr>
            <tr>
                <td width="140" align="right"><strong><asp:Label runat="server" ID="lblApprovalDateHead" Text="Approval Date/Time:" CssClass="MSINetBodyText"></asp:Label></strong></td>
                <td align="left" width="800" colspan="2"><asp:Label runat="server" ID="lblApprovalDate" CssClass="MSINetBodyText"></asp:Label>
                    <asp:Label ID="Label4" runat="server" Text="."></asp:Label></td>
            </tr>
            <tr>
                <td width="140" align="right"><strong><asp:Label runat="server" ID="lblApprovedByHead" Text="Approved By:" CssClass="MSINetBodyText"></asp:Label></strong></td>
                <td align="left" width="298"><asp:Label runat="server" ID="lblApprovedBy" Text="" CssClass="MSINetBodyText"></asp:Label></td>
                <td align="left" width="502" >
                        <asp:LinkButton ID="lnkCreateInvoice" runat="server" Text="Generate Invoice and Finalize Week" OnClick="lnkCreateInvoice_Click"></asp:LinkButton>
                        <asp:HiddenField ID="hdnClientApprovalId" runat="server" />
                        <asp:HiddenField ID="hdnWeekEndDate" runat="server" />
                </td>
            </tr>
        </table>
        <br /><br />
     </asp:Panel>
     <asp:Panel runat="server" ID="PanelNoApproval" Visible="false" style="border:solid 1px #cccccc;" Width="100%">
        <table width="100%" border="0" cellpadding="5" cellspacing="0">
            <tr>
                <td colspan="3" align="left"><asp:Label runat="server" ID="Label5" CssClass="MSINetSectionHeading" Text="Hours for this week have been approved."></asp:Label></td>
            </tr>
            <tr>
                <td width="140" align="right"><strong><asp:Label runat="server" ID="Label6" Text="Approval Date/Time:" CssClass="MSINetBodyText"></asp:Label></strong></td>
                <td align="left" width="800" colspan="2"><asp:Label runat="server" ID="Label7" CssClass="MSINetBodyText"></asp:Label>
                    <asp:Label ID="Label8" runat="server" Text="."></asp:Label></td>
            </tr>
            <tr>
                <td width="140" align="right"><strong><asp:Label runat="server" ID="Label9" Text="Approved By:" CssClass="MSINetBodyText"></asp:Label></strong></td>
                <td align="left" width="298"><asp:Label runat="server" ID="Label10" Text="" CssClass="MSINetBodyText"></asp:Label></td>
                <td align="left" width="502" >
                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Generate Invoice and Finalize Week" OnClick="lnkCreateInvoice_Click"></asp:LinkButton>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                </td>
            </tr>
        </table>
        <br /><br />
     </asp:Panel>
     

    <asp:Repeater ID="rptrHoursReport" OnItemDataBound="rptrHoursReport_ItemDataBound"  OnItemCommand="rptrHoursReport_ItemCommand" runat="server">
        <HeaderTemplate>
             <table width="100%" cellpadding="0" cellspacing="0" >
                <!--JHM<th style="display:table-header-group;">-->
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
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay1" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay2" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay3" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay4" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay5" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay6" Runat="server" Text=""/></td>
                    <td colspan="1" style="mso-number-format:'m\/d'; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay7" Runat="server" Text=""/></td>
                </tr>
                <tr>
                    <td ID="seqNum" Visible = "true" runat="server" style="padding:2pt 2pt 2pt 2pt; width:40px; height:30px; color:#ffffff; background-color:#003776; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Seq #</td>
                    <td style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Badge #</td>
                    <td style="padding:2pt 2pt 2pt 2pt; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Employee Name</td>
                    <asp:Panel ID="pnlJobCodeHead" runat="server" Visible="false">
                        <td style="padding:2pt 2pt 2pt 2pt; width:80px; height:30px; color:#ffffff; background-color:#003776; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Job Code</td>
                    </asp:Panel>
                    <asp:Panel ID="pnlPayRateHead" runat="server" Visible="false">
                        <td style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Pay Rate</td>
                    </asp:Panel>
                    <td id="tdMonHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay1" runat="server" /></td>
                    <td id="tdTueHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay2" runat="server" /></td>
                    <td id="tdWedHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay3" runat="server" /></td>
                    <td id="tdThuHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay4" runat="server" /></td>
                    <td id="tdFriHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay5" runat="server" /></td>
                    <td id="tdSatHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay6" runat="server" /></td>
                    <td id="tdSunHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay7" runat="server" /></td>
                    <td id="tdRegHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:70px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Reg</td>
                    <td id="tdOTHeader"  runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">OT</td>
                    <asp:PlaceHolder ID="tdApproveHeaderPH" runat="server"></asp:PlaceHolder>
                    <!-- <td id="tdApproveHeader" runat="server" style="background-color:#003776; text-align:center;"><asp:Button ID="btnApprove"  Visible="false" Text="Approve"  runat="server" CommandArgument='' CommandName="approve" /></td>-->
                    <td id="tdTotalHours"  runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Multi Dept. Hours</td>
                    <td id="tdBonusPay" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <span runat="server" id="spanUpdBonuses">Bonus</span>
                    </td>
                    <td id="tdBonusPayMU" visible="false" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <span runat="server" id="spanUpdBonusesMU">BonusMU</span>
                    </td>
                </tr>
          </HeaderTemplate>
             
          <ItemTemplate>
             <tr id="trDepartment" runat="server">
                   <%-- empty td element added and colspan reduced 12 -> 11 --%>
                   <td id="tdDeptSpc" runat="server"></td>
                   <td id="tdDepartmentHead" runat="server" colspan="11" style="height:17pt; border-bottom:solid 0pt #cccccc; padding:12pt 12pt 12pt 12pt; color:Black; text-align:left; font-family:Arial; font-size:12pt; font-weight:bold;"><asp:Label ID="lblDepartment" Runat="server"/><asp:HiddenField ID="hdnDept" runat="server" /></td>
             </tr>
             <tr class="DlyHrs">
                     <td id="tdEmpCntSpc" runat="server" style="width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;">
                     <asp:Label ID="lblEmpCnt" Runat="server"></asp:Label>
                     </td>
                   <td id="Td1" runat="server" visible="true" style="cursor: hand; width:50px; height:17pt; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;">
                    <asp:Panel Visible="true" runat="server" ID="pnlPlusMinus">
                        <img id="i<%# GetBoundEmployeeID(false) %>" src="../Images/plus.gif" alt="" />
                        <asp:Label ID="lblBadgeNumber" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/>
                    </asp:Panel>
                    <asp:Label ID="lblBadgeNumberExcel" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/>
                   </td>
                   <td style="width:250px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblLastName" Text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>' Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>' Runat="server"/>&nbsp;<asp:Label ID="lblUnApproved" Text='**' Visible="false" Runat="server"/></td>
                   <asp:Panel ID="pnlJobCode" runat="server" Visible="false">
                        <td style="width:80px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblJobCode" Runat="server"/></td>
                   </asp:Panel>
                   <asp:Panel ID="pnlPayRate" runat="server" Visible="false">
                        <td style="width:50px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblPayRate" Runat="server"/></td>
                   </asp:Panel>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdMon" runat="server" ><asp:Label ID="lblWeekDay1Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdTue" runat="server" ><asp:Label ID="lblWeekDay2Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdWed" runat="server" ><asp:Label ID="lblWeekDay3Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdThu" runat="server" ><asp:Label ID="lblWeekDay4Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdFri" runat="server" ><asp:Label ID="lblWeekDay5Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdSat" runat="server" ><asp:Label ID="lblWeekDay6Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdSun" runat="server" ><asp:Label ID="lblWeekDay7Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdReg" runat="server" ><asp:Label ID="lblTotalHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdOT" runat="server" ><asp:Label ID="lblOTHours" Runat="server"/></td>
                   <%-- <!--<td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdTotal" runat="server" ><asp:Label ID="lblTotalHoursAllDepts" Runat="server"/></td>-->--%>
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdTotalWeek" runat="server" ><asp:Label ID="lblTotalWeek" Runat="server"/></td>                   
                   <td style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdBonusItem" runat="server" ><asp:Label ID="lblBonusItem" Runat="server"/></td>                   
                   <td visible="false" style="mso-number-format:\#\,\#\#0\.00; text-align:right"  id="tdBonusItemMU" runat="server" ><asp:Label ID="lblBonusItemMU" Runat="server"/></td>                   
                   
                   <td id="tdApproveHours" visible="false" enableviewstate="true" Runat="server">
                   <asp:CheckBox runat="server" Visible="false" ID="chkBoxApprove"></asp:CheckBox><asp:HiddenField ID="hdnBadgeNumber" runat="server" /><asp:HiddenField ID="hdnEmployeePunchList" runat="server" />
                   </td>
            </tr>
                <!--<%--<asp:Panel ID="hrDetails" runat="server">--%>-->
                    <asp:Repeater id="rptrHoursReportDetail" OnItemDataBound="rptrHoursReportDetail_ItemDataBound" runat="server" >
                        <HeaderTemplate>
                        <tr style="display:none;" id="d<%# GetBoundEmployeeID(true) %>">
                        <td colspan="11">
                            <table class="tblDetail" cellspacing="0" cellpadding="0" width="100%">
                                <thead>
                                <tr style="width:920">
                                    <th style='width:200px;'><asp:Label ID="Label11" runat="server" Font-Size="14px">Check In</asp:Label></th>
                                    <th style='width:200px;'><asp:Label ID="Label12" runat="server" Font-Size="14px">Check Out</asp:Label></th>
                                    <th style='width:160px;'><asp:Label ID="Label13" runat="server" Font-Size="14px">Hours</asp:Label></th>
                                </tr>
                                </thead>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td style='width:200px;'><asp:Label Font-Size="14px" ID="lblCheckIn" Runat="server"/></td>
                                <td style='width:200px;'><asp:Label Font-Size="14px" ID="lblCheckOut" Runat="server"/></td>
                                <td style='width:160px;'><asp:Label Font-Size="14px" ID="lblDetailHours" Runat="server"/></td>
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
                                    <td>Check Out</td>
                                    <td>Hours</td>
                                </tr>
                </HeaderTemplate>
                 
                <ItemTemplate>
                     <tr class="HrsDetail">
                           <td><asp:Label ID="lblCheckIn" Runat="server"/><!--<input id="hdnPunchInId" type="hidden" runat="server" />--></td>
                           <td><asp:Label ID="lblCheckOut" Runat="server"/><!--<input id="hdnPunchOutId" type="hidden" runat="server" />--></td>
                           <td><asp:Label ID="lblDetailHours" Runat="server"/></td>
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
            <%-- white department totals line --%>
            <tr id="trDepartmentTotals" runat="server">
                   <td id="tdDepartmentTotalsSpc" runat="server"></td>
                   <td id="tdDepartmentTotalLabel" runat="server" colspan="2" style="border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right;  height:17pt;font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDepartmentTotalLabel" Runat="server"/></td>
                   <td id="tdDeptSpacer" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">---</td>
                   <td id="tdDeptMonTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay1Hours" Runat="server"/></td>
                   <td id="tdDeptTueTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay2Hours" Runat="server"/></td>
                   <td id="tdDeptWedTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay3Hours" Runat="server"/></td>
                   <td id="tdDeptThuTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay4Hours" Runat="server"/></td>
                   <td id="tdDeptFriTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay5Hours" Runat="server"/></td>
                   <td id="tdDeptSatTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay6Hours" Runat="server"/></td>
                   <td id="tdDeptSunTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay7Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotTotalHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotOTHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotBonus" Text="99.99" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label Visible="false" ID="lblDeptTotBonusMU" Text="99.99" Runat="server"/></td>
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
                <tr id="trGrandTotals" runat="server">
                   <td id="tdGrandTotalsSpc" runat="server" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right;"></td>
                   <td id="tdGrandTotalLabel" runat="server" colspan="2" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Grand Totals:</td>
                   <td id="tdGTSpacer" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">---</td>
                   <td id="tdMonGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay1Hours" Runat="server"/></td>
                   <td id="tdTueGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay2Hours" Runat="server"/></td>
                   <td id="tdWedGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay3Hours" Runat="server"/></td>
                   <td id="tdThuGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay4Hours" Runat="server"/></td>
                   <td id="tdFriGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay5Hours" Runat="server"/></td>
                   <td id="tdSatGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay6Hours" Runat="server"/></td>
                   <td id="tdSunGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay7Hours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandTotalHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandOTHours" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandTotBonus" Text="9999.99" Runat="server"/></td>
                   <td style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:0pt 0pt 0pt 0pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandTotBonusMU" Text="9999.99" Runat="server"/></td>
                </tr>
                
             </table>
          </FooterTemplate>
       </asp:Repeater>
       <div id='popup'></div>
