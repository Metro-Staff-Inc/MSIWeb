<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhoneBlast.aspx.cs" Inherits="MSI.Web.MSINet.PhoneBlast" %>
<%@ Register Src="~/Controls/MSINetPhoneBlast.ascx" TagName="MSINetPhoneBlast" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Phone Blast!</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <div id="main" runat="server" align="center">
    <form runat="server">
        <div><uc1:MastHead ID="ctlMastHead" runat="server" /></div><br />
        <div><uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" /></div><br />
        <div><uc3:MSINetMainMenu ID="MainMenu2" runat="server" SelectedSection="HoursReport" /></div>
        <div><uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Phone Blast" /></div>
        <uc6:MSINetPhoneBlast id="ctlPhoneBlast" runat="server"></uc6:MSINetPhoneBlast>
    </form>
    </div>
</body>
</html>
