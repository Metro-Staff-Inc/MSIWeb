<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PunchPhotos.aspx.cs" Inherits="MSI.Web.MSINet.TestPage" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <title>Punch Photos</title>

    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/jquery.fancybox.css?v=2.0.6" media="screen" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .Progress
    {
        background-image: url(../images/ajax-loader.gif);
        background-position: center;
        background-repeat: no-repeat;
        cursor: wait;
        padding: 10px;
        width: 200px;
        height: 100px;
    }        
    #availableEmp
    {
        padding:0px;
        width:368px;
        height:220px;
        text-align:left;
        overflow:auto;
    }
    </style>
    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox.pack.js" type="text/javascript"></script>

    <script type="text/javascript">

        var webServiceLoc;

        var months = ["Jan ", "Feb ", "Mar ", "April ", "May ", "Jun ", "Jul ", "Aug ", "Sept ", "Oct ", "Nov ", "Dec "];

        function getPics(id) {
            var data = "id=" + id;
            //alert(data);
            $.get("../Roster/" + $('input[id$="clientID"]').val() + "/Files", data, processResponse);
        }


        function processResponse(data) {
            $('#divFancyBox').html("");
            if (data.length == 0) {
                //alert("Hola!");
                $('#divFancyBox').append("<a class='fancybox' rel='group' href='missingPunchPic.jpg' title='No Punch Data Available!'></a>");
            }
            for (var i = 0, len = data.length; i < len; i++) {
                var idx = data[i].indexOf("__") + 2;
                y = data[i].substring(idx, idx + 4);
                mo = data[i].substring(idx + 4, idx + 6);
                d = data[i].substring(idx + 6, idx + 8);
                h = data[i].substring(idx + 9, idx + 11);
                m = data[i].substring(idx + 11, idx + 13);
                s = data[i].substring(idx + 13, idx + 15);
                ampm = "am";
                if( h >= 12 )
                {
                    ampm = "pm";
                    h -= 12;
                    if( h == 0 )
                        h = 12;
                }
                dt = months[parseInt(mo, 10)-1] + d + ", " + y + " " + h + ":" + m + ":" + s + " " + ampm;
                var a = "<a class='fancybox' title='" + dt + "' rel='group' href='";
                a += data[i].toString() + "'></a>";
                $('#divFancyBox').append(a);
            }
            $(".fancybox").fancybox();
            $(".fancybox").first().click();
        }

    </script>

</head>
<body>
    <div visible="false">
        <input type="hidden" runat="server" id="clientID"/>
        <input type="hidden" runat="server" id="webServiceLoc"/>
    </div>
<div id="divBody" align="center">
    <form id="form1" runat="server">
    <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="TestPage" />
        <br />
        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="View Punch Images" />
        <br />

<!--
            <div id="divEmpInfo">
                <span>Employee ID:</span><input type="text" id="txtEmpID" />
                <input type="button" id="btnEmpID" value="Go" />
            </div>
-->

    </form>
    <div id="divEmpInfo"> <!-- TABLE GOES HERE -->
        <table style="width:400px" border="1" cellpadding="4" cellspacing="0">
            <tr>
                <td class="availableEmp">
                    <fieldset>
                        <legend>Employee Pool Parameters</legend>
                        <table>
                        <tr>
                            <td>ID Number:</td>
                            <td><input id="txtIdNum" type="text" /></td>
                            <td><input type="button" value="GO" id="btnIdNum"/></td>
                        </tr>
                        <tr>
                            <td>Last Name:</td>
                            <td><input id="txtName" type="text" /></td>
                            <td><input type="button" value="GO" id="btnName"/></td>
                        </tr>
                        <tr>
                            <td>Worked:</td>
                            <td><select id="ddlLastWorked">
                                <option value="1">Yesterday</option>
                                <option value="2">2 Days Ago</option>
                                <option value="7">In the Last Week</option>
                                <option value="32">In the Last Month</option>
                            </select></td>
                            <td><input type="button" value="GO" id="btnLastWorked"/></td>
                        </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr><td>
                <div style="width:auto; height:auto">
                    <fieldset>
                    <legend>Available Employees</legend>
                    <div id="availableEmp">
                    </div>
                     </fieldset>
                </div>
            </td>
            </tr>
        </table>
    </div>
</div>

<div id="divFancyBox"></div>
<script language="javascript" src="../Scripts/Pictures.js" type="text/javascript"></script>
</body>
</html>
