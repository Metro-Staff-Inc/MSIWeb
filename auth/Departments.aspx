<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Departments.aspx.cs" Inherits="MSI.Web.MSINet.Departments" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Department List</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script language="javascript" src="../Scripts/Departments.js" type="text/javascript"></script>
    <style type="text/css">
        .inactive
        {
            background-color:#DDAAAA;
        }
        .highLight
        {
            background-color: yellow;
        }
    </style>
</head>
<body>
    <div id="main" runat="server" align="center">
        <form id="Form2" runat="server">
            <div><uc1:MastHead ID="ctlMastHead" runat="server" /></div><br />
            <div><uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" /></div><br />
            <div><uc3:MSINetMainMenu ID="MainMenu2" runat="server" SelectedSection="HoursReport" /></div>
            <div><uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Department Info" /></div>
        </form>
    </div>
    <div id="depts" runat="server" align="center">

    </div>
</body>
</html>
