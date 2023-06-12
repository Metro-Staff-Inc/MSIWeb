<%@ Page Language="C#" EnableViewState="false" EnableViewStateMac="false" AutoEventWireup="true" CodeFile="CheckIn.aspx.cs" Inherits="MSI.Web.MSINet.CheckIn" %>
<%@ Register Src="~/Controls/MSINetCheckIn.ascx" TagName="MSINetCheckIn" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <meta http-equiv="refresh" content="600" />
</head>
<body><div align="center">
    <form id="form1" runat="server">
    <div>
    <uc1:MastHead ID="MastHead1" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="CheckInOut" />
                
        <uc4:MSINetSectionHeader ID="ctlSectionHead" runat="server" SectionHeader="Check In / Check Out" />
        <br />
        <uc5:MSINetCheckIn id="ctlCheckIn" runat="server" DisplayMode="Swipe">
        </uc5:MSINetCheckIn>

    </form>
 </div>    
</body>
</html>