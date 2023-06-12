<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientRoster.aspx.cs" Inherits="MSI.Web.MSINet.ClientRoster" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MSINetClientRoster.ascx" TagName="MSINetClientRoster" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="AdminHead" runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
    #tblAvailableEmployees input
    {
        /*font-size:smaller;*/
    }
    #availableEmp
    {
        padding:0px;
        width:448px;
        height:310px;
        text-align:left;
        overflow:auto;
    }
    #currentEmp
    {
        padding:0px;
        width:800px;
        height:310px;
        text-align:left;
        overflow:auto;
    }
    .Row
    {
        background-color:#FFFFFF;
    }
    .AltRow
    {
        background-color:#DDDDFF;
    }
    .ValueChanged
    {
        background-color:#FFDDDD;
    }
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
            <uc1:MastHead ID="ctlMastHead" runat="server" />
            <br />
            <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" />
            <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Update Client Roster" />
            <br />
            <uc6:MSINetClientRoster ID="ctlClientRoster" runat="server" />
            <br />
    </div>
    </form>
</body>
</html>
