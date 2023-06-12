<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeeDNR.aspx.cs" Inherits="auth_EmployeeDNR" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Info</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script src="../Scripts/DNR.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
</head>
<body>
<div align="center">
    <form id="form1" runat="server" >

    <div visible="false">
        <input type="hidden" runat="server" id="userID"/>
        <input type="hidden" runat="server" id="webServiceLoc" />
        <input type="hidden" runat="server" id="hdnRemoveDnr" value="false" />
    </div>
    
    <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="DaysWorkedReport" />
        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Employee DNR Info" />

        <div id="divBody">
        <div id="dnrRecords" runat="server">
        </div>
        <div id="dnrID" runat="server">
            <span>Employee ID:</span><input type="text" id="empID" />
            <input type="button" value="GO" id="btnID"/>
            <span id="empName"></span>
        </div>        
        <div id="dnrInfo" runat="server">
            <table>
            <tr><td><span>DNR Client:</span></td><td><select disabled="disabled" id="ddlClient"></select></td>
                <td><span>Shift:</span></td><td><select disabled="disabled" id="ddlShift"></select></td></tr>
            <tr><td><span>Location:</span></td><td><select disabled="disabled" id="ddlLocation"></select></td></tr>
            <tr><td><span>Supervisor:</span></td><td><input disabled="disabled" id="super" type="text" value="Unknown"/></td>
                <td><span>Start:</span></td><td><input disabled="disabled" id="start" type="text" /></td></tr>
            <tr><td><span>DNR Reason:</span></td><td><select disabled="disabled" id="ddlReason"></select></td>
                <td colspan="2"><button id="btnDNR" type="button" disabled="disabled">DNR</button></td></tr>
            </table>
        </div>
        </div>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    </form>
 </div>    
</body>
</html>
