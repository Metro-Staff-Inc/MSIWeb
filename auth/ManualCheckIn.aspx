<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManualCheckIn.aspx.cs" Inherits="MSI.Web.MSINet.ManualCheckIn" %>

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
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <div>
    <uc1:MastHead ID="MastHead1" runat="server" />
    </div>
        &nbsp;&nbsp;<uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="CheckInOut" />
        <uc4:MSINetSectionHeader ID="ctlSectionHead" runat="server" SectionHeader="Manual Check In / Check Out" />
        <asp:Panel ID="pnlMain" runat="server" Height="373px" HorizontalAlign="Center" Width="999px">
            <uc5:MSINetCheckIn id="ctlCheckIn" runat="server" DisplayMode="Manual">
            </uc5:MSINetCheckIn>
        </asp:Panel>
    </form>
 </div>    
</body>
</html>