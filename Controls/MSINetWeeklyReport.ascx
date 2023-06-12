<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetWeeklyReport.ascx.cs" Inherits="MSI.Web.Controls.MSINetHoursReportNew" %>
<script language="javascript" type="text/javascript">
// <![CDATA[

    function btnPrintTest_onclick() {
        alert("Here we go!");
    }

// ]]>
</script>

<link href="../Includes/MSIMain.css" rel="stylesheet" type="text/css" />

<link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
<link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
<link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
<link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
<script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/WeeklyReport.js" type="text/javascript"></script>
<link href="../Includes/flexigrid.pack.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/flexigrid.pack.js" type="text/javascript"></script>

<input type="hidden" runat="server" id="userID" value="" />
<input type="hidden" runat="server" id="clientID" value="" />

<div id="reportMain">
    <div id="reportParams">
    <span>Week of:</span><br />
    <input id="weekEnd" type="text" size="10" />
    <input id="btnWeekEnd" type="button" value="GO" />
    <input id="btnPrintTest" type="button" value="PRINT!" onclick="return btnPrintTest_onclick()" />
    </div>
    <div id="report">
    </div>
</div>